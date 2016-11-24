using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static mClient.Maps.Grid.GridDefines;

namespace mClient.Maps.Grid
{
    public class Cell
    {
        #region Declarations

        private ushort grid_x;
        private ushort grid_y;
        private ushort cell_x;
        private ushort cell_y;
        private byte nocreate;
        private ushort reserved;

        #endregion

        #region Constructors

        public Cell(CellPair p)
        {
            grid_x = (ushort)(p.XCoord / MAX_NUMBER_OF_CELLS);
            grid_y = (ushort)(p.YCoord / MAX_NUMBER_OF_CELLS);
            cell_x = (ushort)(p.XCoord % MAX_NUMBER_OF_CELLS);
            cell_y = (ushort)(p.YCoord % MAX_NUMBER_OF_CELLS);
            nocreate = 0;
            reserved = 0;
        }

        #endregion

        #region Properties

        public ushort GridX { get { return grid_x; } }
        public ushort GridY { get { return grid_y; } }
        public ushort CellX { get { return cell_x; } }
        public ushort CellY { get { return cell_y; } }
        public bool NoCreate { get { return nocreate > 0; } }

        #endregion
    }
}
