using System;

namespace mClient.Constants
{
    public static class PlayerConstants
    {
        public const int PLAYER_MAX_SKILLS = 127;
        public const int PLAYER_EXPLORED_ZONES_SIZE = 64;

        public const int MAX_POWERS = 5;
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

    public enum Stats
    {
        STAT_STRENGTH = 0,
        STAT_AGILITY = 1,
        STAT_STAMINA = 2,
        STAT_INTELLECT = 3,
        STAT_SPIRIT = 4
    }

    public enum MainSpec
    {
        NONE = 0,
        MAGE_SPEC_FIRE = 41,
        MAGE_SPEC_FROST = 61,
        MAGE_SPEC_ARCANE = 81,
        WARRIOR_SPEC_ARMS = 161,
        WARRIOR_SPEC_PROTECTION = 163,
        WARRIOR_SPEC_FURY = 164,
        ROGUE_SPEC_COMBAT = 181,
        ROGUE_SPEC_ASSASSINATION = 182,
        ROGUE_SPEC_SUBTELTY = 183,
        PRIEST_SPEC_DISCIPLINE = 201,
        PRIEST_SPEC_HOLY = 202,
        PRIEST_SPEC_SHADOW = 203,
        SHAMAN_SPEC_ELEMENTAL = 261,
        SHAMAN_SPEC_RESTORATION = 262,
        SHAMAN_SPEC_ENHANCEMENT = 263,
        DRUID_SPEC_FERAL = 281,
        DRUID_SPEC_RESTORATION = 282,
        DRUID_SPEC_BALANCE = 283,
        WARLOCK_SPEC_DESTRUCTION = 301,
        WARLOCK_SPEC_AFFLICTION = 302,
        WARLOCK_SPEC_DEMONOLOGY = 303,
        HUNTER_SPEC_BEASTMASTERY = 361,
        HUNTER_SPEC_SURVIVAL = 362,
        HUNTER_SPEC_MARKSMANSHIP = 363,
        PALADIN_SPEC_RETRIBUTION = 381,
        PALADIN_SPEC_HOLY = 382,
        PALADIN_SPEC_PROTECTION = 383
    }
}
