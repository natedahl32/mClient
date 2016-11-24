using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mClient.Maps.Grid;
using System.Configuration;

namespace mClient.Maps
{
    public class TerrainInfo
    {
        #region Declarations

        private int mMapId;
        private string mMapName;

        private GridMap[,] m_GridMaps = new GridMap[GridDefines.MAX_NUMBER_OF_GRIDS, GridDefines.MAX_NUMBER_OF_GRIDS];
        private object m_GridMapLock = new object();

        #endregion

        #region Constructors

        public TerrainInfo(int mapId, string mapName)
        {
            mMapId = mapId;
            mMapName = (string.IsNullOrEmpty(mapName) ? "UNNAMEDMAP\x0" : mapName); // used for loading VMAPs
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the path of the Map files
        /// </summary>
        public string MapFilePath
        {
            get { return ConfigurationManager.AppSettings["MapsLocation"]; }
        }

        #endregion

        #region Public Methods

        public GridMap Load(uint x, uint y)
        {
            if (x >= GridDefines.MAX_NUMBER_OF_GRIDS) throw new ApplicationException("MAX_NUMBER_OF_GRIDS exceeded");
            if (y >= GridDefines.MAX_NUMBER_OF_GRIDS) throw new ApplicationException("MAX_NUMBER_OF_GRIDS exceeded");

            // check if GridMap already loaded
            GridMap pMap = m_GridMaps[x, y];
            if (pMap == null)
                pMap = LoadMapAndVMap(x, y);

            return pMap;
        }

        #endregion

        #region Private Methods

        private GridMap LoadMapAndVMap(uint x, uint y)
        {
            if (m_GridMaps[x, y] == null)
            {
                lock (m_GridMapLock)
                {
                    if (m_GridMaps[x, y] == null)
                    {
                        GridMap map = new GridMap();

                        // load this tile :: maps/MMMXXYY.map
                        string filepath = MapFilePath + getMapFile((int)x, (int)y);
                        map.LoadData(filepath);

                        m_GridMaps[x, y] = map;

                        // let's see how far this gets us. we may need to load VMAPS as well and get that for the height
                    }
                }
            }

            return m_GridMaps[x, y];
        }

        private string getMapFile(int x, int y)
        {
            return string.Format("{0}{1}{2}.map", mMapId.ToString("D3"), x.ToString("D2"), y.ToString("D2"));
        }

        #endregion
    }
}
