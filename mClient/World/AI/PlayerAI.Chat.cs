using mClient.Constants;
using mClient.Shared;

namespace mClient.World.AI
{
    public class PlayerChatHandler
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
        public void HandleChat(ChatMsg type, WoWGuid senderGuid, string senderName, string message, string channel)
        {
            switch(type)
            {
                case ChatMsg.Battleground:
                case ChatMsg.Guild:
                case ChatMsg.Party:
                case ChatMsg.Raid:
                case ChatMsg.RaidLeader:
                case ChatMsg.RaidWarning:
                    HandleGroupChatMessage(senderGuid, senderName, message);
                    break;
                case ChatMsg.Whisper:
                    HandleWhisperMessage(senderGuid, senderName, message);
                    break;
                case ChatMsg.MonsterEmote:
                case ChatMsg.MonsterParty:
                case ChatMsg.MonsterSay:
                case ChatMsg.MonsterWhisper:
                case ChatMsg.MonsterYell:
                case ChatMsg.RaidBossEmote:
                case ChatMsg.RaidBossWhisper:
                    HandleNpcMessage(senderGuid, senderName, message);
                    break;
                case ChatMsg.Channel:
                    HandleCustomChannelMessage(senderGuid, senderName, message, channel);
                    break;
                default:
                    HandleGenericChatMessage(senderGuid, senderName, message);
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
        private void HandleGenericChatMessage(WoWGuid senderGuid, string senderName, string message)
        {

        }

        /// <summary>
        /// Handles group chat messages
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        private void HandleGroupChatMessage(WoWGuid senderGuid, string senderName, string message)
        {
            // Nothing to do with an empty message
            if (string.IsNullOrEmpty(message)) return;

            if (message.Trim().ToLower() == "stay")
                Player.PlayerAI.ClearFollowTarget();
            else if (message.Trim().ToLower() == "follow")
                Player.PlayerAI.FollowTarget(senderGuid);
        }

        /// <summary>
        /// Handles personal chat messages
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        private void HandleWhisperMessage(WoWGuid senderGuid, string senderName, string message)
        {

        }

        /// <summary>
        /// Handles NPC message
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        private void HandleNpcMessage(WoWGuid senderGuid, string senderName, string message)
        {

        }

        /// <summary>
        /// Handles a message to a custom chat channel
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        /// <param name="channel"></param>
        private void HandleCustomChannelMessage(WoWGuid senderGuid, string senderName, string message, string channel)
        {

        }

        #endregion
    }
}
