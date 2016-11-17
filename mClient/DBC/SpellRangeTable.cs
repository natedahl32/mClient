using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class SpellRangeTable : DBCFile
    {
        private Dictionary<uint, SpellRangeEntry> mSpellRangeEntries = new Dictionary<uint, SpellRangeEntry>();

        #region Singleton

        static readonly SpellRangeTable instance = new SpellRangeTable();

        static SpellRangeTable() { }

        SpellRangeTable() : base(@"SpellRange.dbc")
        { }

        public static SpellRangeTable Instance { get { return instance; } }

        #endregion

        protected override void dataLoaded()
        {
            for (uint i = 0; i < Records; i++)
            {
                var entry = new SpellRangeEntry();
                entry.ID = getFieldAsUint32(i, 0);
                entry.MinimumRange = getFieldAsFloat(i, 1);
                entry.MaximumRange = getFieldAsFloat(i, 2);

                mSpellRangeEntries.Add(entry.ID, entry);
            }
        }

        public SpellRangeEntry getByID(uint Id)
        {
            if (mSpellRangeEntries.ContainsKey(Id))
                return mSpellRangeEntries[Id];
            return null;
        }
    }
}
