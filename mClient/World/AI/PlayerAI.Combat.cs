using System.Linq;
using FluentBehaviourTree;
using mClient.Clients;
using mClient.World.AI.Activity.Combat;

namespace mClient.World.AI
{
    public partial class PlayerAI
    {
        protected IBehaviourTreeNode CreateCombatAITree()
        {
            var builder = new BehaviourTreeBuilder();
            return builder
                .Sequence("combat-sequece")
                    .Do("Activity That Halts Combat?", t =>
                    {
                        if (CurrentActivity != null && CurrentActivity.HaltsCombat)
                            return BehaviourTreeStatus.Failure;
                        return BehaviourTreeStatus.Success;
                    })
                    .Do("Is In Combat?", t => IsInCombat())
                    .Do("Select Target", t => SelectTarget())
                    .Do("Is In Melee?", t => MoveToMelee())
                    .Do("Attack!!!", t => Attack())
                 .End()
                 .Build();
        }

        /// <summary>
        /// Determines whether or not the player is in combat
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus IsInCombat()
        {
            if (Player.IsInCombat)
                return BehaviourTreeStatus.Success;
            else
            {
                mTargetSelection = null;
                mIsAttackingTarget = false;
            }
            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Determines our target
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus SelectTarget()
        {
            // TODO: Select a target more intelligently
            // Set our target
            if (TargetSelection != null && TargetSelection.IsDead)
            {
                // Remove enemy and clear target
                Player.RemoveEnemy(TargetSelection.Guid.GetOldGuid());
                SetTargetSelection(null);
                // We may no longer be in combat
                if (!Player.IsInCombat)
                    return BehaviourTreeStatus.Failure;
            }
                
            if (TargetSelection == null)
                SetTargetSelection(Client.objectMgr.getObject(Player.EnemyList.FirstOrDefault()));
            return BehaviourTreeStatus.Success;
        }

        /// <summary>
        /// Makes sure we are in melee range if we are a melee combatant
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus MoveToMelee()
        {
            // If the player is not a melee player return success
            if (!Player.ClassLogic.IsMelee)
                return BehaviourTreeStatus.Success;

            // We are melee, if we are not in melee range then we need to set our follow target
            if (NotInMeleeRange)
            {
                Client.movementMgr.FollowTarget = mTargetSelection;
                return BehaviourTreeStatus.Running;
            }

            return BehaviourTreeStatus.Success;
        }

        /// <summary>
        /// Attacks the current target
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus Attack()
        {
            // If we are no longer in combat, fail out
            if (!Player.IsInCombat) return BehaviourTreeStatus.Failure;

            // If we don't have a target selection than fail
            if (mTargetSelection == null) return BehaviourTreeStatus.Failure;

            // If we are not attacking and we have a target selection then start doing some damage
            if (!mIsAttackingTarget)
            {
                // Start attacking, this applies
                Client.Attack(mTargetSelection.Guid.GetOldGuid());
                mIsAttackingTarget = true;

                // Start the do damage activity
                StartActivity(new DoDamage(this));
            }

            return BehaviourTreeStatus.Running;
        }
    }
}
