using System;

namespace mClient.Constants
{
    public static class PlayerConstants
    {
        public const int PLAYER_MAX_SKILLS = 127;
        public const int PLAYER_EXPLORED_ZONES_SIZE = 64;
    }

    public static class RacialTraits
    {
        public const uint BERSERKING_ALL = 26297;
        public const uint BLOOD_FURY_ALL = 20572;
        public const uint ESCAPE_ARTIST_ALL = 20589;
        public const uint PERCEPTION_ALL = 20600;
        public const uint SHADOWMELD_ALL = 20580;
        public const uint STONEFORM_ALL = 20594;
        public const uint WAR_STOMP_ALL = 20549;
        public const uint WILL_OF_THE_FORSAKEN_ALL = 7744;
    }

    public enum Powers : UInt32
    {
        POWER_MANA = 0,
        POWER_RAGE = 1,
        POWER_FOCUS = 2,
        POWER_ENERGY = 3,
        POWER_HAPPINESS = 4,
        POWER_HEALTH = 0xFFFFFFFE
    }
}
