using mClient.Constants;
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Skill
{
    public class SkillData
    {
        #region Constructors

        public SkillData(SkillType skill, uint value, uint bonus)
        {
            Skill = skill;
            SkillValue = value.LoPart();
            SkillMaxValue = value.HiPart();
            TempBonus = bonus.LoPart();
            PermanentBonus = bonus.HiPart();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the skill this data is for
        /// </summary>
        public SkillType Skill { get; private set; }

        /// <summary>
        /// Gets the skill value not including bonuses
        /// </summary>
        public ushort SkillValue { get; private set; }

        /// <summary>
        /// Gets the maximum skill value
        /// </summary>
        public ushort SkillMaxValue { get; private set; }

        /// <summary>
        /// Gets the temporary bonus for this skill
        /// </summary>
        public ushort TempBonus { get; private set; }

        /// <summary>
        /// Gets the permanent bonus for this skill
        /// </summary>
        public ushort PermanentBonus { get; private set; }

        /// <summary>
        /// Gets the total value of the skill including bonuses
        /// </summary>
        public uint TotalValue
        {
            get
            {
                uint value = (uint)SkillValue + TempBonus + PermanentBonus;
                return value < 0 ? 0 : value;
            }
        }

        #endregion
    }
}
