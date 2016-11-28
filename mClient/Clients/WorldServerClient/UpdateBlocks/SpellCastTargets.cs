using mClient.Constants;
using mClient.Network;
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Clients.UpdateBlocks
{
    public class SpellCastTargets
    {
        #region Constructors

        public SpellCastTargets()
        {
            TargetsMask = 0;
            UnitTargetGuid = null;
            ItemGuid = null;
            CorpseGuid = null;
            SourceLocation = null;
            DestinationLocation = null;
        }

        #endregion

        #region Properties

        public SpellTargetFlags TargetsMask { get; set; }

        public WoWGuid UnitTargetGuid { get; set; }

        public WoWGuid ItemGuid { get; set; }

        public WoWGuid CorpseGuid { get; set; }

        public Coordinate SourceLocation { get; set; }

        public Coordinate DestinationLocation { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reads spell cast targets object from a packet
        /// </summary>
        /// <param name="packet"></param>
        public void ReadFromPacket(PacketIn packet)
        {
            // Spell cast targets
            TargetsMask = (SpellTargetFlags)packet.ReadUInt16();

            if (TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_UNIT) ||
                TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_PVP_CORPSE) ||
                TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_OBJECT) ||
                TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_CORPSE_ALLY) ||
                TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_UNK2))
            {
                UnitTargetGuid = packet.ReadPackedGuidToWoWGuid();
            }

            if (TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_ITEM) ||
                TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_TRADE_ITEM))
            {
                ItemGuid = packet.ReadPackedGuidToWoWGuid();
            }

            if (TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_SOURCE_LOCATION))
            {
                SourceLocation = new Coordinate(packet.ReadFloat(), packet.ReadFloat(), packet.ReadFloat());
            }

            if (TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_DEST_LOCATION))
            {
                DestinationLocation = new Coordinate(packet.ReadFloat(), packet.ReadFloat(), packet.ReadFloat());
            }

            if (TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_STRING))
                packet.ReadString();
        }

        /// <summary>
        /// Writes spell cast targets to a packet
        /// </summary>
        /// <param name="packet"></param>
        public void WriteToPacket(ref PacketOut packet)
        {
            packet.Write((ushort)TargetsMask);

            if (UnitTargetGuid != null && 
                (TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_UNIT) ||
                TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_OBJECT) ||
                TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_CORPSE_ALLY) ||
                TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_UNK2)))
            {
                packet.WritePackedUInt64(UnitTargetGuid.GetOldGuid());
            }

            if (ItemGuid != null &&
                (TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_ITEM) ||
                TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_TRADE_ITEM)))
            {
                packet.WritePackedUInt64(ItemGuid.GetOldGuid());
            }

            if (SourceLocation != null && TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_SOURCE_LOCATION))
            {
                packet.Write(SourceLocation.X);
                packet.Write(SourceLocation.Y);
                packet.Write(SourceLocation.Z);
            }

            if (DestinationLocation != null && TargetsMask.Has(SpellTargetFlags.TARGET_FLAG_DEST_LOCATION))
            {
                packet.Write(DestinationLocation.X);
                packet.Write(DestinationLocation.Y);
                packet.Write(DestinationLocation.Z);
            }
        }

        #endregion
    }
}
