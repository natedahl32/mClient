﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mClient.Shared;
using mClient.Network;
using mClient.Crypt;
using mClient.Constants;
using mClient.Terrain;

namespace mClient.Clients
{
    public partial class WorldServerClient
    {
        #region Handlers

        /// <summary>
        /// Handles attack swings not in range
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_ATTACKSWING_NOTINRANGE)]
        public void HandleAttackSwingNotInRange(PacketIn packet)
        {
            // We are trying to attack but we aren't close enough, get closer
            player.PlayerAI.NotInMeleeRange = true;
        }

        /// <summary>
        /// Handles attacks where we are facing the wrong direction
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_ATTACKSWING_BADFACING)]
        public void HandleAttackSwingBadFacing(PacketIn packet)
        {
            // TODO: Face the target!
            var angle = TerrainMgr.CalculateAngle(objectMgr.getPlayerObject().Position, player.PlayerAI.TargetSelection.Position);
            objectMgr.getPlayerObject().Position.O = angle;
            SendMovementPacket(WorldServerOpCode.MSG_MOVE_SET_FACING);
        }

        /// <summary>
        /// Handles attacks where we are attacking a dead target
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_ATTACKSWING_DEADTARGET)]
        public void HandleAttackSwingDeadTarget(PacketIn packet)
        {
            // TODO: Change targets and remove the target from the enemy list
            var i = 0;
        }

        /// <summary>
        /// Handles attacks where we can't attack our target
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_ATTACKSWING_CANT_ATTACK)]
        public void HandleAttackSwingCantAttack(PacketIn packet)
        {
            // TODO: Not sure what to do here. Change targets?
            var i = 0;
        }

        /// <summary>
        /// Handles spell cast start
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_SPELL_START)]
        public void HandleSpellStart(PacketIn packet)
        {
            // Get guid of caster or item
            byte mask = packet.ReadByte();
            WoWGuid itemOrCasterGuid = new WoWGuid(mask, packet.ReadBytes(WoWGuid.BitCount8(mask)));

            // Get caster guid
            mask = packet.ReadByte();
            WoWGuid casterGuid = new WoWGuid(mask, packet.ReadBytes(WoWGuid.BitCount8(mask)));

            var spellId = packet.ReadUInt32();
            var castFlags = packet.ReadUInt16();
            var delay = packet.ReadUInt32();

            // SpellCastTarget
            var targetMask = packet.ReadUInt16();
            if ((targetMask & (2 | 16 | 512 | 2048 | 4096 | 32768 | 65536)) > 1)
            {
                // Get target guid
                mask = packet.ReadByte();
                WoWGuid targetGuid = new WoWGuid(mask, packet.ReadBytes(WoWGuid.BitCount8(mask)));
                // TODO: Track healing spells here for Raids
            }
        }

        /// <summary>
        /// Handles spell cast complete
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_SPELL_GO)]
        public void HandleSpellGo(PacketIn packet)
        {
            // TODO: Basically same packet structure as SPELL_START, I believe this one means it has completed though.
            // Not sure if we actually need this or not (probably for a healing manager)
        }

        /// <summary>
        /// Handles spell cast non melee damage log
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_SPELLNONMELEEDAMAGELOG)]
        public void HandleSpellNonMeleeDamageLog(PacketIn packet)
        {
            // TODO: Not sure if we want to handle this or not
        }

        /// <summary>
        /// Handles attack start
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_ATTACKSTART)]
        public void HandleAttackStart(PacketIn packet)
        {
            var attackerGuid = packet.ReadUInt64();
            var victimGuid = packet.ReadUInt64();
            
            // If the attacker is in our party, than start attacking as well
            if (player.CurrentGroup != null && player.CurrentGroup.IsInGroup(attackerGuid))
            {
                // On second thought, we don't actually want to do this. If an enemy is misclicked we don't want to
                // run in and start attacking until we are told to do so or until the enemy starts attacking someone in
                // our party.

                //var victim = new WoWGuid(victimGuid);
                //player.CurrentGroup.GetGroupMember(attackerGuid).AddEnemy(victim);
                //player.AddEnemy(victim);
            }
                
            // If someone in the group is being attacked, add the attacker as an enemy
            if (player.CurrentGroup != null && player.CurrentGroup.IsInGroup(victimGuid))
            {
                var attacker = new WoWGuid(attackerGuid);
                player.CurrentGroup.GetGroupMember(victimGuid).AddEnemy(attacker);
                player.AddEnemy(attacker);
            }

            // If it's us, reset the not in melee flag
            if (attackerGuid == player.PlayerObject.Guid.GetOldGuid() || victimGuid == player.PlayerObject.Guid.GetOldGuid())
                player.PlayerAI.NotInMeleeRange = false;
        }

        /// <summary>
        /// Handles kill logs
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_PARTYKILLLOG)]
        public void HandlePartyKillLog(PacketIn packet)
        {
            var killingBlowGuid = packet.ReadUInt64();
            var victimGuid = packet.ReadUInt64();
            player.RemoveEnemy(victimGuid);

            // Try to loot every NPC that is killed in case there is a quest item or something on it that we need
            player.AddLootable(new WoWGuid(victimGuid));
        }

        /// <summary>
        /// Handle attack state updates to monitor health
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_ATTACKERSTATEUPDATE)]
        public void HandleAttackerStateUpdate(PacketIn packet)
        {
            var hitInfo = packet.ReadUInt32();

            // Attacker
            WoWGuid attackerGuid = packet.ReadPackedGuidToWoWGuid();

            // Target
            WoWGuid targetGuid = packet.ReadPackedGuidToWoWGuid();

            // Damage applied (full damage taking into account absorbs, resists, and blocks
            var fullDamageApplied = packet.ReadUInt32();

            // Check if the target is us or someone in our party. If so, make sure the enemy is in our list
            if (player.CurrentGroup != null && player.CurrentGroup.IsInGroup(targetGuid.GetOldGuid()))
            {
                player.CurrentGroup.GetGroupMember(targetGuid.GetOldGuid()).AddEnemy(attackerGuid);
                player.AddEnemy(attackerGuid);
            }
            // Check if someone in our group is attacking something
            if (player.CurrentGroup != null && player.CurrentGroup.IsInGroup(attackerGuid.GetOldGuid()))
            {
                player.CurrentGroup.GetGroupMember(attackerGuid.GetOldGuid()).AddEnemy(targetGuid);
                player.AddEnemy(targetGuid);
            }
        }

        #endregion

        #region Actions

        /// <summary>
        /// Starts attacking a target
        /// </summary>
        /// <param name="target"></param>
        public void Attack(Object target)
        {
            Attack(target.Guid.GetOldGuid());
        }

        /// <summary>
        /// Starts attacking the target with a guid
        /// </summary>
        /// <param name="targetGuid"></param>
        public void Attack(UInt64 targetGuid)
        {
            SetTarget(targetGuid);

            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_ATTACKSWING);
            packet.Write(targetGuid);
            Send(packet);
        }

        /// <summary>
        /// Sets our target selection to the object with this guid
        /// </summary>
        /// <param name="targetGuid"></param>
        public void SetTarget(UInt64 targetGuid)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_SET_SELECTION);
            packet.Write(targetGuid);
            Send(packet);
        }

        #endregion
    }
}
