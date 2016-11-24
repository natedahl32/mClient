using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Maps
{
    public class MMapTileHeader
    {
        #region Declarations

        public const uint MMAP_MAGIC = 0x4d4d4150;
        public const uint MMAP_VERSION = 4;
        public const int DT_NAVMESH_VERSION = 7;

        public uint mmapMagic;
        public uint dtVersion;
        public uint mmapVersion;
        public uint size;
        public bool useLiquids = true;

        #endregion

        #region Constructors

        public MMapTileHeader()
        {
            mmapMagic = MMAP_MAGIC;
            dtVersion = DT_NAVMESH_VERSION;
            mmapVersion = MMAP_VERSION;
            size = 0;
        }

        #endregion
    }
}
