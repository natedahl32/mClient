using mClient.Maps;
using mClient.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#if DT_POLYREF64
using dtPolyRef = System.UInt64;
using dtTileRef = System.UInt64;
#else
using dtPolyRef = System.UInt32;
using dtTileRef = System.UInt32;
#endif

namespace mClient.Pathfinding
{

    [Flags]
    public enum PathType
    {
        PATHFIND_BLANK = 0x0000,   // path not built yet
        PATHFIND_NORMAL = 0x0001,   // normal path
        PATHFIND_SHORTCUT = 0x0002,   // travel through obstacles, terrain, air, etc (old behavior)
        PATHFIND_INCOMPLETE = 0x0004,   // we have partial path to follow - getting closer to target
        PATHFIND_NOPATH = 0x0008,   // no valid path at all or error in generating one
        PATHFIND_NOT_USING_PATH = 0x0010    // used when we are either flying/swiming or on map w/o mmaps
    }

    /// <summary>
    /// Mimics the PathFinder class in CMangos
    /// </summary>
    public class PathFinder
    {
        #region Declarations

        public const int INVALID_POLYREF = 0;
        public const int VERTEX_SIZE = 3;
        public const int MAX_PATH_LENGTH = 74;
        public const int MAX_POINT_PATH_LENGTH = 74;
        public const float SMOOTH_PATH_STEP_SIZE = 4.0f;
        public const float SMOOTH_PATH_SLOP = 0.3f;

        private dtPolyRef[] m_pathPolyRefs;                         // array of detour polygon references
        private uint m_polyLength;                                  // number of polygons in the path

        private List<Vector3> m_pathPoints;                         // our actual (x,y,z) path to the target
        private PathType m_type;                                    // tells what kind of path this is

        private bool m_useStraightPath;                             // type of path will be generated
        private bool m_forceDestination;                            // when set, we will always arrive at given point
        private uint m_pointPathLimit;                              // limit point path size; min(this, MAX_POINT_PATH_LENGTH)

        private Vector3 m_startPosition;                            // {x, y, z} of current location
        private Vector3 m_endPosition;                              // {x, y, z} of the destination
        private Vector3 m_actualEndPosition;                        // {x, y, z} of the closest possible point to given destination

        private Clients.Unit m_sourceUnit;                          // the unit that is moving
        private Detour.dtNavMesh m_navMesh;                         // the nav mesh
        private Detour.dtNavMeshQuery m_navMeshQuery;               // the nav mesh query used to find the path

        private Detour.dtQueryFilter m_filter;                      // use single filter for all movements, update it when needed

        #endregion

        #region Constructors

        public PathFinder(Clients.PlayerObj owner, uint mapID, uint intanceID)
        {
            m_polyLength = 0;
            m_type = PathType.PATHFIND_BLANK;
            m_useStraightPath = false;
            m_forceDestination = false;
            m_pointPathLimit = MAX_POINT_PATH_LENGTH;
            m_sourceUnit = owner;
            m_navMesh = null;
            m_navMeshQuery = null;

            uint mapId = mapID;
            m_navMesh = MMapManager.Instance.GetNavMesh(mapId);
            m_navMeshQuery = MMapManager.Instance.GetNavMeshQuery(mapId, intanceID);

            createFilter();
        }

        #endregion

        #region Properties

        public Vector3 getStartPosition { get { return m_startPosition; } }

        public Vector3 getEndPosition { get { return m_endPosition; } }

        public Vector3 getActualEndPosition { get { return m_actualEndPosition; } }

        public IList<Vector3> getPath { get { return m_pathPoints; } }
        public PathType getPathType { get { return m_type; } }

        #endregion

        #region Public Methods

