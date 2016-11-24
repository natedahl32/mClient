using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Maps.Grid
{
    public static class GridDefines
    {
        #region Constants

        public const int MAX_NUMBER_OF_GRIDS = 64;

        public const float SIZE_OF_GRIDS = 533.33333f;
        public const int CENTER_GRID_ID = (MAX_NUMBER_OF_GRIDS / 2);

        public const float CENTER_GRID_OFFSET = (SIZE_OF_GRIDS / 2);

        public const int MIN_GRID_DELAY = (60 * 1000); // (MINUTE * IN_MILLISECONDS)
        public const int MIN_MAP_UPDATE_DELAY = 50;

        public const int MAX_NUMBER_OF_CELLS     = 16;
        public const float SIZE_OF_GRID_CELL       = (SIZE_OF_GRIDS / MAX_NUMBER_OF_CELLS);

        public const int CENTER_GRID_CELL_ID = (MAX_NUMBER_OF_CELLS * MAX_NUMBER_OF_GRIDS / 2);
        public const float CENTER_GRID_CELL_OFFSET = (SIZE_OF_GRID_CELL / 2);

        public const int TOTAL_NUMBER_OF_CELLS_PER_MAP    = (MAX_NUMBER_OF_GRIDS * MAX_NUMBER_OF_CELLS);

        public const int MAP_RESOLUTION = 128;

        public const float MAP_SIZE = (SIZE_OF_GRIDS * MAX_NUMBER_OF_GRIDS);
        public const float MAP_HALFSIZE           = (MAP_SIZE / 2);

        #endregion

        #region Classes

        public class CoordPair
        {
            #region Declarations

            private int mLimit;
            private int x_coord;
            private int y_coord;

            #endregion

            #region Constructors

            public CoordPair(int x, int y, int limit)
            {
                x_coord = x;
                y_coord = y;
                mLimit = limit;
            }

            public CoordPair(CoordPair pair, int limit)
            {
                x_coord = pair.XCoord;
                y_coord = pair.YCoord;
                mLimit = limit;
            }

            #endregion

            #region Properties

            public int XCoord { get { return x_coord; } }

            public int YCoord { get { return y_coord; } }

            #endregion

            #region Public Methods

            /// <summary>
            /// Replaces the operator assignment overload
            /// </summary>
            /// <param name="c1"></param>
            public void Assign(CoordPair c1)
            {
                x_coord = c1.x_coord;
                y_coord = c1.y_coord;
            }

            public CoordPair normalize()
            {
                x_coord = Math.Min(x_coord, mLimit - 1);
                y_coord = Math.Min(y_coord, mLimit - 1);
                return this;
            }

            public override bool Equals(object obj)
            {
                var c2 = obj as CoordPair;
                if (c2 == null)
                    return false;
                return this == c2;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            #endregion

            #region Operator Overloads

            public static bool operator ==(CoordPair c1, CoordPair c2)
            {
                return c1.x_coord == c2.x_coord && c1.y_coord == c2.y_coord;
            }

            public static bool operator !=(CoordPair c1, CoordPair c2)
            {
                return c1.x_coord != c2.x_coord || c1.y_coord != c2.y_coord;
            }

            public static CoordPair operator <<(CoordPair c1, int val)
            {
                if (c1.x_coord > val)
                    c1.x_coord -= val;
                else
                    c1.x_coord = 0;
                return c1;
            }

            #endregion
        }

        public class GridPair : CoordPair
        {
            #region Constructors

            public GridPair(int x, int y) : base(x, y, MAX_NUMBER_OF_GRIDS)
            {   }

            #endregion
        }

        public class CellPair : CoordPair
        {
            #region Constructors

            public CellPair(int x, int y) : base(x, y, TOTAL_NUMBER_OF_CELLS_PER_MAP)
            { }

            #endregion
        }

        #endregion

        #region Methods

        public static void NormalizeMapCoord(ref float c)
        {
            if (c > MAP_HALFSIZE - 0.5f)
                c = MAP_HALFSIZE - 0.5f;
            else if (c < -(MAP_HALFSIZE - 0.5f))
                c = -(MAP_HALFSIZE - 0.5f);
        }

        public static GridPair ComputeGridPair(float x, float y)
        {
            return ComputeGridPair(x, y, CENTER_GRID_OFFSET, SIZE_OF_GRIDS, CENTER_GRID_ID);
        }

        public static CellPair ComputeCellPair(float x, float y)
        {
            return ComputeCellPair(x, y, CENTER_GRID_CELL_OFFSET, SIZE_OF_GRID_CELL, CENTER_GRID_CELL_ID);
        }

        private static CellPair ComputeCellPair(float x, float y, float center_offset, float size, int center_val)
        {
            // calculate and store temporary values in double format for having same result as same mySQL calculations
            double x_offset = ((double)x - center_offset) / size;
            double y_offset = ((double)y - center_offset) / size;

            int x_val = (int)(x_offset + center_val + 0.5);
            int y_val = (int)(y_offset + center_val + 0.5);
            return new CellPair(x_val, y_val);
        }

        private static GridPair ComputeGridPair(float x, float y, float center_offset, float size, int center_val)
        {
            // calculate and store temporary values in double format for having same result as same mySQL calculations
            double x_offset = ((double)x - center_offset) / size;
            double y_offset = ((double)y - center_offset) / size;

            int x_val = (int)(x_offset + center_val + 0.5);
            int y_val = (int)(y_offset + center_val + 0.5);
            return new GridPair(x_val, y_val);
        }

        #endregion
    }
}
