using System;
using System.ComponentModel.DataAnnotations;

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

    public enum PlayerFlags
    {
        PLAYER_FLAGS_NONE = 0x00000000,
        PLAYER_FLAGS_GROUP_LEADER = 0x00000001,
        PLAYER_FLAGS_AFK = 0x00000002,
        PLAYER_FLAGS_DND = 0x00000004,
        PLAYER_FLAGS_GM = 0x00000008,
        PLAYER_FLAGS_GHOST = 0x00000010,
        PLAYER_FLAGS_RESTING = 0x00000020,
        PLAYER_FLAGS_UNK7 = 0x00000040,       // admin?
        PLAYER_FLAGS_FFA_PVP = 0x00000080,
        PLAYER_FLAGS_CONTESTED_PVP = 0x00000100,       // Player has been involved in a PvP combat and will be attacked by contested guards
        PLAYER_FLAGS_IN_PVP = 0x00000200,
        PLAYER_FLAGS_HIDE_HELM = 0x00000400,
        PLAYER_FLAGS_HIDE_CLOAK = 0x00000800,
        PLAYER_FLAGS_PARTIAL_PLAY_TIME = 0x00001000,       // played long time
        PLAYER_FLAGS_NO_PLAY_TIME = 0x00002000,       // played too long time
        PLAYER_FLAGS_UNK15 = 0x00004000,
        PLAYER_FLAGS_UNK16 = 0x00008000,       // strange visual effect (2.0.1), looks like PLAYER_FLAGS_GHOST flag
        PLAYER_FLAGS_SANCTUARY = 0x00010000,       // player entered sanctuary
        PLAYER_FLAGS_TAXI_BENCHMARK = 0x00020000,       // taxi benchmark mode (on/off) (2.0.1)
        PLAYER_FLAGS_PVP_TIMER = 0x00040000,       // 3.0.2, pvp timer active (after you disable pvp manually)
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
        [Display(Name = "")]
        NONE = 0,
        [Display(Name = "Fire")]
        MAGE_SPEC_FIRE = 41,
        [Display(Name = "Frost")]
        MAGE_SPEC_FROST = 61,
        [Display(Name = "Arcane")]
        MAGE_SPEC_ARCANE = 81,
        [Display(Name = "Arms")]
        WARRIOR_SPEC_ARMS = 161,
        [Display(Name = "Protection")]
        WARRIOR_SPEC_PROTECTION = 163,
        [Display(Name = "Fury")]
        WARRIOR_SPEC_FURY = 164,
        [Display(Name = "Combat")]
        ROGUE_SPEC_COMBAT = 181,
        [Display(Name = "Assassination")]
        ROGUE_SPEC_ASSASSINATION = 182,
        [Display(Name = "Subtelty")]
        ROGUE_SPEC_SUBTELTY = 183,
        [Display(Name = "Discipline")]
        PRIEST_SPEC_DISCIPLINE = 201,
        [Display(Name = "Holy")]
        PRIEST_SPEC_HOLY = 202,
        [Display(Name = "Shadow")]
        PRIEST_SPEC_SHADOW = 203,
        [Display(Name = "Elemental")]
        SHAMAN_SPEC_ELEMENTAL = 261,
        [Display(Name = "Restoration")]
        SHAMAN_SPEC_RESTORATION = 262,
        [Display(Name = "Enhancement")]
        SHAMAN_SPEC_ENHANCEMENT = 263,
        [Display(Name = "Feral")]
        DRUID_SPEC_FERAL = 281,
        [Display(Name = "Restoration")]
        DRUID_SPEC_RESTORATION = 282,
        [Display(Name = "Balance")]
        DRUID_SPEC_BALANCE = 283,
        [Display(Name = "Destruction")]
        WARLOCK_SPEC_DESTRUCTION = 301,
        [Display(Name = "Affliction")]
        WARLOCK_SPEC_AFFLICTION = 302,
        [Display(Name = "Demonology")]
        WARLOCK_SPEC_DEMONOLOGY = 303,
        [Display(Name = "Beast Mastery")]
        HUNTER_SPEC_BEASTMASTERY = 361,
        [Display(Name = "Survival")]
        HUNTER_SPEC_SURVIVAL = 362,
        [Display(Name = "Marksmanship")]
        HUNTER_SPEC_MARKSMANSHIP = 363,
        [Display(Name = "Retribution")]
        PALADIN_SPEC_RETRIBUTION = 381,
        [Display(Name = "Holy")]
        PALADIN_SPEC_HOLY = 382,
        [Display(Name = "Protection")]
        PALADIN_SPEC_PROTECTION = 383
    }

    public enum UnitStandStateType
    {
        UNIT_STAND_STATE_STAND = 0,
        UNIT_STAND_STATE_SIT = 1,
        UNIT_STAND_STATE_SIT_CHAIR = 2,
        UNIT_STAND_STATE_SLEEP = 3,
        UNIT_STAND_STATE_SIT_LOW_CHAIR = 4,
        UNIT_STAND_STATE_SIT_MEDIUM_CHAIR = 5,
        UNIT_STAND_STATE_SIT_HIGH_CHAIR = 6,
        UNIT_STAND_STATE_DEAD = 7,
        UNIT_STAND_STATE_KNEEL = 8
    }
}
