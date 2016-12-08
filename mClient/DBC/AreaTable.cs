using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class AreaTable : DBCFile
    {
        private Dictionary<uint, AreaEntry> mAreaEntries = new Dictionary<uint, AreaEntry>();

        #region Singleton

        static readonly AreaTable instance = new AreaTable();

        static AreaTable() { }

        AreaTable() : base(@"AreaTable.dbc")
        { }

        public static AreaTable Instance { get { return instance; } }

        #endregion

        protected override void dataLoaded()
        {
            for (uint i = 0; i < Records; i++)
            {
                var entry = new AreaEntry();
                entry.ID = getFieldAsUint32(i, 0);
                entry.MapID = getFieldAsUint32(i, 1);
                entry.ParentAreaID = getFieldAsUint32(i, 2);
                entry.ExploreFlag = getFieldAsUint32(i, 3);
                entry.Flags = getFieldAsUint32(i, 4);
                entry.AreaLevel = getFieldAsInt32(i, 10);
                entry.AreaName = getStringForField(i, 11);
                entry.TeamFaction = getFieldAsUint32(i, 20);
                entry.LiquidTypeOverride = getFieldAsUint32(i, 24);

                mAreaEntries.Add(entry.ID, entry);
            }
        }

        public AreaEntry getByZoneId(uint zoneId)
        {
            if (mAreaEntries.ContainsKey(zoneId))
                return mAreaEntries[zoneId];
            return null;
        }
    }
}
