using mClient.Constants;
using System;

namespace mClient.World.Items
{
    public class ItemStat
    {
        /// <summary>
        /// Gets or sets the stat type
        /// </summary>
        public ItemModType StatType { get; set; }

        /// <summary>
        /// Gets or sets the stat value
        /// </summary>
        public UInt32 StatValue { get; set; }
    }
}
