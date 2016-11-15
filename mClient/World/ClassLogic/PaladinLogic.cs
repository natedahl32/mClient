using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class PaladinLogic : PlayerClassLogic
    {
        #region Constructors

        public PaladinLogic(Player player) : base(player)
        {
        }

        #endregion

        #region Public Methods

        public override void InitializeSpells()
        {

        }

        #endregion

        #region Paladin Constants

        public static class Spells
        {
            public const uint BLESSING_OF_KINGS_1 = 20217;
            public const uint BLESSING_OF_MIGHT_1 = 19740;
            public const uint BLESSING_OF_SANCTUARY_1 = 20911;
            public const uint BLESSING_OF_WISDOM_1 = 19742;
            public const uint CLEANSE_1 = 4987;
            public const uint CONCENTRATION_AURA_1 = 19746;
            public const uint CONSECRATION_1 = 26573;
            public const uint DEVOTION_AURA_1 = 465;
            public const uint DIVINE_FAVOR_1 = 20216;
            public const uint DIVINE_INTERVENTION_1 = 19752;
            public const uint DIVINE_PROTECTION_1 = 498;
            public const uint DIVINE_SHIELD_1 = 642;
            public const uint EXORCISM_1 = 879;
            public const uint FIRE_RESISTANCE_AURA_1 = 19891;
            public const uint FLASH_OF_LIGHT_1 = 19750;
            public const uint FROST_RESISTANCE_AURA_1 = 19888;
            public const uint GREATER_BLESSING_OF_KINGS_1 = 25898;
            public const uint GREATER_BLESSING_OF_MIGHT_1 = 25782;
            public const uint GREATER_BLESSING_OF_SANCTUARY_1 = 25899;
            public const uint GREATER_BLESSING_OF_WISDOM_1 = 25894;
            public const uint HAMMER_OF_JUSTICE_1 = 853;
            public const uint HAMMER_OF_WRATH_1 = 24275;
            public const uint HAND_OF_FREEDOM_1 = 1044;
            public const uint BLESSING_OF_PROTECTION_1 = 1022;
            public const uint BLESSING_OF_SACRIFICE_1 = 6940;
            public const uint HAND_OF_SALVATION_1 = 1038;
            public const uint HOLY_LIGHT_1 = 635;
            public const uint HOLY_SHIELD_1 = 20925;
            public const uint HOLY_SHOCK_1 = 20473;
            public const uint HOLY_WRATH_1 = 2812;
            public const uint JUDGEMENT_1 = 20271;
            public const uint LAY_ON_HANDS_1 = 633;
            public const uint PURIFY_1 = 1152;
            public const uint REDEMPTION_1 = 7328;
            public const uint REPENTANCE_1 = 20066;
            public const uint RETRIBUTION_AURA_1 = 7294;
            public const uint RIGHTEOUS_FURY_1 = 25780;
            public const uint SEAL_OF_COMMAND_1 = 20375;
            public const uint SEAL_OF_JUSTICE_1 = 20164;
            public const uint SEAL_OF_LIGHT_1 = 20165;
            public const uint SEAL_OF_RIGHTEOUSNESS_1 = 21084;
            public const uint SEAL_OF_WISDOM_1 = 20166;
            public const uint SEAL_OF_THE_CRUSADER_1 = 21082;
            public const uint SENSE_UNDEAD_1 = 5502;
            public const uint SHADOW_RESISTANCE_AURA_1 = 19876;
            public const uint TURN_EVIL_1 = 10326;

            // Judgement auras on target
            public const uint JUDGEMENT_OF_WISDOM = 20355; // rank 2: 20354; rank 1: 20186
            public const uint JUDGEMENT_OF_JUSTICE = 20184;
            public const uint JUDGEMENT_OF_THE_CRUSADER = 20303;  // rank 5: 20302; rank 4: 20301; rank 3: 20300; rank 2: 20188; rank 1: 21183
        }

        #endregion
    }
}
