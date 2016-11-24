using mClient.Maps;
using mClient.Maps.Grid;
using mClient.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static mClient.Maps.Grid.GridDefines;

#if DT_POLYREF64
using dtPolyRef = System.UInt64;
using dtTileRef = System.UInt64;
#else
using dtPolyRef = System.UInt32;
using dtTileRef = System.UInt32;
#endif

namespace mClient.Pathfinding
{
    public static class MapMoveHelper
    {
        public static float GetZPolyBoundaryForLocation(Clients.PlayerObj player, uint mapId, uint instanceId, Vector3 location)
        {
            var cell = new Cell(ComputeCellPair(player.Position.X, player.Position.Y));
            var gridPair = new GridPair(cell.GridX, cell.GridY);

            var pathfinder = new PathFinder(player, mapId, instanceId);
            pathfinder.calculate(location.X, location.Y, location.Z, false);
            return pathfinder.getActualEndPosition.Z;
        }
    }
}
