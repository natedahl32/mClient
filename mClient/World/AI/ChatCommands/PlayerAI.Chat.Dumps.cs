using mClient.DBC;
using mClient.Shared;
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

        private List<string> mAllDumpCommands = new List<string>() { DUMP_SPELL_COMMAND };

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

            // Make sure this is a combat command
            if (split[0].ToLower() != "dump") return false;

            // If no sub command send correct usage
            if (split.Length <= 1)
            {
                var usageCommands = "dump (";
                // Return the correct usage for a combat command
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "The correct usage for the 'dump' command is:");
                usageCommands += string.Join("|", mAllCombatCommands);
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
                    var dumpInfo = spell.DumpInfo();

                    // Log it
                    Log.WriteLine(LogType.Normal, dumpInfo);

                    return true;
            }

            // No command found
            return false;
        }
    }
}
