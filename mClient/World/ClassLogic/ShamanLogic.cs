using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class ShamanLogic : PlayerClassLogic
    {
        #region Constructors

        public ShamanLogic(Player player) : base(player)
        {
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
