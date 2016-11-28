using mClient.Clients;
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Train
{
    public class SetHearthstone : BaseActivity
    {
        #region Declarations

        private Clients.Unit mInnkeeper;
        private bool mDone = false;

        #endregion

        #region Constructors

        public SetHearthstone(Clients.Unit innkeeper, PlayerAI ai) : base(ai)
        {
            if (innkeeper == null) throw new ArgumentNullException("innkeeper");
            mInnkeeper = innkeeper;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Setting Hearthstone"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            // If the innkeeper does not have the innkeeper flag, we are done
            if (!mInnkeeper.IsInnkeeper) mDone = true;
        }

        public override void Process()
        {
            // Check if we are done
            if (mDone)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // Are we in range to accept the innkeeper?
            if (PlayerAI.Client.movementMgr.CalculateDistance(mInnkeeper.Position) > MovementMgr.MINIMUM_FOLLOW_DISTANCE)
            {
                PlayerAI.SetFollowTarget(mInnkeeper);
                return;
            }

            // Send bind activator and complete
            PlayerAI.Client.SetHearthstone(mInnkeeper.Guid.GetOldGuid());
            mDone = true;
        }

        #endregion
    }
}
