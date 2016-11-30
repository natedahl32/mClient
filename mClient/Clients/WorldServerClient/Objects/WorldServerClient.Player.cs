using mClient.Constants;
using mClient.Shared;
using mClient.World.Guild;
using mClient.World.Items;
using mClient.World.Quest;
using mClient.World.Skill;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Clients
{
    public class PlayerObj : Unit
    {
        #region Declarations

        private ConcurrentDictionary<int, Container> mInventoryBags = new ConcurrentDictionary<int, Container>();
        private ConcurrentDictionary<int, Item> mInventory = new ConcurrentDictionary<int, Item>();
        private ConcurrentDictionary<EquipmentSlots, Item> mEquippedItems = new ConcurrentDictionary<EquipmentSlots, Item>();

        #endregion

        #region Constructors

        public PlayerObj(WoWGuid guid) : base(guid)
        {
            // Fill equipment slots with nothing
            for (int i = (int)EquipmentSlots.EQUIPMENT_SLOT_START; i < (int)EquipmentSlots.EQUIPMENT_SLOT_END; i++)
                mEquippedItems.TryAdd((EquipmentSlots)i, null);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets all quest ids the player currently has in their Quest Log
        /// </summary>
        public IEnumerable<QuestLogItem> Quests
        {
            get
            {
                var quests = new List<QuestLogItem>();
                for (var i = (int)PlayerFields.PLAYER_QUEST_LOG_1_1; i < (int)PlayerFields.PLAYER_QUEST_LOG_LAST_3; i += QuestConstants.MAX_QUEST_OFFSET)
                {
                    if (GetFieldValue(i) > 0)
                    {
                        var questId = GetFieldValue(i);
                        var counterAndState = GetFieldValue(i + (int)QuestSlotOffsets.QUEST_COUNT_STATE_OFFSET);
                        var time = GetFieldValue(i + (int)QuestSlotOffsets.QUEST_TIME_OFFSET);
                        quests.Add(new QuestLogItem(questId, counterAndState, time));
                    }
                }
                    
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

        /// <summary>
        /// Gets the durability percentage for all equipped items
        /// </summary>
        public float EquipmentDurabilityPercentage
        {
            get
            {
                uint totalMaxDurability = 0;
                uint totalDurability = 0;
                foreach (var item in EquippedItems)
                {
                    if (item.MaxDurability > 0)
                    {
                        totalDurability += item.Durability;
                        totalMaxDurability += item.MaxDurability;
                    }
                }

                if (totalMaxDurability == 0)
                    return 0f;

                return totalDurability / totalMaxDurability;
            }
        }

        /// <summary>
        /// Gets all equipped items on the payer
        /// </summary>
        public IEnumerable<Item> EquippedItems
        {
            get
            {
                var items = new List<Item>();
                for (int i = (int)EquipmentSlots.EQUIPMENT_SLOT_START; i < (int)EquipmentSlots.EQUIPMENT_SLOT_END; i++)
                {
                    var item = GetItemInEquipmentSlot((EquipmentSlots)i);
                    if (item != null)
                        items.Add(item);
                }
                return items;
            }
        }

        /// <summary>
        /// Gets all skill data for this player
        /// </summary>
        public IEnumerable<SkillData> Skills
        {
            get
            {
                var skills = new List<SkillData>();
                for (int i = (int)PlayerFields.PLAYER_SKILL_INFO_1_1; i < (int)PlayerFields.PLAYER_CHARACTER_POINTS1; i += 3)
                {
                    var skill = GetFieldValue(i);
                    var value = GetFieldValue(i + 1);
                    var bonus = GetFieldValue(i + 2);
                    if (skill > 0)
                        skills.Add(new SkillData((SkillType)skill, value, bonus));
                }
                return skills;
            }
        }

        /// <summary>
        /// Gets the amount of money the player has
        /// </summary>
        public uint Money
        {
            get { return GetFieldValue((int)PlayerFields.PLAYER_FIELD_COINAGE); }
        }

        /// <summary>
        /// Gets the amount of gold the player has
        /// </summary>
        public uint Gold
        {
            get { return Convert.ToUInt32(Math.Floor((decimal)Money / (int)MoneyConstants.GOLD)); }
        }

        /// <summary>
        /// Gets the amount of silver the player has
        /// </summary>
        public uint Silver
        {
            get
            {
                var modGold = Money % (int)MoneyConstants.GOLD;
                return Convert.ToUInt32(Math.Floor((decimal)modGold / (int)MoneyConstants.SILVER));
            }
        }

        /// <summary>
        /// Gets the amount of copper the player has
        /// </summary>
        public uint Copper
        {
            get
            {
                var modGold = Money % (int)MoneyConstants.GOLD;
                var modSilver = modGold % (int)MoneyConstants.SILVER;
                return modSilver;
            }
        }

        /// <summary>
        /// Gets a string that display how much money this player has
        /// </summary>
        public string MoneyDisplayString
        {
            get
            {
                var display = string.Empty;
                if (Gold > 0)
                    display += Gold.ToString() + "g ";
                if (Silver > 0 || Gold > 0)
                    display += Silver.ToString() + "s ";
                // Always display copper since it's the least denomination
                display += Copper.ToString() + "c ";
                return display;
            }
        }

        /// <summary>
        /// Get player flags for this player
        /// </summary>
        public PlayerFlags PlayerFlag
        {
            get { return (PlayerFlags)GetFieldValue((int)PlayerFields.PLAYER_FLAGS); }
        }

        /// <summary>
        /// Gets the ID of the guild the player is in
        /// </summary>
        public uint GuildId
        {
            get { return GetFieldValue((int)PlayerFields.PLAYER_GUILDID); }
        }

        /// <summary>
        /// Gets the guild rank the player is in
        /// </summary>
        public uint GuildRank
        {
            get { return GetFieldValue((int)PlayerFields.PLAYER_GUILDRANK); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets whether or not the player has the specified skill
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool HasSkill(SkillType skill)
        {
            var skillData = Skills.Where(s => s.Skill == skill).SingleOrDefault();
            return skillData != null;
        }

        /// <summary>
        /// Gets the current value of the skill for the player
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public uint SkillValue(SkillType skill)
        {
            var skillData = Skills.Where(s => s.Skill == skill).SingleOrDefault();
            if (skillData != null)
                return skillData.TotalValue;
            return 0;
        }

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
        /// Gets items that are currently equipped by inventory type
        /// </summary>
        /// <param name="invType"></param>
        /// <returns></returns>
        public IEnumerable<Item> GetEquippedItemsByInventoryType(InventoryType invType)
        {
            var items = new List<Item>();
            var equipmentSlotsToCheck = new List<EquipmentSlots>();
            switch (invType)
            {
                case InventoryType.INVTYPE_HEAD:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_HEAD);
                    break;
                case InventoryType.INVTYPE_NECK:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_NECK);
                    break;
                case InventoryType.INVTYPE_SHOULDERS:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_SHOULDERS);
                    break;
                case InventoryType.INVTYPE_CHEST:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_CHEST);
                    break;
                case InventoryType.INVTYPE_WAIST:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_WAIST);
                    break;
                case InventoryType.INVTYPE_LEGS:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_LEGS);
                    break;
                case InventoryType.INVTYPE_FEET:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_FEET);
                    break;
                case InventoryType.INVTYPE_WRISTS:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_WRISTS);
                    break;
                case InventoryType.INVTYPE_HANDS:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_HANDS);
                    break;
                case InventoryType.INVTYPE_FINGER:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_FINGER1);
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_FINGER2);
                    break;
                case InventoryType.INVTYPE_TRINKET:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_TRINKET1);
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_TRINKET2);
                    break;
                case InventoryType.INVTYPE_WEAPON:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_MAINHAND);
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_OFFHAND);
                    break;
                case InventoryType.INVTYPE_WEAPONMAINHAND:
                case InventoryType.INVTYPE_2HWEAPON:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_MAINHAND);
                    break;
                case InventoryType.INVTYPE_SHIELD:
                case InventoryType.INVTYPE_WEAPONOFFHAND:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_OFFHAND);
                    break;
                case InventoryType.INVTYPE_RANGED:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_RANGED);
                    break;
                case InventoryType.INVTYPE_CLOAK:
                    equipmentSlotsToCheck.Add(EquipmentSlots.EQUIPMENT_SLOT_BACK);
                    break;
                default:
                    break;
            }

            // Check each equipment slot for an item and return it
            foreach (var slot in equipmentSlotsToCheck)
            {
                var item = GetItemInEquipmentSlot(slot);
                if (item != null)
                    items.Add(item);
            }

            return items;
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
        /// Gets the first inventory item slot that contains the passed item id
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public InventoryItemSlot GetInventoryItem(uint itemId)
        {
            var inventoryItemSlot = InventoryItems.Where(i => i.Item != null && i.Item.BaseInfo != null && i.Item.BaseInfo.ItemId == itemId).FirstOrDefault();
            return inventoryItemSlot;
        }

        /// <summary>
        /// Gets the first inventory item slot that contains the passed item guid
        /// </summary>
        /// <param name="itemGuid"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public InventoryItemSlot GetInventoryItem(WoWGuid itemGuid)
        {
            var inventoryItemSlot = InventoryItems.Where(i => i.Item != null && i.Item.Guid.GetOldGuid() == itemGuid.GetOldGuid()).FirstOrDefault();
            return inventoryItemSlot;
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
                    {
                        var currentItem = mInventory[slot];
                        mInventory.TryUpdate(slot, null, currentItem);
                    }
                    else
                    {
                        var holdingBag = Bags.Where(b => b.Bag == bag).SingleOrDefault();
                        if (holdingBag != null)
                            (holdingBag.Item as Container).ClearSlot(slot);
                    }
                }
                    

                // Set the new bag to it's position
                if (mInventoryBags.ContainsKey(equipToSlot))
                {
                    var currentBag = mInventoryBags[equipToSlot];
                    mInventoryBags.TryUpdate(equipToSlot, newBag, currentBag);
                }
                else
                    mInventoryBags.TryAdd(equipToSlot, newBag);

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
                // Guild
                if (x == (int)PlayerFields.PLAYER_GUILDID)
                {
                    var guild = GuildManager.Instance.Get(value);
                    if (guild == null)
                        client.GuildQuery(value);
                }
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

        /// <summary>
        /// Returns the quest in the player log by quest id. Returns null if the quest is not in the quest log
        /// </summary>
        /// <param name="questId"></param>
        /// <returns></returns>
        public QuestLogItem GetQuestInLog(uint questId)
        {
            byte slot = 0;
            for (var i = (int)PlayerFields.PLAYER_QUEST_LOG_1_1; i < (int)PlayerFields.PLAYER_QUEST_LOG_LAST_3; i += QuestConstants.MAX_QUEST_OFFSET)
            {
                if (GetFieldValue(i) == questId)
                {
                    var id = GetFieldValue(i);
                    var counterState = GetFieldValue(i + (int)QuestSlotOffsets.QUEST_COUNT_STATE_OFFSET);
                    var time = GetFieldValue(i + (int)QuestSlotOffsets.QUEST_TIME_OFFSET);
                    return new QuestLogItem(id, counterState, time);
                }

                slot++;
            }

            return null;
        }

        /// <summary>
        /// Removes an item from inventory
        /// </summary>
        /// <param name="slot">Slot to remove</param>
        public void RemoveInventoryItemSlot(InventoryItemSlot slot)
        {
            // Inventory items
            if (slot.Bag == ItemConstants.INVENTORY_SLOT_BAG_0)
            {
                var currentItem = mInventory[slot.Slot];
                mInventory.TryUpdate(slot.Slot, null, currentItem);
            }
            // Bags
            else
            {
                var bag = mInventoryBags.Where(b => b.Key == slot.Bag).SingleOrDefault();
                if (bag.Value != null)
                    bag.Value.ClearSlot(slot.Slot);
            }
        }

        /// <summary>
        /// Removes an item from inventory
        /// </summary>
        /// <param name="item">Item to remove</param>
        public void RemoveItemFromInventory(Clients.Item item)
        {
            // Get the inventory slot that holds the item
            var invSlot = GetInventoryItem(item.Guid);
            RemoveInventoryItemSlot(invSlot);
        }

        /// <summary>
        /// Checks if we have this item in inventory, equipped, or on our keyring
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool HasItemInInventory(uint itemId)
        {
            // Check inventory items
            if (InventoryItems.Any(i => i.Item != null && i.Item.ObjectFieldEntry == itemId))
                return true;
            // Check equipped items
            if (EquippedItems.Any(i => i != null && i.ObjectFieldEntry == itemId))
                return true;
            return false;
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
                    {
                        var currentContainer = mInventoryBags[equippedSlotValue];
                        mInventoryBags.TryUpdate(equippedSlotValue, container, currentContainer);
                    }
                    else
                    {
                        if (container != null)
                            mInventoryBags.TryAdd(equippedSlotValue, container);
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
                    {
                        var currentItem = mEquippedItems[equippedSlot];
                        mEquippedItems.TryUpdate(equippedSlot, item, currentItem);
                    }
                    else
                    {
                        if (item != null)
                            mEquippedItems.TryAdd(equippedSlot, item);
                    }
                }
            }
            // Inventory items
            else if (slot < (int)PlayerFields.PLAYER_FIELD_BANK_SLOT_1)
            {
                var inventorySlot = ((slot - (int)PlayerFields.PLAYER_FIELD_PACK_SLOT_1) / 2) + (int)InventoryPackSlots.INVENTORY_SLOT_ITEM_START;
                if (mInventory.ContainsKey(inventorySlot))
                {
                    var currentItem = mInventory[inventorySlot];
                    mInventory.TryUpdate(inventorySlot, item, currentItem);
                }
                else
                {
                    if (item != null)
                        mInventory.TryAdd(inventorySlot, item);
                }
            }
            // Bank slots
            else if (slot < (int)PlayerFields.PLAYER_FIELD_BANKBAG_SLOT_1)
            {
                // TODO: Handle bank slots
            }
            // Bank bag slots
            else if (slot < (int)PlayerFields.PLAYER_FIELD_VENDORBUYBACK_SLOT_1)
            {

            }
            // Vendor buyback slot
            else if (slot < (int)PlayerFields.PLAYER_FIELD_KEYRING_SLOT_1)
            {
                // Not sure we really care about this, mabye a TODO?
            }
            // Keyring
            else
            {
                // TODO: Handle keys
            }
        }

        #endregion
    }
}
