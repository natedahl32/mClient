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
                    flags = flags | SpellTargetFlags.TARGET_FLAG_UNK1;
            }
            else if (target.Type == ObjectType.Item || target.Type == ObjectType.Container)
                flags = SpellTargetFlags.TARGET_FLAG_ITEM;
            else if (target.Type == ObjectType.Corpse)
                flags = SpellTargetFlags.TARGET_FLAG_CORPSE;
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

            if (flags.Has(SpellTargetFlags.TARGET_FLAG_CORPSE | SpellTargetFlags.TARGET_FLAG_PVP_CORPSE))
                packet.WritePackedUInt64(target.Guid.GetOldGuid());

            Send(packet);
        }

        #endregion
    }
}
