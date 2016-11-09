using mClient.Constants;
using mClient.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class LootMessage : ActivityMessage
    {
        #region Constructors

        protected LootMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public LootMessage() : this(WorldServerOpCode.SMSG_LOOT_RESPONSE)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the amount of coin available in the loot
        /// </summary>
        public UInt32 CoinAmount { get; set; }

        /// <summary>
        /// Gets or sets the items available to loot
        /// </summary>
        public List<LootItem> Items { get; set; }

        #endregion
    }
}