        public bool calculate(float destX, float destY, float destZ, bool forceDest)
        {
            Vector3 dest = new Vector3(destX, destY, destZ);
            setEndPosition(dest);

            float x = m_sourceUnit.Position.X;
            float y = m_sourceUnit.Position.Y;
            float z = m_sourceUnit.Position.Z;

            Vector3 start = new Vector3(x, y, z);
            setStartPosition(start);

            m_forceDestination = forceDest;

            // make sure navMesh works - we can run on map w/o mmap
            // check if the start and end point have a .mmtile loaded (can we pass via not loaded tile on the way?)
            if (m_navMesh == null || m_navMeshQuery == null || !HaveTile(start) || !HaveTile(dest))
            {
                BuildShortcut();
                m_type = PathType.PATHFIND_NORMAL | PathType.PATHFIND_NOT_USING_PATH;
                return true;
            }

            updateFilter();

            BuildPolyPath(start, dest);
            return true;
        }

        public void setUseStraightPath(bool useStraightPath)
        {
            m_useStraightPath = useStraightPath;
        }

        public void setPathLengthLimit(float distance)
        {
            m_pointPathLimit = Math.Min((uint)(distance / SMOOTH_PATH_STEP_SIZE), MAX_POINT_PATH_LENGTH);
        }

        #endregion

        #region Private Methods

        private void setStartPosition(Vector3 point) { m_startPosition = point; }
        private void setEndPosition(Vector3 point) { m_endPosition = point; }
        private void setActualEndPosition(Vector3 point) { m_actualEndPosition = point; }

        private void clear()
        {
            m_polyLength = 0;
            m_pathPoints.Clear();
        }

        private bool inRange(Vector3 p1, Vector3 p2, float r, float h)
        {
            Vector3 d = p1 - p2;
            return (d.X * d.X + d.Y * d.Y) < r * r && Math.Abs(d.Z) < h;
        }

        private bool inRangeYZX(float[] v1, float[] v2, float r, float h)
        {
            float dx = v2[0] - v1[0];
            float dy = v2[1] - v1[1];
            float dz = v2[2] - v1[2];
            return (dx * dx + dz * dz) < r * r && Math.Abs(dy) < h;
        }

        private float dist3DSqr(Vector3 p1, Vector3 p2)
        {
            return (p1 - p2).squaredLength();
        }

        private dtPolyRef getPathPolyByPosition(dtPolyRef[] polyPath, uint polyPathSize, float[] point, float distance)
        {
            if (polyPath.Length == 0 || polyPathSize == 0)
                return INVALID_POLYREF;

            dtPolyRef nearestPoly = INVALID_POLYREF;
            float minDist2d = float.MaxValue;
            float minDist3d = 0.0f;

            for (uint i = 0; i < polyPathSize; ++i)
            {
                float[] closestPoint = new float[VERTEX_SIZE];
                bool posOverPoly = false;
                uint dtResult = m_navMeshQuery.closestPointOnPoly(polyPath[i], point, ref closestPoint, ref posOverPoly);
                if (Detour.dtStatusFailed(dtResult))
                    continue;

                float d = Detour.dtVdist2DSqr(point, closestPoint);
                if (d < minDist2d)
                {
                    minDist2d = d;
                    nearestPoly = polyPath[i];
                    minDist3d = Detour.dtVdistSqr(point, closestPoint);
                }

                if (minDist2d < 1.0f)
                    break;
            }

            distance = (float)Math.Sqrt(minDist3d);
            return (minDist2d < 3.0f) ? nearestPoly : INVALID_POLYREF;
        }

