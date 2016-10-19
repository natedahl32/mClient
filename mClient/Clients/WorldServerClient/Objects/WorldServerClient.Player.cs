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

        private WorldServerClient mClient = null;
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
        /// Gets the world server client associated with this object
        /// </summary>
        protected WorldServerClient Client { get { return mClient; } }

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
                var packItems = mInventory.Where(kvp => kvp.Value != null).Select(kvp => new InventoryItemSlot() { Slot = kvp.Key, Item = kvp.Value }).ToList();
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
                if (equippedSlotValue >= (int)EquipmentSlots.EQUIPMENT_SLOT_END)
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
                        container.UpdateItemsInContainer(client);
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
                    mInventory[inventorySlot] = item;
                else
                {
                    if (item != null)
                        mInventory.Add(inventorySlot, item);
                }
            }
            // TOOD: Need to add bank bags, bank slots, and keyring to this as well
        }

        #endregion
    }
}
