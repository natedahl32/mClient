using mClient.Shared;
using mClient.World.AI.Activity.Quest;
using mClient.World.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI
{
    public partial class PlayerChatHandler
    {
        private const string QUEST_DROP_COMMAND = "drop";
        private const string QUEST_LIST_COMMAND = "list";
        private const string QUEST_IGNORE_COMMAND = "ignore";
        private const string QUEST_INTERACT_COMMAND = "interact";

        private List<string> mAllQuestCommands = new List<string>() { QUEST_DROP_COMMAND, QUEST_LIST_COMMAND, QUEST_IGNORE_COMMAND, QUEST_INTERACT_COMMAND };

        /// <summary>
        /// Handles all quest commands
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool HandleQuestCommands(WoWGuid senderGuid, string senderName, string message)
        {
            var split = message.Split(new string[] { " " }, StringSplitOptions.None);

            // Make sure this is a quest command
            if (split[0].ToLower() != "quest") return false;

            // If no sub command send correct usage
            if (split.Length <= 1)
            {
                var usageCommands = "quest (";
                // Return the correct usage for a quest command
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "The correct usage for the 'quest' command is:");
                usageCommands += string.Join("|", mAllQuestCommands);
                usageCommands += ")";
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, usageCommands);
                return true;
            }

            // Get the sender object, if available.
            var sender = Player.PlayerAI.Client.objectMgr.getObject(senderGuid) as Clients.Unit;

            string questTitle = string.Empty;
            switch (split[1].ToLower())
            {
                // quest drop [quest name] - drops the quest that matches the name from the quest log
                case QUEST_DROP_COMMAND:
                    // Get the title of the quest
                    questTitle = GetQuestTitleFromInput(split);

                    // If we have an empty quest title, send back a message
                    if (string.IsNullOrEmpty(questTitle) || string.IsNullOrEmpty(questTitle.Trim()))
                        Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "You must supply the full name of the quest you want me to drop.");
                    else
                        Player.DropQuest(questTitle.Trim());
                    return true;

                // quest list - lists all quests in the quest log to chat
                case QUEST_LIST_COMMAND:
                    Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "I have the following quests in my quest log.");
                    foreach (var quest in Player.PlayerObject.Quests)
                    {
                        var questInfo = QuestManager.Instance.Get(quest.QuestId);
                        if (questInfo != null)
                        {
                            var quest_msg = (questInfo.QuestLevel > 0 ? "[" + questInfo.QuestLevel.ToString() + "] " : "[1] ");
                            quest_msg += questInfo.QuestName;
                            if (quest.IsComplete)
                                quest_msg += " (Complete)";
                            Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, quest_msg);
                        }
                    }

                    return true;

                case QUEST_IGNORE_COMMAND:
                    // Get the title of the quest
                    questTitle = GetQuestTitleFromInput(split);

                    // If we have an empty quest title, send back a message
                    if (string.IsNullOrEmpty(questTitle) || string.IsNullOrEmpty(questTitle.Trim()))
                        Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "You must supply the full name of the quest you want me to ignore.");
                    else
                        Player.IgnoreQuest(questTitle.Trim());

                    return true;

                case QUEST_INTERACT_COMMAND:
                    // If our sender cannot be found we can't carryout this command
                    if (sender == null) return false;

                    // Get the target of the sender
                    var sendersTarget = Player.PlayerAI.Client.objectMgr.getObject(sender.TargetGuid) as Clients.Unit;
                    if (sendersTarget == null)
                        return false;

                    // Goes through gossip menus of the target
                    Player.PlayerAI.StartActivity(new GossipWithNpc(sendersTarget, Player.PlayerAI));

                    return true;
            }

            // No command found
            return false;
        }

        /// <summary>
        /// Gets a quest title from input message
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        private string GetQuestTitleFromInput(string[] inputs)
        {
            var questTitle = string.Empty;
            for (int i = 2; i < inputs.Length; i++)
                if (!string.IsNullOrEmpty(inputs[i]))
                    questTitle += inputs[i] + " ";
            return questTitle;
        }
    }
}