        private dtPolyRef getPolyByLocation(float[] point, ref float distance)
        {
            // first we check the current path
            // if the current path doesn't contain the current poly,
            // we need to use the expensive navMesh.findNearestPoly
            dtPolyRef polyRef = getPathPolyByPosition(m_pathPolyRefs, m_polyLength, point, distance);
            if (polyRef != INVALID_POLYREF)
                return polyRef;

            // we don't have it in our old path
            // try to get it by findNearestPoly()
            // first try with low search box
            float[] extents = new float[VERTEX_SIZE] { 3.0f, 5.0f, 3.0f };
            float[] closestPoint = new float[VERTEX_SIZE] { 0.0f, 0.0f, 0.0f };
            uint dtResult = m_navMeshQuery.findNearestPoly(point, extents, m_filter, ref polyRef, ref closestPoint);
            if (Detour.dtStatusSucceed(dtResult) && polyRef != INVALID_POLYREF)
            {
                distance = Detour.dtVdist(closestPoint, point);
                return polyRef;
            }

            // still nothing ..
            // try with bigger search box
            extents[1] = 200.0f;
            dtResult = m_navMeshQuery.findNearestPoly(point, extents, m_filter, ref polyRef, ref closestPoint);
            if (Detour.dtStatusSucceed(dtResult) && polyRef != INVALID_POLYREF)
            {
                distance = Detour.dtVdist(closestPoint, point);
                return polyRef;
            }

            return INVALID_POLYREF;
        }

        private bool HaveTile(Vector3 p)
        {
            int tx = 0, ty = 0;
            float[] point = new float[VERTEX_SIZE] { p.Y, p.Z, p.X };

            m_navMesh.calcTileLoc(point, ref tx, ref ty);
            return (m_navMesh.getTileAt(tx, ty, 0) != null);
        }

