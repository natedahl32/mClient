using mClient.Constants;
using mClient.Shared;
using mClient.World.Items;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Clients
{
    public class Container : Item
    {
        #region Declarations

        private ConcurrentDictionary<int, Item> mInventory = new ConcurrentDictionary<int, Item>();

        #endregion

        #region Constructors

        public Container(WoWGuid guid) : base(guid)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The inventory or bank bag slot of this container
        /// </summary>
        public int InventoryBagSlot { get; set; }

        /// <summary>
        /// Gets the number of slots this container has
        /// </summary>
        public uint NumberOfSlots { get { return GetFieldValue((int)ContainerFields.CONTAINER_FIELD_NUM_SLOTS); } }

        /// <summary>
        /// Gets all items in this container
        /// </summary>
        public IEnumerable<InventoryItemSlot> ItemsInContainer
        {
            get
            {
                return mInventory.Where(kvp => kvp.Value != null).Select(kvp => new InventoryItemSlot() { Bag = InventoryBagSlot, Slot = kvp.Key, Item = kvp.Value }).ToList();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears the slot in the bag
        /// </summary>
        /// <param name="slot"></param>
        public void ClearSlot(int slot)
        {
            if (mInventory.ContainsKey(slot))
            {
                var currentItem = mInventory[slot];
                mInventory.TryUpdate(slot, null, currentItem);
            }
        }

        /// <summary>
        /// Updates the items in the container
        /// </summary>
        /// <param name="client"></param>
        public void UpdateItemsInContainer(WorldServerClient client)
        {
            for (int i = (int)ContainerFields.CONTAINER_FIELD_SLOT_1; i <= (int)ContainerFields.CONTAINER_FIELD_SLOT_LAST; i += 2)
            {
                var slot = (i - (int)ContainerFields.CONTAINER_FIELD_SLOT_1) / 2;
                var guid = GetWoWGuid(GetFieldValue(i), GetFieldValue(i + 1));
                var item = client.objectMgr.getObject(guid) as Item;

                // Add the item to the containers inventory
                if (mInventory.ContainsKey(slot))
                {
                    var currentItem = mInventory[slot];
                    mInventory.TryUpdate(slot, item, currentItem);
                }
                else
                {
                    if (item != null)
                        mInventory.TryAdd(slot, item);
                }

                // If we have an item object, check if it's in the item manager and if not, query for it
                if (item != null)
                {
                    if (ItemManager.Instance.Get(item.ObjectFieldEntry) == null)
                        client.QueryItemPrototype(item.ObjectFieldEntry);
                }
            }
        }

        #endregion
    }
}
