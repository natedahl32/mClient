using mClient.Constants;

namespace mClient.World.AI.Activity.Messages
{
    public class RollOnLootMessage : ActivityMessage
    {
        #region Constructors

        protected RollOnLootMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public RollOnLootMessage() : this(WorldServerOpCode.SMSG_LOOT_START_ROLL)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the guid of the source of loot (creature or chest)
        /// </summary>
        public ulong LootSourceGuid { get; set; }

        /// <summary>
        /// Gets or sets the item slot the loot is in
        /// </summary>
        public uint ItemSlot { get; set; }

        /// <summary>
        /// Gets or sets the id of the item we are rolling on
        /// </summary>
        public uint ItemId { get; set; }

        /// <summary>
        /// Gets or sets the id of the random suffix for the item
        /// </summary>
        public uint RandomSuffix { get; set; }

        /// <summary>
        /// Gets or sets the id of the random property id for the item
        /// </summary>
        public uint RandomPropertyId { get; set; }

        /// <summary>
        /// Gets or sets the timeout of the roll
        /// </summary>
        public uint RollTimeout { get; set; }

        /// <summary>
        /// Gets or sets the roll options available to us
        /// </summary>
        public byte RollOptionMask { get; set; }

        #endregion
    }
}
