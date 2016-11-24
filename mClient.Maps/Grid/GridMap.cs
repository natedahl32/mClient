using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Maps.Grid
{
    public class GridMap
    {
        #region Declarations

        public const string MAP_MAGIC = "MAPS";
        public const string MAP_VERSION_MAGIC = "z1.3";
        public const string MAP_AREA_MAGIC = "AREA";
        public const string MAP_HEIGHT_MAGIC = "MHGT";
        public const string MAP_LIQUID_MAGIC = "MLIQ";

        public const uint MAP_AREA_NO_AREA = 0x0001;

        public const uint MAP_HEIGHT_NO_HEIGHT  = 0x0001;
        public const uint MAP_HEIGHT_AS_INT16   = 0x0002;
        public const uint MAP_HEIGHT_AS_INT8    = 0x0004;

        public const uint MAP_LIQUID_NO_TYPE    = 0x0001;
        public const uint MAP_LIQUID_NO_HEIGHT  = 0x0002;

        public const float MAX_HEIGHT = 100000.0f;                     // can be use for find ground height at surface
        public const float INVALID_HEIGHT = -100000.0f;                     // for check, must be equal to VMAP_INVALID_HEIGHT, real value for unknown height is VMAP_INVALID_HEIGHT_VALUE
        public const float INVALID_HEIGHT_VALUE = -200000.0f;                     // for return, must be equal to VMAP_INVALID_HEIGHT_VALUE, check value for unknown height is VMAP_INVALID_HEIGHT
        public const float MAX_FALL_DISTANCE = 250000.0f;                     // "unlimited fall" to find VMap ground if it is available, just larger than MAX_HEIGHT - INVALID_HEIGHT
        public const float DEFAULT_HEIGHT_SEARCH = 10.0f;                     // default search distance to find height at nearby locations
        public const float DEFAULT_WATER_SEARCH = 50.0f;                     // default search distance to case detection water level

        static ushort[] holetab_h = new ushort[4] { 0x1111, 0x2222, 0x4444, 0x8888 };
        static ushort[] holetab_v = new ushort[4] { 0x000F, 0x00F0, 0x0F00, 0xF000 };

        private ushort[,] m_holes = new ushort[16, 16];
        private uint m_flags;

        // Area data
        ushort m_gridArea;
        ushort[] m_area_map;

        // Height level data
        float m_gridHeight;
        float m_gridIntHightMultiplier;

        // Liquid data
        ushort m_liquidType;
        byte m_liquid_offX;
        byte m_liquid_offY;
        byte m_liquid_width;
        byte m_liquid_height;
        float m_liquidLevel;
        ushort[] m_liquidEntry;
        byte[] m_liquidFlags;
        float[] m_liquidMap;

        ushort[] m_uint16_V9;
        ushort[] m_uint16_V8;

        byte[] m_uint8_V9;
        byte[] m_uint8_V8;

        float[] m_V9;
        float[] m_V8;

        // Delegate for get height method
        private delegate float getHeightDelegate(float x, float y);

        // Function pointer reference for get height method
        private getHeightDelegate m_gridGetHeight;

        #endregion

        #region Public Methods

        /// <summary>
        /// Load grid map data
        /// </summary>
        /// <param name="filename"></param>
        public void LoadData(string filename)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                var gridMapHeader = new GridMapFileHeader();
                gridMapHeader.Read(reader);

                // load the area data
                if (gridMapHeader.areaMapOffset > 0)
                {
                    GridMapAreaHeader areaHeader = new GridMapAreaHeader();
                    areaHeader.Read(reader, gridMapHeader.areaMapOffset);

                    if (areaHeader.flags != (areaHeader.flags & MAP_AREA_NO_AREA))
                    {
                        m_area_map = new ushort[16 * 16];
                        for (int i = 0; i < (16 * 16); i++)
                            m_area_map[i] = reader.ReadUInt16();
                    }
                }

                // load the holes data
                if (gridMapHeader.holesOffset > 0)
                {
                    // Seek to holes offset
                    reader.BaseStream.Seek(gridMapHeader.holesOffset, SeekOrigin.Begin);

                    // read holes data
                    for (int i = 0; i < 16; i++)
                        for (int j = 0; j < 16; j++)
                            m_holes[i, j] = reader.ReadUInt16();
                }

                // load the height data
                if (gridMapHeader.heightMapOffset > 0)
                {
                    GridMapHeightHeader heightHeader = new GridMapHeightHeader();
                    heightHeader.Read(reader, gridMapHeader.heightMapOffset);

                    m_gridHeight = heightHeader.gridHeight;
                    if (heightHeader.flags != (heightHeader.flags & MAP_HEIGHT_NO_HEIGHT))
                    {
                        if (heightHeader.flags == (heightHeader.flags & MAP_HEIGHT_AS_INT16))
                        {
                            m_uint16_V9 = new ushort[129 * 129];
                            m_uint16_V8 = new ushort[128 * 128];
                            for (int i = 0; i < (129 * 129); i++)
                                m_uint16_V9[i] = reader.ReadUInt16();
                            for (int i = 0; i < (128 * 128); i++)
                                m_uint16_V8[i] = reader.ReadUInt16();
                            m_gridIntHightMultiplier = (heightHeader.gridMaxHeight - heightHeader.gridHeight) / 65535;
                            m_gridGetHeight = getHeightFromUint16;
                        }
                        else if (heightHeader.flags == (heightHeader.flags & MAP_HEIGHT_AS_INT8))
                        {
                            m_uint8_V9 = new byte[129 * 129];
                            m_uint8_V8 = new byte[128 * 128];
                            for (int i = 0; i < (129 * 129); i++)
                                m_uint8_V9[i] = reader.ReadByte();
                            for (int i = 0; i < (128 * 128); i++)
                                m_uint8_V8[i] = reader.ReadByte();
                            m_gridIntHightMultiplier = (heightHeader.gridMaxHeight - heightHeader.gridHeight) / 255;
                            m_gridGetHeight = getHeightFromUint8;
                        }
                        else
                        {
                            m_V9 = new float[129 * 129];
                            m_V8 = new float[128 * 128];
                            for (int i = 0; i < (129 * 129); i++)
                                m_V9[i] = reader.ReadSingle();
                            for (int i = 0; i < (128 * 128); i++)
                                m_V8[i] = reader.ReadSingle();
                            m_gridGetHeight = getHeightFromFloat;
                        }
                    }
                    else
                        m_gridGetHeight = getHeightFromFlat;
                }

                // load the liquid data
                if (gridMapHeader.liquidMapOffset > 0)
                {
                    GridMapLiquidHeader liquidHeader = new GridMapLiquidHeader();
                    liquidHeader.Read(reader, gridMapHeader.liquidMapOffset);

                    m_liquidType = liquidHeader.liquidType;
                    m_liquid_offX = liquidHeader.offsetX;
                    m_liquid_offY = liquidHeader.offsetY;
                    m_liquid_width = liquidHeader.width;
                    m_liquid_height = liquidHeader.height;
                    m_liquidLevel = liquidHeader.liquidLevel;

                    if (liquidHeader.flags != (liquidHeader.flags & MAP_LIQUID_NO_TYPE))
                    {
                        m_liquidEntry = new ushort[16 * 16];
                        for (int i = 0; i < 16 * 16; i++)
                            m_liquidEntry[i] = reader.ReadUInt16();

                        m_liquidFlags = new byte[16 * 16];
                        for (int i = 0; i < 16 * 16; i++)
                            m_liquidFlags[i] = reader.ReadByte();
                    }

                    if (liquidHeader.flags != (liquidHeader.flags & MAP_LIQUID_NO_HEIGHT))
                    {
                        m_liquidMap = new float[m_liquid_width * m_liquid_height];
                        for (int i = 0; i < m_liquid_width * m_liquid_height; i++)
                            m_liquidMap[i] = reader.ReadSingle();
                    }
                }
            }
        }

        private float getHeightFromFloat(float x, float y)
        {
            if (m_V8 == null || m_V9 == null)
                return INVALID_HEIGHT_VALUE;

            x = GridDefines.MAP_RESOLUTION * (32 - x / GridDefines.SIZE_OF_GRIDS);
            y = GridDefines.MAP_RESOLUTION * (32 - y / GridDefines.SIZE_OF_GRIDS);

            int x_int = (int)x;
            int y_int = (int)y;
            x -= x_int;
            y -= y_int;
            x_int &= (GridDefines.MAP_RESOLUTION - 1);
            y_int &= (GridDefines.MAP_RESOLUTION - 1);

            if (isHole(x_int, y_int))
                return INVALID_HEIGHT_VALUE;

            // Height stored as: h5 - its v8 grid, h1-h4 - its v9 grid
            // +--------------> X
            // | h1-------h2     Coordinates is:
            // | | \  1  / |     h1 0,0
            // | |  \   /  |     h2 0,1
            // | | 2  h5 3 |     h3 1,0
            // | |  /   \  |     h4 1,1
            // | | /  4  \ |     h5 1/2,1/2
            // | h3-------h4
            // V Y
            // For find height need
            // 1 - detect triangle
            // 2 - solve linear equation from triangle points
            // Calculate coefficients for solve h = a*x + b*y + c

            float a, b, c;
            if (x + y < 1)
            {
                if (x > y)
                {
                    float h1 = m_V9[(x_int) * 129 + y_int];
                    float h2 = m_V9[(x_int + 1) * 129 + y_int];
                    float h5 = 2 * m_V8[x_int * 128 + y_int];
                    a = h2 - h1;
                    b = h5 - h1 - h2;
                    c = h1;
                }
                else
                {
                    float h1 = m_V9[x_int * 129 + y_int];
                    float h3 = m_V9[x_int * 129 + y_int + 1];
                    float h5 = 2 * m_V8[x_int * 128 + y_int];
                    a = h5 - h1 - h3;
                    b = h3 - h1;
                    c = h1;
                }
            }
            else
            {
                if (x > y)
                {
                    float h2 = m_V9[(x_int + 1) * 129 + y_int];
                    float h4 = m_V9[(x_int + 1) * 129 + y_int + 1];
                    float h5 = 2 * m_V8[x_int * 129 + y_int];
                    a = h2 + h4 - h5;
                    b = h4 - h2;
                    c = h5 - h4;
                }
                else
                {
                    float h3 = m_V9[(x_int) * 129 + y_int + 1];
                    float h4 = m_V9[(x_int + 1) * 129 + y_int + 1];
                    float h5 = 2 * m_V8[x_int * 128 + y_int];
                    a = h4 - h3;
                    b = h3 + h4 - h5;
                    c = h5 - h4;
                }
            }

            // Calculate height
            return a * x + b * y + c;
        }

        private float getHeightFromUint16(float x, float y)
        {
            if (m_uint16_V8 == null || m_uint16_V9 == null)
                return m_gridHeight;

            x = GridDefines.MAP_RESOLUTION * (32 - x / GridDefines.SIZE_OF_GRIDS);
            y = GridDefines.MAP_RESOLUTION * (32 - y / GridDefines.SIZE_OF_GRIDS);

            int x_int = (int)x;
            int y_int = (int)y;
            x -= x_int;
            y -= y_int;
            x_int &= (GridDefines.MAP_RESOLUTION - 1);
            y_int &= (GridDefines.MAP_RESOLUTION - 1);

            int a, b, c;
            // index of start array
            int V9_h1_ptr = (x_int * 128 + x_int + y_int);
            if (x + y < 1)
            {
                if (x > y)
                {
                    // 1 triangle (h1, h2, h5 points)
                    int h1 = m_uint16_V9[V9_h1_ptr];
                    int h2 = m_uint16_V9[V9_h1_ptr + 129];
                    int h5 = 2 * m_uint16_V8[x_int * 128 + y_int];
                    a = h2 - h1;
                    b = h5 - h1 - h2;
                    c = h1;
                }
                else
                {
                    // 2 trinagle (h1, h3, h5 points)
                    int h1 = m_uint16_V9[V9_h1_ptr];
                    int h3 = m_uint16_V9[V9_h1_ptr + 1];
                    int h5 = 2 * m_uint16_V8[x_int * 128 + y_int];
                    a = h5 - h1 - h3;
                    b = h3 - h1;
                    c = h1;
                }
            }
            else
            {
                if (x > y)
                {
                    // 3 triangle (h2, h4, h5 points)
                    int h2 = m_uint16_V9[V9_h1_ptr + 129];
                    int h4 = m_uint16_V9[V9_h1_ptr + 130];
                    int h5 = 2 * m_uint16_V8[x_int * 128 + y_int];
                    a = h2 + h4 - h5;
                    b = h4 - h2;
                    c = h5 - h4;
                }
                else
                {
                    // 4 triangle (h3, h4, h5 points)
                    int h3 = m_uint16_V9[V9_h1_ptr + 1];
                    int h4 = m_uint16_V9[V9_h1_ptr + 130];
                    int h5 = 2 * m_uint16_V8[x_int * 128 + y_int];
                    a = h4 - h3;
                    b = h3 + h4 - h5;
                    c = h5 - h4;
                }
            }

            // Calculate height
            return (float)((a * x) + (b * y) + c) * m_gridIntHightMultiplier + m_gridHeight;
        }

        private float getHeightFromUint8(float x, float y)
        {
            if (m_uint16_V8 == null || m_uint16_V9 == null)
                return m_gridHeight;

            x = GridDefines.MAP_RESOLUTION * (32 - x / GridDefines.SIZE_OF_GRIDS);
            y = GridDefines.MAP_RESOLUTION * (32 - y / GridDefines.SIZE_OF_GRIDS);

            int x_int = (int)x;
            int y_int = (int)y;
            x -= x_int;
            y -= y_int;
            x_int &= (GridDefines.MAP_RESOLUTION - 1);
            y_int &= (GridDefines.MAP_RESOLUTION - 1);

            int a, b, c;
            int V9_h1_ptr = (x_int * 128 + x_int + y_int);
            if (x + y < 1)
            {
                if (x > y)
                {
                    // 1 triangle (h1, h2, h5 points)
                    int h1 = m_uint8_V9[V9_h1_ptr];
                    int h2 = m_uint8_V9[V9_h1_ptr + 129];
                    int h5 = 2 * m_uint8_V8[x_int * 128 + y_int];
                    a = h2 - h1;
                    b = h5 - h1 - h2;
                    c = h1;
                }
                else
                {
                    // 2 triangle (h1, h2, h5 points)
                    int h1 = m_uint8_V9[V9_h1_ptr];
                    int h3 = m_uint8_V9[V9_h1_ptr + 1];
                    int h5 = 2 * m_uint8_V8[x_int * 128 + y_int];
                    a = h5 - h1 - h3;
                    b = h3 - h1;
                    c = h1;
                }
            }
            else
            {
                if (x > y)
                {
                    // 3 triangle (h2, h4, h5 points)
                    int h2 = m_uint8_V9[V9_h1_ptr + 129];
                    int h4 = m_uint8_V9[V9_h1_ptr + 130];
                    int h5 = 2 * m_uint8_V8[x_int * 128 + y_int];
                    a = h2 + h4 - h5;
                    b = h4 - h2;
                    c = h5 - h4;
                }
                else
                {
                    // 4 triangle (h3, h4, h5 points)
                    int h3 = m_uint8_V9[V9_h1_ptr + 1];
                    int h4 = m_uint8_V9[V9_h1_ptr + 130];
                    int h5 = 2 * m_uint8_V8[x_int * 128 + y_int];
                    a = h4 - h3;
                    b = h3 + h4 - h5;
                    c = h5 - h4;
                }
            }

            // Calculate height
            return (float)((a * x) + (b * y) + c) * m_gridIntHightMultiplier + m_gridHeight;
        }

        private float getHeightFromFlat(float x, float y)
        {
            return m_gridHeight;
        }

        private bool isHole(int row, int col)
        {
            int cellRow = row / 8;
            int cellCol = col / 8;
            int holeRow = row % 8 / 2;
            int holeCol = (col - (cellCol * 8)) / 2;

            ushort hole = m_holes[cellRow, cellCol];

            return (hole & holetab_h[holeCol] & holetab_v[holeRow]) != 0;
        }

        #endregion
    }
}
