using mClient.Clients;
using mClient.World.AI.Activity.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Death
{
    public class ReleaseSpirit : BaseActivity
    {
        #region Constructors

        public ReleaseSpirit(PlayerAI ai) : base(ai)
        {
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Release Spirit"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();
            PlayerAI.Client.ReclaimCorpse();
        }

        public override void Process()
        {
            if (!PlayerAI.Player.PlayerObject.IsDead)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // If we are not within follow distance of our corpse than teleport to it
            if (PlayerAI.Client.movementMgr.CalculateDistance(PlayerAI.Player.PlayerCorpse.Position) >= MovementMgr.MINIMUM_FOLLOW_DISTANCE)
            {
                PlayerAI.StartActivity(new TeleportToCoordinate(PlayerAI.Player.MapID, PlayerAI.Player.PlayerCorpse.Position, PlayerAI));
                return;
            }
        }

        #endregion
    }
}