        private void BuildPolyPath(Vector3 startPos, Vector3 endPos)
        {
            float distToStartPoly = 0f, distToEndPoly = 0f;
            float[] startPoint = new float[VERTEX_SIZE] { startPos.Y, startPos.Z, startPos.X };
            float[] endPoint = new float[VERTEX_SIZE] { endPos.Y, endPos.Z, endPos.X };

            dtPolyRef startPoly = getPolyByLocation(startPoint, ref distToStartPoly);
            dtPolyRef endPoly = getPolyByLocation(endPoint, ref distToEndPoly);

            uint dtResult = 0;

            // we have a hole in our mesh
            // make shortcut path and mark it as NOPATH ( with flying exception )
            // its up to caller how he will use this info
            if (startPoly == INVALID_POLYREF || endPoly == INVALID_POLYREF)
            {
                BuildShortcut();

                if (m_sourceUnit.Type == Constants.ObjectType.Unit)
                {
                    // Check fo swimming or flying shortcut
                    // TODO: Checks for swimming and flying
                }
                else
                    m_type = PathType.PATHFIND_NOPATH;

                return;
            }

            //we may need a better number here
            bool farFromPoly = (distToStartPoly > 7.0f || distToEndPoly > 7.0f);
            if (farFromPoly)
            {
                bool buildShortcut = false;
                if (m_sourceUnit.Type == Constants.ObjectType.Unit)
                {
                    // TODO: If we are underwater we should build shortcut
                }

                if (buildShortcut)
                {
                    BuildShortcut();
                    m_type = PathType.PATHFIND_NORMAL | PathType.PATHFIND_NOT_USING_PATH;
                    return;
                }
                else
                {
                    float[] closestPoint = new float[VERTEX_SIZE];
                    // we may want to use closestPointOnPolyBoundary instead
                    bool posOverPoly = false;
                    dtResult = m_navMeshQuery.closestPointOnPoly(endPoly, endPoint, ref closestPoint, ref posOverPoly);
                    if (Detour.dtStatusSucceed(dtResult))
                    {
                        Detour.dtVcopy(ref endPoint, closestPoint);
                        setActualEndPosition(new Vector3(endPoint[2], endPoint[0], endPoint[1]));
                    }

                    m_type = PathType.PATHFIND_INCOMPLETE;
                }
            }

            // *** poly path generating logic ***

            // start and end are on same polygon
            // just need to move in straight line
            if (startPoly == endPoly)
            {
                BuildShortcut();

                m_pathPolyRefs[0] = startPoly;
                m_polyLength = 1;

                m_type = farFromPoly ? PathType.PATHFIND_INCOMPLETE : PathType.PATHFIND_NORMAL;
                return;
            }

            // look for startPoly/endPoly in current path
            // TODO: we can merge it with getPathPolyByPosition() loop
            bool startPolyFound = false;
            bool endPolyFound = false;
            uint pathStartIndex = 0, 
                 pathEndIndex = 0;

            if (m_polyLength > 0)
            {
                for (pathStartIndex = 0; pathStartIndex < m_polyLength; ++pathStartIndex)
                {
                    if (m_pathPolyRefs[pathStartIndex] == startPoly)
                    {
                        startPolyFound = true;
                        break;
                    }
                }

                for (pathEndIndex = m_polyLength - 1; pathEndIndex > pathStartIndex; --pathEndIndex)
                {
                    if (m_pathPolyRefs[pathEndIndex] == endPoly)
                    {
                        endPolyFound = true;
                        break;
                    }
                }
            }

            if (startPolyFound && endPolyFound)
            {
                // we moved along the path and the target did not move out of our old poly-path
                // our path is a simple subpath case, we have all the data we need
                // just "cut" it out

                m_polyLength = pathEndIndex - pathStartIndex + 1;
                uint[] tempSource = new uint[0];
                Array.Copy(m_pathPolyRefs, pathStartIndex, tempSource, 0, m_polyLength * sizeof(dtPolyRef));
                Array.Copy(tempSource, m_pathPolyRefs, m_polyLength * sizeof(dtPolyRef));
            }
            else if (startPolyFound && !endPolyFound)
            {
                // we are moving on the old path but target moved out
                // so we have atleast part of poly-path ready

                m_polyLength -= pathStartIndex;

                // try to adjust the suffix of the path instead of recalculating entire length
                // at given interval the target cannot get too far from its last location
                // thus we have less poly to cover
                // sub-path of optimal path is optimal

                // take ~80% of the original length
                // TODO : play with the values here
                uint prefixPolyLength = (uint)(m_polyLength * 0.8f + 0.5f);

                uint[] tempSource = new uint[0];
                Array.Copy(m_pathPolyRefs, pathStartIndex, tempSource, 0, prefixPolyLength * sizeof(dtPolyRef));
                Array.Copy(tempSource, m_pathPolyRefs, prefixPolyLength * sizeof(dtPolyRef));

                dtPolyRef suffixStartPoly = m_pathPolyRefs[prefixPolyLength - 1];

                // we need any point on our suffix start poly to generate poly-path, so we need last poly in prefix data
                float[] suffixEndPoint = new float[VERTEX_SIZE];
                bool posOverPoly = false;
                dtResult = m_navMeshQuery.closestPointOnPoly(suffixStartPoly, endPoint, ref suffixEndPoint, ref posOverPoly);
                if (Detour.dtStatusFailed(dtResult))
                {
                    // we can hit offmesh connection as last poly - closestPointOnPoly() don't like that
                    // try to recover by using prev polyref
                    --prefixPolyLength;
                    suffixStartPoly = m_pathPolyRefs[prefixPolyLength - 1];
                    dtResult = m_navMeshQuery.closestPointOnPoly(suffixStartPoly, endPoint, ref suffixEndPoint, ref posOverPoly);
                    if (Detour.dtStatusFailed(dtResult))
                    {
                        // suffixStartPoly is still invalid, error state
                        BuildShortcut();
                        m_type = PathType.PATHFIND_NOPATH;
                        return;
                    }
                }

                // generate suffix
                uint suffixPolyLength = 0;

                // gets the out path to send to find path
                uint[] outPath = new uint[suffixPolyLength];
                int tempSuffixPolyLength = 0;

                dtResult = m_navMeshQuery.findPath(
                                            suffixStartPoly,
                                            endPoly,
                                            suffixEndPoint,
                                            endPoint,
                                            m_filter,
                                            ref outPath,
                                            ref tempSuffixPolyLength,
                                            (int)(MAX_PATH_LENGTH - prefixPolyLength));

                // Handles copying the ref values back to our actual member variables
                suffixPolyLength = (uint)tempSuffixPolyLength;
                Array.Copy(outPath, 0, m_pathPolyRefs, prefixPolyLength - 1, outPath.Length);

                if (suffixPolyLength == 0 || Detour.dtStatusFailed(dtResult))
                {
                    // this is probably an error state, but we'll leave it
                    // and hopefully recover on the next Update
                    // we still need to copy our preffix
                }

                // new path = prefix + suffix - overlap
                m_polyLength = prefixPolyLength + suffixPolyLength - 1;
            }
            else
            {
                // either we have no path at all -> first run
                // or something went really wrong -> we aren't moving along the path to the target
                // just generate new path

                // free and invalidate old path data
                clear();

                int tempPolyLength = 0;
                dtResult = m_navMeshQuery.findPath(
                                            startPoly,
                                            endPoly,
                                            startPoint,
                                            endPoint,
                                            m_filter,
                                            ref m_pathPolyRefs,
                                            ref tempPolyLength,
                                            MAX_PATH_LENGTH);

                // Handles copying the ref values back to our actual member variables
                m_polyLength = (uint)tempPolyLength;

                if (m_polyLength == 0 || Detour.dtStatusFailed(dtResult))
                {
                    // TODO: sLog.outError("%u's Path Build failed: 0 length path", m_sourceUnit->GetGUIDLow());
                    BuildShortcut();
                    m_type = PathType.PATHFIND_NOPATH;
                    return;
                }
            }

            // by now we know what type of path we can get
            if (m_pathPolyRefs[m_polyLength - 1] == endPoly && !m_type.HasFlag(PathType.PATHFIND_INCOMPLETE))
                m_type = PathType.PATHFIND_NORMAL;
            else
                m_type = PathType.PATHFIND_INCOMPLETE;

            // generate the point-path out of our up-to-date poly-path
            BuildPointPath(startPoint, endPoint);
        }

