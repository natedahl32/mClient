using System;

namespace mClient.World.Items
{
    public class ItemSpellEffect
    {
        /// <summary>
        /// Gets or sets the spell id
        /// </summary>
        public UInt32 SpellId { get; set; }

        /// <summary>
        /// Gets or sets the spell trigger
        /// </summary>
        public UInt32 SpellTrigger { get; set; }

        /// <summary>
        /// Gets or sets the number of charges
        /// </summary>
        public UInt32 SpellCharges { get; set; }
    }
}
