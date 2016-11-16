using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class SkillRaceClassInfoTable : DBCFile
    {
        private Dictionary<uint, SkillRaceClassInfoEntry> mSkillRaceClassInfoEntries = new Dictionary<uint, SkillRaceClassInfoEntry>();

        #region Singleton

        static readonly SkillRaceClassInfoTable instance = new SkillRaceClassInfoTable();

        static SkillRaceClassInfoTable() { }

        SkillRaceClassInfoTable() : base(@"SkillRaceClassInfo.dbc")
        { }

        public static SkillRaceClassInfoTable Instance { get { return instance; } }

        #endregion

        protected override void dataLoaded()
        {
            for (uint i = 0; i < Records; i++)
            {
                var entry = new SkillRaceClassInfoEntry();
                entry.ID = getFieldAsUint32(i, 0);
                entry.SkillID = getFieldAsUint32(i, 1);
                entry.RaceMask = getFieldAsUint32(i, 2);
                entry.ClassMask = getFieldAsUint32(i, 3);
                entry.Flags = getFieldAsUint32(i, 4);
                entry.RequiredLevel = getFieldAsUint32(i, 5);

                mSkillRaceClassInfoEntries.Add(entry.ID, entry);
            }
        }

        public SkillRaceClassInfoEntry getByID(uint ID)
        {
            return mSkillRaceClassInfoEntries[ID];
        }

        public IEnumerable<SkillRaceClassInfoEntry> getBySkillID(uint skillID)
        {
            return mSkillRaceClassInfoEntries.Values.Where(s => s.SkillID == skillID);
        }
    }
}