        private void BuildPointPath(float[] startPoint, float[] endPoint)
        {
            float[] pathPoints = new float[MAX_POINT_PATH_LENGTH * VERTEX_SIZE];
            int pointCount = 0;
            uint dtResult = Detour.DT_FAILURE;
            if (m_useStraightPath)
            {
                byte[] temp = new byte[0];
                uint[] temp2 = new uint[0];
                dtResult = m_navMeshQuery.findStraightPath(
                                            startPoint,
                                            endPoint,
                                            m_pathPolyRefs,
                                            (int)m_polyLength,
                                            pathPoints,
                                            ref temp,
                                            ref temp2,
                                            ref pointCount,
                                            (int)m_pointPathLimit,
                                            0);
            }
            else
            {
                dtResult = findSmoothPath(
                                startPoint,
                                endPoint,
                                m_pathPolyRefs,
                                m_polyLength,
                                pathPoints,
                                pointCount,
                                m_pointPathLimit);
            }

            if (pointCount < 2 || Detour.dtStatusFailed(dtResult))
            {
                // only happens if pass bad data to findStraightPath or navmesh is broken
                // single point paths can be generated here
                // TODO : check the exact cases
                // TODO: DEBUG_FILTER_LOG(LOG_FILTER_PATHFINDING, "++ PathFinder::BuildPointPath FAILED! path sized %d returned\n", pointCount);
                BuildShortcut();
                m_type = PathType.PATHFIND_NOPATH;
                return;
            }

            m_pathPoints.resize(pointCount);
            for (uint i = 0; i < pointCount; ++i)
                m_pathPoints[(int)i] = new Vector3(pathPoints[i * VERTEX_SIZE + 2], pathPoints[i * VERTEX_SIZE], pathPoints[i * VERTEX_SIZE + 1]);

            // first point is always our current location - we need the next one
            setActualEndPosition(m_pathPoints[pointCount - 1]);

            // force the given destination, if needed
            if (m_forceDestination && (!m_type.HasFlag(PathType.PATHFIND_NORMAL) || !inRange(getEndPosition, getActualEndPosition, 1.0f, 1.0f)))
            {
                // we may want to keep partial subpath
                if (dist3DSqr(getActualEndPosition, getEndPosition) < 0.3f * dist3DSqr(getStartPosition, getEndPosition))
                {
                    setActualEndPosition(getEndPosition);
                    m_pathPoints[m_pathPoints.Count - 1] = getEndPosition;
                }
                else
                {
                    setActualEndPosition(getEndPosition);
                    BuildShortcut();
                }

                m_type = PathType.PATHFIND_NORMAL | PathType.PATHFIND_NOT_USING_PATH;
            }
        }

