using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Creature
{
    public class CreatureInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets the entry id of the creature template
        /// </summary>
        public UInt32 CreatureId { get; set; }

        /// <summary>
        /// Gets or sets the creature name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sub-name of the creature (not sure what this is?)
        /// </summary>
        public string SubName { get; set; }

        /// <summary>
        /// Gets or sets the creature flags
        /// </summary>
        public UInt32 CreatureFlags { get; set; }

        /// <summary>
        /// Gets or sets the creature type
        /// </summary>
        public UInt32 CreatureType { get; set; }

        /// <summary>
        /// Gets or sets the creature family
        /// </summary>
        public UInt32 CreatureFamily { get; set; }

        /// <summary>
        /// Gets or sets the creature rank
        /// </summary>
        public UInt32 CreatureRank { get; set; }

        #endregion
    }
}
