using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class SkillLineTable : DBCFile
    {
        private Dictionary<uint, SkillLineEntry> mSkillLineEntries = new Dictionary<uint, SkillLineEntry>();

        #region Singleton

        static readonly SkillLineTable instance = new SkillLineTable();

        static SkillLineTable() { }

        SkillLineTable() : base(@"SkillLine.dbc")
        { }

        public static SkillLineTable Instance { get { return instance; } }

        #endregion

        protected override void dataLoaded()
        {
            for (uint i = 0; i < Records; i++)
            {
                var entry = new SkillLineEntry();
                entry.Id = getFieldAsUint32(i, 0);
                entry.CategoryId = getFieldAsInt32(i, 1);
                entry.Name = getStringForField(i, 3);
                entry.SpellIcon = getFieldAsUint32(i, 21);

                mSkillLineEntries.Add(entry.Id, entry);
            }
        }

        public SkillLineEntry getById(uint id)
        {
            if (mSkillLineEntries.ContainsKey(id))
                return mSkillLineEntries[id];
            return null;
        }
    }
}
