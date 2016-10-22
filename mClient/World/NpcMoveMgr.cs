using mClient.Clients;
using mClient.Constants;
using mClient.Shared;
using mClient.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World
{
    public class NpcMoveMgr
    {
        #region Declarations

        [DllImport("winmm.dll", EntryPoint = "timeGetTime")]
        public static extern uint MM_GetTime();

        public MovementFlag Flag = new MovementFlag();

        Unit mUnit;
        Coordinate oldLocation;
        UInt32 lastUpdateTime;
        TerrainMgr terrainMgr;

        #endregion

        #region Constructors

        public NpcMoveMgr(Unit unit, TerrainMgr terrainManager)
        {
            mUnit = unit;
            this.terrainMgr = terrainManager;
            lastUpdateTime = MM_GetTime();
        }

        #endregion


        #region Public Methods

        public void UpdatePosition()
        {
            double h; double speed;
            uint time = MM_GetTime();
            UInt32 diff = (time - lastUpdateTime);
            lastUpdateTime = time;

            if (Flag.IsMoveFlagSet(MovementFlags.MOVEMENTFLAG_FORWARD))
            {
                speed = 7.0;
            }
            else
                return;

            float predictedDX = 0;
            float predictedDY = 0;

            if (oldLocation == null)
                oldLocation = mUnit.Position;


            h = mUnit.Position.O;

            float dt = (float)diff / 1000f;
            float dx = (float)Math.Cos(h) * (float)speed * dt;
            float dy = (float)Math.Sin(h) * (float)speed * dt;

            predictedDX = dx;
            predictedDY = dy;

            Coordinate loc = mUnit.Position;
            float realDX = loc.X - oldLocation.X;
            float realDY = loc.Y - oldLocation.Y;

            float predictDist = (float)Math.Sqrt(predictedDX * predictedDX + predictedDY * predictedDY);
            float realDist = (float)Math.Sqrt(realDX * realDX + realDY * realDY);

            if (predictDist > 0.0)
            {

                Coordinate expected = new Coordinate(loc.X + predictedDX, loc.Y + predictedDY, mUnit.Position.Z, mUnit.Position.O);
                //expected = terrainMgr.getZ(expected);
                mUnit.Position = expected;

            }

            oldLocation = loc;
        }

        #endregion
    }
}
