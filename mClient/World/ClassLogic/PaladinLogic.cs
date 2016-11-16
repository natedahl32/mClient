using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class PaladinLogic : PlayerClassLogic
    {
        #region Declarations

        // Retribution
        protected uint RETRIBUTION_AURA,
               SEAL_OF_COMMAND,
               GREATER_BLESSING_OF_WISDOM,
               GREATER_BLESSING_OF_MIGHT,
               BLESSING_OF_WISDOM,
               BLESSING_OF_MIGHT,
               HAMMER_OF_JUSTICE,
               RIGHTEOUS_FURY,
               JUDGEMENT;

        // Holy
        protected uint FLASH_OF_LIGHT,
               HOLY_LIGHT,
               DIVINE_SHIELD,
               HAMMER_OF_WRATH,
               CONSECRATION,
               CONCENTRATION_AURA,
               DIVINE_FAVOR,
               HOLY_SHOCK,
               HOLY_WRATH,
               LAY_ON_HANDS,
               EXORCISM,
               REDEMPTION,
               SEAL_OF_JUSTICE,
               SEAL_OF_LIGHT,
               SEAL_OF_RIGHTEOUSNESS,
               SEAL_OF_WISDOM,
               SEAL_OF_THE_CRUSADER,
               PURIFY,
               CLEANSE;

        // Protection
        protected uint GREATER_BLESSING_OF_KINGS,
               BLESSING_OF_KINGS,
               BLESSING_OF_PROTECTION,
               SHADOW_RESISTANCE_AURA,
               DEVOTION_AURA,
               FIRE_RESISTANCE_AURA,
               FROST_RESISTANCE_AURA,
               DEFENSIVE_STANCE,
               BERSERKER_STANCE,
               BATTLE_STANCE,
               DIVINE_SACRIFICE,
               DIVINE_PROTECTION,
               DIVINE_INTERVENTION,
               HOLY_SHIELD,
               AVENGERS_SHIELD,
               RIGHTEOUS_DEFENSE,
               BLESSING_OF_SANCTUARY,
               GREATER_BLESSING_OF_SANCTUARY,
               BLESSING_OF_SACRIFICE,
               SHIELD_OF_RIGHTEOUSNESS,
               HAND_OF_RECKONING,
               HAMMER_OF_THE_RIGHTEOUS,
               HAND_OF_FREEDOM,
               HAND_OF_SALVATION,
               REPENTANCE,
               SENSE_UNDEAD;

        // cannot be protected
        protected uint FORBEARANCE;

        //Non-Stacking buffs
        protected uint PRAYER_OF_SHADOW_PROTECTION;

        #endregion

        #region Constructors

        public PaladinLogic(Player player) : base(player)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether or not the player has any buffs to give out (including self buffs)
        /// </summary>
        public override bool HasOOCBuffs
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all group members that need a buff
        /// </summary>
        public override Dictionary<SpellEntry, IList<Player>> GroupMembersNeedingOOCBuffs
        {
            get
            {
                return new Dictionary<SpellEntry, IList<Player>>();
            }
        }

        /// <summary>
        /// Gets whether or not this player is a melee combatant
        /// </summary>
        public override bool IsMelee
        {
            get
            {
                // TODO: Dependant upon spec
                return true;
            }
        }

        /// <summary>
        /// Gets the next spell to cast in a DPS rotation for the class
        /// </summary>
        public override SpellEntry NextSpellInRotation
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Ignores spells that we think we should learn. These are generally broken spells in the DBC files that we can't weed out using normal methods
        /// </summary>
        public override IEnumerable<uint> IgnoreLearningSpells
        {
            get
            {
                return new List<uint>();
            }
        }

        #endregion

        #region Public Methods

        public override void InitializeSpells()
        {
            base.InitializeSpells();

            // Spells
            BLESSING_OF_KINGS = InitSpell(Spells.BLESSING_OF_KINGS_1);
            BLESSING_OF_MIGHT = InitSpell(Spells.BLESSING_OF_MIGHT_1);
            BLESSING_OF_SANCTUARY = InitSpell(Spells.BLESSING_OF_SANCTUARY_1);
            BLESSING_OF_WISDOM = InitSpell(Spells.BLESSING_OF_WISDOM_1);
            CLEANSE = InitSpell(Spells.CLEANSE_1);
            CONCENTRATION_AURA = InitSpell(Spells.CONCENTRATION_AURA_1);
            CONSECRATION = InitSpell(Spells.CONSECRATION_1);
            DEVOTION_AURA = InitSpell(Spells.DEVOTION_AURA_1);
            DIVINE_FAVOR = InitSpell(Spells.DIVINE_FAVOR_1);
            DIVINE_INTERVENTION = InitSpell(Spells.DIVINE_INTERVENTION_1);
            DIVINE_PROTECTION = InitSpell(Spells.DIVINE_PROTECTION_1);
            DIVINE_SHIELD = InitSpell(Spells.DIVINE_SHIELD_1);
            EXORCISM = InitSpell(Spells.EXORCISM_1);
            FIRE_RESISTANCE_AURA = InitSpell(Spells.FIRE_RESISTANCE_AURA_1);
            FLASH_OF_LIGHT = InitSpell(Spells.FLASH_OF_LIGHT_1);
            FROST_RESISTANCE_AURA = InitSpell(Spells.FROST_RESISTANCE_AURA_1);
            GREATER_BLESSING_OF_KINGS = InitSpell(Spells.GREATER_BLESSING_OF_KINGS_1);
            GREATER_BLESSING_OF_MIGHT = InitSpell(Spells.GREATER_BLESSING_OF_MIGHT_1);
            GREATER_BLESSING_OF_SANCTUARY = InitSpell(Spells.GREATER_BLESSING_OF_SANCTUARY_1);
            GREATER_BLESSING_OF_WISDOM = InitSpell(Spells.GREATER_BLESSING_OF_WISDOM_1);
            HAMMER_OF_JUSTICE = InitSpell(Spells.HAMMER_OF_JUSTICE_1);
            HAMMER_OF_WRATH = InitSpell(Spells.HAMMER_OF_WRATH_1);
            HAND_OF_FREEDOM = InitSpell(Spells.HAND_OF_FREEDOM_1);
            BLESSING_OF_PROTECTION = InitSpell(Spells.BLESSING_OF_PROTECTION_1);
            BLESSING_OF_SACRIFICE = InitSpell(Spells.BLESSING_OF_SACRIFICE_1);
            HAND_OF_SALVATION = InitSpell(Spells.HAND_OF_SALVATION_1);
            HOLY_LIGHT = InitSpell(Spells.HOLY_LIGHT_1);
            HOLY_SHIELD = InitSpell(Spells.HOLY_SHIELD_1);
            HOLY_SHOCK = InitSpell(Spells.HOLY_SHOCK_1);
            HOLY_WRATH = InitSpell(Spells.HOLY_WRATH_1);
            JUDGEMENT = InitSpell(Spells.JUDGEMENT_1);
            LAY_ON_HANDS = InitSpell(Spells.LAY_ON_HANDS_1);
            PURIFY = InitSpell(Spells.PURIFY_1);
            REDEMPTION = InitSpell(Spells.REDEMPTION_1);
            REPENTANCE = InitSpell(Spells.REPENTANCE_1);
            RETRIBUTION_AURA = InitSpell(Spells.RETRIBUTION_AURA_1);
            RIGHTEOUS_FURY = InitSpell(Spells.RIGHTEOUS_FURY_1);
            SEAL_OF_COMMAND = InitSpell(Spells.SEAL_OF_COMMAND_1);
            SEAL_OF_JUSTICE = InitSpell(Spells.SEAL_OF_JUSTICE_1);
            SEAL_OF_LIGHT = InitSpell(Spells.SEAL_OF_LIGHT_1);
            SEAL_OF_RIGHTEOUSNESS = InitSpell(Spells.SEAL_OF_RIGHTEOUSNESS_1);
            SEAL_OF_WISDOM = InitSpell(Spells.SEAL_OF_WISDOM_1);
            SEAL_OF_THE_CRUSADER = InitSpell(Spells.SEAL_OF_THE_CRUSADER_1);
            SENSE_UNDEAD = InitSpell(Spells.SENSE_UNDEAD_1);
            SHADOW_RESISTANCE_AURA = InitSpell(Spells.SHADOW_RESISTANCE_AURA_1);
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
