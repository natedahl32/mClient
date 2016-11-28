
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mClient.World.Items
{
    public class ItemManager : AbstractObjectManager<ItemInfo>
    {
        #region Singleton

        static readonly ItemManager instance = new ItemManager();
        
        static ItemManager() { }

        ItemManager() { }

        public static ItemManager Instance { get { return instance; } }

        #endregion

        #region Properties

        protected override string SerializeToFile
        {
            get
            {
                return "items.json";
            }
        }

        #endregion

        #region Public Methods

        public override bool Exists(ItemInfo obj)
        {
            if (obj == null) return false;
            lock(mLock)
                return mObjects.Any(i => i.ItemId == obj.ItemId);
        }

        public bool Exists(UInt32 itemId)
        {
            lock (mLock)
                return mObjects.Any(i => i.ItemId == itemId);
        }

        public override ItemInfo Get(uint id)
        {
            lock (mLock)
                return mObjects.Where(i => i != null && i.ItemId == id).SingleOrDefault();
        }

        #endregion
    }
}
