using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Combat
{
    public class DoDamage : BaseActivity
    {
        #region Constructors

        public DoDamage(PlayerAI ai) : base(ai)
        {
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Do Damage!!"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();
        }

        public override void Process()
        {
            // Complete the activity if:
            if (PlayerAI.TargetSelection == null)
                PlayerAI.CompleteActivity();
            if (!PlayerAI.Player.IsInCombat)
                PlayerAI.CompleteActivity();

            // TOOD: Determine what abilities/spells to use here for damage purposes. Basically our rotation.
        }

        #endregion
    }
}
