using mClient.Constants;
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class TalentTable : DBCFile
    {
        private Dictionary<uint, TalentEntry> mTalentEntries = new Dictionary<uint, TalentEntry>();

        #region Singleton

        static readonly TalentTable instance = new TalentTable();

        static TalentTable() { }

        TalentTable() : base(@"Talent.dbc")
        { }

        public static TalentTable Instance { get { return instance; } }

        #endregion

        protected override void dataLoaded()
        {
            for (uint i = 0; i < Records; i++)
            {
                var entry = new TalentEntry();
                entry.TalentId = getFieldAsUint32(i, 0);
                entry.TalentTabId = getFieldAsUint32(i, 1);
                entry.Row = getFieldAsUint32(i, 2);
                entry.Column = getFieldAsUint32(i, 3);
                entry.RankID = new uint[SpellConstants.MAX_TALENT_RANK];
                entry.RankID[0] = getFieldAsUint32(i, 4);
                entry.RankID[1] = getFieldAsUint32(i, 5);
                entry.RankID[2] = getFieldAsUint32(i, 6);
                entry.RankID[3] = getFieldAsUint32(i, 7);
                entry.RankID[4] = getFieldAsUint32(i, 8);
                entry.DependsOnTalent = getFieldAsUint32(i, 13);
                entry.DependsOnTalentRank = getFieldAsUint32(i, 16);
                entry.DependsOnSpell = getFieldAsUint32(i, 20);

                mTalentEntries.Add(entry.TalentId, entry);
            }
        }

        public TalentEntry getBySpell(uint spellId)
        {
            foreach (var talent in mTalentEntries.Values)
            {
                if (talent.RankID[0] == spellId ||
                    talent.RankID[1] == spellId ||
                    talent.RankID[2] == spellId ||
                    talent.RankID[3] == spellId ||
                    talent.RankID[4] == spellId)
                    return talent;
            }

            return null;
        }

        public TalentSpellPos getTalentSpellPosBySpell(uint spellId)
        {
            foreach (var talent in mTalentEntries.Values)
            {
                if (talent.RankID[0] == spellId)
                    return new TalentSpellPos() { TalentId = (ushort)talent.TalentId, Rank = 1 };
                if (talent.RankID[1] == spellId)
                    return new TalentSpellPos() { TalentId = (ushort)talent.TalentId, Rank = 2 };
                if (talent.RankID[2] == spellId)
                    return new TalentSpellPos() { TalentId = (ushort)talent.TalentId, Rank = 3 };
                if (talent.RankID[3] == spellId)
                    return new TalentSpellPos() { TalentId = (ushort)talent.TalentId, Rank = 4 };
                if (talent.RankID[4] == spellId)
                    return new TalentSpellPos() { TalentId = (ushort)talent.TalentId, Rank = 5 };
            }

            return null;
        }

        #region Classes

        public class TalentSpellPos
        {
            public ushort TalentId { get; set; }
            public byte Rank { get; set; }
        }

        #endregion
    }
}
