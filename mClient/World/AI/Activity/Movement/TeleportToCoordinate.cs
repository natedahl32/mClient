using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Movement
{
    public class TeleportToCoordinate : BaseActivity
    {
        #region Declarations

        private UInt32 mMapId;
        private Coordinate mTeleportTo;

        #endregion

        #region Constructors

        public TeleportToCoordinate(UInt32 mapId, Coordinate coordinate, PlayerAI ai) : base(ai)
        {
            mMapId = mapId;
            mTeleportTo = coordinate;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Teleport To Coordinate"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();
            PlayerAI.Client.Teleport(mMapId, mTeleportTo);
        }

        public override void Process()
        {
            // if we are at the spot we teleported to we are done
            if (PlayerAI.Player.Position == mTeleportTo)
            {
                PlayerAI.CompleteActivity();
                return;
            }
        }

        #endregion
    }
}
