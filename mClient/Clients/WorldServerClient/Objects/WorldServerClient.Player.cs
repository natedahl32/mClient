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
        public PlayerObj(WoWGuid guid) : base(guid)
        {
        }

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
        public IEnumerable<ItemInfo> InventoryItems
        {
            get
            {
                var items = new List<ItemInfo>();
                for (int i = (int)PlayerFields.PLAYER_FIELD_PACK_SLOT_1; i <= (int)PlayerFields.PLAYER_FIELD_PACK_SLOT_LAST; i++)
                {
                    var itemId = GetFieldValue(i);
                    if (itemId > 0)
                    {
                        var item = ItemManager.Instance.GetItem(itemId);
                        if (item != null)
                            items.Add(item);
                    }
                }
                return items;
            }
        }

        /// <summary>
        /// Gets all items in the player currently has equipped
        /// </summary>
        public IEnumerable<ItemInfo> EquippedItems
        {
            get
            {
                var items = new List<ItemInfo>();
                for (int i = (int)PlayerFields.PLAYER_FIELD_INV_SLOT_HEAD; i < (int)PlayerFields.PLAYER_FIELD_PACK_SLOT_1; i++)
                {
                    var itemId = GetFieldValue(i);
                    if (itemId > 0)
                    {
                        var item = ItemManager.Instance.GetItem(itemId);
                        if (item != null)
                            items.Add(item);
                    }
                }
                return items;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the item currently equipped in the given slot
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public ItemInfo GetItemInEquipmentSlot(EquipmentSlots slot)
        {
            // TOOD: THis isn't going to work. We have guids and the manager has id's
            var index = (int)PlayerFields.PLAYER_FIELD_INV_SLOT_HEAD + (int)slot;
            var itemId = GetFieldValue(index);
            var item = ItemManager.Instance.GetItem(itemId);
            return item;
        }

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
                    if (ItemManager.Instance.GetItem(value) == null)
                        client.QueryItemPrototype(Convert.ToUInt64(value));
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
    }
}
