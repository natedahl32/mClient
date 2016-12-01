using mClient.Constants;
using mClient.DBC;
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
        private const string PROFESSION_LIST_COMMAND = "list";
        private const string PROFESSION_ALCHEMY_COMMAND = "alchemy";
        private const string PROFESSION_BLACKSMITHING_COMMAND = "blacksmithing";
        private const string PROFESSION_LEATHERWORKING_COMMAND = "leatherworking";
        private const string PROFESSION_ENCHANTING_COMMAND = "enchanting";
        private const string PROFESSION_TAILORING_COMMAND = "tailoring";
        private const string PROFESSION_COOKING_COMMAND = "cooking";
        private const string PROFESSION_FIRST_AID_COMMAND = "firstaid";

        private List<string> mAllProfessionCommands = new List<string>() { PROFESSION_LIST_COMMAND,
                                                                           PROFESSION_ALCHEMY_COMMAND,
                                                                           PROFESSION_BLACKSMITHING_COMMAND,
                                                                           PROFESSION_COOKING_COMMAND,
                                                                           PROFESSION_ENCHANTING_COMMAND,
                                                                           PROFESSION_FIRST_AID_COMMAND,
                                                                           PROFESSION_LEATHERWORKING_COMMAND,
                                                                           PROFESSION_TAILORING_COMMAND};

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
                // prof list - sends list of professions and skill values in chat
                case PROFESSION_LIST_COMMAND:
                    // Get all profession skills for this player
                    var professionSkills = Player.PlayerObject.ProfessionSkills;

                    // Send professions in chat
                    Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, "My professions are:");
                    foreach (var prof in professionSkills)
                        Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, string.Format("{0} ({1})", GetProfessionSkillName(prof), Player.PlayerObject.SkillValue(prof)));

                    return true;

                case PROFESSION_ALCHEMY_COMMAND:
                case PROFESSION_BLACKSMITHING_COMMAND:
                case PROFESSION_COOKING_COMMAND:
                case PROFESSION_ENCHANTING_COMMAND:
                case PROFESSION_FIRST_AID_COMMAND:
                case PROFESSION_LEATHERWORKING_COMMAND:
                case PROFESSION_TAILORING_COMMAND:

                    // Get the skill type
                    var skill = GetSkillFromSubCommand(split[1].ToLower());
                    if (skill == 0)
                        return true;

                    // Get all tradespells for this skill
                    var professionSpells = Player.GetProfessionSpellsForSkill(skill);
                    if (professionSpells.Count() == 0)
                        Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, string.Format("I have no recipes for {0}", GetProfessionSkillName(skill)));
                    else
                    {
                        Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, string.Format("I have the following {0} recipes:", GetProfessionSkillName(skill)));
                        foreach (var s in professionSpells)
                            Player.PlayerAI.Client.SendChatMsg(Constants.ChatMsg.Party, Constants.Languages.Universal, BuildChatMessageForRecipe(s));
                    }

                    return true;
            }

            // No command found
            return false;
        }

        /// <summary>
        /// Builds a chat message to send back for a profession recipe
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        private string BuildChatMessageForRecipe(SkillLineAbilityEntry ability)
        {
            var recipeSkillLevel = GetRecipeSkillColor(Player.GetProfessionRecipeSkillLevel(ability));
            var spell = SpellTable.Instance.getSpell(ability.SpellId);
            var createsItem = spell.CreatesItem;
            if (createsItem != null)
                return $"{ability.Spell.SpellName} [{recipeSkillLevel}] --> {createsItem.ItemGameLink}";
            else
                return $"{ability.Spell.SpellName} [{recipeSkillLevel}]";
        }

        /// <summary>
        /// Gets the color value
        /// </summary>
        /// <param name="recipeSkillLevel"></param>
        /// <returns></returns>
        private string GetRecipeSkillColor(ProfessionRecipeSkillLevel recipeSkillLevel)
        {
            switch(recipeSkillLevel)
            {
                case ProfessionRecipeSkillLevel.RECIPE_SKILL_LEVEL_ORANGE:
                    return "ORANGE";
                case ProfessionRecipeSkillLevel.RECIPE_SKILL_LEVEL_YELLOW:
                    return "YELLOW";
                case ProfessionRecipeSkillLevel.RECIPE_SKILL_LEVEL_GREEN:
                    return "GREEN";
                default:
                    return "GREY";
            }
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

        /// <summary>
        /// Returns the skill type associated with the passed subcommand
        /// </summary>
        /// <param name="subcommand"></param>
        /// <returns></returns>
        private SkillType GetSkillFromSubCommand(string subcommand)
        {
            switch (subcommand)
            {
                case PROFESSION_ALCHEMY_COMMAND:
                    return SkillType.SKILL_ALCHEMY;
                case PROFESSION_BLACKSMITHING_COMMAND:
                    return SkillType.SKILL_BLACKSMITHING;
                case PROFESSION_COOKING_COMMAND:
                    return SkillType.SKILL_COOKING;
                case PROFESSION_ENCHANTING_COMMAND:
                    return SkillType.SKILL_ENCHANTING;
                case PROFESSION_FIRST_AID_COMMAND:
                    return SkillType.SKILL_FIRST_AID;
                case PROFESSION_LEATHERWORKING_COMMAND:
                    return SkillType.SKILL_LEATHERWORKING;
                case PROFESSION_TAILORING_COMMAND:
                    return SkillType.SKILL_TAILORING;
            }
            return 0;
        }
    }
}
