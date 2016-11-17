using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class SpellRangeEntry
    {
        #region Properties

        public uint ID { get; set; }

        public float MinimumRange { get; set; }

        public float MaximumRange { get; set; }

        #endregion
    }
}
