using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class HunterLogic : PlayerClassLogic
    {
        #region Constructors

        public HunterLogic(Player player) : base(player)
        {
        }

        #endregion

        #region Hunter Constants

        public static class Spells
        {
            public const uint ARCANE_SHOT_1 = 3044;
            public const uint ASPECT_OF_THE_BEAST_1 = 13161;
            public const uint ASPECT_OF_THE_CHEETAH_1 = 5118;
            public const uint ASPECT_OF_THE_HAWK_1 = 13165;
            public const uint ASPECT_OF_THE_MONKEY_1 = 13163;
            public const uint ASPECT_OF_THE_PACK_1 = 13159;
            public const uint ASPECT_OF_THE_WILD_1 = 20043;
            public const uint AUTO_SHOT_1 = 75;
            public const uint BEAST_LORE_1 = 1462;
            public const uint CALL_PET_1 = 883;
            public const uint CONCUSSIVE_SHOT_1 = 5116;
            public const uint DETERRENCE_1 = 19263;
            public const uint DISENGAGE_1 = 781;
            public const uint DISMISS_PET_1 = 2641;
            public const uint DISTRACTING_SHOT_1 = 20736;
            public const uint EAGLE_EYE_1 = 6197;
            public const uint EXPLOSIVE_TRAP_1 = 13813;
            public const uint EYES_OF_THE_BEAST_1 = 1002;
            public const uint FEED_PET_1 = 6991;
            public const uint FEIGN_DEATH_1 = 5384;
            public const uint FLARE_1 = 1543;
            public const uint FREEZING_TRAP_1 = 1499;
            public const uint FROST_TRAP_1 = 13809;
            public const uint HUNTERS_MARK_1 = 1130;
            public const uint IMMOLATION_TRAP_1 = 13795;
            public const uint MEND_PET_1 = 136;
            public const uint MONGOOSE_BITE_1 = 1495;
            public const uint MULTISHOT_1 = 2643;
            public const uint RAPID_FIRE_1 = 3045;
            public const uint RAPTOR_STRIKE_1 = 2973;
            public const uint REVIVE_PET_1 = 982;
            public const uint SCARE_BEAST_1 = 1513;
            public const uint SCORPID_STING_1 = 3043;
            public const uint SERPENT_STING_1 = 1978;
            public const uint TAME_BEAST_1 = 1515;
            public const uint TRACK_BEASTS_1 = 1494;
            public const uint TRACK_DEMONS_1 = 19878;
            public const uint TRACK_DRAGONKIN_1 = 19879;
            public const uint TRACK_ELEMENTALS_1 = 19880;
            public const uint TRACK_GIANTS_1 = 19882;
            public const uint TRACK_HIDDEN_1 = 19885;
            public const uint TRACK_HUMANOIDS_1 = 19883;
            public const uint TRACK_UNDEAD_1 = 19884;
            public const uint TRANQUILIZING_SHOT_1 = 19801;
            public const uint VIPER_STING_1 = 3034;
            public const uint VOLLEY_1 = 1510;
            public const uint WING_CLIP_1 = 2974;
            public const uint AIMED_SHOT_1 = 19434;
            public const uint BESTIAL_WRATH_1 = 19574;
            public const uint BLACK_ARROW_1 = 3674;
            public const uint COUNTERATTACK_1 = 19306;
            public const uint INTIMIDATION_1 = 19577;
            public const uint READINESS_1 = 23989;
            public const uint SCATTER_SHOT_1 = 19503;
            public const uint TRUESHOT_AURA_1 = 19506;
            public const uint WYVERN_STING_1 = 19386;
        }

        #endregion
    }
}
