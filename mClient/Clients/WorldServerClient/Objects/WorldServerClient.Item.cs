using mClient.Constants;
using mClient.Shared;
using mClient.World.Items;
using System;
using System.Linq;

namespace mClient.Clients
{
    public class Item : Object
    {
        #region Constructors

        public Item(WoWGuid guid) : base(guid)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the base item info
        /// </summary>
        public ItemInfo BaseInfo
        {
            get { return ItemManager.Instance.Get(this.ObjectFieldEntry); }
        }

        /// <summary>
        /// Gets the guid of the object that this item is contained in (if any)
        /// </summary>
        public WoWGuid ContainedInGuid
        {
            get
            {
                var guidLong = GetGuid(GetFieldValue((int)ItemFields.ITEM_FIELD_CONTAINED), GetFieldValue((int)ItemFields.ITEM_FIELD_CONTAINED + 1));
                return new WoWGuid(guidLong);
            }
        }

        /// <summary>
        /// Gets the current stack count of this item
        /// </summary>
        public uint StackCount
        {
            get { return GetFieldValue((int)ItemFields.ITEM_FIELD_STACK_COUNT); }
        }

        /// <summary>
        /// Gets the current durability of the item
        /// </summary>
        public uint Durability
        {
            get { return GetFieldValue((int)ItemFields.ITEM_FIELD_DURABILITY); }
        }

        #endregion
    }

    public class InventoryItemSlot
    {
        public int Slot { get; set; }
        public Item Item { get; set; }
    }
}
