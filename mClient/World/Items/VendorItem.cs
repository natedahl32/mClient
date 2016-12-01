using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Items
{
    public class VendorItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the index slot this is available at from the vendor
        /// </summary>
        public uint ItemIndex { get; set; }

        /// <summary>
        /// Gets or sets the id of the item available
        /// </summary>
        public uint ItemId { get; set; }

        /// <summary>
        /// Gets or sets the count of this item available from the vendor
        /// </summary>
        public uint CurrentVendorCount { get; set; }

        /// <summary>
        /// Gets or sets the price of the item
        /// </summary>
        public uint Price { get; set; }

        /// <summary>
        /// Gets or sets the number of items bought when purchasing this item
        /// </summary>
        public uint BuyCount { get; set; }

        /// <summary>
        /// Gets the base item info for this vendor item
        /// </summary>
        public ItemInfo Item
        {
            get { return ItemManager.Instance.Get(ItemId); }
        }

        #endregion
    }
}
