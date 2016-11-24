using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Maps.Grid
{
    public class GridMapAreaHeader
    {
        #region Declarations

        public uint fourcc;
        public ushort flags;
        public ushort gridArea;

        #endregion

        #region Public Methods

        public void Read(BinaryReader reader, uint offset)
        {
            // seek to area offset
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);

            fourcc = reader.ReadUInt32();
            flags = reader.ReadUInt16();
            gridArea = reader.ReadUInt16();
        }

        #endregion
    }
}
