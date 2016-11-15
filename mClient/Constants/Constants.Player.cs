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

    public enum PowerType : UInt32
    {
        Mana = 0,
        Rage = 1,
        Focus = 2,
        Energy = 3,
        Happiness = 4,
        Health = 0xFFFFFFFE
    }
}
