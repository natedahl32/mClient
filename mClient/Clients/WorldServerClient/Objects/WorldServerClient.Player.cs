using mClient.Constants;
using mClient.Shared;
using mClient.World.Items;
using mClient.World.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Clients
{
    public class PlayerObj : Unit
    {
        #region Declarations

        private Dictionary<int, Container> mInventoryBags = new Dictionary<int, Container>();
        private Dictionary<int, Item> mInventory = new Dictionary<int, Item>();
        private Dictionary<EquipmentSlots, Item> mEquippedItems = new Dictionary<EquipmentSlots, Item>();

        #endregion

        #region Constructors

        public PlayerObj(WoWGuid guid) : base(guid)
        {
            // Fill equipment slots with nothing
            for (int i = (int)EquipmentSlots.EQUIPMENT_SLOT_START; i < (int)EquipmentSlots.EQUIPMENT_SLOT_END; i++)
                mEquippedItems.Add((EquipmentSlots)i, null);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets all quest ids the player currently has in their Quest Log
        /// </summary>
        public IEnumerable<UInt32> Quests
        {
            get
            {
                var quests = new List<UInt32>();
                for (var i = (int)PlayerFields.PLAYER_QUEST_LOG_1_1; i < (int)PlayerFields.PLAYER_QUEST_LOG_LAST_3; i += QuestConstants.MAX_QUEST_OFFSET)
                    if (GetFieldValue(i) > 0)
                        quests.Add(GetFieldValue(i));
                return quests;
            }
        }

        /// <summary>
        /// Gets whether or not the players quest log is full
        /// </summary>
        public bool IsQuestLogFull
        {
            get { return Quests.Count() >= QuestConstants.MAX_QUEST_LOG_SIZE; }
        }

        /// <summary>
        /// Gets all items in the inventory of the player
        /// </summary>
        public IEnumerable<InventoryItemSlot> InventoryItems
        {
            get
            {
                // All items in pack and all items in each bag we have
                var packItems = mInventory.Where(kvp => kvp.Value != null).Select(kvp => new InventoryItemSlot() { Bag = ItemConstants.INVENTORY_SLOT_BAG_0, Slot = kvp.Key, Item = kvp.Value }).ToList();
                foreach (var bag in mInventoryBags.Values.Where(b => b != null))
                    packItems.AddRange(bag.ItemsInContainer);
                return packItems;
            }
        }

        /// <summary>
        /// Gets all bags the player currently has equipped
        /// </summary>
        public IEnumerable<InventoryItemSlot> Bags
        {
            get { return mInventoryBags.Where(b => b.Value != null).Select(b => new InventoryItemSlot() { Slot = b.Key, Item = b.Value }); }
        }

        /// <summary>
        /// Gets the bag with the smallest number of slots
        /// </summary>
        public Container SmallestBag
        {
            get
            {
                // If we don't have any equipped bags return null
                if (mInventoryBags.Count == 0) return null;

                var minimumSlots = mInventoryBags.Where(b => b.Value != null).Select(b => b.Value.NumberOfSlots).Min();
                var bag = mInventoryBags.Where(b => b.Value != null && b.Value.NumberOfSlots == minimumSlots).FirstOrDefault();
                if (!bag.Equals(new KeyValuePair<int, Container>()))
                    return bag.Value;
                return null;
            }
        }

        /// <summary>
        /// Gets the total number of bags currently equipped
        /// </summary>
        public int NumberOfEquippedBags
        {
            get { return mInventoryBags.Where(b => b.Value != null).Count(); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the item currently equipped in the given slot
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public Item GetItemInEquipmentSlot(EquipmentSlots slot)
        {
            return mEquippedItems[slot];
        }

        /// <summary>
        /// Gets the inventory item in the slot
        /// </summary>
        /// <param name="bag"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public InventoryItemSlot GetInventoryItem(int bag, int slot)
        {
            InventoryItemSlot item = null;
            if (bag == ItemConstants.INVENTORY_SLOT_BAG_0)
                item = InventoryItems.Where(i => i.Slot == slot).SingleOrDefault();
            else
            {
                // Get the bag
                var inBag = Bags.Where(b => b.Slot == bag).SingleOrDefault();
                if (inBag == null)
                    return null;
                // Make sure it's a container
                var container = inBag.Item as Container;
                if (container == null)
                    return null;
                item = container.ItemsInContainer.Where(i => i.Slot == slot).SingleOrDefault();
            }

            return item;
        }

        /// <summary>
        /// Equips the item in the bag and slot
        /// </summary>
        /// <param name="bag"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public bool AutoEquipItem(int bag, int slot)
        {
            // Retrieve the item we are equipping
            InventoryItemSlot item = GetInventoryItem(bag, slot);

            // Make sure we have an item
            if (item == null || item.Item == null || item.Item.BaseInfo == null) return false;

            // Get the slot we want to equip this item in
            int equipToSlot = -1;
            if (item.Item.BaseInfo.InventoryType == InventoryType.INVTYPE_BAG)
            {
                // If we have the maximum number of bags, choose the slot with lowest number of slots in it
                if (NumberOfEquippedBags == ItemConstants.MAX_NUMBER_OF_EQUIPPABLE_BAGS)
                {
                    var minSlots = Bags.Select(b => (b.Item as Container).NumberOfSlots).Min();
                    equipToSlot = Bags.Where(b => (b.Item as Container).NumberOfSlots == minSlots).FirstOrDefault().Slot;
                }
                else
                {
                    // Find the first bag slot that is open
                    for (int s = (int)InventorySlots.INVENTORY_SLOT_BAG_START; s <= (int)InventorySlots.INVENTORY_SLOT_BAG_END; s++)
                        if (!Bags.Any(b => b.Slot == s))
                        {
                            equipToSlot = s;
                            break;
                        }
                }
            }
            else if (item.Item.BaseInfo.InventoryType == InventoryType.INVTYPE_FINGER)
            {
                if (GetItemInEquipmentSlot(EquipmentSlots.EQUIPMENT_SLOT_FINGER1) == null)
                    equipToSlot = (int)EquipmentSlots.EQUIPMENT_SLOT_FINGER1;
                else if (GetItemInEquipmentSlot(EquipmentSlots.EQUIPMENT_SLOT_FINGER2) == null)
                    equipToSlot = (int)EquipmentSlots.EQUIPMENT_SLOT_FINGER2;
                else
                    equipToSlot = (int)EquipmentSlots.EQUIPMENT_SLOT_FINGER1;
            }
            else if (item.Item.BaseInfo.InventoryType == InventoryType.INVTYPE_TRINKET)
            {
                if (GetItemInEquipmentSlot(EquipmentSlots.EQUIPMENT_SLOT_TRINKET1) == null)
                    equipToSlot = (int)EquipmentSlots.EQUIPMENT_SLOT_TRINKET1;
                else if (GetItemInEquipmentSlot(EquipmentSlots.EQUIPMENT_SLOT_TRINKET2) == null)
                    equipToSlot = (int)EquipmentSlots.EQUIPMENT_SLOT_TRINKET2;
                else
                    equipToSlot = (int)EquipmentSlots.EQUIPMENT_SLOT_TRINKET1;
            }
            else
                equipToSlot = (int)item.Item.GetEquipSlotByInventoryType();

            // If we couldn't find a slot to equip to, exit out
            if (equipToSlot == -1) return false;

            // Equip the item  
            return EquipItem(bag, slot, equipToSlot);
        }

        /// <summary>
        /// Equips the item that is currently in the bag slot to the equipToSlot
        /// </summary>
        /// <param name="bag"></param>
        /// <param name="slot"></param>
        /// <param name="equipToSlot"></param>
        /// <returns></returns>
        public bool EquipItem(int bag, int slot, int equipToSlot)
        {
            // Get the item we are equippping
            var item = GetInventoryItem(bag, slot);
            if (item == null || item.Item == null) return false;

            Item currentlyEquipped = null;
            // Handle bags differently
            if (item.Item.BaseInfo.InventoryType == InventoryType.INVTYPE_BAG)
            {
                // Get the currently equipped bag (if any)
                if (Bags.Any(b => b.Slot == equipToSlot))
                    currentlyEquipped = Bags.Where(b => b.Slot == equipToSlot).SingleOrDefault().Item;

                // Hold a reference to the new bag
                var newBag = item.Item as Container;

                // Send the currently equipped bag back to inventory
                if (currentlyEquipped != null)
                    item.Item = currentlyEquipped;
                else
                {
                    // Clear out the item from inventory since it will be equipped
                    if (bag == ItemConstants.INVENTORY_SLOT_BAG_0)
                        mInventory[slot] = null;
                    else
                    {
                        var holdingBag = Bags.Where(b => b.Bag == bag).SingleOrDefault();
                        if (holdingBag != null)
                            (holdingBag.Item as Container).ClearSlot(slot);
                    }
                }
                    

                // Set the new bag to it's position
                if (mInventoryBags.ContainsKey(equipToSlot))
                    mInventoryBags[equipToSlot] = newBag;
                else
                    mInventoryBags.Add(equipToSlot, newBag);

                return true;
            }
            
            // Non bag item    
            currentlyEquipped = GetItemInEquipmentSlot((EquipmentSlots)equipToSlot);

            return true;
        }


        /// <summary>
        /// Overrides the base SetField to handle some player specific values that get set
        /// </summary>
        /// <param name="client"></param>
        /// <param name="x"></param>
        /// <param name="value"></param>
        public override void SetField(WorldServerClient client, int x, uint value)
        {
            // Can be null when sent from base class
            if (client != null)
            {
                // Quests
                if (x >= (int)PlayerFields.PLAYER_QUEST_LOG_1_1 && x <= (int)PlayerFields.PLAYER_QUEST_LOG_LAST_3)
                {
                    // If we are updating a quest, make sure our quest manager has the quest
                    for (var i = (int)PlayerFields.PLAYER_QUEST_LOG_1_1; i < (int)PlayerFields.PLAYER_QUEST_LOG_LAST_3; i += QuestConstants.MAX_QUEST_OFFSET)
                        if (x == i)
                            if (QuestManager.Instance.Get(value) == null)
                                client.QueryQuest(value);
                }
                // Items in inventory and bag slots
                else if (x >= (int)PlayerFields.PLAYER_FIELD_INV_SLOT_HEAD && x <= (int)PlayerFields.PLAYER_FIELD_KEYRING_SLOT_LAST)
                {
                    for (var i = (int)PlayerFields.PLAYER_FIELD_INV_SLOT_HEAD + 1; i < (int)PlayerFields.PLAYER_FIELD_KEYRING_SLOT_LAST; i += 2)
                    {
                        if (x == i)
                        {
                            var guid = GetGuid(GetFieldValue(x - 1), value);
                            var itemObj = client.objectMgr.getObject(new WoWGuid(guid));
                            if (itemObj != null)
                            {
                                // Add the item object to the inventory
                                AddOrUpdateInventoryItem(client, i - 1, itemObj as Item);

                                // Get the prototype for this item if we don't have it yet
                                if (ItemManager.Instance.Get(itemObj.ObjectFieldEntry) == null)
                                    client.QueryItemPrototype(itemObj.ObjectFieldEntry);
                            }
                            else
                                AddOrUpdateInventoryItem(client, i - 1, null);
                        }
                    }
                }
            }
            

            // Call base to actually perform the set
            base.SetField(client, x, value);
        }

        /// <summary>
        /// Adds a quest to the player
        /// </summary>
        /// <param name="questId"></param>
        public void AddQuest(UInt32 questId)
        {
            for (var i = (int)PlayerFields.PLAYER_QUEST_LOG_1_1; i < (int)PlayerFields.PLAYER_QUEST_LOG_LAST_3; i += QuestConstants.MAX_QUEST_OFFSET)
            {
                if (GetFieldValue(i) == 0)
                {
                    SetField(i, questId);
                    return;
                }
            }
        }

        /// <summary>
        /// Drops a quest based on the id
        /// </summary>
        /// <param name="questId"></param>
        public void DropQuest(UInt32 questId)
        {
            for (var i = (int)PlayerFields.PLAYER_QUEST_LOG_1_1; i < (int)PlayerFields.PLAYER_QUEST_LOG_LAST_3; i += QuestConstants.MAX_QUEST_OFFSET)
            {
                if (GetFieldValue(i) == questId)
                {
                    SetField(i, 0);
                    SetField(i + 1, 0);
                    SetField(i + 2, 0);
                    return;
                }
            }
        }

        /// <summary>
        /// Gets the slot the quest is in the log
        /// </summary>
        /// <param name="questId"></param>
        /// <returns></returns>
        public byte GetQuestSlot(UInt32 questId)
        {
            byte slot = 0;
            for (var i = (int)PlayerFields.PLAYER_QUEST_LOG_1_1; i < (int)PlayerFields.PLAYER_QUEST_LOG_LAST_3; i += QuestConstants.MAX_QUEST_OFFSET)
            {
                if (GetFieldValue(i) == questId)
                    return slot;
                slot++;
            }

            return QuestConstants.MAX_QUEST_LOG_SIZE;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds or updates an inventory item slot
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="item"></param>
        private void AddOrUpdateInventoryItem(WorldServerClient client, int slot, Item item)
        {
            // Equipped items
            if (slot < (int)PlayerFields.PLAYER_FIELD_PACK_SLOT_1)
            {
                var equippedSlotValue = (slot - (int)PlayerFields.PLAYER_FIELD_INV_SLOT_HEAD) / 2;
                if (equippedSlotValue >= (int)InventorySlots.INVENTORY_SLOT_BAG_START && equippedSlotValue <= (int)InventorySlots.INVENTORY_SLOT_BAG_END)
                {
                    // This is a bag
                    var container = item as Container;
                    if (mInventoryBags.ContainsKey(equippedSlotValue))
                        mInventoryBags[equippedSlotValue] = container;
                    else
                    {
                        if (container != null)
                            mInventoryBags.Add(equippedSlotValue, container);
                    }

                    // Update the items in the container
                    if (container != null)
                    {
                        container.InventoryBagSlot = equippedSlotValue;
                        container.UpdateItemsInContainer(client);
                    }
                }
                else
                {
                    // This is an equipped item
                    var equippedSlot = (EquipmentSlots)equippedSlotValue;
                    if (mEquippedItems.ContainsKey(equippedSlot))
                        mEquippedItems[equippedSlot] = item;
                    else
                    {
                        if (item != null)
                            mEquippedItems.Add(equippedSlot, item);
                    }
                }
            }
            // Inventory items
            else if (slot < (int)PlayerFields.PLAYER_FIELD_BANK_SLOT_1)
            {
                var inventorySlot = (slot - (int)PlayerFields.PLAYER_FIELD_PACK_SLOT_1) / 2;
                if (mInventory.ContainsKey(inventorySlot))
                    mInventory[inventorySlot + (int)InventoryPackSlots.INVENTORY_SLOT_ITEM_START] = item;
                else
                {
                    if (item != null)
                        mInventory.Add(inventorySlot + (int)InventoryPackSlots.INVENTORY_SLOT_ITEM_START, item);
                }
            }
            // TOOD: Need to add bank bags, bank slots, and keyring to this as well
        }

        #endregion
    }
}
