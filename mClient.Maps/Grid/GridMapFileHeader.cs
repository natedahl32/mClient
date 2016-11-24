using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Maps.Grid
{
    public class GridMapFileHeader
    {
        #region Declarations

        public uint mapMagic;
        public uint versionMagic;
        public uint areaMapOffset;
        public uint areaMapSize;
        public uint heightMapOffset;
        public uint heightMapSize;
        public uint liquidMapOffset;
        public uint liquidMapSize;
        public uint holesOffset;
        public uint holesSize;

        #endregion

        #region Public Methods

        public void Read(BinaryReader reader)
        {
            mapMagic = reader.ReadUInt32();
            versionMagic = reader.ReadUInt32();
            areaMapOffset = reader.ReadUInt32();
            areaMapSize = reader.ReadUInt32();
            heightMapOffset = reader.ReadUInt32();
            heightMapSize = reader.ReadUInt32();
            liquidMapOffset = reader.ReadUInt32();
            liquidMapSize = reader.ReadUInt32();
            holesOffset = reader.ReadUInt32();
            holesSize = reader.ReadUInt32();
        }

        #endregion
    }
}
