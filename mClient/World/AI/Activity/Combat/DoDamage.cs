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
            // If our target is dead, remove it from the enemy list. If we are dead do the same.
            if ((PlayerAI.TargetSelection != null && PlayerAI.TargetSelection.IsDead) || PlayerAI.Player.PlayerObject.IsDead)
            {
                PlayerAI.SetTargetSelection(null);
                PlayerAI.CompleteActivity();
                return;
            }

            // Complete the activity if:
            if (PlayerAI.TargetSelection == null)
            {
                PlayerAI.CompleteActivity();
                return;
            }
            if (!PlayerAI.Player.IsInCombat)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // Determine what abilities/spells to use here for damage purposes. Basically our rotation.
            var spell = PlayerAI.Player.ClassLogic.NextSpellInRotation;
            if (spell != null)
            {
                PlayerAI.StartActivity(new CastSingleTargetSpell(spell, PlayerAI));
                return;
            }

            // If we are a melee class then set the target as our follow target
            if (PlayerAI.Player.ClassLogic.IsMelee)
            {
                PlayerAI.SetFollowTarget(PlayerAI.TargetSelection);
                if (!PlayerAI.IsAttackingTargetSelection)
                    PlayerAI.Client.Attack(PlayerAI.TargetSelection);
            }
                
        }

        #endregion
    }
}
