using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if DT_POLYREF64
using dtPolyRef = System.UInt64;
using dtTileRef = System.UInt64;
#else
using dtPolyRef = System.UInt32;
using dtTileRef = System.UInt32;
#endif

namespace mClient.Maps
{
    public class MMapManager
    {
        #region Singleton

        static readonly MMapManager instance = new MMapManager();

        static MMapManager() { }

        MMapManager()
        {
            loadedTiles = 0;
            loadedMaps = new Dictionary<uint, MMapData>();

            // check if our mmaps file path exists
            if (!Directory.Exists(MMapFilePath)) throw new ApplicationException($"MMapFilePath {MMapFilePath} does not exist!");
        }

        public static MMapManager Instance { get { return instance; } }

        #endregion

        #region Declarations

        private uint loadedTiles;
        private Dictionary<uint, MMapData> loadedMaps;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the path of the MMap files
        /// </summary>
        public string MMapFilePath
        {
            get { return ConfigurationManager.AppSettings["MMapFilePath"]; }
        }

        public uint getLoadedTilesCount { get { return loadedTiles; } }

        public uint getLoadedMapsCount { get { return (uint)loadedMaps.Count; } }

        #endregion

        #region Public Methods

        public bool loadMap(uint mapId, int x, int y)
        {
            // make sure the map is loaded and ready to load tiles
            if (!loadMapData(mapId))
                return false;

            // get this map data
            MMapData mmap = loadedMaps[mapId];
            if (mmap == null || mmap.Mesh == null) throw new ApplicationException("Map Data and/or Mesh is null");

            // check if we already have this tile loaded
            uint packedGridPos = packTileID(x, y);
            if (mmap.MMapLoadedTiles.ContainsKey(packedGridPos))
            {
                // TODO: sLog.outError("MMAP:loadMap: Asked to load already loaded navmesh tile. %03u%02i%02i.mmtile", mapId, x, y);
                return false;
            }

            // load this tile :: mmaps/MMMXXYY.mmtile
            string filepath = MMapFilePath + getMapTileFile(mapId, x, y);
            byte[] rawTileData;
            MMapTileHeader tileHeader = null;
            using (BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open)))
            {
                tileHeader = reader.ReadMMapTileHeader();

                if (tileHeader.mmapMagic != MMapTileHeader.MMAP_MAGIC)
                {
                    // TODO: sLog.outError("MMAP:loadMap: Bad header in mmap %03u%02i%02i.mmtile", mapId, x, y);
                    return false;
                }

                if (tileHeader.mmapVersion != MMapTileHeader.MMAP_VERSION)
                {
                    // TODO: sLog.outError("MMAP:loadMap: %03u%02i%02i.mmtile was built with generator v%i, expected v%i",
                    //                      mapId, x, y, fileHeader.mmapVersion, MMAP_VERSION);
                    return false;
                }

                rawTileData = reader.ReadBytes((int)tileHeader.size);
            }
                
            if (rawTileData == null || rawTileData.Length == 0)
            {
                // TODO: sLog.outError("MMAP:loadMap: Bad header or data in mmap %03u%02i%02i.mmtile", mapId, x, y);
                return false;
            }

            Detour.dtRawTileData tileData = new Detour.dtRawTileData();
            tileData.FromBytes(rawTileData, 0);
            dtTileRef tileRef = 0;

            uint dtResult = mmap.Mesh.addTile(tileData, (int)Detour.dtTileFlags.DT_TILE_FREE_DATA, 0, ref tileRef);
            if (Detour.dtStatusFailed(dtResult))
            {
                // TODO: sLog.outError("MMAP:loadMap: Could not load %03u%02i%02i.mmtile into navmesh", mapId, x, y);
                return false;
            }

