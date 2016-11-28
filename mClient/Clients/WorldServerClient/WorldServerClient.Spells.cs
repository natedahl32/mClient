using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mClient.Shared;
using mClient.Network;
using mClient.Crypt;
using mClient.Constants;
using mClient.World.AI.Activity.Messages;
using mClient.Clients.UpdateBlocks;

namespace mClient.Clients
{
    public partial class WorldServerClient
    {
        #region Handlers

        /// <summary>
        /// Handles an update that a cast of ours failed
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_CAST_FAILED)]
        public void HandleCastFailed(PacketIn packet)
        {
            var spellId = packet.ReadUInt32();
            var status = packet.ReadByte();

            if (status == 2)
            {
                var result = (SpellCastResult)packet.ReadByte();
                switch (result)
                {
                    case SpellCastResult.SPELL_FAILED_REQUIRES_SPELL_FOCUS:
                        var requiredSpellFocus = packet.ReadUInt32();
                        break;
                    case SpellCastResult.SPELL_FAILED_REQUIRES_AREA:
                        break;
                    case SpellCastResult.SPELL_FAILED_EQUIPPED_ITEM_CLASS:
                        var equippedItemClass = packet.ReadUInt32();
                        var equippedItemSubClassMask = packet.ReadUInt32();
                        var equippedItemInventoryMask = packet.ReadUInt32();
                        break;
                }

                var message = new SpellCastFailedMessage()
                {
                    SpellId = spellId,
                    Result = result
                };
                player.PlayerAI.SendMessageToAllActivities(message);
            }

            // Otherwise cast status was OK (doesn't get sent as far as I know)
        }

        /// <summary>
        /// Handles an update that a spell was interrupted
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_SPELL_FAILURE)]
        public void HandleSpellFailure(PacketIn packet)
        {
            var casterGuid = packet.ReadPackedGuidToWoWGuid();
            var spellId = packet.ReadUInt32();

            // Means a spell was interrupted, send a message
            var message = new SpellCastInterruptedMessage() 
            {
                CasterGuid = casterGuid,
                SpellId = spellId
            };
            player.PlayerAI.SendMessageToAllActivities(message);
        }

        /// <summary>
        /// Handles an update that a spell cast was completed
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_SPELL_GO)]
        public void HandleSpellGo(PacketIn packet)
        {
            var casterOrItemGuid = packet.ReadPackedGuidToWoWGuid(); // could be item if spell was cast from an item, otherwise caster
            var casterGuid2 = packet.ReadPackedGuidToWoWGuid(); // always the caster (unit) that cast the spell, even if it was using an item

            var spellId = packet.ReadUInt32();
            var castFlags = packet.ReadUInt16();

            // hits
            var targetCount = packet.ReadByte();
            for (int i = 0; i < targetCount; i++)
            {
                var targetGuid = packet.ReadUInt64();
            }

            // miss
            var miss = packet.ReadByte();
            for (int i = 0; i < miss; i++)
            {
                var targetGuid = packet.ReadUInt64();
                var missCondition = (SpellMissInfo)packet.ReadByte();
                if (missCondition == SpellMissInfo.SPELL_MISS_REFLECT)
                    packet.ReadByte(); // reflect result
            }

            // Spell cast targets
            var spellCastTargets = new SpellCastTargets();
            spellCastTargets.ReadFromPacket(packet);

            // TODO: Get ammo information if we need it, don't think we do though

            // Send spell go message
            var message = new SpellCastGoMessage()
            {
                ItemOrCasterGuid = casterOrItemGuid,
                CasterGuid = casterGuid2,
                SpellId = spellId,
                CastFlags = castFlags,
                UnitTarget = (spellCastTargets.UnitTargetGuid != null ? objectMgr.getObject(spellCastTargets.UnitTargetGuid) : null)
            };
            player.PlayerAI.SendMessageToAllActivities(message);

            // If we are the player who cast this spell then let the player handle it
            if (casterGuid2.GetOldGuid() == player.Guid.GetOldGuid())
                player.HandleSpellGo(message);
        }

        #endregion

        #region Actions

        public void CastSpell(Object target, UInt32 SpellId)
        {
            SpellTargetFlags flags = 0;

            if (target == objectMgr.getPlayerObject())
                flags = SpellTargetFlags.TARGET_FLAG_SELF;
            else if (target.Type == ObjectType.Unit || target.Type == ObjectType.Player)
                flags = SpellTargetFlags.TARGET_FLAG_UNIT;
            else if (target.Type == ObjectType.Object || target.Type == ObjectType.GameObject)
            {
                flags = SpellTargetFlags.TARGET_FLAG_OBJECT;
                if ((target as GameObject) != null && (target as GameObject).BaseInfo.GameObjectType == GameObjectType.Chest)
                    flags = flags | SpellTargetFlags.TARGET_FLAG_GAMEOBJECT_ITEM;
            }
            else if (target.Type == ObjectType.Item || target.Type == ObjectType.Container)
                flags = SpellTargetFlags.TARGET_FLAG_ITEM;
            else if (target.Type == ObjectType.Corpse)
                flags = SpellTargetFlags.TARGET_FLAG_CORPSE_ALLY;
            else
            {
                flags = SpellTargetFlags.TARGET_FLAG_UNIT;
            }

            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_CAST_SPELL);
            packet.Write(SpellId);
            packet.Write((UInt16)flags);
            
            // 0x18A02
            if (flags.Has(SpellTargetFlags.TARGET_FLAG_UNIT | SpellTargetFlags.TARGET_FLAG_UNK2 | 
                          SpellTargetFlags.TARGET_FLAG_OBJECT | SpellTargetFlags.TARGET_FLAG_OBJECT_UNK |
                          SpellTargetFlags.TARGET_FLAG_ITEM | SpellTargetFlags.TARGET_FLAG_TRADE_ITEM))
            {
                packet.WritePackedUInt64(target.Guid.GetOldGuid());
            }

            // 0x20
            if (flags.Has(SpellTargetFlags.TARGET_FLAG_SOURCE_LOCATION))
            {
                packet.Write(objectMgr.getPlayerObject().Position.X);
                packet.Write(objectMgr.getPlayerObject().Position.Y);
                packet.Write(objectMgr.getPlayerObject().Position.Z);
            }

            // 0x40
            if (flags.Has(SpellTargetFlags.TARGET_FLAG_DEST_LOCATION))
            {
                packet.Write(target.Position.X);
                packet.Write(target.Position.Y);
                packet.Write(target.Position.Z);
            }

            if (flags.Has(SpellTargetFlags.TARGET_FLAG_CORPSE_ALLY | SpellTargetFlags.TARGET_FLAG_PVP_CORPSE))
                packet.WritePackedUInt64(target.Guid.GetOldGuid());

            Send(packet);
        }

        #endregion
    }
}
