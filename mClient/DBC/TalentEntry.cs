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
    }
}
