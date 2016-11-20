using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Combat
{
    public class ChangeTarget : BaseActivity
    {
        #region Declarations

        private Clients.Unit mNewTarget;

        #endregion

        #region Constructors

        public ChangeTarget(Clients.Unit newTarget, PlayerAI ai) : base(ai)
        {
            if (newTarget == null) throw new ArgumentNullException("newTarget");
            mNewTarget = newTarget;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Changing Target"; }
        }

        #endregion

        #region Public Methods

        public override void Process()
        {
            // If the target is now set
            if (PlayerAI.TargetSelection != null && PlayerAI.TargetSelection.Guid.GetOldGuid() == mNewTarget.Guid.GetOldGuid())
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // If the target is now dead, don't change to it (we get stuck if this activity gets paused and when we come back the target is dead)
            if (mNewTarget.IsDead)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // Change targets
            PlayerAI.SetTargetSelection(mNewTarget);
        }

        #endregion
    }
}
