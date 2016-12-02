using mClient.Clients;
using mClient.Constants;
using mClient.DBC;
using mClient.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class ShamanLogic : PlayerClassLogic
    {
        #region Declarations

        // ENHANCEMENT
        protected uint ROCKBITER_WEAPON,
               STONESKIN_TOTEM,
               LIGHTNING_SHIELD,
               FLAMETONGUE_WEAPON,
               STRENGTH_OF_EARTH_TOTEM,
               FOCUSED,
               FROSTBRAND_WEAPON,
               FROST_RESISTANCE_TOTEM,
               FLAMETONGUE_TOTEM,
               FIRE_RESISTANCE_TOTEM,
               WINDFURY_WEAPON,
               GROUNDING_TOTEM,
               NATURE_RESISTANCE_TOTEM,
               WIND_FURY_TOTEM,
               STORMSTRIKE,
               WRATH_OF_AIR_TOTEM,
               EARTH_ELEMENTAL_TOTEM,
               BLOODLUST;

        // RESTORATION
        protected uint HEALING_WAVE,
               LESSER_HEALING_WAVE,
               ANCESTRAL_SPIRIT,
               TREMOR_TOTEM,
               HEALING_STREAM_TOTEM,
               MANA_SPRING_TOTEM,
               CHAIN_HEAL,
               MANA_TIDE_TOTEM,
               EARTH_SHIELD,
               CURE_DISEASE_SHAMAN,
               CURE_POISON_SHAMAN,
               NATURES_SWIFTNESS_SHAMAN;

        // ELEMENTAL
        protected uint LIGHTNING_BOLT,
               EARTH_SHOCK,
               STONECLAW_TOTEM,
               FLAME_SHOCK,
               SEARING_TOTEM,
               PURGE,
               FIRE_NOVA_TOTEM,
               FROST_SHOCK,
               MAGMA_TOTEM,
               CHAIN_LIGHTNING,
               FIRE_ELEMENTAL_TOTEM,
               EARTHBIND_TOTEM,
               ELEMENTAL_MASTERY,
               ASTRAL_RECALL,
               CLEANSING_TOTEM,
               GHOST_WOLF,
               SENTRY_TOTEM,
               WATER_BREATHING;

        // totem buffs
        protected uint STRENGTH_OF_EARTH_EFFECT,
               FLAMETONGUE_EFFECT,
               MAGMA_TOTEM_EFFECT,
               STONECLAW_EFFECT,
               FIRE_RESISTANCE_EFFECT,
               FROST_RESISTANCE_EFFECT,
               GROUDNING_EFFECT,
               NATURE_RESISTANCE_EFFECT,
               STONESKIN_EFFECT,
               WINDFURY_EFFECT,
               WRATH_OF_AIR_EFFECT,
               CLEANSING_TOTEM_EFFECT,
               HEALING_STREAM_EFFECT,
               MANA_SPRING_EFFECT,
               TREMOR_TOTEM_EFFECT,
               EARTHBIND_EFFECT;

        #endregion

        #region Constructors

        public ShamanLogic(Player player) : base(player)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of this class
        /// </summary>
        public override string ClassName
        {
            get { return "Shaman"; }
        }

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
            ANCESTRAL_SPIRIT = InitSpell(Spells.ANCESTRAL_SPIRIT_1);
            ASTRAL_RECALL = InitSpell(Spells.ASTRAL_RECALL_1);
            BLOODLUST = InitSpell(Spells.BLOODLUST_1);
            CHAIN_HEAL = InitSpell(Spells.CHAIN_HEAL_1);
            CHAIN_LIGHTNING = InitSpell(Spells.CHAIN_LIGHTNING_1);
            CLEANSING_TOTEM = InitSpell(Spells.CLEANSING_TOTEM_1);
            CURE_DISEASE_SHAMAN = InitSpell(Spells.CURE_DISEASE_SHAMAN_1);
            CURE_POISON_SHAMAN = InitSpell(Spells.CURE_POISON_SHAMAN_1);
            EARTH_ELEMENTAL_TOTEM = InitSpell(Spells.EARTH_ELEMENTAL_TOTEM_1);
            EARTH_SHIELD = InitSpell(Spells.EARTH_SHIELD_1);
            EARTH_SHOCK = InitSpell(Spells.EARTH_SHOCK_1);
            EARTHBIND_TOTEM = InitSpell(Spells.EARTHBIND_TOTEM_1);
            ELEMENTAL_MASTERY = InitSpell(Spells.ELEMENTAL_MASTERY_1);
            FIRE_ELEMENTAL_TOTEM = InitSpell(Spells.FIRE_ELEMENTAL_TOTEM_1);
            FIRE_NOVA_TOTEM = InitSpell(Spells.FIRE_NOVA_1);
            FIRE_RESISTANCE_TOTEM = InitSpell(Spells.FIRE_RESISTANCE_TOTEM_1);
            FLAME_SHOCK = InitSpell(Spells.FLAME_SHOCK_1);
            FLAMETONGUE_TOTEM = InitSpell(Spells.FLAMETONGUE_TOTEM_1);
            FLAMETONGUE_WEAPON = InitSpell(Spells.FLAMETONGUE_WEAPON_1);
            FROST_RESISTANCE_TOTEM = InitSpell(Spells.FROST_RESISTANCE_TOTEM_1);
            FROST_SHOCK = InitSpell(Spells.FROST_SHOCK_1);
            FROSTBRAND_WEAPON = InitSpell(Spells.FROSTBRAND_WEAPON_1);
            GHOST_WOLF = InitSpell(Spells.GHOST_WOLF_1);
            GROUNDING_TOTEM = InitSpell(Spells.GROUNDING_TOTEM_1);
            HEALING_STREAM_TOTEM = InitSpell(Spells.HEALING_STREAM_TOTEM_1);
            HEALING_WAVE = InitSpell(Spells.HEALING_WAVE_1);
            LESSER_HEALING_WAVE = InitSpell(Spells.LESSER_HEALING_WAVE_1);
            LIGHTNING_BOLT = InitSpell(Spells.LIGHTNING_BOLT_1);
            LIGHTNING_SHIELD = InitSpell(Spells.LIGHTNING_SHIELD_1);
            MAGMA_TOTEM = InitSpell(Spells.MAGMA_TOTEM_1);
            MANA_SPRING_TOTEM = InitSpell(Spells.MANA_SPRING_TOTEM_1);
            MANA_TIDE_TOTEM = InitSpell(Spells.MANA_TIDE_TOTEM_1);
            NATURE_RESISTANCE_TOTEM = InitSpell(Spells.NATURE_RESISTANCE_TOTEM_1);
            NATURES_SWIFTNESS_SHAMAN = InitSpell(Spells.NATURES_SWIFTNESS_SHAMAN_1);
            PURGE = InitSpell(Spells.PURGE_1);
            ROCKBITER_WEAPON = InitSpell(Spells.ROCKBITER_WEAPON_1);
            SEARING_TOTEM = InitSpell(Spells.SEARING_TOTEM_1);
            SENTRY_TOTEM = InitSpell(Spells.SENTRY_TOTEM_1);
            STONECLAW_TOTEM = InitSpell(Spells.STONECLAW_TOTEM_1);
            STONESKIN_TOTEM = InitSpell(Spells.STONESKIN_TOTEM_1);
            STORMSTRIKE = InitSpell(Spells.STORMSTRIKE_1);
            STRENGTH_OF_EARTH_TOTEM = InitSpell(Spells.STRENGTH_OF_EARTH_TOTEM_1);
            TREMOR_TOTEM = InitSpell(Spells.TREMOR_TOTEM_1);
            WATER_BREATHING = InitSpell(Spells.WATER_BREATHING_1);
            WIND_FURY_TOTEM = InitSpell(Spells.WINDFURY_TOTEM_1);
            WINDFURY_WEAPON = InitSpell(Spells.WINDFURY_WEAPON_1);
            WRATH_OF_AIR_TOTEM = InitSpell(Spells.WRATH_OF_AIR_TOTEM_1);

            // Totem effects
            STRENGTH_OF_EARTH_EFFECT = InitSpell(Spells.STRENGTH_OF_EARTH_EFFECT_1);
            FLAMETONGUE_EFFECT = InitSpell(Spells.FLAMETONGUE_EFFECT_1);
            MAGMA_TOTEM_EFFECT = InitSpell(Spells.MAGMA_TOTEM_EFFECT_1);
            STONECLAW_EFFECT = InitSpell(Spells.STONECLAW_EFFECT_1);
            FIRE_RESISTANCE_EFFECT = InitSpell(Spells.FIRE_RESISTANCE_EFFECT_1);
            FROST_RESISTANCE_EFFECT = InitSpell(Spells.FROST_RESISTANCE_EFFECT_1);
            GROUDNING_EFFECT = InitSpell(Spells.GROUDNING_EFFECT_1);
            NATURE_RESISTANCE_EFFECT = InitSpell(Spells.NATURE_RESISTANCE_EFFECT_1);
            STONESKIN_EFFECT = InitSpell(Spells.STONESKIN_EFFECT_1);
            WINDFURY_EFFECT = InitSpell(Spells.WINDFURY_EFFECT_1);
            WRATH_OF_AIR_EFFECT = InitSpell(Spells.WRATH_OF_AIR_EFFECT_1);
            CLEANSING_TOTEM_EFFECT = InitSpell(Spells.CLEANSING_TOTEM_EFFECT_1);
            MANA_SPRING_EFFECT = InitSpell(Spells.MANA_SPRING_EFFECT_1);
            TREMOR_TOTEM_EFFECT = InitSpell(Spells.TREMOR_TOTEM_EFFECT_1);
            EARTHBIND_EFFECT = InitSpell(Spells.EARTHBIND_EFFECT_1);
        }

        public override float CompareItems(ItemInfo item1, ItemInfo item2)
        {
            // Get the base value of the compare
            var baseCompare = base.CompareItems(item1, item2);

            float item1Score = 0f;
            float item2Score = 0f;

            // Compare DPS of a weapon for druids in feral spec
            if (Player.TalentSpec == MainSpec.SHAMAN_SPEC_ENHANCEMENT)
            {
                if (item1.ItemClass == ItemClass.ITEM_CLASS_WEAPON && item2.ItemClass == ItemClass.ITEM_CLASS_WEAPON)
                {
                    // TODO: Calculate slow vs fast on mainhand/offhand weapons
                    item1Score += (item1.DPS * 0.9f);
                    item2Score += (item2.DPS * 0.9f);
                }
            }

            // Reduce armor score so it isn't overvalued. There can be a lot on items
            float item1Armor = item1.Resistances[SpellSchools.SPELL_SCHOOL_NORMAL] / 20f;
            float item2Armor = item2.Resistances[SpellSchools.SPELL_SCHOOL_NORMAL] / 20f;

            item1Score += (item1Armor * 0.1f);
            item2Score += (item2Armor * 0.1f);

            var newCompare = item1Score - item2Score;
            return baseCompare + newCompare;
        }

        #endregion

        #region Private Methods

        protected override void SetStatWeights()
        {
            base.SetStatWeights();

            if (Player.TalentSpec == MainSpec.SHAMAN_SPEC_ELEMENTAL)
            {
                mStatWeights[ItemModType.ITEM_MOD_STAMINA] = 0.45f;
                mStatWeights[ItemModType.ITEM_MOD_SPIRIT] = 0.1f;
                mStatWeights[ItemModType.ITEM_MOD_INTELLECT] = 0.9f;
                mStatWeights[ItemModType.ITEM_MOD_STRENGTH] = 0.01f;
                mStatWeights[ItemModType.ITEM_MOD_AGILITY] = 0.01f;
                mStatWeights[ItemModType.ITEM_MOD_MANA] = 0.6f;
                mStatWeights[ItemModType.ITEM_MOD_HEALTH] = 0.5f;
            }
            else if (Player.TalentSpec == MainSpec.SHAMAN_SPEC_ENHANCEMENT)
            {
                mStatWeights[ItemModType.ITEM_MOD_STAMINA] = 0.45f;
                mStatWeights[ItemModType.ITEM_MOD_SPIRIT] = 0.01f;
                mStatWeights[ItemModType.ITEM_MOD_INTELLECT] = 0.3f;
                mStatWeights[ItemModType.ITEM_MOD_STRENGTH] = 0.7f;
                mStatWeights[ItemModType.ITEM_MOD_AGILITY] = 0.9f;
                mStatWeights[ItemModType.ITEM_MOD_MANA] = 0.35f;
                mStatWeights[ItemModType.ITEM_MOD_HEALTH] = 0.5f;
            }
            else if (Player.TalentSpec == MainSpec.SHAMAN_SPEC_RESTORATION)
            {
                mStatWeights[ItemModType.ITEM_MOD_STAMINA] = 0.65f;
                mStatWeights[ItemModType.ITEM_MOD_SPIRIT] = 0.2f;
                mStatWeights[ItemModType.ITEM_MOD_INTELLECT] = 0.9f;
                mStatWeights[ItemModType.ITEM_MOD_STRENGTH] = 0.01f;
                mStatWeights[ItemModType.ITEM_MOD_AGILITY] = 0.01f;
                mStatWeights[ItemModType.ITEM_MOD_MANA] = 0.7f;
                mStatWeights[ItemModType.ITEM_MOD_HEALTH] = 0.6f;
            }
        }

        #endregion

        #region Rogue Constants

        public static class Reagents
        {
            public const uint ANKH = 17030;
        }

        public static class Spells
        {
            public const uint ANCESTRAL_SPIRIT_1 = 2008;
            public const uint ASTRAL_RECALL_1 = 556;
            public const uint BLOODLUST_1 = 2825;
            public const uint CHAIN_HEAL_1 = 1064;
            public const uint CHAIN_LIGHTNING_1 = 421;
            public const uint CLEANSING_TOTEM_1 = 8170;
            public const uint CURE_DISEASE_SHAMAN_1 = 2870;
            public const uint CURE_POISON_SHAMAN_1 = 526;
            public const uint EARTH_ELEMENTAL_TOTEM_1 = 2062;
            public const uint EARTH_SHIELD_1 = 974;
            public const uint EARTH_SHOCK_1 = 8042;
            public const uint EARTHBIND_TOTEM_1 = 2484;
            public const uint ELEMENTAL_MASTERY_1 = 16166;
            public const uint FIRE_ELEMENTAL_TOTEM_1 = 2894;
            public const uint FIRE_NOVA_1 = 1535;
            public const uint FIRE_RESISTANCE_TOTEM_1 = 8184;
            public const uint FLAME_SHOCK_1 = 8050;
            public const uint FLAMETONGUE_TOTEM_1 = 8227;
            public const uint FLAMETONGUE_WEAPON_1 = 8024;
            public const uint FROST_RESISTANCE_TOTEM_1 = 8181;
            public const uint FROST_SHOCK_1 = 8056;
            public const uint FROSTBRAND_WEAPON_1 = 8033;
            public const uint GHOST_WOLF_1 = 2645;
            public const uint GROUNDING_TOTEM_1 = 8177;
            public const uint HEALING_STREAM_TOTEM_1 = 5394;
            public const uint HEALING_WAVE_1 = 331;
            public const uint LESSER_HEALING_WAVE_1 = 8004;
            public const uint LIGHTNING_BOLT_1 = 403;
            public const uint LIGHTNING_SHIELD_1 = 324;
            public const uint MAGMA_TOTEM_1 = 8190;
            public const uint MANA_SPRING_TOTEM_1 = 5675;
            public const uint MANA_TIDE_TOTEM_1 = 16190;
            public const uint NATURE_RESISTANCE_TOTEM_1 = 10595;
            public const uint NATURES_SWIFTNESS_SHAMAN_1 = 16188;
            public const uint PURGE_1 = 370;
            public const uint ROCKBITER_WEAPON_1 = 8017;
            public const uint SEARING_TOTEM_1 = 3599;
            public const uint SENTRY_TOTEM_1 = 6495;
            public const uint STONECLAW_TOTEM_1 = 5730;
            public const uint STONESKIN_TOTEM_1 = 8071;
            public const uint STORMSTRIKE_1 = 17364;
            public const uint STRENGTH_OF_EARTH_TOTEM_1 = 8075;
            public const uint TREMOR_TOTEM_1 = 8143;
            public const uint WATER_BREATHING_1 = 131;
            public const uint WATER_WALKING_1 = 546;
            public const uint WINDFURY_TOTEM_1 = 8512;
            public const uint WINDFURY_WEAPON_1 = 8232;
            public const uint WRATH_OF_AIR_TOTEM_1 = 3738;

            //Totem Buffs
            public const uint STRENGTH_OF_EARTH_EFFECT_1 = 8076;
            public const uint FLAMETONGUE_EFFECT_1 = 8026;
            public const uint MAGMA_TOTEM_EFFECT_1 = 8188;
            public const uint STONECLAW_EFFECT_1 = 5728;
            public const uint FIRE_RESISTANCE_EFFECT_1 = 8185;
            public const uint FROST_RESISTANCE_EFFECT_1 = 8182;
            public const uint GROUDNING_EFFECT_1 = 8178;
            public const uint NATURE_RESISTANCE_EFFECT_1 = 10596;
            public const uint STONESKIN_EFFECT_1 = 8072;
            public const uint WINDFURY_EFFECT_1 = 8515;
            public const uint WRATH_OF_AIR_EFFECT_1 = 2895;
            public const uint CLEANSING_TOTEM_EFFECT_1 = 8172;
            public const uint MANA_SPRING_EFFECT_1 = 5677;
            public const uint TREMOR_TOTEM_EFFECT_1 = 8145;
            public const uint EARTHBIND_EFFECT_1 = 6474;
        }

        #endregion
    }
}
