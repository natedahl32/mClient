using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class TalentTabTable : DBCFile
    {
        private Dictionary<uint, TalentTabEntry> mTalentTabEntries = new Dictionary<uint, TalentTabEntry>();

        #region Singleton

        static readonly TalentTabTable instance = new TalentTabTable();

        static TalentTabTable() { }

        TalentTabTable() : base(@"TalentTab.dbc")
        { }

        public static TalentTabTable Instance { get { return instance; } }

        #endregion

        protected override void dataLoaded()
        {
            for (uint i = 0; i < Records; i++)
            {
                var entry = new TalentTabEntry();
                entry.TalentTabId = getFieldAsUint32(i, 0);
                entry.TalentTabName = getStringForField(i, 1);
                entry.NameFlags = getFieldAsUint32(i, 9);
                entry.SpellIcon = getFieldAsUint32(i, 10);
                entry.RaceMask = getFieldAsUint32(1, 11);
                entry.ClassMask = getFieldAsUint32(i, 12);
                entry.TabPage = getFieldAsUint32(i, 13);

                mTalentTabEntries.Add(entry.TalentTabId, entry);
            }
        }
    }
}
