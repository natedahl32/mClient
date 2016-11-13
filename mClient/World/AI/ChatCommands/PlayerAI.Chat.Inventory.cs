using mClient.Shared;
using mClient.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI
{
    public partial class PlayerChatHandler
    {
        private const string INV_LIST_COMMAND = "list";
        private const string INV_DESTROY_COMMAND = "destroy";

        private List<string> mAllInvCommands = new List<string>() { INV_LIST_COMMAND, INV_DESTROY_COMMAND };

        /// <summary>
        /// Handles all quest commands
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool HandleInventoryCommands(WoWGuid senderGuid, string senderName, string message)
        {
            var split = message.Split(new string[] { " " }, StringSplitOptions.None);

            // Make sure this is a quest command
            if (split[0].ToLower() != "inv" && split[0].ToLower() != "inventory") return false;

            // If no sub command send correct usage
            if (split.Length <= 1)
            {
                var usageCommands = "inv|inventory (";
                // Return the correct usage for a quest command
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "The correct usage for the 'inventory' command is:");
                usageCommands += string.Join("|", mAllInvCommands);
                usageCommands += ")";
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, usageCommands);
                return true;
            }

            switch (split[1].ToLower())
            {
                case INV_LIST_COMMAND:
                    Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "I have the following items in my bags.");
                    foreach (var invSlot in Player.PlayerObject.InventoryItems)
                    {
                        var item_msg = invSlot.Item.ItemGameLink + " x" + invSlot.Item.StackCount.ToString();
                        Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, item_msg);
                    }

                    return true;

                case INV_DESTROY_COMMAND:
                    var itemId = ItemInfo.ExtractItemId(message);

                    return true;
            }

            // No command found
            return false;
        }
    }
}
