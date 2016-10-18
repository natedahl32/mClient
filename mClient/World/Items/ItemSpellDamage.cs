using mClient.Constants;
using System;

namespace mClient.World.Items
{
    public class ItemDamage
    {
        /// <summary>
        /// Gets or sets the damage type
        /// </summary>
        public SpellSchools DamageType { get; set; }

        /// <summary>
        /// Gets or sets the minimum damage
        /// </summary>
        public float MinDamage { get; set; }

        /// <summary>
        /// Gets or sets the maximum damage
        /// </summary>
        public float MaxDamage { get; set; }
    }
}
