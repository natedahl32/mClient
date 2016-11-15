using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class WarlockLogic : PlayerClassLogic
    {
        #region Constructors

        public WarlockLogic(Player player) : base(player)
        {
        }

        #endregion

        #region Warlock Constants

        public static class Stones
        {
            public const uint FIRESTONE = 13699;
            public const uint LESSER_FIRESTONE = 1254;
            public const uint GREATER_FIRESTONE = 13700;
            public const uint MAJOR_FIRESTONE = 13701;

            public const uint SPELLSTONE = 5522;
            public const uint GREATER_SPELLSTONE = 13602;
            public const uint MAJOR_SPELLSTONE = 13603;

            public const uint MINOR_SOULSTONE = 5232;
            public const uint LESSER_SOULSTONE = 16892;
            public const uint SOULSTONE = 16893;
            public const uint GREATER_SOULSTONE = 16895;
            public const uint MAJOR_SOULSTONE = 16896;
        }

        public static class Demons
        {
            public const uint DEMON_IMP = 416;
            public const uint DEMON_VOIDWALKER = 1860;
            public const uint DEMON_SUCCUBUS = 1863;
            public const uint DEMON_FELHUNTER = 417;
        }

        public static class DemonSpells
        {
            public const uint BLOOD_PACT_1 = 6307;
            public const uint FIREBOLT_1 = 3110;
            public const uint FIRE_SHIELD_1 = 2947;
            // Felhunter
            public const uint DEVOUR_MAGIC_1 = 19505;
            public const uint SPELL_LOCK_1 = 19244;
            // Succubus
            public const uint LASH_OF_PAIN_1 = 7814;
            public const uint SEDUCTION_1 = 6358;
            public const uint SOOTHING_KISS_1 = 6360;
            // Voidwalker
            public const uint CONSUME_SHADOWS_1 = 17767;
            public const uint SACRIFICE_1 = 7812;
            public const uint SUFFERING_1 = 17735;
            public const uint TORMENT_1 = 3716;
        }

        public static class Spells
        {
            public const uint AMPLIFY_CURSE_1 = 18288;
            public const uint BANISH_1 = 710;
            public const uint CONFLAGRATE_1 = 17962;
            public const uint CORRUPTION_1 = 172;
            public const uint CREATE_FIRESTONE_1 = 6366;
            public const uint CREATE_HEALTHSTONE_1 = 6201;
            public const uint CREATE_SOULSTONE_1 = 693;
            public const uint CREATE_SPELLSTONE_1 = 2362;
            public const uint CURSE_OF_AGONY_1 = 980;
            public const uint CURSE_OF_DOOM_1 = 603;
            public const uint CURSE_OF_EXHAUSTION_1 = 18223;
            public const uint CURSE_OF_RECKLESSNESS_1 = 704;
            public const uint CURSE_OF_SHADOW_1 = 17862;
            public const uint CURSE_OF_THE_ELEMENTS_1 = 1490;
            public const uint CURSE_OF_TONGUES_1 = 1714;
            public const uint CURSE_OF_WEAKNESS_1 = 702;
            public const uint DARK_PACT_1 = 18220;
            public const uint DEATH_COIL_WARLOCK_1 = 6789;
            public const uint DEMON_ARMOR_1 = 706;
            public const uint DEMON_SKIN_1 = 687;
            public const uint DETECT_INVISIBILITY_1 = 132;
            public const uint DRAIN_LIFE_1 = 689;
            public const uint DRAIN_MANA_1 = 5138;
            public const uint DRAIN_SOUL_1 = 1120;
            public const uint ENSLAVE_DEMON_1 = 1098;
            public const uint EYE_OF_KILROGG_1 = 126;
            public const uint FEAR_1 = 5782;
            public const uint FEL_DOMINATION_1 = 18708;
            public const uint HEALTH_FUNNEL_1 = 755;
            public const uint HELLFIRE_1 = 1949;
            public const uint HOWL_OF_TERROR_1 = 5484;
            public const uint IMMOLATE_1 = 348;
            public const uint INFERNO_1 = 1122;
            public const uint LIFE_TAP_1 = 1454;
            public const uint RAIN_OF_FIRE_1 = 5740;
            public const uint RITUAL_OF_DOOM_1 = 18540;
            public const uint RITUAL_OF_SUMMONING_1 = 698;
            public const uint SEARING_PAIN_1 = 5676;
            public const uint SENSE_DEMONS_1 = 5500;
            public const uint SHADOW_BOLT_1 = 686;
            public const uint SHADOW_WARD_1 = 6229;
            public const uint SHADOWBURN_1 = 17877;
            public const uint SHOOT_3 = 5019;
            public const uint SIPHON_LIFE_1 = 18265;
            public const uint SOUL_FIRE_1 = 6353;
            public const uint SOUL_LINK_1 = 19028;
            public const uint SUMMON_FELHUNTER_1 = 691;
            public const uint SUMMON_IMP_1 = 688;
            public const uint SUMMON_SUCCUBUS_1 = 712;
            public const uint SUMMON_VOIDWALKER_1 = 697;
            public const uint UNENDING_BREATH_1 = 5697;
        }

        #endregion
    }
}
