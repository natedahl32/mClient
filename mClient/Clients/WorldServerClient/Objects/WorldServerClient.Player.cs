using mClient.Constants;
using mClient.Shared;
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

        #endregion

        #region Public Methods

        public override void SetField(WorldServerClient client, int x, uint value)
        {
            // Can be null when sent from base class
            if (client != null)
            {
                if (x >= (int)PlayerFields.PLAYER_QUEST_LOG_1_1 && x <= (int)PlayerFields.PLAYER_QUEST_LOG_LAST_3)
                {
                    // If we are updating a quest, make sure our quest manager has the quest
                    for (var i = (int)PlayerFields.PLAYER_QUEST_LOG_1_1; i < (int)PlayerFields.PLAYER_QUEST_LOG_LAST_3; i += QuestConstants.MAX_QUEST_OFFSET)
                        if (x == i)
                            if (QuestManager.Instance.GetQuest(value) == null)
                                client.QueryQuest(value);
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
