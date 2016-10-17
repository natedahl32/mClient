using mClient.Constants;
using mClient.Network;
using mClient.Shared;
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

            // Accept the quest
            AcceptQuest(guid, questId);
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
        /// Handles quest confirmation acceptance when starting group/raid quests
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_QUESTUPDATE_FAILEDTIMER)]
        public void HandleQuestTimerFailed(PacketIn packet)
        {
            // Get guid
            var questId = packet.ReadUInt32();

            // Drop the quest if we failed it, we'll have to pick it back up again if we want to complete it
            player.DropQuest(questId);
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

            QuestManager.Instance.AddQuest(questInfo);
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
            for (int i = 0; i < questMenuCount; i++)
            {
                var questId = packet.ReadUInt32();
                packet.ReadUInt32();    // Quest icon
                packet.ReadUInt32();    // Quest level
                packet.ReadString();    // Quest title

                // if the players quest log is not full, accept the quest
                if (!player.PlayerObject.IsQuestLogFull)
                    AcceptQuest(entityGuid, questId);
            }

            // Remove flag from AI, telling them we got the quests
            player.PlayerAI.WaitingToAcceptQuests = false;

            // Remove the quest giver as well so we don't keep trying to get quests from this entity immediately until
            // we can update the quest giver statuses
            player.RemoveQuestGiver(entityGuid);

            // Finally update any quest giver statuses
            GetQuestGiverStatuses();
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
            for (int i = 0; i < questCount; i++)
            {
                var questId = packet.ReadUInt32();
                packet.ReadUInt32();    // Quest icon
                packet.ReadUInt32();    // Quest level
                packet.ReadString();    // Quest title

                // if the players quest log is not full, accept the quest
                if (!player.PlayerObject.IsQuestLogFull)
                    AcceptQuest(entityGuid, questId);
            }

            // Remove flag from AI, telling them we got the quests
            player.PlayerAI.WaitingToAcceptQuests = false;

            // Remove the quest giver as well so we don't keep trying to get quests from this entity immediately until
            // we can update the quest giver statuses
            player.RemoveQuestGiver(entityGuid);

            // Finally update any quest giver statuses
            GetQuestGiverStatuses();
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
            // Send response to server
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_QUESTGIVER_ACCEPT_QUEST);
            packet.Write(questGiverGuid);
            packet.Write(questId);
            Send(packet);

            // Add quest to the player and then query for the quest data
            player.PlayerObject.AddQuest(questId);
            QueryQuest(questId);
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
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_GOSSIP_HELLO);
            packet.Write(guid);
            Send(packet);
        }

        #endregion
    }
}
