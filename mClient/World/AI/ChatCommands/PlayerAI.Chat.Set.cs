using mClient.Shared;
using mClient.World.AI.Activity.Train;
using System;
using System.Collections.Generic;

namespace mClient.World.AI
{
    public partial class PlayerChatHandler
    {
        private const string SET_HEARTHSTONE_COMMAND = "hearthstone";

        private List<string> mAllSetCommands = new List<string>() { SET_HEARTHSTONE_COMMAND };

        /// <summary>
        /// Handles all set commands
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool HandleSetCommands(WoWGuid senderGuid, string senderName, string message)
        {
            var split = message.Split(new string[] { " " }, StringSplitOptions.None);

            // Make sure this is a set command
            if (split[0].ToLower() != "set") return false;

            // If no sub command send correct usage
            if (split.Length <= 1)
            {
                var usageCommands = "set (";
                // Return the correct usage for a combat command
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "The correct usage for the 'set' command is:");
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
                case SET_HEARTHSTONE_COMMAND:
                    // Get the target of the sender
                    var sendersTarget = Player.PlayerAI.Client.objectMgr.getObject(sender.TargetGuid) as Clients.Unit;
                    if (sendersTarget == null)
                        return false;

                    // Make sure the target is an innkeeper
                    if (!sendersTarget.IsInnkeeper)
                    {
                        Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "I can't set my hearthstone, your target is not an Innkeeper.");
                        return true;
                    }

                    // Set our hearthstone bind point using the guid of the unit selected by the sender (must be an INNKEEPER)
                    Player.PlayerAI.StartActivity(new SetHearthstone(sendersTarget, Player.PlayerAI));

                    return true;
            }

            // No command found
            return false;
        }
    }
}