        private void BuildShortcut()
        {
            clear();

            // make two point path, our curr pos is the start, and dest is the end
            m_pathPoints.resize(2);

            // set start and a default next position
            m_pathPoints[0] = getStartPosition;
            m_pathPoints[1] = getActualEndPosition;

            m_type = PathType.PATHFIND_SHORTCUT;
        }

        private NavTerrain getNavTerrain(float x, float y, float z)
        {
            GridMapLiquidData data = null;
            // TODO: Get terrain from the source unit

            //switch (data.type_flags)
            //{
            //    case MAP_LIQUID_TYPE_WATER:
            //    case MAP_LIQUID_TYPE_OCEAN:
            //        return NAV_WATER;
            //    case MAP_LIQUID_TYPE_MAGMA:
            //        return NAV_MAGMA;
            //    case MAP_LIQUID_TYPE_SLIME:
            //        return NAV_SLIME;
            //    default:
            //        return NAV_GROUND;
            //}

            return NavTerrain.NAV_GROUND; // Returning NAV_GROUND by default for now
        }

        private void createFilter()
        {
            NavTerrain includeFlags = 0;
            NavTerrain excludeFlags = 0;

            // We are players, we can always walk
            includeFlags |= (NavTerrain.NAV_GROUND | NavTerrain.NAV_WATER);

            m_filter.setIncludeFlags((ushort)includeFlags);
            m_filter.setExcludeFlags((ushort)excludeFlags);

            updateFilter();
        }

        private void updateFilter()
        {
            // do nothing for now. not sure if we need to for players or not
        }

        private uint fixupCorridor(dtPolyRef[] path, uint npath, uint maxPath, dtPolyRef[] visited, uint nvisited)
        {
            int furthestPath = -1;
            int furthestVisited = -1;

            // Find furthest common polygon
            for (int i = (int)(npath - 1); i >= 0; --i)
            {
                bool found = false;
                for (int j = (int)(nvisited - 1); j >= 0; --j)
                {
                    if (path[i] == visited[j])
                    {
                        furthestPath = i;
                        furthestVisited = j;
                        found = true;
                    }
                }
                if (found)
                    break;
            }

            // If no intersection found just return current path.
            if (furthestPath == -1 || furthestVisited == -1)
                return npath;

            // Concatenate paths.

            // Adjust beginning of the buffer to include the visited.
            uint req = nvisited - (uint)furthestVisited;
            uint orig = (furthestPath + 1) < npath ? (uint)furthestPath + 1 : npath;
            uint size = npath > orig ? npath - orig : 0;
            if (req + size > maxPath)
                size = maxPath - req;

            return req + size;
        }

