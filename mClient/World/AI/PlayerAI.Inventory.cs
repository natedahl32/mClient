
using FluentBehaviourTree;
using mClient.Clients;
using mClient.Constants;
using mClient.Terrain;
using mClient.World.AI.Activity.BuySell;
using mClient.World.AI.Activity.Item;
using System;
using System.Linq;

namespace mClient.World.AI
{
    public partial class PlayerAI
    {
        private const float MAX_VENDOR_DISTANCE = 10.0f;

        protected IBehaviourTreeNode CreateInventoryAITree()
        {
            // TODO: Need to code in a wait for rez if we are in combat
            var builder = new BehaviourTreeBuilder();
            return builder
                .Selector("inventory-selector")
                    .Sequence("Can Run?")
                        .Inverter("Invert Has Activity")
                            .Do("Has Current Activity?", t =>            // inventory logic doesn't run if we have an activity we are currently doing
                            {
                                if (HasCurrentActivity)
                                    return BehaviourTreeStatus.Failure;
                                return BehaviourTreeStatus.Success;
                            })
                        .End()
                    .End()
                    .Selector("Handle Quest Start Items in Inventory")
                        .Do("Start Quests from Items in Bag", t => StartQuestItemsInInventory())
                    .End()
                    .Selector("Handle Bags in Inventory")
                        .Do("Equip better bag in inventory", t => EquipBag())
                    .End()
                    .Selector("Handle Upgrades in Inventory")
                        .Do("Equip better items in inventory", t => EquipUpgradesInInventory())
                    .End()
                    .Sequence("Sell Items in Inventory")
                        .Do("Has Items for Sale", t => HasForSellItemsInInventory())
                        .Do("Is Near Vendor", t => NearVendor())
                        .Do("Sell Items", t => SellToVendor())
                    .End()
                    .Sequence("Buy Required Items")
                        .Do("Has Required Items", t => HasRequiredItems())
                        .Do("Has Vendor In Range With Items", t => IsVendorInRangeWithRequiredItems())
                    .End()
                 .End()
                 .Build();
        }

        /// <summary>
        /// Does the player have any items that are required but not in inventory
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus HasRequiredItems()
        {
            if (Player.RequiredItemsThatAreNeeded.Count() > 0)
                return BehaviourTreeStatus.Success;
            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Checks for a vendor that is in range that has our required items in inventory
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus IsVendorInRangeWithRequiredItems()
        {
            foreach (var unit in Client.objectMgr.GetAllUnits())
            {
                if (unit.IsVendor && TerrainMgr.CalculateDistance(Player.Position, unit.Position) <= MAX_VENDOR_DISTANCE)
                {
                    // If we don't have this vendors inventory list we will visit them just in case OR they have items that we need
                    if (unit.VendorItemsAvailable == null || unit.VendorItemsAvailable.Any(vi => Player.RequiredItemsThatAreNeeded.Any(ri => ri.ItemId == vi.ItemId)))
                    {
                        StartActivity(new BuyRequiredItemsFromVendor(unit, this));
                        return BehaviourTreeStatus.Success;
                    }
                }
            }

            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Finds any upgrades in inventory and equips them
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus EquipUpgradesInInventory()
        {
            foreach (var invItem in Player.PlayerObject.InventoryItems)
                if (invItem != null && invItem.Item != null && invItem.Item.BaseInfo != null && Player.IsItemAnUpgrade(invItem.Item))
                {
                    // If the item is a dual-spot inventory type, we can't autoequip it or it will always go to the first slot of the inventory type.
                    // In this case we need to equip it to a specific slot
                    if (invItem.Item.BaseInfo.InventoryType == InventoryType.INVTYPE_TRINKET ||
                        invItem.Item.BaseInfo.InventoryType == InventoryType.INVTYPE_FINGER ||
                        (invItem.Item.BaseInfo.InventoryType == InventoryType.INVTYPE_WEAPON && Player.HasSpell(SpellConstants.DUAL_WIELD)))
                    {
                        StartActivity(new EquipItemFromInventory(invItem, this));
                        return BehaviourTreeStatus.Success;
                    }

                    // Otherwise we can auto equip
                    StartActivity(new AutoEquipItemFromInventory(invItem, this));
                    return BehaviourTreeStatus.Success;
                }

            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Starts any quest items that are found in inventory
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus StartQuestItemsInInventory()
        {
            var itemsThatStartQuest = Player.PlayerObject.InventoryItems.Where(i => i.Item.BaseInfo != null && i.Item.BaseInfo.StartsQuestId > 0).ToList();
            foreach (var item in itemsThatStartQuest)
            {
                if (!Player.PlayerObject.Quests.Any(q => q.QuestId == item.Item.BaseInfo.StartsQuestId))
                {
                    // TODO: Start the quest from the item
                    //return BehaviourTreeStatus.Success;
                }
            }

            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Equips a bag that is in inventory
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus EquipBag()
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
                        if (!Player.PlayerObject.AutoEquipItem(itemSlot.Bag, itemSlot.Slot))
                            return BehaviourTreeStatus.Failure;
                        Client.AutoEquipItem((byte)itemSlot.Bag, (byte)itemSlot.Slot);
                        return BehaviourTreeStatus.Success;
                    }

                    // if any of our bags have less slots than this bag then lets equip this bag
                    if (container.NumberOfSlots > Player.PlayerObject.SmallestBag.NumberOfSlots)
                    {
                        // TODO: First we need to make sure the bag we want to equip is not in the bag we are replacing or we will hit an error
                        // TOOD: Equip the bag in place of the smallest bag
                        //return BehaviourTreeStatus.Success;
                    }
                }
            }

            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Checks if there are items we want to sell in our inventory
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus HasForSellItemsInInventory()
        {
            // Simply checks for non-useful items in inventory that have a sell price
            if (Player.PlayerObject.InventoryItems.Any(i => i.Item != null && Player.ShouldSellItem(i.Item)))
                return BehaviourTreeStatus.Success;

            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Checks if we are near a vendor to sell to
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus NearVendor()
        {
            // Check for vendors within distance of us
            foreach (var unit in Client.objectMgr.GetAllUnits())
                if (unit.IsVendor && TerrainMgr.CalculateDistance(Player.Position, unit.Position) <= MAX_VENDOR_DISTANCE)
                    return BehaviourTreeStatus.Success;

            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Sells items to a vendor
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus SellToVendor()
        {
            foreach (var unit in Client.objectMgr.GetAllUnits())
            {
                if (unit.IsVendor && TerrainMgr.CalculateDistance(Player.Position, unit.Position) <= MAX_VENDOR_DISTANCE)
                {
                    StartActivity(new SellItems(unit, this));
                    return BehaviourTreeStatus.Success;
                }
            }

            return BehaviourTreeStatus.Failure;
        }
    }
}
