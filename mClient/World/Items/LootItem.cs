using mClient.Constants;
using mClient.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Items
{
    public class LootItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the item id of the loot
        /// </summary>
        public UInt32 ItemId { get; set; }

        /// <summary>
        /// Gets or sets the number of items of this type
        /// </summary>
        public UInt32 Count { get; set; }

        /// <summary>
        /// Gets or sets the random property id
        /// </summary>
        public int RandomPropertyId { get; set; }

        /// <summary>
        /// Gets the loot slot this item is in
        /// </summary>
        public byte LootSlot { get; set; }

        /// <summary>
        /// Gets the loot slot type
        /// 0 = get
        /// 1 = look only
        /// 2 = master select
        /// </summary>
        public byte LootSlotType { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reads the data from a packet
        /// </summary>
        /// <param name="packet"></param>
        public void Read(PacketIn packet)
        {
            LootSlot = packet.ReadByte();
            ItemId = packet.ReadUInt32();
            Count = packet.ReadUInt32();
            packet.ReadUInt32();    // Display info id
            packet.ReadUInt32();    // 0
            RandomPropertyId = packet.ReadInt32();
            LootSlotType = packet.ReadByte();
        }

        #endregion
    }
}
