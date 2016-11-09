
using FluentBehaviourTree;
using mClient.Clients;
using mClient.World.AI.Activity.Loot;
using System;
using System.Linq;

namespace mClient.World.AI
{
    public partial class PlayerAI
    {
        protected IBehaviourTreeNode CreateLootAITree()
        {
            // TODO: Need to code in a wait for rez if we are in combat
            var builder = new BehaviourTreeBuilder();
            return builder
                .Sequence("loot-sequence")
                    .Do("Has Current Activity?", t =>            // loot logic doesn't run if we have an activity we are currently doing
                    {
                        if (HasCurrentActivity)
                            return BehaviourTreeStatus.Failure;
                        return BehaviourTreeStatus.Success;
                    })
                    .Do("Do we have loot?", t => CheckForLootable())
                    .Do("Do we have an object for a lootable?", t => CheckForLootableObjects())
                    .Do("Loot", t => LootObject())
                 .End()
                 .Build();
        }

        /// <summary>
        /// Checks that the player has a lootable guid
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus CheckForLootable()
        {
            if (Player.Lootable.Any())
                return BehaviourTreeStatus.Success;
            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Checks that we have a lootable object we can actually loot
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus CheckForLootableObjects()
        {
            var exists = false;
            foreach (var lootable in Player.Lootable)
            {
                var obj = Client.objectMgr.getObject(lootable);
                if (obj != null)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
                return BehaviourTreeStatus.Failure;
            return BehaviourTreeStatus.Success;
        }

        /// <summary>
        /// Loots the first object in the list
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus LootObject()
        {
            foreach (var lootable in Player.Lootable)
            {
                var obj = Client.objectMgr.getObject(lootable);
                if (obj != null)
                {
                    StartActivity(new LootObject(obj, this));
                    return BehaviourTreeStatus.Success;
                }
            }

            return BehaviourTreeStatus.Failure;
        }
    }
}
