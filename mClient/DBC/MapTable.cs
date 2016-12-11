using mClient.Constants;
using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Shared
{
    public class MapTable : DBCFile
    {
        private Dictionary<uint, MapEntry> mMapEntries = new Dictionary<uint, MapEntry>();

        #region Singleton

        static readonly MapTable instance = new MapTable();

        static MapTable() { }

        MapTable() : base(@"Map.dbc")
        { }

        public static MapTable Instance { get { return instance; } }

        #endregion

        protected override void dataLoaded()
        {
            for (uint i = 0; i < Records; i++)
            {
                var entry = new MapEntry();
                entry.Id = getFieldAsUint32(i, 0);
                entry.MapType = (MapTypes)getFieldAsUint32(i, 2);
                entry.Name = getStringForField(i, 4);
                entry.LinkedZone = getFieldAsUint32(i, 19);

                mMapEntries.Add(entry.Id, entry);
            }
        }

        public MapEntry getById(uint id)
        {
            if (mMapEntries.ContainsKey(id))
                return mMapEntries[id];
            return null;
        }
    }
}
