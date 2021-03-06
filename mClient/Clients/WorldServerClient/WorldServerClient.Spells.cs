﻿using System;
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
        public void CastSpell(Object target, UInt32 SpellId)
        {
            SpellTargetFlags flags = 0;

            if (target == objectMgr.getPlayerObject())
                flags = SpellTargetFlags.Self;
            else
            {
                flags = SpellTargetFlags.Unit;
                //Target(target as Unit);
            }

            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_CAST_SPELL);
            packet.Write(SpellId);
            packet.Write((byte)0); // unk flags in WCell

            packet.Write((UInt32)flags);

            // 0x18A02
            if (flags.Has(SpellTargetFlags.SpellTargetFlag_Dynamic_0x10000 | SpellTargetFlags.Corpse | SpellTargetFlags.Object |
                SpellTargetFlags.PvPCorpse | SpellTargetFlags.Unit))
            {
                packet.Write(target.Guid.GetNewGuid());
            }

            // 0x1010
            if (flags.Has(SpellTargetFlags.TradeItem | SpellTargetFlags.Item))
            {
                packet.Write(target.Guid.GetNewGuid());
            }

            // 0x20
            if (flags.Has(SpellTargetFlags.SourceLocation))
            {
                packet.Write(objectMgr.getPlayerObject().Position.X);
                packet.Write(objectMgr.getPlayerObject().Position.Y);
                packet.Write(objectMgr.getPlayerObject().Position.Z);
            }

            // 0x40
            if (flags.Has(SpellTargetFlags.DestinationLocation))
            {
                packet.Write(target.Guid.GetNewGuid());

                packet.Write(target.Position.X);
                packet.Write(target.Position.Y);
                packet.Write(target.Position.Z);

            }

            Send(packet);
        }

     }
}
