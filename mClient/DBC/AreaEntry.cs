using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class AreaEntry
    {
        public uint ID { get; set; }
        public uint MapID { get; set; }
        public uint ParentAreaID { get; set; }
        public uint ExploreFlag { get; set; }
        public uint Flags { get; set; }
        public int AreaLevel { get; set; }
        public string AreaName { get; set; }
        public uint TeamFaction { get; set; }
        public uint LiquidTypeOverride { get; set; }
    }
}
