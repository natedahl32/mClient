using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Death
{
    public class Repop : BaseActivity
    {
        #region Declaration

        private bool mRepopped = false;

        #endregion

        #region Constructors

        public Repop(PlayerAI ai) : base(ai)
        {
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Repopping After Death"; }
        }

        #endregion

        #region Public Methods

        public override void Process()
        {
            // If our expectation elapses send the repop request again
            if (ExpectationHasElapsed) mRepopped = false;

            // If we are a ghost that means we have repopped
            if (PlayerAI.Player.PlayerObject.PlayerFlag.HasFlag(PlayerFlags.PLAYER_FLAGS_GHOST))
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // Otherwise send a repop request to the server
            if (!mRepopped)
            {
                PlayerAI.Client.Repop();
                mRepopped = true;
                Expect(() => PlayerAI.Player.PlayerObject.PlayerFlag.HasFlag(PlayerFlags.PLAYER_FLAGS_GHOST), 5000);
            }
        }

        #endregion
    }
}
