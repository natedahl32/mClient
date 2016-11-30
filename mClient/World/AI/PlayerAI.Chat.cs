using mClient.Clients;
using mClient.Constants;
using mClient.Shared;
using System;

namespace mClient.World.AI
{
    public partial class PlayerChatHandler
    {
        #region Constructors

        public PlayerChatHandler(Player player)
        {
            this.Player = player;
        }

        #endregion

        #region Properties

        public Player Player { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles a chat message
        /// </summary>
        public void HandleChat(WorldServerClient client, ChatMsg type, WoWGuid senderGuid, string senderName, string message, string channel)
        {
            // trim termination character from end of message
            message = message.Replace("\0", "");

            switch(type)
            {
                case ChatMsg.Battleground:
                case ChatMsg.Guild:
                case ChatMsg.Party:
                case ChatMsg.Raid:
                case ChatMsg.RaidLeader:
                case ChatMsg.RaidWarning:
                    HandleGroupChatMessage(client, senderGuid, senderName, message);
                    break;
                case ChatMsg.Whisper:
                    HandleWhisperMessage(client, senderGuid, senderName, message);
                    break;
                case ChatMsg.MonsterEmote:
                case ChatMsg.MonsterParty:
                case ChatMsg.MonsterSay:
                case ChatMsg.MonsterWhisper:
                case ChatMsg.MonsterYell:
                case ChatMsg.RaidBossEmote:
                case ChatMsg.RaidBossWhisper:
                    HandleNpcMessage(client, senderGuid, senderName, message);
                    break;
                case ChatMsg.Channel:
                    HandleCustomChannelMessage(client, senderGuid, senderName, message, channel);
                    break;
                default:
                    HandleGenericChatMessage(client, senderGuid, senderName, message);
                    break;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles generic chat messages
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        private void HandleGenericChatMessage(WorldServerClient client, WoWGuid senderGuid, string senderName, string message)
        {

        }

        /// <summary>
        /// Handles group chat messages
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        private void HandleGroupChatMessage(WorldServerClient client, WoWGuid senderGuid, string senderName, string message)
        {
            // Nothing to do with an empty message
            if (string.IsNullOrEmpty(message)) return;

            if (HandleChatCommands(senderGuid, senderName, message)) return;
            if (HandleCombatCommands(senderGuid, senderName, message)) return;
            if (HandleQuestCommands(senderGuid, senderName, message)) return;
            if (HandleInventoryCommands(senderGuid, senderName, message)) return;
            if (HandleSkillCommands(senderGuid, senderName, message)) return;
            if (HandleProfessionCommands(senderGuid, senderName, message)) return;
            if (HandleSetCommands(senderGuid, senderName, message)) return;
        }

        /// <summary>
        /// Handles personal chat messages
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        private void HandleWhisperMessage(WorldServerClient client, WoWGuid senderGuid, string senderName, string message)
        {
            // Nothing to do with an empty message
            if (string.IsNullOrEmpty(message)) return;

            if (HandleDumpCommands(senderGuid, senderName, message)) return;
            if (HandleChatCommands(senderGuid, senderName, message)) return;
            if (HandleCombatCommands(senderGuid, senderName, message)) return;
            if (HandleQuestCommands(senderGuid, senderName, message)) return;
            if (HandleInventoryCommands(senderGuid, senderName, message)) return;
            if (HandleSkillCommands(senderGuid, senderName, message)) return;
            if (HandleProfessionCommands(senderGuid, senderName, message)) return;
            if (HandleSetCommands(senderGuid, senderName, message)) return;
        }

        /// <summary>
        /// Handles NPC message
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        private void HandleNpcMessage(WorldServerClient client, WoWGuid senderGuid, string senderName, string message)
        {

        }

        /// <summary>
        /// Handles a message to a custom chat channel
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        /// <param name="channel"></param>
        private void HandleCustomChannelMessage(WorldServerClient client, WoWGuid senderGuid, string senderName, string message, string channel)
        {

        }

        /// <summary>
        /// Handles all chat commands
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool HandleChatCommands(WoWGuid senderGuid, string senderName, string message)
        {
            if (message.Trim().ToLower() == "stay")
            {
                // Create a new query for the player
                var query = new QueryQueue(QueryQueueType.Name, senderGuid.GetOldGuid());
                query.AddCallback((o) => Player.IssueMoveCommand((PlayerObj)o, MoveCommands.Stay));
                var obj = Player.PlayerAI.Client.GetOrQueueObject(query);
                if (obj != null)
                    Player.IssueMoveCommand((PlayerObj)obj, MoveCommands.Stay);
                return true;
            }
            else if (message.Trim().ToLower() == "follow")
            {
                // Create a new query for the player
                var query = new QueryQueue(QueryQueueType.Name, senderGuid.GetOldGuid());
                query.AddCallback((o) => Player.IssueMoveCommand((PlayerObj)o, MoveCommands.Follow));
                var obj = Player.PlayerAI.Client.GetOrQueueObject(query);
                if (obj != null)
                    Player.IssueMoveCommand((PlayerObj)obj, MoveCommands.Follow);
                return true;
            }

            return false;
        }

        #endregion
    }
}