        private bool getSteerTarget(float[] startPos, float[] endPos, float minTargetDist, dtPolyRef[] path, uint pathSize, float[] steerPos, ref byte steerPosFlag, ref dtPolyRef steerPosRef)
        {
            // Find steer target
            const uint MAX_STEER_POINTS = 3;
            float[] steerPath = new float[MAX_STEER_POINTS * VERTEX_SIZE];
            byte[] steerPathFlags = new byte[MAX_STEER_POINTS];
            dtPolyRef[] steerPathPolys = new dtPolyRef[MAX_STEER_POINTS];
            int nsteerPath = 0;
            uint dtResult = m_navMeshQuery.findStraightPath(startPos, endPos, path, (int)pathSize, steerPath, ref steerPathFlags, ref steerPathPolys, ref nsteerPath, (int)MAX_STEER_POINTS, 0);
            if (nsteerPath == 0 || Detour.dtStatusFailed(dtResult))
                return false;

            // Find vertex far enough to steer to
            uint ns = 0;
            while (ns < nsteerPath)
            {
                // Stop at Off-Mesh link or when point is further than slop away.
                float[] tempSteerPath = new float[3];
                Array.Copy(steerPath, ns * VERTEX_SIZE, tempSteerPath, 0, 3);

                if ((steerPathFlags[ns] & (byte)Detour.dtStraightPathFlags.DT_STRAIGHTPATH_OFFMESH_CONNECTION) == steerPathFlags[ns] || !inRangeYZX(tempSteerPath, startPos, minTargetDist, 1000.0f))
                    break;
                ++ns;
            }
            // Failed to find good point to steer to
            if (ns >= nsteerPath)
                return false;

            float[] tempSteerPath2 = new float[3];
            Array.Copy(steerPath, ns * VERTEX_SIZE, tempSteerPath2, 0, 3);
            Detour.dtVcopy(ref steerPos, tempSteerPath2);
            steerPos[1] = startPos[1]; // keep Z value
            steerPosFlag = steerPathFlags[ns];
            steerPosRef = steerPathPolys[ns];

            return true;
        }

