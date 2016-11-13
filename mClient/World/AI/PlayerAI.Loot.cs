
using FluentBehaviourTree;
using mClient.Clients;
using mClient.Terrain;
using mClient.World.AI.Activity.Loot;
using System;
using System.Linq;

namespace mClient.World.AI
{
    public partial class PlayerAI
    {
        private const float MAX_DISTANCE_FOR_CHEST = 10.0f;

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
                    .Selector("Choose Loot Type")
                        .Sequence("Lootable Corpses")
                            .Do("Do we have loot?", t => CheckForLootable())
                            .Do("Do we have an object for a lootable?", t => CheckForLootableObjects())
                            .Do("Loot", t => LootObject())
                        .End()
                        .Sequence("Find Quest Objects or Chests Near Us")
                            .Do("Has Gameobjects To Loot", t => LootableChests())
                            .Do("Loot Close Chest", t => LootCloseChest())
                        .End()
                    .End()
                 .End()
                 .Build();
        }

        /// <summary>
        /// Checks for a quest object that is near us
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus LootableChests()
        {
            // Are there any game objects near us that we can loot?
            var gameObjects = Client.objectMgr.getObjectArray().Where(o => o != null && o.Type == Constants.ObjectType.GameObject).Cast<Clients.GameObject>().ToList();
            if (gameObjects.Any(o => o.BaseInfo.GameObjectType == Constants.GameObjectType.Chest && !o.HasBeenLooted && o.CanInteract))
                return BehaviourTreeStatus.Success;

            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Finds a chest gameobject that is close and tries to loot it
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus LootCloseChest()
        {
            // Get game objects
            var gameObjects = Client.objectMgr.getObjectArray().Where(o => o != null && o.Type == Constants.ObjectType.GameObject).Cast<Clients.GameObject>().ToList();
            // Check for chests that we haven't looted yet and can interact with
            foreach (var chest in gameObjects.Where(o => o.BaseInfo.GameObjectType == Constants.GameObjectType.Chest && !o.HasBeenLooted && o.CanInteract))
            {
                // If the chest has not been looted yet and it is within distance for us to loot
                var chestGO = chest as Clients.GameObject;
                if (TerrainMgr.CalculateDistance(Player.Position, chest.Position) <= MAX_DISTANCE_FOR_CHEST)
                {
                    // Start a new activity
                    StartActivity(new LootGameObject(chest, this));
                    return BehaviourTreeStatus.Success;
                }
            }

            return BehaviourTreeStatus.Failure;
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
                    // Don't  loot if this is a unit that has been looted
                    var unitGO = obj as Unit;
                    if (unitGO != null && unitGO.HasBeenLooted)
                        continue;

                    StartActivity(new LootObject(obj, this));
                    return BehaviourTreeStatus.Success;
                }
            }

            return BehaviourTreeStatus.Failure;
        }
    }
}
