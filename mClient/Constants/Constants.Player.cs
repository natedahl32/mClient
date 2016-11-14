using System;

namespace mClient.Constants
{
    public static class PlayerConstants
    {
        public const int PLAYER_MAX_SKILLS = 127;
        public const int PLAYER_EXPLORED_ZONES_SIZE = 64;
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
