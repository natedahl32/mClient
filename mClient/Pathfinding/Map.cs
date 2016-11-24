using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mClient.Maps.Grid;

namespace mClient.Maps
{
    public class Map
    {
        #region Declarations

        private int mMapId;
        private bool[,] m_bLoadedGrids = new bool[GridDefines.MAX_NUMBER_OF_GRIDS, GridDefines.MAX_NUMBER_OF_GRIDS];
        private TerrainInfo mTerrainData;

        #endregion

        #region Constructors

        public Map(int mapId)
        {
            mMapId = mapId;
            mTerrainData = new TerrainInfo(mapId, string.Empty);
        }

        #endregion

        #region Public Methods

        public float GetHeight(float x, float y, float z)
        {
            //float staticHeight = mTerrainData.GetHeightStatic(x, y, z);

            // Get Dynamic Height around static Height (if valid)
            //float dynSearchHeight = 2.0f + (z < staticHeight ? staticHeight : z);
            //return Math.Max(staticHeight, m_dyn_tree.getHeight(x, y, dynSearchHeight, dynSearchHeight - staticHeight));

            // TODO: Need to write a lot of code for this
            return z;
        }

        public void LoadMapAndVMap(int gx, int gy)
        {
            if (m_bLoadedGrids[gx, gy])
                return;


        }

        #endregion
    }
}
