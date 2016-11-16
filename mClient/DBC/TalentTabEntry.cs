using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class TalentTabEntry
    {
        #region Properties

        public uint TalentTabId { get; set; }
        public string TalentTabName { get; set; }
        public uint NameFlags { get; set; }
        public uint SpellIcon { get; set; }
        public uint RaceMask { get; set; }
        public uint ClassMask { get; set; }
        public uint TabPage { get; set; }

        #endregion
    }
}
