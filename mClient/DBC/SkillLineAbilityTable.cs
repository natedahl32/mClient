using mClient.Shared;
using System.Collections.Generic;
using System.Linq;

namespace mClient.DBC
{
    public class SkillLineAbilityTable : DBCFile
    {
        private Dictionary<uint, SkillLineAbilityEntry> mSkillLineAbilityEntries = new Dictionary<uint, SkillLineAbilityEntry>();

        #region Singleton

        static readonly SkillLineAbilityTable instance = new SkillLineAbilityTable();

        static SkillLineAbilityTable() { }

        SkillLineAbilityTable() : base(@"SkillLineAbility.dbc")
        { }

        public static SkillLineAbilityTable Instance { get { return instance; } }

        #endregion

        protected override void dataLoaded()
        {
            for (uint i = 0; i < Records; i++)
            {
                var entry = new SkillLineAbilityEntry();
                entry.Id = getFieldAsUint32(i, 0);
                entry.SkillLineId = getFieldAsUint32(i, 1);
                entry.SpellId = getFieldAsUint32(i, 2);
                entry.CharacterRacesFlag = getFieldAsUint32(i, 3);
                entry.CharacterClassesFlag = getFieldAsUint32(i, 4);
                entry.RequiredSkillValue = getFieldAsUint32(i, 7);
                entry.ParentSpellId = getFieldAsUint32(i, 8);
                entry.AcquireMethod = getFieldAsUint32(i, 9);
                entry.SkillGreyLevel = getFieldAsUint32(i, 10);
                entry.SkillYellowLevel = getFieldAsUint32(i, 11);
                entry.RequiredTrainPoints = getFieldAsUint32(i, 14);

                mSkillLineAbilityEntries.Add(entry.Id, entry);
            }
        }

        public uint getParentForSpell(uint spellId)
        {
            foreach (var skillLineAbility in mSkillLineAbilityEntries.Values.Where(s => s.SpellId == spellId))
                if (skillLineAbility.ParentSpellId > 0)
                    return skillLineAbility.ParentSpellId;
            return 0;
        }
    }
}
