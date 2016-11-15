using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class DruidLogic : PlayerClassLogic
    {
        #region Constructors

        public DruidLogic(Player player) : base(player)
        {
        }

        #endregion

        #region Druid Constants

        public static class Reagents
        {
            public const uint WILD_THORNROOT = 17026;
        }

        public static class Procs
        {
            public const uint ECLIPSE_SOLAR_1 = 48517;
            public const uint ECLIPSE_LUNAR_1 = 48518;
        }

        public static class Spells
        {
            public const uint ABOLISH_POISON_1 = 2893;
            public const uint AQUATIC_FORM_1 = 1066;
            public const uint BARKSKIN_1 = 22812;
            public const uint BASH_1 = 5211;
            public const uint BEAR_FORM_1 = 5487;
            public const uint CAT_FORM_1 = 768;
            public const uint CHALLENGING_ROAR_1 = 5209;
            public const uint CLAW_1 = 1082;
            public const uint COWER_1 = 8998;
            public const uint CURE_POISON_1 = 8946;
            public const uint DASH_1 = 1850;
            public const uint DEMORALIZING_ROAR_1 = 99;
            public const uint DIRE_BEAR_FORM_1 = 9634;
            public const uint ENRAGE_1 = 5229;
            public const uint ENTANGLING_ROOTS_1 = 339;
            public const uint FAERIE_FIRE_1 = 770;
            public const uint FAERIE_FIRE_FERAL_1 = 16857;
            public const uint FERAL_CHARGE_BEAR_1 = 16979;
            public const uint FEROCIOUS_BITE_1 = 22568;
            public const uint FRENZIED_REGENERATION_1 = 22842;
            public const uint GIFT_OF_THE_WILD_1 = 21849;
            public const uint GROWL_1 = 6795;
            public const uint HEALING_TOUCH_1 = 5185;
            public const uint HIBERNATE_1 = 2637;
            public const uint HURRICANE_1 = 16914;
            public const uint INNERVATE_1 = 29166;
            public const uint INSECT_SWARM_1 = 5570;
            public const uint MARK_OF_THE_WILD_1 = 1126;
            public const uint MAUL_1 = 6807;
            public const uint MOONFIRE_1 = 8921;
            public const uint MOONKIN_FORM_1 = 24858;
            public const uint NATURES_GRASP_1 = 16689;
            public const uint NATURES_SWIFTNESS_DRUID_1 = 17116;
            public const uint OMEN_OF_CLARITY_1 = 16864;
            public const uint POUNCE_1 = 9005;
            public const uint PROWL_1 = 5215;
            public const uint RAKE_1 = 1822;
            public const uint RAVAGE_1 = 6785;
            public const uint REBIRTH_1 = 20484;
            public const uint REGROWTH_1 = 8936;
            public const uint REJUVENATION_1 = 774;
            public const uint REMOVE_CURSE_DRUID_1 = 2782;
            public const uint RIP_1 = 1079;
            public const uint SHRED_1 = 5221;
            public const uint SOOTHE_ANIMAL_1 = 2908;
            public const uint STARFIRE_1 = 2912;
            public const uint SWIFTMEND_1 = 18562;
            public const uint SWIPE_BEAR_1 = 779;
            public const uint THORNS_1 = 467;
            public const uint TIGERS_FURY_1 = 5217;
            public const uint TRANQUILITY_1 = 740;
            public const uint TRAVEL_FORM_1 = 783;
            public const uint WRATH_1 = 5176;

            public const uint ECLIPSE_1 = 48525;
        }

        #endregion
    }
}
