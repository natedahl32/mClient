using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Maps
{
    [Flags]
    public enum NavTerrain
    {
        NAV_EMPTY = 0x00,
        NAV_GROUND = 0x01,
        NAV_MAGMA = 0x02,
        NAV_SLIME = 0x04,
        NAV_WATER = 0x08,
        NAV_UNUSED1 = 0x10,
        NAV_UNUSED2 = 0x20,
        NAV_UNUSED3 = 0x40,
        NAV_UNUSED4 = 0x80
        // we only have 8 bits
    }
}
