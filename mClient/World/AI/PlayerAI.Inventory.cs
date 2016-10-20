﻿
using FluentBehaviourTree;
using mClient.Clients;
using mClient.Constants;

namespace mClient.World.AI
{
    public partial class PlayerAI
    {
        protected IBehaviourTreeNode CreateInventoryAITree()
        {
            // TODO: Need to code in a wait for rez if we are in combat
            var builder = new BehaviourTreeBuilder();
            return builder
                .Selector("inventory-selector")
                    .Selector("Handle Quest Start Items in Inventory")
                        .Do("Start Quests from Items in Bag", t =>
                        {
                            // TODO: Check bags for quest items and start any quests we don't already have
                            return BehaviourTreeStatus.Failure;
                        })
                    .End()
                    .Selector("Handle Bags in Inventory")
                        .Do("Equip better bag in inventory", t =>
                        {
                            var smallestBag = Player.PlayerObject.SmallestBag;

                            // First find any bag that has more slots than the bags we currently have
                            foreach (var itemSlot in Player.PlayerObject.InventoryItems)
                            {
                                var container = itemSlot.Item as Container;
                                if (container != null)
                                {
                                    // if we don't have all bag slots filled, we can autoequip the bag
                                    if (Player.PlayerObject.NumberOfEquippedBags < ItemConstants.MAX_NUMBER_OF_EQUIPPABLE_BAGS)
                                    {
                                        Player.PlayerObject.AutoEquipItem(itemSlot.Bag, itemSlot.Slot);
                                        Client.AutoEquipItem((byte)itemSlot.Bag, (byte)itemSlot.Slot);
                                        return BehaviourTreeStatus.Success;
                                    }
                                        
                                    // if any of our bags have less slots than this bag then lets equip this bag
                                    if (container.NumberOfSlots > Player.PlayerObject.SmallestBag.NumberOfSlots)
                                    {
                                        // TODO: First we need to make sure the bag we want to equip is not in the bag we are replacing or we will hit an error
                                        // TOOD: Equip the bag in place of the smallest bag
                                        return BehaviourTreeStatus.Success;
                                    }
                                }
                            }

                            return BehaviourTreeStatus.Failure;
                        })
                    .End()
                    .Selector("Handle Upgrades in Inventory")
                        .Do("Equip better items in inventory", t =>
                        {
                            // TOOD: Find upgrades and equip them
                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                 .End()
                 .Build();
        }
    }
}
