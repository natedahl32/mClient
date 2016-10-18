
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mClient.World.Items
{
    public class ItemManager
    {
        #region Declarations

        private Dictionary<UInt32, ItemInfo> mItems = new Dictionary<uint, ItemInfo>();
        private Object mLock = new Object();

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets the instance of quest manager
        /// </summary>
        public static ItemManager Instance
        {
            get { return Singleton<ItemManager>.Instance; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets whether or not the manager contains the item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool ContainsItem(UInt32 itemId)
        {
            return mItems.ContainsKey(itemId);
        }

        /// <summary>
        /// Gets item info based on the id of the item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public ItemInfo GetItem(UInt32 itemId)
        {
            ItemInfo item = null;
            mItems.TryGetValue(itemId, out item);
            return item;
        }

        /// <summary>
        /// Adds an item to the manager
        /// </summary>
        /// <param name="itemInfo"></param>
        public void AddItem(ItemInfo itemInfo)
        {
            if (!mItems.ContainsKey(itemInfo.ItemId))
                lock (mLock)
                    mItems.Add(itemInfo.ItemId, itemInfo);
        }

        /// <summary>
        /// Adds or updates an item in the manager
        /// </summary>
        /// <param name="itemInfo"></param>
        public void AddOrUpdateQuest(ItemInfo itemInfo)
        {
            if (itemInfo == null) return;
            ItemInfo existing = null;
            mItems.TryGetValue(itemInfo.ItemId, out existing);
            if (existing == null)
            {
                lock (mLock)
                    mItems.Add(itemInfo.ItemId, itemInfo);
                return;
            }

            // Already exists, remove it and then re-add it
            lock (mLock)
            {
                mItems[existing.ItemId] = itemInfo;
            }
        }

        #endregion
    }
}
