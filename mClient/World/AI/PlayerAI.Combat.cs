using System.Linq;
using FluentBehaviourTree;
using mClient.Clients;

namespace mClient.World.AI
{
    public partial class PlayerAI
    {
        protected IBehaviourTreeNode CreateCombatAITree()
        {
            var builder = new BehaviourTreeBuilder();
            return builder
                .Sequence("combat-sequece")
                    .Do("Is In Combat?", t =>
                    {
                        if (Player.IsInCombat)
                            return BehaviourTreeStatus.Success;
                        return BehaviourTreeStatus.Failure;
                    })
                    .Do("Is In Melee?", t =>
                    {
                        // If the player is not a melee player return success
                        if (!Player.IsMelee)
                            return BehaviourTreeStatus.Success;

                        // TODO: Select a target more intelligently
                        // Set our target
                        SetTargetSelection(Client.objectMgr.getObject(Player.EnemyList.FirstOrDefault()));

                        // We are melee, if we are not in melee range then we need to set our follow target
                        if (NotInMeleeRange)
                        {
                            // If the enemy is moving don't follow them yet, clear the follow target and 
                            // wait for them to come to us
                            //if ((mTargetSelection as Unit).IsMoving)
                            //{
                            //    Client.movementMgr.FollowTarget = null;
                            //    return BehaviourTreeStatus.Running;
                            //}
                                
                            Client.movementMgr.FollowTarget = mTargetSelection;
                            return BehaviourTreeStatus.Running;
                        }

                        return BehaviourTreeStatus.Success;
                    })
                    .Do("Attack!!!", t =>
                    {
                        // If we are no longer in combat, fail out
                        if (!Player.IsInCombat) return BehaviourTreeStatus.Failure;

                        // TOOD: Spell Casters should cast a spell here. BUT first we need to check range on the target
                        if (!mIsAttackingTarget && mTargetSelection != null)
                        {
                            Client.Attack(mTargetSelection.Guid.GetOldGuid());
                            mIsAttackingTarget = true;
                        }

                        return BehaviourTreeStatus.Running;
                    })
                 .End()
                 .Build();
        }
    }
}
