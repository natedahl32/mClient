using mClient.Shared;
using mClient.World.AI.Activity.Trade;
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
        private const string INV_TRADE_COMMAND = "trade";

        private List<string> mAllInvCommands = new List<string>() { INV_LIST_COMMAND, INV_DESTROY_COMMAND, INV_TRADE_COMMAND };

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

            // Make sure this is an inventory command
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
                // inv list - lists all items in inventory (not equipped)
                case INV_LIST_COMMAND:
                    Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "I have the following items in my bags.");
                    foreach (var invSlot in Player.PlayerObject.InventoryItems)
                    {
                        var item_msg = invSlot.Item.ItemGameLink + " x" + invSlot.Item.StackCount.ToString();
                        Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, item_msg);
                    }

                    return true;

                // inv destroy [ITEM_LINK] - destroys one item linked
                // inv destroy # [ITEM_LINK] - destroyes the specified number of items linked
                case INV_DESTROY_COMMAND:
                    var itemId = ItemInfo.ExtractItemId(message);
                    var inventorySlot = Player.PlayerObject.GetInventoryItem(itemId);
                    var count = 1;
                    // If the count was passed in as a parameter use that
                    if (split.Length >= 3)
                    {
                        int definedCount = 0;
                        if (int.TryParse(split[2], out definedCount))
                            count = definedCount;
                    }
                    if (inventorySlot != null)
                    {
                        if (count > 1)
                            Player.PlayerAI.Client.DestroyItem(inventorySlot, (byte)count);
                        else
                            Player.PlayerAI.Client.DestroyItem(inventorySlot);
                    }
                        

                    return true;

                // inv trade [ITEM_LINK] [ITEM_LINK] ... - trades all items linked to the sender
                case INV_TRADE_COMMAND:
                    // Get all id's of items we are trading
                    var itemIds = ItemInfo.ExtractItemIds(message);
                    if (itemIds.Count <= 0)
                        return true;

                    // Trading is a little involved, so we push an activity to handle the activity
                    Player.PlayerAI.StartActivity(new BeginTrade(senderGuid, itemIds, Player.PlayerAI));

                    return true;
            }

            // No command found
            return false;
        }
    }
}
