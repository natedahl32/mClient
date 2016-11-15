﻿using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI
{
    public partial class PlayerChatHandler
    {
        private const string COMBAT_ATTACK_COMMAND = "attack";

        private List<string> mAllCombatCommands = new List<string>() { COMBAT_ATTACK_COMMAND };

        /// <summary>
        /// Handles all quest commands
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool HandleCombatCommands(WoWGuid senderGuid, string senderName, string message)
        {
            var split = message.Split(new string[] { " " }, StringSplitOptions.None);

            // Make sure this is a combat command
            if (split[0].ToLower() != "combat") return false;

            // If no sub command send correct usage
            if (split.Length <= 1)
            {
                var usageCommands = "combat (";
                // Return the correct usage for a combat command
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "The correct usage for the 'combat' command is:");
                usageCommands += string.Join("|", mAllCombatCommands);
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
                case COMBAT_ATTACK_COMMAND:
                    // Get the target of the sender
                    var sendersTarget = Player.PlayerAI.Client.objectMgr.getObject(sender.TargetGuid) as Clients.Unit;
                    if (sendersTarget == null)
                        return false;

                    // TODO: If we already have enemies in our list, we need a way to force the bot to start
                    // attacking this target since they were commanded to do so. Thinking the best way is to issue
                    // a change target activity that has the bot change targets.

                    // Add them to our enemy list, this have them start combat with the target
                    Player.AddEnemy(sender.TargetGuid);

                    return true;
            }

            // No command found
            return false;
        }
    }
}