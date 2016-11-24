using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Maps.Grid
{
    public class GridMapHeightHeader
    {
        #region Declarations

        public uint fourcc;
        public uint flags;
        public float gridHeight;
        public float gridMaxHeight;

        #endregion

        #region Public Methods

        public void Read(BinaryReader reader, uint offset)
        {
            // Seek to offset
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);

            fourcc = reader.ReadUInt32();
            flags = reader.ReadUInt32();
            gridHeight = reader.ReadSingle();
            gridMaxHeight = reader.ReadSingle();
        }

        #endregion
    }
}
