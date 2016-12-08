using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Quest
{
    public class GossipMenuItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the index in the gossip menu for this menu item
        /// </summary>
        public uint GossipMenuIndex { get; set; }

        /// <summary>
        /// Gets or sets the text sent for this gossip menu item
        /// </summary>
        public string GossipText { get; set; }

        #endregion
    }
}
