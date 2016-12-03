using mClient.Clients;
using mClient.World.AI.Activity.Movement;

namespace mClient.World.AI.Activity.Death
{
    public class ReleaseSpirit : BaseActivity
    {
        #region Declarations

        private bool mReclaimed = false;

        #endregion

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

        public override void Process()
        {
            if (!PlayerAI.Player.PlayerObject.IsDead || ExpectationHasElapsed)
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
            else
            {
                if (!mReclaimed)
                {
                    PlayerAI.Client.ReclaimCorpse();
                    mReclaimed = true;
                    // Expect that we are no longer dead within two seconds
                    Expect(() => !PlayerAI.Player.PlayerObject.IsDead);
                }
                // Wait to come back alive
            }
        }

        #endregion
    }
}
