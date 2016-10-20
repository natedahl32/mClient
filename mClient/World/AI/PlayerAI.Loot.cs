
using FluentBehaviourTree;
using mClient.Clients;
using System;
using System.Linq;

namespace mClient.World.AI
{
    public partial class PlayerAI
    {
        #region Declarations

        // Flag that determines whether or not we are looting, so we don't start doing something else
        private bool mIsLooting = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether or not the player is currently looting an object
        /// </summary>
        public bool IsLooting { get { return mIsLooting; } set { mIsLooting = value; } }

        #endregion

        protected IBehaviourTreeNode CreateLootAITree()
        {
            // TODO: Need to code in a wait for rez if we are in combat
            var builder = new BehaviourTreeBuilder();
            return builder
                .Sequence("loot-sequence")
                    .Do("Do we have loot?", t =>
                    {
                        if (Player.Lootable.Any())
                            return BehaviourTreeStatus.Success;
                        return BehaviourTreeStatus.Failure;
                    })
                    .Do("Am I near the lootable object?", t =>
                    {
                        // Get the first lootable in the list
                        var lootable = Player.Lootable.FirstOrDefault();
                        if (lootable == null) return BehaviourTreeStatus.Failure;

                        // Make sure we have an object for it
                        var obj = Client.objectMgr.getObject(lootable);
                        if (obj == null) return BehaviourTreeStatus.Failure;

                        // Are we close enough to our lootable
                        var distance = Client.movementMgr.CalculateDistance(obj.Position);
                        if (distance > MovementMgr.MINIMUM_FOLLOW_DISTANCE)
                        {
                            // Set our follow target and then exit out as running
                            SetFollowTarget(obj);
                            return BehaviourTreeStatus.Running;
                        }

                        // We are close enough
                        return BehaviourTreeStatus.Success;
                    })
                    .Do("Give me my loot!!", t =>
                    {
                        // If we are already looting
                        if (IsLooting) return BehaviourTreeStatus.Success;

                        // Get the first loot obj in the list
                        var guid = Player.Lootable.FirstOrDefault();

                        // Set our flag
                        IsLooting = true;
                        Client.Loot(guid);
                        Player.RemoveLootable(guid);

                        return BehaviourTreeStatus.Success;
                    })
                 .End()
                 .Build();
        }
    }
}
