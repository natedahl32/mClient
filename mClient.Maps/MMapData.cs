using System;
using System.Collections.Generic;
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
    public class MMapData
    {
        #region Declarations

        private Detour.dtNavMesh navMesh;
        private Dictionary<uint, Detour.dtNavMeshQuery> navMeshQueries;
        private Dictionary<uint, dtTileRef> mmapLoadedTiles;

        #endregion

        #region Constructors

        public MMapData(Detour.dtNavMesh mesh)
        {
            if (mesh == null) throw new ArgumentNullException("mesh");
            navMesh = mesh;
            navMeshQueries = new Dictionary<uint, Detour.dtNavMeshQuery>();
            mmapLoadedTiles = new Dictionary<uint, dtTileRef>();
        }

        #endregion

        #region Properties

        public Detour.dtNavMesh Mesh { get { return navMesh; } }

        public IReadOnlyDictionary<uint, dtTileRef> MMapLoadedTiles
        {
            get { return mmapLoadedTiles; }
        }

        public IReadOnlyDictionary<uint, Detour.dtNavMeshQuery> NavMeshQueries
        {
            get { return navMeshQueries; }
        }

        #endregion

        #region Public Methods

        public void AddTile(uint packedGridPos, dtTileRef tileRef)
        {
            if (mmapLoadedTiles.ContainsKey(packedGridPos))
                return;
            mmapLoadedTiles.Add(packedGridPos, tileRef);
        }

        public void AddNavMeshQuery(uint instanceId, Detour.dtNavMeshQuery query)
        {
            if (navMeshQueries.ContainsKey(instanceId))
                return;
            navMeshQueries.Add(instanceId, query);
        }

        public void RemoveTile(uint packedGridPos)
        {
            if (mmapLoadedTiles.ContainsKey(packedGridPos))
                mmapLoadedTiles.Remove(packedGridPos);
        }

        public void RemoveNavMeshQuery(uint instanceId)
        {
            if (navMeshQueries.ContainsKey(instanceId))
                navMeshQueries.Remove(instanceId);
        }

        #endregion
    }
}
