using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class PriestLogic : PlayerClassLogic
    {
        #region Declarations

        // holy
        protected uint CLEARCASTING,
               DESPERATE_PRAYER,
               FLASH_HEAL,
               GREATER_HEAL,
               HEAL,
               HOLY_FIRE,
               HOLY_NOVA,
               LESSER_HEAL,
               MANA_BURN,
               PRAYER_OF_HEALING,
               RENEW,
               RESURRECTION,
               SHACKLE_UNDEAD,
               SMITE,
               CURE_DISEASE,
               ABOLISH_DISEASE,
               PRIEST_DISPEL_MAGIC;

        // ranged
        protected uint SHOOT;

        // shadowmagic
        protected uint FADE,
               SHADOW_WORD_PAIN,
               MIND_BLAST,
               SCREAM,
               MIND_FLAY,
               DEVOURING_PLAGUE,
               SHADOW_PROTECTION,
               PRAYER_OF_SHADOW_PROTECTION,
               SHADOWFORM,
               VAMPIRIC_EMBRACE;

        // discipline
        protected uint POWER_WORD_SHIELD,
               INNER_FIRE,
               POWER_WORD_FORTITUDE,
               PRAYER_OF_FORTITUDE,
               FEAR_WARD,
               POWER_INFUSION,
               MASS_DISPEL,
               DIVINE_SPIRIT,
               PRAYER_OF_SPIRIT,
               INNER_FOCUS,
               ELUNES_GRACE,
               LEVITATE,
               LIGHTWELL,
               MIND_CONTROL,
               MIND_SOOTHE,
               MIND_VISION,
               PSYCHIC_SCREAM,
               SILENCE;

        #endregion

        #region Constructors

        public PriestLogic(Player player) : base(player)
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
                return false;
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
            ABOLISH_DISEASE = InitSpell(Spells.ABOLISH_DISEASE_1);
            CURE_DISEASE = InitSpell(Spells.CURE_DISEASE_1);
            DESPERATE_PRAYER = InitSpell(Spells.DESPERATE_PRAYER_1);
            DEVOURING_PLAGUE = InitSpell(Spells.DEVOURING_PLAGUE_1);
            PRIEST_DISPEL_MAGIC = InitSpell(Spells.DISPEL_MAGIC_1);
            DIVINE_SPIRIT = InitSpell(Spells.DIVINE_SPIRIT_1);
            ELUNES_GRACE = InitSpell(Spells.ELUNES_GRACE_1);
            FADE = InitSpell(Spells.FADE_1);
            FEAR_WARD = InitSpell(Spells.FEAR_WARD_1);
            FLASH_HEAL = InitSpell(Spells.FLASH_HEAL_1);
            GREATER_HEAL = InitSpell(Spells.GREATER_HEAL_1);
            HEAL = InitSpell(Spells.HEAL_1);
            HOLY_FIRE = InitSpell(Spells.HOLY_FIRE_1);
            HOLY_NOVA = InitSpell(Spells.HOLY_NOVA_1);
            INNER_FIRE = InitSpell(Spells.INNER_FIRE_1);
            INNER_FOCUS = InitSpell(Spells.INNER_FOCUS_1);
            LESSER_HEAL = InitSpell(Spells.LESSER_HEAL_1);
            LEVITATE = InitSpell(Spells.LEVITATE_1);
            LIGHTWELL = InitSpell(Spells.LIGHTWELL_1);
            MANA_BURN = InitSpell(Spells.MANA_BURN_1);
            MIND_BLAST = InitSpell(Spells.MIND_BLAST_1);
            MIND_CONTROL = InitSpell(Spells.MIND_CONTROL_1);
            MIND_FLAY = InitSpell(Spells.MIND_FLAY_1);
            MIND_SOOTHE = InitSpell(Spells.MIND_SOOTHE_1);
            MIND_VISION = InitSpell(Spells.MIND_VISION_1);
            POWER_INFUSION = InitSpell(Spells.POWER_INFUSION_1);
            POWER_WORD_FORTITUDE = InitSpell(Spells.POWER_WORD_FORTITUDE_1);
            POWER_WORD_SHIELD = InitSpell(Spells.POWER_WORD_SHIELD_1);
            PRAYER_OF_FORTITUDE = InitSpell(Spells.PRAYER_OF_FORTITUDE_1);
            PRAYER_OF_HEALING = InitSpell(Spells.PRAYER_OF_HEALING_1);
            PRAYER_OF_SHADOW_PROTECTION = InitSpell(Spells.PRAYER_OF_SHADOW_PROTECTION_1);
            PRAYER_OF_SPIRIT = InitSpell(Spells.PRAYER_OF_SPIRIT_1);
            PSYCHIC_SCREAM = InitSpell(Spells.PSYCHIC_SCREAM_1);
            RENEW = InitSpell(Spells.RENEW_1);
            RESURRECTION = InitSpell(Spells.RESURRECTION_1);
            SHACKLE_UNDEAD = InitSpell(Spells.SHACKLE_UNDEAD_1);
            SHADOW_PROTECTION = InitSpell(Spells.SHADOW_PROTECTION_1);
            SHADOW_WORD_PAIN = InitSpell(Spells.SHADOW_WORD_PAIN_1);
            SHADOWFORM = InitSpell(Spells.SHADOWFORM_1);
            SHOOT = InitSpell(Spells.SHOOT_1);
            SMITE = InitSpell(Spells.SMITE_1);
            SILENCE = InitSpell(Spells.SILENCE_1);
            VAMPIRIC_EMBRACE = InitSpell(Spells.VAMPIRIC_EMBRACE_1);
        }

        #endregion

        #region Priest Constants

        public static class Reagents
        {
            public const uint SACRED_CANDLE = 17029;
        }

        public static class Spells
        {
            public const uint ABOLISH_DISEASE_1 = 552;
            public const uint CURE_DISEASE_1 = 528;
            public const uint DESPERATE_PRAYER_1 = 19236;
            public const uint DEVOURING_PLAGUE_1 = 2944;
            public const uint DISPEL_MAGIC_1 = 527;
            public const uint DIVINE_SPIRIT_1 = 14752;
            public const uint ELUNES_GRACE_1 = 2651;
            public const uint FADE_1 = 586;
            public const uint FEAR_WARD_1 = 6346;
            public const uint FLASH_HEAL_1 = 2061;
            public const uint GREATER_HEAL_1 = 2060;
            public const uint HEAL_1 = 2054;
            public const uint HOLY_FIRE_1 = 14914;
            public const uint HOLY_NOVA_1 = 15237;
            public const uint INNER_FIRE_1 = 588;
            public const uint INNER_FOCUS_1 = 14751;
            public const uint LESSER_HEAL_1 = 2050;
            public const uint LEVITATE_1 = 1706;
            public const uint LIGHTWELL_1 = 724;
            public const uint MANA_BURN_1 = 8129;
            public const uint MIND_BLAST_1 = 8092;
            public const uint MIND_CONTROL_1 = 605;
            public const uint MIND_FLAY_1 = 15407;
            public const uint MIND_SOOTHE_1 = 453;
            public const uint MIND_VISION_1 = 2096;
            public const uint POWER_INFUSION_1 = 10060;
            public const uint POWER_WORD_FORTITUDE_1 = 1243;
            public const uint POWER_WORD_SHIELD_1 = 17;
            public const uint PRAYER_OF_FORTITUDE_1 = 21562;
            public const uint PRAYER_OF_HEALING_1 = 596;
            public const uint PRAYER_OF_SHADOW_PROTECTION_1 = 27683;
            public const uint PRAYER_OF_SPIRIT_1 = 27681;
            public const uint PSYCHIC_SCREAM_1 = 8122;
            public const uint RENEW_1 = 139;
            public const uint RESURRECTION_1 = 2006;
            public const uint SHACKLE_UNDEAD_1 = 9484;
            public const uint SHADOW_PROTECTION_1 = 976;
            public const uint SHADOW_WORD_PAIN_1 = 589;
            public const uint SHADOWFORM_1 = 15473;
            public const uint SHOOT_1 = 5019;
            public const uint SILENCE_1 = 15487;
            public const uint SMITE_1 = 585;
            public const uint VAMPIRIC_EMBRACE_1 = 15286;
            public const uint WEAKNED_SOUL = 6788;
        }

        #endregion
    }
}
