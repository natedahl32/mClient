using mClient.Shared;
using mClient.World.AI.Activity.Train;
using System;
using System.Collections.Generic;

namespace mClient.World.AI
{
    public partial class PlayerChatHandler
    {
        private const string SKILL_LEARN_COMMAND = "learn";

        private List<string> mAllSkillCommands = new List<string>() { SKILL_LEARN_COMMAND };

        /// <summary>
        /// Handles all skill commands
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool HandleSkillCommands(WoWGuid senderGuid, string senderName, string message)
        {
            var split = message.Split(new string[] { " " }, StringSplitOptions.None);

            // Make sure this is a set command
            if (split[0].ToLower() != "skill") return false;

            // If no sub command send correct usage
            if (split.Length <= 1)
            {
                var usageCommands = "skill (";
                // Return the correct usage for a combat command
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "The correct usage for the 'skill' command is:");
                usageCommands += string.Join("|", mAllSkillCommands);
                usageCommands += ")";
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, usageCommands);
                return true;
            }

            // Get the sender object. If they cannot be found ignore the command
            var sender = Player.PlayerAI.Client.objectMgr.getObject(senderGuid) as Clients.Unit;
            if (sender == null)
                return false;

            switch (split[1].ToLower())
            {
                // combat attack - attacks the senders target
                case SKILL_LEARN_COMMAND:
                    // Get the target of the sender
                    var sendersTarget = Player.PlayerAI.Client.objectMgr.getObject(sender.TargetGuid) as Clients.Unit;
                    if (sendersTarget == null)
                        return false;

                    // Make sure the target is a trainer
                    if (!sendersTarget.IsTrainer)
                    {
                        Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "I can't learn skill from your target, they are not a trainer.");
                        return true;
                    }

                    // Learn skills from the senders target
                    Player.PlayerAI.StartActivity(new TrainSkill(sendersTarget, Player.PlayerAI));

                    return true;
            }

            // No command found
            return false;
        }
    }
}
