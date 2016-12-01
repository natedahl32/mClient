using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class SkillLineAbilityEntry
    {
        #region Properties

        public uint Id { get; set; }
        public uint SkillLineId { get; set; }
        public uint SpellId { get; set; }
        public uint CharacterRacesFlag { get; set; }
        public uint CharacterClassesFlag { get; set; }
        public uint RequiredSkillValue { get; set; }
        public uint ParentSpellId { get; set; }
        public uint AcquireMethod { get; set; }
        public uint SkillGreyLevel { get; set; }
        public uint SkillYellowLevel { get; set; }
        public uint RequiredTrainPoints { get; set; }

        public uint SkillGreenLevel
        {
            get { return (SkillGreyLevel + SkillYellowLevel) / 2; }
        }

        public SpellEntry Spell
        {
            get { return SpellTable.Instance.getSpell(SpellId); }
        }

        public SpellEntry ParentSpell
        {
            get { return SpellTable.Instance.getSpell(ParentSpellId); }
        }

        public SkillType Skill
        {
            get { return (SkillType)SkillLineId; }
        }

        #endregion
    }
}
