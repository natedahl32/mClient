using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class TalentEntry
    {
        #region Properties

        public uint TalentId { get; set; }
        public uint TalentTabId { get; set; }
        public uint Row { get; set; }
        public uint Column { get; set; }
        public uint[] RankID { get; set; }
        public uint DependsOnTalent { get; set; }
        public uint DependsOnTalentRank { get; set; }
        public uint DependsOnSpell { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the rank of this spell in the talent group
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        public int GetRankForSpell(uint spellId)
        {
            for (int i = 0; i < SpellConstants.MAX_TALENT_RANK; i++)
                if (RankID[i] == spellId)
                    return i;
            return -1;
        }

        #endregion
    }
}
