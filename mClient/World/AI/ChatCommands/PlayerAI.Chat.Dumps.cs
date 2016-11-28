using mClient.DBC;
using mClient.Shared;
using mClient.World.Items;
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
        private const string DUMP_SPELL_COMMAND = "spell";
        private const string DUMP_ITEM_COMMAND = "item";
        private const string DUMP_QUEST_COMMAND = "quest";

        private List<string> mAllDumpCommands = new List<string>() { DUMP_SPELL_COMMAND, DUMP_ITEM_COMMAND, DUMP_QUEST_COMMAND };

        /// <summary>
        /// Handles all dump commands
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool HandleDumpCommands(WoWGuid senderGuid, string senderName, string message)
        {
            var split = message.Split(new string[] { " " }, StringSplitOptions.None);

            // Make sure this is a dump command
            if (split[0].ToLower() != "dump") return false;

            // If no sub command send correct usage
            if (split.Length <= 1)
            {
                var usageCommands = "dump (";
                // Return the correct usage for a combat command
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "The correct usage for the 'dump' command is:");
                usageCommands += string.Join("|", mAllDumpCommands);
                usageCommands += ")";
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, usageCommands);
                return true;
            }

            switch (split[1].ToLower())
            {
                // combat attack - attacks the senders target
                case DUMP_SPELL_COMMAND:
                    // Get the spell to dump data for
                    uint spellId = 0;
                    if (!uint.TryParse(split[2], out spellId))
                        return false;

                    // Get the spell and dump info
                    var spell = SpellTable.Instance.getSpell(spellId);
                    var spellDump = spell.DumpInfo();

                    // Log it
                    Log.WriteLine(LogType.Normal, spellDump);

                    return true;

                case DUMP_ITEM_COMMAND:
                    // Try to parse item id first
                    uint itemId = 0;
                    if (!uint.TryParse(split[2], out itemId))
                    {
                        // Now try to extract an item id from the link
                        itemId = ItemInfo.ExtractItemId(message);
                        if (itemId <= 0)
                            return false;
                    }
                    
                    var item = ItemManager.Instance.Get(itemId);
                    if (item == null)
                        return false;

                    var itemDump = item.DumpInfo();

                    // Log it
                    Log.WriteLine(LogType.Normal, itemDump);

                    return true;

                case DUMP_QUEST_COMMAND:
                    // Get the quest to dump data for
                    uint questId = 0;
                    if (!uint.TryParse(split[2], out questId))
                        return false;

                    // Get the spell and dump info
                    var quest = QuestManager.Instance.Get(questId);
                    var questDump = quest.DumpInfo();

                    // Log it
                    Log.WriteLine(LogType.Normal, questDump);

                    return true;
            }

            // No command found
            return false;
        }
    }
}
