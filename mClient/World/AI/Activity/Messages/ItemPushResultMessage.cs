using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class ItemPushResultMessage : ActivityMessage
    {
        #region Constructors

        protected ItemPushResultMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public ItemPushResultMessage() : this(WorldServerOpCode.SMSG_ITEM_PUSH_RESULT)
        {   }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the guid of the player receiving this item
        /// </summary>
        public ulong PlayerGuid { get; set; }

        /// <summary>
        /// Gets or sets whether or not the item was looted. If not looted, it was received from an NPC
        /// </summary>
        public bool Looted { get; set; }

        /// <summary>
        /// Gets or sets whether or not the item was created. If not created, it was received from an NPC
        /// </summary>
        public bool Created { get; set; }

        /// <summary>
        /// Gets or sets the bag slot the item was put in
        /// </summary>
        public byte BagSlot { get; set; }

        /// <summary>
        /// Gets or sets the item slot the item was put in
        /// </summary>
        public uint ItemSlot { get; set; }

        /// <summary>
        /// Gets or sets the id of the item
        /// </summary>
        public uint ItemId { get; set; }

        /// <summary>
        /// Gets or sets the suffix factor of the item
        /// </summary>
        public uint ItemSuffixFactor { get; set; }

        /// <summary>
        /// Gets or sets the random property id of the item
        /// </summary>
        public uint RandomPropertyId { get; set; }

        /// <summary>
        /// Gets or sets the count of items
        /// </summary>
        public uint Count { get; set; }

        /// <summary>
        /// Gets or sets the count of item in inventory
        /// </summary>
        public uint CountInInventory { get; set; }

        #endregion
    }
}