            mmap.AddTile(packedGridPos, tileRef);
            loadedTiles++;
            return true;
        }

        public bool unloadMap(uint mapId, int x, int y)
        {
            // check if we have this map loaded
            if (!loadedMaps.ContainsKey(mapId))
            {
                // TODO: DEBUG_FILTER_LOG(LOG_FILTER_MAP_LOADING, "MMAP:unloadMap: Asked to unload not loaded navmesh map. %03u%02i%02i.mmtile", mapId, x, y);
                return false;
            }

            MMapData mmap = loadedMaps[mapId];

            // check if we have this tile loaded
            uint packedGridPos = packTileID(x, y);
            if (!mmap.MMapLoadedTiles.ContainsKey(packedGridPos))
            {
                // TODO: DEBUG_FILTER_LOG(LOG_FILTER_MAP_LOADING, "MMAP:unloadMap: Asked to unload not loaded navmesh tile. %03u%02i%02i.mmtile", mapId, x, y);
                return false;
            }

            dtTileRef tileRef = mmap.MMapLoadedTiles[packedGridPos];

            // unload, and mark as non loaded
            Detour.dtRawTileData rawTileData;
            uint dtResult = mmap.Mesh.removeTile(tileRef, out rawTileData);
            if (Detour.dtStatusFailed(dtResult))
            {
                // TODO: sLog.outError("MMAP:unloadMap: Could not unload %03u%02i%02i.mmtile from navmesh", mapId, x, y);
                return false;
            }

            mmap.RemoveTile(packedGridPos);
            loadedTiles--;
            return true;
        }

        public bool unloadMap(uint mapId)
        {
            if (!loadedMaps.ContainsKey(mapId))
            {
                // TODO: DEBUG_FILTER_LOG(LOG_FILTER_MAP_LOADING, "MMAP:unloadMap: Asked to unload not loaded navmesh map %03u", mapId);
                return false;
            }

            // unload all tiles from given map
            MMapData mmap = loadedMaps[mapId];
            foreach (var tile in mmap.MMapLoadedTiles)
            {
                uint x = (tile.Key >> 16);
                uint y = (tile.Key & 0x0000FFFF);
                Detour.dtRawTileData rawTileData;
                uint dtResult = mmap.Mesh.removeTile(tile.Value, out rawTileData);
                if (Detour.dtStatusFailed(dtResult))
                {
                    // TODO: sLog.outError("MMAP:unloadMap: Could not unload %03u%02i%02i.mmtile from navmesh", mapId, x, y);
                }
                else
                {
                    loadedTiles--;
                }
            }

            mmap = null;
            loadedMaps.Remove(mapId);
            return true;
        }

        public bool unloadMapInstance(uint mapId, uint instanceId)
        {
            if (!loadedMaps.ContainsKey(mapId))
            {
                // TODO: DEBUG_FILTER_LOG(LOG_FILTER_MAP_LOADING, "MMAP:unloadMap: Asked to unload not loaded navmesh map %03u", mapId);
                return false;
            }

            MMapData mmap = loadedMaps[mapId];
            if (!mmap.NavMeshQueries.ContainsKey(instanceId))
            {
                // TODO: DEBUG_FILTER_LOG(LOG_FILTER_MAP_LOADING, "MMAP:unloadMapInstance: Asked to unload not loaded dtNavMeshQuery mapId %03u instanceId %u", mapId, instanceId);
                return false;
            }

            Detour.dtNavMeshQuery query = mmap.NavMeshQueries[instanceId];
            query = null;
            mmap.RemoveNavMeshQuery(instanceId);
            return true;
        }

        public Detour.dtNavMeshQuery GetNavMeshQuery(uint mapId, uint instanceId)
        {
            if (!loadedMaps.ContainsKey(mapId))
                return null;

            MMapData mmap = loadedMaps[mapId];
            if (!mmap.NavMeshQueries.ContainsKey(instanceId))
            {
                Detour.dtNavMeshQuery query = new Detour.dtNavMeshQuery();
                uint dtResult = query.init(mmap.Mesh, 1024);
                if (Detour.dtStatusFailed(dtResult))
                {
                    query = null;
                    // TODO: sLog.outError("MMAP:GetNavMeshQuery: Failed to initialize dtNavMeshQuery for mapId %03u instanceId %u", mapId, instanceId);
                    return null;
                }

                mmap.AddNavMeshQuery(instanceId, query);
            }

            return mmap.NavMeshQueries[instanceId];
        }

        public Detour.dtNavMesh GetNavMesh(uint mapId)
        {
            if (!loadedMaps.ContainsKey(mapId))
                return null;

            return loadedMaps[mapId].Mesh;
        }

        #endregion

        #region Private Methods

        private bool loadMapData(uint mapId)
        {
            // If we already have this map loaded
            if (loadedMaps.ContainsKey(mapId))
                return true;

            // load and init dtNavMesh - read parameters from file
            string mapFilePath = MMapFilePath + getMapFile(mapId);
            Detour.dtNavMeshParams @params = null;
            using (BinaryReader reader = new BinaryReader(File.Open(mapFilePath, FileMode.Open)))
                @params = reader.ReadDetourNavMeshParams();

            if (@params == null) throw new ApplicationException($"Unable to read dtNavMeshParams from map file {mapFilePath}");

            Detour.dtNavMesh mesh = null;
            uint dtResult = mesh.init(@params);
            if (Detour.dtStatusFailed(dtResult))
            {
                mesh = null;
                // TODO: Log something
                return false;
            }

            MMapData mmap_data = new MMapData(mesh);
            loadedMaps.Add(mapId, mmap_data);
            return true;
        }

        private uint packTileID(int x, int y)
        {
            return (uint)(x << 16 | y);
        }

        private string getMapFile(uint mapId)
        {
            return string.Format("{0}.mmap", mapId.ToString("D3"));
        }

        private string getMapTileFile(uint mapId, int x, int y)
        {
            return string.Format("{0}{1}{2}.mmtile", mapId.ToString("D3"), x.ToString("D2"), y.ToString("D2"));
        }

        #endregion
    }
}
