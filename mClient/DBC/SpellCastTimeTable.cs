using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class SpellCastTimeTable : DBCFile
    {
        private Dictionary<uint, SpellCastTimeEntry> mSpellCastTimeEntries = new Dictionary<uint, SpellCastTimeEntry>();

        #region Singleton

        static readonly SpellCastTimeTable instance = new SpellCastTimeTable();

        static SpellCastTimeTable() { }

        SpellCastTimeTable() : base(@"SpellCastTimes.dbc")
        { }

        public static SpellCastTimeTable Instance { get { return instance; } }

        #endregion

        protected override void dataLoaded()
        {
            for (uint i = 0; i < Records; i++)
            {
                var entry = new SpellCastTimeEntry();
                entry.ID = getFieldAsUint32(i, 0);
                entry.CastTime = getFieldAsInt32(i, 1);
                entry.CastTimePerLevel = getFieldAsInt32(i, 2);
                entry.MinimumCastTime = getFieldAsInt32(i, 3);

                mSpellCastTimeEntries.Add(entry.ID, entry);
            }
        }

        public SpellCastTimeEntry getById(uint Id)
        {
            if (mSpellCastTimeEntries.ContainsKey(Id))
                return mSpellCastTimeEntries[Id];
            return null;
        }
    }
}
