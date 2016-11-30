using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Items
{
    /// <summary>
    /// Class that holds information pertaining to items that are required by a player
    /// </summary>
    public class RequiredItemData
    {
        #region Properties

        /// <summary>
        /// Gets or sets the id of the item that is required
        /// </summary>
        public uint ItemId { get; set; }

        /// <summary>
        /// Gets or sets the item count that we should have on hand
        /// </summary>
        public uint ItemCount { get; set; }

        #endregion
    }
}
