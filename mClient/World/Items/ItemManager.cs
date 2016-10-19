
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mClient.World.Items
{
    public class ItemManager : AbstractObjectManager<ItemManager, ItemInfo>
    {
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
            return mObjects.Any(i => i.ItemId == obj.ItemId);
        }

        public bool Exists(UInt32 itemId)
        {
            return mObjects.Any(i => i.ItemId == itemId);
        }

        public override ItemInfo Get(uint id)
        {
            return mObjects.Where(i => i.ItemId == id).SingleOrDefault();
        }

        #endregion
    }
}
