using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class ItemRandomPropertiesTable : DBCFile
    {
        private Dictionary<uint, ItemRandomPropertiesEntry> mItemRandomPropertyEntries = new Dictionary<uint, ItemRandomPropertiesEntry>();

        #region Singleton

        static readonly ItemRandomPropertiesTable instance = new ItemRandomPropertiesTable();

        static ItemRandomPropertiesTable() { }

        ItemRandomPropertiesTable() : base(@"ItemRandomProperties.dbc")
        { }

        public static ItemRandomPropertiesTable Instance { get { return instance; } }

        #endregion

        protected override void dataLoaded()
        {
            for (uint i = 0; i < Records; i++)
            {
                var entry = new ItemRandomPropertiesEntry();
                entry.ID = getFieldAsUint32(i, 0);
                entry.EnchantId = new uint[3];
                entry.EnchantId[0] = getFieldAsUint32(i, 2);
                entry.EnchantId[1] = getFieldAsUint32(i, 3);
                entry.EnchantId[3] = getFieldAsUint32(i, 4);

                mItemRandomPropertyEntries.Add(entry.ID, entry);
            }
        }

        public ItemRandomPropertiesEntry getById(uint id)
        {
            if (mItemRandomPropertyEntries.ContainsKey(id))
                return mItemRandomPropertyEntries[id];
            return null;
        }
    }
}
