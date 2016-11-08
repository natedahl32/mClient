using mClient.Constants;
using mClient.Network;
using mClient.Shared;
using mClient.World.AI.Activity.Messages;
using mClient.World.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Clients
{
    public partial class WorldServerClient
    {
        #region Packet Handlers

        /// <summary>
        /// Handles quest givers offering us a quest
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_QUESTGIVER_QUEST_DETAILS)]
        public void HandleQuestGiverQuestDetails(PacketIn packet)
        {
            // Get guid
            var guid = packet.ReadUInt64(); // Quest giver guid
            var questId = packet.ReadUInt32();

            // Send message to activities
            var message = new QuestListMessage() { FromEntityGuid = guid, QuestIdList = new List<uint>() { questId }  };
            player.PlayerAI.SendMessageToAllActivities(message);
        }

        /// <summary>
        /// Handles quest confirmation acceptance when starting group/raid quests
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_QUEST_CONFIRM_ACCEPT)]
        public void HandleQuestConfirmAccept(PacketIn packet)
        {
            // Get guid
            var questId = packet.ReadUInt32();

            // Accept the quest
            ConfirmAcceptQuest(questId);
        }

        /// <summary>
        /// Handles quest failed timer
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_QUESTUPDATE_FAILEDTIMER)]
        public void HandleQuestTimerFailed(PacketIn packet)
        {
            // Get guid
            var questId = packet.ReadUInt32();

            // Drop the quest if we failed it, we'll have to pick it back up again if we want to complete it
            player.DropQuest(questId);

            // Get the quest from the quest manager
            var quest = QuestManager.Instance.Get(questId);
            if (quest != null)
                SendChatMsg(ChatMsg.Party, Languages.Universal, string.Format("My quest '{0}' failed. I dropped the quest.", quest.QuestName));
        }

        /// <summary>
        /// Handles quest failure
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_QUESTUPDATE_FAILED)]
        public void HandleQuestFailed(PacketIn packet)
        {
            // Get guid
            var questId = packet.ReadUInt32();

            // Drop the quest if we failed it, we'll have to pick it back up again if we want to complete it
            player.DropQuest(questId);

            // Get the quest from the quest manager
            var quest = QuestManager.Instance.Get(questId);
            if (quest != null)
                SendChatMsg(ChatMsg.Party, Languages.Universal, string.Format("My quest '{0}' failed. I dropped the quest.", quest.QuestName));
        }

        [PacketHandlerAtribute(WorldServerOpCode.SMSG_QUEST_QUERY_RESPONSE)]
        public void HandleQuestQueryResponse(PacketIn packet)
        {
            // Used to construct a QuestInfo for our Quest Manager
            var questId = packet.ReadUInt32();
            packet.ReadUInt32();
            var questLevel = packet.ReadUInt32();
            packet.ReadUInt32();
            var questType = (QuestTypes)packet.ReadUInt32();

            packet.ReadUInt32();
            packet.ReadUInt32();

            packet.ReadUInt32();
            packet.ReadUInt32();

            var nextQuestInChain = packet.ReadUInt32();

            packet.ReadUInt32();
            packet.ReadUInt32();
            packet.ReadUInt32();
            packet.ReadUInt32();
            var questFlags = (QuestFlags)packet.ReadUInt32();

            // Quest rewards
            for (int i = 0; i < 4; i++)
            {
                packet.ReadUInt32();
                packet.ReadUInt32();
            }
            for (int i = 0; i < 6; i++)
            {
                packet.ReadUInt32();
                packet.ReadUInt32();
            }

            var questPointMapId = packet.ReadUInt32();
            var questPointX = packet.ReadFloat();
            var questPointY = packet.ReadFloat();
            packet.ReadUInt32();

            var questTitle = packet.ReadString();

            var questInfo = new QuestInfo()
            {
                QuestId = questId,
                QuestName = questTitle,
                QuestLevel = questLevel,
                QuestType = questType,
                NextQuestInChain = nextQuestInChain,
                QuestFlags = questFlags,
                QuestPointMapId = questPointMapId,
                QuestPoint = new Coords3() { X = questPointX, Y = questPointY }
            };

            QuestManager.Instance.Add(questInfo);
        }

        /// <summary>
        /// Handles quest giver statuses in our LOS
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_QUESTGIVER_STATUS_MULTIPLE)]
        public void HandleQuestGiverStatusMultiple(PacketIn packet)
        {
            // Get number of status updates
            var count = packet.ReadUInt32();
            var questGivers = new List<QuestGiver>();

            for (int i = 0; i < count; i++)
            {
                var guid = packet.ReadUInt64();
                var status = (QuestGiverStatus)packet.ReadByte();
                questGivers.Add(new QuestGiver() { Guid = guid, Status = status });
            }

            player.UpdateQuestGivers(questGivers);
        }

        /// <summary>
        /// Retrieves the quest list from an entity
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_GOSSIP_MESSAGE)]
        public void HandleGossipMenuList(PacketIn packet)
        {
            var entityGuid = packet.ReadUInt64();
            packet.ReadUInt32();    // title text id

            var gossipMenuCount = packet.ReadUInt32();
            for (int i = 0; i < gossipMenuCount; i++)
            {
                // TODO: Might want to do something with these in the future
                var menuIndex = packet.ReadUInt32();
                packet.ReadUInt32();    // Menu icon
                packet.ReadUInt32();    // Menu coded
                packet.ReadString();    // Text for gossip item
            }

            var questMenuCount = packet.ReadUInt32();
            var quests = new List<UInt32>();
            for (int i = 0; i < questMenuCount; i++)
            {
                var questId = packet.ReadUInt32();
                packet.ReadUInt32();    // Quest icon
                packet.ReadUInt32();    // Quest level
                packet.ReadString();    // Quest title
                quests.Add(questId);
            }

            // Send message to activities
            var message = new QuestListMessage() { FromEntityGuid = entityGuid, QuestIdList = quests };
            player.PlayerAI.SendMessageToAllActivities(message);

            /*
            // First, try to complete all the quests if we have them in our log to make room for new ones
            foreach (var q in quests)
                if (player.PlayerObject.GetQuestSlot(q) < QuestConstants.MAX_QUEST_LOG_SIZE)
                    CompleteQuest(entityGuid, q);

            // if the players quest log is not full, accept the quest
            foreach (var q in quests)
                if (!player.PlayerObject.IsQuestLogFull)
                    AcceptQuest(entityGuid, q);

            // Remove flag from AI, telling them we got the quests
            player.PlayerAI.WaitingToAcceptQuests = false;

            // Remove the quest giver as well so we don't keep trying to get quests from this entity immediately until
            // we can update the quest giver statuses
            player.RemoveQuestGiver(entityGuid);

            // Finally update any quest giver statuses
            GetQuestGiverStatuses();
            */
        }

        /// <summary>
        /// Retrieves the quest list from an entity
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_QUESTGIVER_QUEST_LIST)]
        public void HandleQuestList(PacketIn packet)
        {
            var entityGuid = packet.ReadUInt64();
            packet.ReadString();    // Title of gossip menu
            packet.ReadUInt32();    // Player emote
            packet.ReadUInt32();    // NPC emote

            var questCount = packet.ReadByte();
            var quests = new List<UInt32>();
            for (int i = 0; i < questCount; i++)
            {
                var questId = packet.ReadUInt32();
                packet.ReadUInt32();    // Quest icon
                packet.ReadUInt32();    // Quest level
                packet.ReadString();    // Quest title
                quests.Add(questId);
            }

            // Send message to activities
            var message = new QuestListMessage() { FromEntityGuid = entityGuid, QuestIdList = quests };
            player.PlayerAI.SendMessageToAllActivities(message);

            /*
            // First, try to complete all the quests if we have them in our log to make room for new ones
            foreach (var q in quests)
                if (player.PlayerObject.GetQuestSlot(q) < QuestConstants.MAX_QUEST_LOG_SIZE)
                    CompleteQuest(entityGuid, q);

            // if the players quest log is not full, accept the quest
            foreach (var q in quests)
                if (!player.PlayerObject.IsQuestLogFull)
                    AcceptQuest(entityGuid, q);


            // Remove flag from AI, telling them we got the quests
            player.PlayerAI.WaitingToAcceptQuests = false;

            // Remove the quest giver as well so we don't keep trying to get quests from this entity immediately until
            // we can update the quest giver statuses
            player.RemoveQuestGiver(entityGuid);

            // Finally update any quest giver statuses
            GetQuestGiverStatuses();
            */
        }

        /// <summary>
        /// Handles quest giver request items when trying to completing a quest
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_QUESTGIVER_REQUEST_ITEMS)]
        public void HandleQuestGiverRequestItems(PacketIn packet)
        {
            var entityGuid = packet.ReadUInt64();
            var questId = packet.ReadUInt32();
            var questTitle = packet.ReadString();    // Quest title
            packet.ReadString();    // Request items text
            packet.ReadUInt32();    // Emote delay
            packet.ReadUInt32();    // Emote Id

            // TODO: Finish this up by sending a message via chat as to what items are still missing
            // from the quest requirements and for what quest. Not certain under what circumstances we would
            // receive this op code.
        }

        /// <summary>
        /// Handles quest giver offer reward options for a quest
        /// </summary>
        /// <param name="packet"></param>
        /// <remarks>This is not called when we have completed the quest. Do not add items to inventory at this point.</remarks>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_QUESTGIVER_OFFER_REWARD)]
        public void HandleQuestGiverOfferReward(PacketIn packet)
        {
            var entityGuid = packet.ReadUInt64();
            var questId = packet.ReadUInt32();
            packet.ReadString();    // Quest title
            packet.ReadString();    // Offer reward text
            packet.ReadUInt32();    // Auto complete

            var emoteCount = packet.ReadUInt32();
            for (int i = 0; i < emoteCount; i++)
            {
                packet.ReadUInt32();    // Emote delay
                packet.ReadUInt32();    // Emote Id
            }

            // Rewards we have a choice of selecting
            var rewardsChoiceItemCount = packet.ReadUInt32();
            var rewardItems = new List<QuestOfferRewards.RewardItem>();
            for (uint i = 0; i < rewardsChoiceItemCount; i++)
            {
                var itemId = packet.ReadUInt32();
                var itemCount = packet.ReadUInt32();
                packet.ReadUInt32();    // Display Info
                rewardItems.Add(new QuestOfferRewards.RewardItem() { ItemId = itemId, ItemCount = itemCount, ItemChoiceIndex = i });
            }

            // Send a message with the reward items to the activities
            var message = new QuestOfferRewards() { RewardItems = rewardItems };
            player.PlayerAI.SendMessageToAllActivities(message);

            // I don't think we need anything below this. It is just for display purposes for a client.

            // Rewards we get by default
            //var rewardsCount = packet.ReadUInt32();
            //for (int i = 0; i < rewardsCount; i++)
            //{
            //    var itemId = packet.ReadUInt32();
            //    var itemCount = packet.ReadUInt32();
            //    packet.ReadUInt32();    // Display Info
            //}

            //var moneyReward = packet.ReadUInt32();
            //packet.ReadUInt32();        // Rewards spell, do we need this?
            //packet.ReadUInt32();        // Casted spell on us, do we need this?

        }

        /// <summary>
        /// Handles quest giver offer reward options for a quest
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_QUESTGIVER_QUEST_COMPLETE)]
        public void HandleCompletedQuest(PacketIn packet)
        {
            var questId = packet.ReadUInt32();
            packet.ReadUInt32();    // unknown

            // Send message to all activities
            var message = new QuestCompleteMessage() { QuestId = questId };
            player.PlayerAI.SendMessageToAllActivities(message);

            // Not sure that we need anything below here or not yet.

            //var experienceAwarded = packet.ReadUInt32();
            //var rewardedMoney = packet.ReadUInt32();

            //var rewardItemsCount = packet.ReadUInt32();
            //for (int i = 0; i < rewardItemsCount; i++)
            //{
            //    var itemId = packet.ReadUInt32();
            //    var itemCount = packet.ReadUInt32();
            //    // Note - I don't think we need these at all. We will get a SMSG_ITEM_PUSH_RESULT from
            //    // the server for all rewards.
            //}
        }

        /// <summary>
        /// Handles an update that a quest we have is completed
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_QUESTUPDATE_COMPLETE)]
        public void HandleQuestCompleteUpdate(PacketIn packet)
        {
            var questId = packet.ReadUInt32();

            // Get the quest from the quest manager
            var quest = QuestManager.Instance.Get(questId);
            if (quest != null)
                SendChatMsg(ChatMsg.Party, Languages.Universal, string.Format("My quest '{0}' is complete.", quest.QuestName));
        }

        /// <summary>
        /// Handles an update that a quest we have is completed
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_QUESTUPDATE_ADD_KILL)]
        public void HandleQuestKillUpdate(PacketIn packet)
        {
            var questId = packet.ReadUInt32();
            var creatureTemplateEntry = packet.ReadUInt32();
            var killCount = packet.ReadUInt32();
            var requiredCount = packet.ReadUInt32();
            var guid = packet.ReadUInt64(); // not sure, guid of the creature that was killed?

            // Get the quest from the quest manager
            var quest = QuestManager.Instance.Get(questId);
            if (quest != null)
                SendChatMsg(ChatMsg.Party, Languages.Universal, string.Format("Quest '{0}' Update: {1} of {2} killed.", quest.QuestName, killCount, requiredCount));
        }

        #endregion

        #region Actions

        /// <summary>
        /// Queries the server for a quest
        /// </summary>
        /// <param name="questId"></param>
        public void QueryQuest(UInt32 questId)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_QUEST_QUERY);
            packet.Write(questId);
            Send(packet);
        }

        /// <summary>
        /// Accepts a quest that was sent to us
        /// </summary>
        /// <param name="questGiverGuid"></param>
        /// <param name="questId"></param>
        public void AcceptQuest(UInt64 questGiverGuid, UInt32 questId)
        {
            // Query for the quest
            QueryQuest(questId);

            // Send response to server
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_QUESTGIVER_ACCEPT_QUEST);
            packet.Write(questGiverGuid);
            packet.Write(questId);
            Send(packet);

            // Add quest to the player and then query for the quest data
            player.PlayerObject.AddQuest(questId);
        }

        /// <summary>
        /// Confirms that we want to accept a quest (I believe for starting group quests)
        /// </summary>
        /// <param name="questId"></param>
        public void ConfirmAcceptQuest(UInt32 questId)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_QUEST_CONFIRM_ACCEPT);
            packet.Write(questId);
            Send(packet);
        }

        /// <summary>
        /// Sends a remove quest request to the server
        /// </summary>
        /// <param name="questId"></param>
        public void RemoveQuest(UInt32 questId)
        {
            // Find the slot this quest is in
            var slot = player.PlayerObject.GetQuestSlot(questId);
            // If we couldn't find the quest slot, we don't have the quest
            if (slot == QuestConstants.MAX_QUEST_LOG_SIZE)
                return;

            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_QUESTLOG_REMOVE_QUEST);
            packet.Write(slot);
            Send(packet);
        }

        /// <summary>
        /// Retrieve all quest giver statuses in our LOS
        /// </summary>
        public void GetQuestGiverStatuses()
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_QUESTGIVER_STATUS_MULTIPLE_QUERY);
            Send(packet);
        }

        /// <summary>
        /// Calls for a gossip menu from an entity (unit, gameobject, etc.) by guid and retrieves the quest
        /// list returned for that entity.
        /// </summary>
        public void GetQuestListFromQuestGiver(UInt64 guid)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_QUESTGIVER_HELLO);
            packet.Write(guid);
            Send(packet);
        }

        /// <summary>
        /// Completes a quest from a quest giver
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="questId"></param>
        public void CompleteQuest(UInt64 guid, UInt32 questId)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_QUESTGIVER_REQUEST_REWARD);
            packet.Write(guid);
            packet.Write(questId);
            Send(packet);
        }

        /// <summary>
        /// Chooses a quest reward based on the choice index given to us
        /// </summary>
        /// <param name="questGiverGuid"></param>
        /// <param name="questId"></param>
        /// <param name="itemChoiceIndex"></param>
        public void ChooseQuestReward(UInt64 questGiverGuid, UInt32 questId, UInt32 itemChoiceIndex)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_QUESTGIVER_CHOOSE_REWARD);
            packet.Write(questGiverGuid);
            packet.Write(questId);
            packet.Write(itemChoiceIndex);
            Send(packet);
        }

        #endregion
    }
}
