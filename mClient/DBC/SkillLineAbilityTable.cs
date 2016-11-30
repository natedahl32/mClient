using mClient.Constants;
using mClient.Shared;
using mClient.World;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

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

        /// <summary>
        /// Gets all skill line ability entries for a spell id
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        public IEnumerable<SkillLineAbilityEntry> getForSpell(uint spellId)
        {
            var entries = new List<SkillLineAbilityEntry>();
            foreach (var skillLineAbility in mSkillLineAbilityEntries.Values.Where(s => s.SpellId == spellId))
                entries.Add(skillLineAbility);
            return entries;
        }

        /// <summary>
        /// Gets all spells in the line for this ability/spell
        /// </summary>
        /// <param name="spelldId"></param>
        /// <returns></returns>
        public IEnumerable<SkillLineAbilityEntry> getAllSpellsInLine(uint spellId)
        {
            var entries = new List<SkillLineAbilityEntry>();

            // Get the first spell in the line and add it
            var firstSpell = getFirstSpellInChain(spellId);
            entries.Add(firstSpell);

            // Get the parent for the first spell and keep getting the parent until we don't have any more
            var parent = getParentForSpell(firstSpell.SpellId);
            while (parent != 0)
            {
                var parents = getForSpell(parent);
                entries.AddRange(parents);
                parent = getParentForSpell(parent);
            }

            // Remove any duplicates we might have
            return entries.DistinctBy(s => s.SpellId);
        }

        /// <summary>
        /// Gets all skill line ability entries for a spell id
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        public IEnumerable<SkillLineAbilityEntry> getForParentSpell(uint spellId)
        {
            var entries = new List<SkillLineAbilityEntry>();
            foreach (var skillLineAbility in mSkillLineAbilityEntries.Values.Where(s => s.ParentSpellId == spellId))
                entries.Add(skillLineAbility);
            return entries;
        }

        /// <summary>
        /// Gets the first spell in a spell chain given a spell id
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        public SkillLineAbilityEntry getFirstSpellInChain(uint spellId)
        {
            // If the spell given is not a parent, then it is the first spell in the chain
            var skillLineAbilities = getForSpell(spellId).ToList();
            foreach (var sla in skillLineAbilities)
            {
                // check if this spell is a parent of another spell
                var parents = getForParentSpell(sla.SpellId).ToList();
                if (parents.Count > 0)
                    return getFirstSpellInChain(parents[0].SpellId);
            }

            if (skillLineAbilities.Count > 0)
                return skillLineAbilities[0];
            return null;
        }

        /// <summary>
        /// Gets all available spells for a player at a given level, taking into account their class and race
        /// </summary>
        /// <param name="level"></param>
        /// <param name="class"></param>
        /// <param name="race"></param>
        /// <returns></returns>
        public IEnumerable<SpellEntry> getAvailableSpellsForPlayer(Player player)
        {
            var spellsAvailable = new List<SpellEntry>();

            ChrClassEntry clsEntry = ChrClassesTable.Instance.getByClassId((uint)player.Class);
            if (clsEntry == null)
                return new List<SpellEntry>();
            uint family = clsEntry.SpellFamily;

            foreach (var skillLineAbility in mSkillLineAbilityEntries.Values)
            {
                // Get spell info
                var spellInfo = SpellTable.Instance.getSpell(skillLineAbility.SpellId);
                if (spellInfo == null)
                    continue;

                // Skip server-side/triggered spells
                if (spellInfo.SpellLevel == 0 || spellInfo.BaseLevel == 0)
                    continue;

                // Skip wrong class/race skills
                if (!player.IsSpellFitByClassAndRace(spellInfo.SpellId))
                    continue;

                // Skip other spell families
                if (spellInfo.SpellFamilyName != family)
                    continue;

                // skip spells with first rank learned as talent (and all talents then also)
                var firstSkill = getFirstSpellInChain(spellInfo.SpellId);
                if (firstSkill != null)
                {
                    uint first_rank = firstSkill.SpellId;
                    if (SpellTable.Instance.GetTalentSpellCost(first_rank) > 0)
                        continue;
                }

                // If the spell requires a max level higher than the player
                if (spellInfo.SpellLevel > player.Level)
                    continue;

                // Add the spell to the available list
                spellsAvailable.Add(spellInfo);
            }

            return spellsAvailable;
        }
    }
}
