using mClient.Constants;
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
        private const string PROFESSION_LIST_COMMAND = "list";

        private List<string> mAllProfessionCommands = new List<string>() { PROFESSION_LIST_COMMAND };

        /// <summary>
        /// Handles all profession commands
        /// </summary>
        /// <param name="senderGuid"></param>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool HandleProfessionCommands(WoWGuid senderGuid, string senderName, string message)
        {
            var split = message.Split(new string[] { " " }, StringSplitOptions.None);

            // Make sure this is a combat command
            if (split[0].ToLower() != "profession" && split[0].ToLower() != "prof") return false;

            // If no sub command send correct usage
            if (split.Length <= 1)
            {
                var usageCommands = "prof|profession (";
                // Return the correct usage for a combat command
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "The correct usage for the 'profession' command is:");
                usageCommands += string.Join("|", mAllProfessionCommands);
                usageCommands += ")";
                Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, usageCommands);
                return true;
            }

            switch (split[1].ToLower())
            {
                // combat attack - attacks the senders target
                case PROFESSION_LIST_COMMAND:
                    // Get all profession skills for this player
                    var professionSkills = Player.PlayerObject.ProfessionSkills;

                    // Send professions in chat
                    Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "My professions are:");
                    foreach (var prof in professionSkills)
                        Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, string.Format("{0} ({1})", GetProfessionSkillName(prof), Player.PlayerObject.SkillValue(prof)));

                    return true;
            }

            // No command found
            return false;
        }

        /// <summary>
        /// Gets the name of the profession skill
        /// </summary>
        /// <param name="skill"></param>
        private string GetProfessionSkillName(SkillType skill)
        {
            if (skill == SkillType.SKILL_ALCHEMY)
                return "Alchemy";
            if (skill == SkillType.SKILL_HERBALISM)
                return "Herbalism";
            if (skill == SkillType.SKILL_BLACKSMITHING)
                return "Blacksmithing";
            if (skill == SkillType.SKILL_MINING)
                return "Mining";
            if (skill == SkillType.SKILL_LEATHERWORKING)
                return "Leatherworking";
            if (skill == SkillType.SKILL_SKINNING)
                return "Skinning";
            if (skill == SkillType.SKILL_ENCHANTING)
                return "Enchanting";
            if (skill == SkillType.SKILL_TAILORING)
                return "Tailoring";
            if (skill == SkillType.SKILL_COOKING)
                return "Cooking";
            if (skill == SkillType.SKILL_FISHING)
                return "Fishing";
            if (skill == SkillType.SKILL_FIRST_AID)
                return "First Aid";
            return string.Empty;
        }
    }
}
