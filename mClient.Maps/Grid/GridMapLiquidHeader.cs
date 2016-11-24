using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Maps.Grid
{
    public class GridMapLiquidHeader
    {
        #region Declarations

        public uint fourcc;
        public ushort flags;
        public ushort liquidType;
        public byte offsetX;
        public byte offsetY;
        public byte width;
        public byte height;
        public float liquidLevel;

        #endregion

        #region Public Methods

        public void Read(BinaryReader reader, uint offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);

            fourcc = reader.ReadUInt32();
            flags = reader.ReadUInt16();
            liquidType = reader.ReadUInt16();
            offsetX = reader.ReadByte();
            offsetY = reader.ReadByte();
            width = reader.ReadByte();
            height = reader.ReadByte();
            liquidLevel = reader.ReadSingle();
        }

        #endregion
    }
}
