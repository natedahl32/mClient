﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Shared
{
    class MapTable : DBCFile
    {
        public MapTable()
            : base(@"Map.dbc")
        {
        }

        public string getMapName(uint mapId)
        {
            for (uint x = 0; x < wdbc_header.nRecords; x++)
            {
                uint id = getFieldAsUint32(x, 0);

                if (id == mapId)
                    return getStringForField(x, 1);
            }
            return null;
        }
    }
}
