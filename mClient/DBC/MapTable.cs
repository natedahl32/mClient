using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Shared
{
    public class MapTable : DBCFile
    {
        #region Singleton

        static readonly MapTable instance = new MapTable();

        static MapTable() { }

        MapTable() : base(@"Map.dbc")
        { }

        public static MapTable Instance { get { return instance; } }

        #endregion

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