        private uint findSmoothPath(float[] startPos, float[] endPos, dtPolyRef[] polyPath, uint polyPathSize, float[] smoothPath, int smoothPathSize, uint maxSmootPathSize)
        {
            maxSmootPathSize = 0;
            uint nsmoothPath = 0;

            dtPolyRef[] polys = new dtPolyRef[MAX_PATH_LENGTH];
            Array.Copy(polyPath, polys, polyPath.Length);
            uint npolys = polyPathSize;

            float[] iterPos = new float[VERTEX_SIZE];
            float[] targetPos = new float[VERTEX_SIZE];
            uint dtResult = m_navMeshQuery.closestPointOnPolyBoundary(polys[0], startPos, ref iterPos);
            if (Detour.dtStatusFailed(dtResult))
                return Detour.DT_FAILURE;

            dtResult = m_navMeshQuery.closestPointOnPolyBoundary(polys[npolys - 1], endPos, ref targetPos);
            if (Detour.dtStatusFailed(dtResult))
                return Detour.DT_FAILURE;

            // Replacing Vcopy since we can't get address of middle of array without unsafe code
            smoothPath[nsmoothPath * VERTEX_SIZE] = iterPos[0];
            smoothPath[nsmoothPath * VERTEX_SIZE + 1] = iterPos[1];
            smoothPath[nsmoothPath * VERTEX_SIZE + 2] = iterPos[2];
            ++nsmoothPath;

            // Move towards target a small advancement at a time until target reached or
            // when ran out of memory to store the path.
            while (npolys > 0 && nsmoothPath < maxSmootPathSize)
            {
                // Find location to steer towards
                float[] steerPos = new float[VERTEX_SIZE];
                byte steerPosFlag = 0;
                dtPolyRef steerPosRef = INVALID_POLYREF;

                if (!getSteerTarget(iterPos, targetPos, SMOOTH_PATH_SLOP, polys, npolys, steerPos, ref steerPosFlag, ref steerPosRef))
                    break;

                bool endOfPath = (steerPosFlag & (byte)Detour.dtStraightPathFlags.DT_STRAIGHTPATH_END) == steerPosFlag;
                bool offMeshConnection = (steerPosFlag & (byte)Detour.dtStraightPathFlags.DT_STRAIGHTPATH_OFFMESH_CONNECTION) == steerPosFlag;

                // Find movement delta
                float[] delta = new float[VERTEX_SIZE];
                Detour.dtVsub(ref delta, steerPos, iterPos);
                float len = (float)Math.Sqrt(Detour.dtVdot(delta, delta));
                // If the steer target is end of path or off-mesh link, do not move past the location.
                if ((endOfPath || offMeshConnection) && len < SMOOTH_PATH_STEP_SIZE)
                    len = 1.0f;
                else
                    len = SMOOTH_PATH_STEP_SIZE / len;

                float[] moveTgt = new float[VERTEX_SIZE];
                Detour.dtVmad(ref moveTgt, iterPos, delta, len);

                // Move
                float[] result = new float[VERTEX_SIZE];
                const uint MAX_VISIT_POLY = 16;
                dtPolyRef[] visited = new uint[MAX_VISIT_POLY];

                int nvisited = 0;
                m_navMeshQuery.moveAlongSurface(polys[0], iterPos, moveTgt, m_filter, ref result, ref visited, ref nvisited, (int)MAX_VISIT_POLY);
                npolys = fixupCorridor(polys, npolys, MAX_PATH_LENGTH, visited, (uint)nvisited);

                m_navMeshQuery.getPolyHeight(polys[0], result, ref result[1]);
                result[1] += 0.5f;
                Detour.dtVcopy(ref iterPos, result);

                // Handle end of path and off-mesh links when close enough
                if (endOfPath && inRangeYZX(iterPos, steerPos, SMOOTH_PATH_SLOP, 1.0f))
                {
                    // Reached end of path
                    Detour.dtVcopy(ref iterPos, targetPos);
                    if (nsmoothPath < maxSmootPathSize)
                    {
                        smoothPath[nsmoothPath * VERTEX_SIZE] = iterPos[0];
                        smoothPath[nsmoothPath * VERTEX_SIZE + 1] = iterPos[1];
                        smoothPath[nsmoothPath * VERTEX_SIZE + 2] = iterPos[2];
                        ++nsmoothPath;
                    }
                    break;
                }
                else if (offMeshConnection && inRangeYZX(iterPos, steerPos, SMOOTH_PATH_SLOP, 1.0f))
                {
                    // Advance the path up to and over the off-mesh connection
                    dtPolyRef prevRef = INVALID_POLYREF;
                    dtPolyRef polyRef = polys[0];
                    uint npos = 0;

                    while(npos < npolys && polyRef != steerPosRef)
                    {
                        prevRef = polyRef;
                        polyRef = polys[npos];
                        ++npos;
                    }

                    for (uint i = npos; i < npolys; ++i)
                        polys[i - npos] = polys[i];

                    npolys -= npos;

                    // Handle the connection
                    dtResult = m_navMesh.getOffMeshConnectionPolyEndPoints(prevRef, polyRef, ref startPos, ref endPos);
                    if (Detour.dtStatusSucceed(dtResult))
                    {
                        if (nsmoothPath < maxSmootPathSize)
                        {
                            smoothPath[nsmoothPath * VERTEX_SIZE] = startPos[0];
                            smoothPath[nsmoothPath * VERTEX_SIZE + 1] = startPos[1];
                            smoothPath[nsmoothPath * VERTEX_SIZE + 2] = startPos[2];
                            ++nsmoothPath;
                        }
                        // Move position at the other side of the off-mesh link.
                        Detour.dtVcopy(ref iterPos, endPos);

                        float height = iterPos[1];
                        m_navMeshQuery.getPolyHeight(polys[0], iterPos, ref height);
                        iterPos[1] = height + 0.5f;
                    }
                }

                // Store results
                if (nsmoothPath < maxSmootPathSize)
                {
                    smoothPath[nsmoothPath * VERTEX_SIZE] = iterPos[0];
                    smoothPath[nsmoothPath * VERTEX_SIZE + 1] = iterPos[1];
                    smoothPath[nsmoothPath * VERTEX_SIZE + 2] = iterPos[2];
                    ++nsmoothPath;
                }
            }

            smoothPathSize = (int)nsmoothPath;

            // this is most likely a loop
            return nsmoothPath < MAX_POINT_PATH_LENGTH ? Detour.DT_SUCCESS : Detour.DT_FAILURE;
        }

        #endregion
    }
}
