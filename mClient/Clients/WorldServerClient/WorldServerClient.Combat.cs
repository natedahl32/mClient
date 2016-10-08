using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mClient.Shared;
using mClient.Network;
using mClient.Crypt;
using mClient.Constants;

namespace mClient.Clients
{
    public partial class WorldServerClient
    {
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
            }
        }

        /// <summary>
        /// Handles spell cast complete
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_SPELL_GO)]
        public void HandleSpellGo(PacketIn packet)
        {
            // TODO: Basically same packet structure as SPELL_START, I believe this one means it has completed though
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
                Attack(victimGuid);
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
            // TODO: Use this data to remove any enemies from our list once they die
        }

        public void Attack(Object target)
        {
            Attack(target.Guid.GetOldGuid());
            //PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_SET_SELECTION);
            //if (objectMgr.getPlayerObject() != null)
            //{
            //    packet.Write(target.Guid.GetNewGuid());
            //}
            //Send(packet);

            //packet = new PacketOut(WorldServerOpCode.CMSG_ATTACKSWING);
            //if (objectMgr.getPlayerObject() != null)
            //{
            //    packet.Write(target.Guid.GetNewGuid());
            //}
            //Send(packet);
        }


        public void Attack(UInt64 targetGuid)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_SET_SELECTION);
            if (objectMgr.getPlayerObject() != null)
            {
                packet.Write(targetGuid);
            }
            Send(packet);

            packet = new PacketOut(WorldServerOpCode.CMSG_ATTACKSWING);
            if (objectMgr.getPlayerObject() != null)
            {
                packet.Write(targetGuid);
            }
            Send(packet);
        }

    }
}
