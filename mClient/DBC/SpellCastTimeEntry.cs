using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class SpellCastTimeEntry
    {
        #region Properties

        public uint ID { get; set; }

        public int CastTime { get; set; }

        public int CastTimePerLevel { get; set; }

        public int MinimumCastTime { get; set; }

        #endregion
    }
}
