using mClient.Constants;
using mClient.Shared;
using mClient.World.Talents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI
{
    public partial class PlayerChatHandler
    {
        private const string TALENTSPEC_LIST_COMMAND = "list";
        private const string TALENTSPEC_SET_COMMAND = "set";

        private List<string> mAllTalentCommands = new List<string>() { TALENTSPEC_LIST_COMMAND, TALENTSPEC_SET_COMMAND };

        /// <summary>
        /// Handles all talent commands
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool HandleTalentCommands(WoWGuid senderGuid, string senderName, string message)
        {
            var split = message.Split(new string[] { " " }, StringSplitOptions.None);

            // Make sure this is a talent command
            if (split[0].ToLower() != "talentspec" && split[0].ToLower() != "ts") return false;

            // If no sub command send correct usage
            if (split.Length <= 1)
            {
                var usageCommands = "ts|talentspec (";
                // Return the correct usage for a combat command
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "The correct usage for the 'talentspec' command is:");
                usageCommands += string.Join("|", mAllTalentCommands);
                usageCommands += ")";
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, usageCommands);
                return true;
            }

            switch (split[1].ToLower())
            {
                // talent list - lists all talent specs available for this bots class
                case TALENTSPEC_LIST_COMMAND:
                    // Get all talents available for this class
                    var talentSpecs = SpecManager.Instance.GetAll().Where(s => s.ForClass == (Classname)Player.Class);
                    Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "The talent specs available for my class are:");

                    foreach (var spec in talentSpecs)
                        if (spec.Id == Player.SpecId)
                            Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, $"{spec.Id}) {spec.Name} [CURRENT SPEC]");
                        else
                            Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, $"{spec.Id}) {spec.Name}");

                    return true;

                case TALENTSPEC_SET_COMMAND:
                    // Get the id passed in
                    var id = Convert.ToUInt32(split[2]);
                    var talentSpec = SpecManager.Instance.Get(id);
                    if (talentSpec == null)
                        Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "That talent spec doesn't exist!");
                    else
                    {
                        // Talent spec must be for the bots class
                        if (talentSpec.ForClass != (Classname)Player.Class)
                            Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "That talent spec is not for my class!");
                        else
                        {
                            Player.SetTalentSpec(id);
                            Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, $"My talent spec is set to {talentSpec.Name}.");
                        }
                    }

                    return true;
            }

            // No command found
            return false;
        }
    }
}
