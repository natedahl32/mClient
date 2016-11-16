using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class SkillRaceClassInfoEntry
    {
        #region Properties

        public uint ID { get; set; }
        public uint SkillID { get; set; }
        public uint RaceMask { get; set; }
        public uint ClassMask { get; set; }
        public uint Flags { get; set; }
        public uint RequiredLevel { get; set; }

        #endregion
    }
}
