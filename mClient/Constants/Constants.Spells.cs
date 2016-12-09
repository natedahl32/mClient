using System;

namespace mClient.Constants
{
    public static class SpellAuras
    {
        public const int GHOST_1 = 8326;
        public const int GHOST_2 = 9036;
        public const int GHOST_WISP = 20584;
    }

    public static class SkillConstants
    {
        public const int MAX_SKILL_TYPE = 763;
    }

    public static class SpellConstants
    {
        public const int MAX_SPELL_SCHOOL = 7;
        public const int MAX_EFFECT_INDEX = 3;
        public const int MAX_TALENT_RANK = 5;

        public const int OPEN_SPELL_ID = 6478;

        public const int MAX_AURAS = UnitFields.UNIT_FIELD_AURA_LAST - UnitFields.UNIT_FIELD_AURA;
    }

    public enum SpellEffectIndex
    {
        EFFECT_INDEX_0 = 0,
        EFFECT_INDEX_1 = 1,
        EFFECT_INDEX_2 = 2
    }

    public enum SpellSchools
    {
        SPELL_SCHOOL_NORMAL = 0,                //< Physical, Armor
        SPELL_SCHOOL_HOLY = 1,
        SPELL_SCHOOL_FIRE = 2,
        SPELL_SCHOOL_NATURE = 3,
        SPELL_SCHOOL_FROST = 4,
        SPELL_SCHOOL_SHADOW = 5,
        SPELL_SCHOOL_ARCANE = 6
    };

    public enum AbilitySkillFlags
    {
        ABILITY_SKILL_NONTRAINABLE = 0x100
    }

    [Flags]
    public enum SpellSchoolMask
    {
        SPELL_SCHOOL_MASK_NONE = 0x00,                       // not exist
        SPELL_SCHOOL_MASK_NORMAL = (1 << SpellSchools.SPELL_SCHOOL_NORMAL), // PHYSICAL (Armor)
        SPELL_SCHOOL_MASK_HOLY = (1 << SpellSchools.SPELL_SCHOOL_HOLY),
        SPELL_SCHOOL_MASK_FIRE = (1 << SpellSchools.SPELL_SCHOOL_FIRE),
        SPELL_SCHOOL_MASK_NATURE = (1 << SpellSchools.SPELL_SCHOOL_NATURE),
        SPELL_SCHOOL_MASK_FROST = (1 << SpellSchools.SPELL_SCHOOL_FROST),
        SPELL_SCHOOL_MASK_SHADOW = (1 << SpellSchools.SPELL_SCHOOL_SHADOW),
        SPELL_SCHOOL_MASK_ARCANE = (1 << SpellSchools.SPELL_SCHOOL_ARCANE),

        // unions

        // 124, not include normal and holy damage
        SPELL_SCHOOL_MASK_SPELL = (SPELL_SCHOOL_MASK_FIRE |
                                     SPELL_SCHOOL_MASK_NATURE | SPELL_SCHOOL_MASK_FROST |
                                     SPELL_SCHOOL_MASK_SHADOW | SPELL_SCHOOL_MASK_ARCANE),
        // 126
        SPELL_SCHOOL_MASK_MAGIC = (SPELL_SCHOOL_MASK_HOLY | SPELL_SCHOOL_MASK_SPELL),

        // 127
        SPELL_SCHOOL_MASK_ALL = (SPELL_SCHOOL_MASK_NORMAL | SPELL_SCHOOL_MASK_MAGIC)
    }


    // Data from SpellLine.dbc (1.12.1 checked)
    public enum SkillType
    {
        SKILL_NONE = 0,

        SKILL_FROST = 6,
        SKILL_FIRE = 8,
        SKILL_ARMS = 26,
        SKILL_COMBAT = 38,
        SKILL_SUBTLETY = 39,
        SKILL_POISONS = 40,
        SKILL_SWORDS = 43,
        SKILL_AXES = 44,
        SKILL_BOWS = 45,
        SKILL_GUNS = 46,
        SKILL_BEAST_MASTERY = 50,
        SKILL_SURVIVAL = 51,
        SKILL_MACES = 54,
        SKILL_2H_SWORDS = 55,
        SKILL_HOLY = 56,
        SKILL_SHADOW = 78,
        SKILL_DEFENSE = 95,
        SKILL_LANG_COMMON = 98,
        SKILL_RACIAL_DWARVEN = 101,
        SKILL_LANG_ORCISH = 109,
        SKILL_LANG_DWARVEN = 111,
        SKILL_LANG_DARNASSIAN = 113,
        SKILL_LANG_TAURAHE = 115,
        SKILL_DUAL_WIELD = 118,
        SKILL_RACIAL_TAUREN = 124,
        SKILL_ORC_RACIAL = 125,
        SKILL_RACIAL_NIGHT_ELF = 126,
        SKILL_FIRST_AID = 129,
        SKILL_FERAL_COMBAT = 134,
        SKILL_STAVES = 136,
        SKILL_LANG_THALASSIAN = 137,
        SKILL_LANG_DRACONIC = 138,
        SKILL_LANG_DEMON_TONGUE = 139,
        SKILL_LANG_TITAN = 140,
        SKILL_LANG_OLD_TONGUE = 141,
        SKILL_SURVIVAL2 = 142,
        SKILL_RIDING_HORSE = 148,
        SKILL_RIDING_WOLF = 149,
        SKILL_RIDING_TIGER = 150,
        SKILL_RIDING_RAM = 152,
        SKILL_SWIMING = 155,
        SKILL_2H_MACES = 160,
        SKILL_UNARMED = 162,
        SKILL_MARKSMANSHIP = 163,
        SKILL_BLACKSMITHING = 164,
        SKILL_LEATHERWORKING = 165,
        SKILL_ALCHEMY = 171,
        SKILL_2H_AXES = 172,
        SKILL_DAGGERS = 173,
        SKILL_THROWN = 176,
        SKILL_HERBALISM = 182,
        SKILL_GENERIC_DND = 183,
        SKILL_RETRIBUTION = 184,
        SKILL_COOKING = 185,
        SKILL_MINING = 186,
        SKILL_PET_IMP = 188,
        SKILL_PET_FELHUNTER = 189,
        SKILL_TAILORING = 197,
        SKILL_ENGINEERING = 202,
        SKILL_PET_SPIDER = 203,
        SKILL_PET_VOIDWALKER = 204,
        SKILL_PET_SUCCUBUS = 205,
        SKILL_PET_INFERNAL = 206,
        SKILL_PET_DOOMGUARD = 207,
        SKILL_PET_WOLF = 208,
        SKILL_PET_CAT = 209,
        SKILL_PET_BEAR = 210,
        SKILL_PET_BOAR = 211,
        SKILL_PET_CROCILISK = 212,
        SKILL_PET_CARRION_BIRD = 213,
        SKILL_PET_CRAB = 214,
        SKILL_PET_GORILLA = 215,
        SKILL_PET_RAPTOR = 217,
        SKILL_PET_TALLSTRIDER = 218,
        SKILL_RACIAL_UNDED = 220,
        SKILL_CROSSBOWS = 226,
        SKILL_WANDS = 228,
        SKILL_POLEARMS = 229,
        SKILL_PET_SCORPID = 236,
        SKILL_ARCANE = 237,
        SKILL_PET_TURTLE = 251,
        SKILL_ASSASSINATION = 253,
        SKILL_FURY = 256,
        SKILL_PROTECTION = 257,
        SKILL_BEAST_TRAINING = 261,
        SKILL_PROTECTION2 = 267,
        SKILL_PET_TALENTS = 270,
        SKILL_PLATE_MAIL = 293,
        SKILL_LANG_GNOMISH = 313,
        SKILL_LANG_TROLL = 315,
        SKILL_ENCHANTING = 333,
        SKILL_DEMONOLOGY = 354,
        SKILL_AFFLICTION = 355,
        SKILL_FISHING = 356,
        SKILL_ENHANCEMENT = 373,
        SKILL_RESTORATION = 374,
        SKILL_ELEMENTAL_COMBAT = 375,
        SKILL_SKINNING = 393,
        SKILL_MAIL = 413,
        SKILL_LEATHER = 414,
        SKILL_CLOTH = 415,
        SKILL_SHIELD = 433,
        SKILL_FIST_WEAPONS = 473,
        SKILL_RIDING_RAPTOR = 533,
        SKILL_RIDING_MECHANOSTRIDER = 553,
        SKILL_RIDING_UNDEAD_HORSE = 554,
        SKILL_RESTORATION2 = 573,
        SKILL_BALANCE = 574,
        SKILL_DESTRUCTION = 593,
        SKILL_HOLY2 = 594,
        SKILL_DISCIPLINE = 613,
        SKILL_LOCKPICKING = 633,
        SKILL_PET_BAT = 653,
        SKILL_PET_HYENA = 654,
        SKILL_PET_OWL = 655,
        SKILL_PET_WIND_SERPENT = 656,
        SKILL_LANG_GUTTERSPEAK = 673,
        SKILL_RIDING_KODO = 713,
        SKILL_RACIAL_TROLL = 733,
        SKILL_RACIAL_GNOME = 753,
        SKILL_RACIAL_HUMAN = 754,
        SKILL_PET_EVENT_RC = 758,
        SKILL_RIDING = 762,
    }

    public enum SpellCastResult
    {
        SPELL_FAILED_AFFECTING_COMBAT = 0x00,
        SPELL_FAILED_ALREADY_AT_FULL_HEALTH = 0x01,
        SPELL_FAILED_ALREADY_AT_FULL_MANA = 0x02,
        SPELL_FAILED_ALREADY_BEING_TAMED = 0x03,
        SPELL_FAILED_ALREADY_HAVE_CHARM = 0x04,
        SPELL_FAILED_ALREADY_HAVE_SUMMON = 0x05,
        SPELL_FAILED_ALREADY_OPEN = 0x06,
        SPELL_FAILED_MORE_POWERFUL_SPELL_ACTIVE = 0x07,
        // SPELL_FAILED_AUTOTRACK_INTERRUPTED          = 0x08, old commented CAST_FAIL_FAILED = 8,-> 29
        SPELL_FAILED_BAD_IMPLICIT_TARGETS = 0x09,
        SPELL_FAILED_BAD_TARGETS = 0x0A,
        SPELL_FAILED_CANT_BE_CHARMED = 0x0B,
        SPELL_FAILED_CANT_BE_DISENCHANTED = 0x0C,
        SPELL_FAILED_CANT_BE_PROSPECTED = 0x0D,
        SPELL_FAILED_CANT_CAST_ON_TAPPED = 0x0E,
        SPELL_FAILED_CANT_DUEL_WHILE_INVISIBLE = 0x0F,
        SPELL_FAILED_CANT_DUEL_WHILE_STEALTHED = 0x10,
        SPELL_FAILED_CANT_TOO_CLOSE_TO_ENEMY = 0x11,
        SPELL_FAILED_CANT_DO_THAT_YET = 0x12,
        SPELL_FAILED_CASTER_DEAD = 0x13,
        SPELL_FAILED_CHARMED = 0x14,
        SPELL_FAILED_CHEST_IN_USE = 0x15,
        SPELL_FAILED_CONFUSED = 0x16,
        SPELL_FAILED_DONT_REPORT = 0x17,     // [-ZERO] need check
        SPELL_FAILED_EQUIPPED_ITEM = 0x18,
        SPELL_FAILED_EQUIPPED_ITEM_CLASS = 0x19,
        SPELL_FAILED_EQUIPPED_ITEM_CLASS_MAINHAND = 0x1A,
        SPELL_FAILED_EQUIPPED_ITEM_CLASS_OFFHAND = 0x1B,
        SPELL_FAILED_ERROR = 0x1C,
        SPELL_FAILED_FIZZLE = 0x1D,
        SPELL_FAILED_FLEEING = 0x1E,
        SPELL_FAILED_FOOD_LOWLEVEL = 0x1F,
        SPELL_FAILED_HIGHLEVEL = 0x20,
        // SPELL_FAILED_HUNGER_SATIATED                = 0x21,
        SPELL_FAILED_IMMUNE = 0x22,
        SPELL_FAILED_INTERRUPTED = 0x23,
        SPELL_FAILED_INTERRUPTED_COMBAT = 0x24,
        SPELL_FAILED_ITEM_ALREADY_ENCHANTED = 0x25,
        SPELL_FAILED_ITEM_GONE = 0x26,
        SPELL_FAILED_ENCHANT_NOT_EXISTING_ITEM = 0x27,
        SPELL_FAILED_ITEM_NOT_READY = 0x28,
        SPELL_FAILED_LEVEL_REQUIREMENT = 0x29,
        SPELL_FAILED_LINE_OF_SIGHT = 0x2A,
        SPELL_FAILED_LOWLEVEL = 0x2B,
        SPELL_FAILED_SKILL_NOT_HIGH_ENOUGH = 0x2C,
        SPELL_FAILED_MAINHAND_EMPTY = 0x2D,
        SPELL_FAILED_MOVING = 0x2E,
        SPELL_FAILED_NEED_AMMO = 0x2F,
        SPELL_FAILED_NEED_REQUIRES_SOMETHING = 0x30,
        SPELL_FAILED_NEED_EXOTIC_AMMO = 0x31,
        SPELL_FAILED_NOPATH = 0x32,
        SPELL_FAILED_NOT_BEHIND = 0x33,
        SPELL_FAILED_NOT_FISHABLE = 0x34,
        SPELL_FAILED_NOT_HERE = 0x35,
        SPELL_FAILED_NOT_INFRONT = 0x36,
        SPELL_FAILED_NOT_IN_CONTROL = 0x37,
        SPELL_FAILED_NOT_KNOWN = 0x38,
        SPELL_FAILED_NOT_MOUNTED = 0x39,
        SPELL_FAILED_NOT_ON_TAXI = 0x3A,
        SPELL_FAILED_NOT_ON_TRANSPORT = 0x3B,
        SPELL_FAILED_NOT_READY = 0x3C,
        SPELL_FAILED_NOT_SHAPESHIFT = 0x3D,
        SPELL_FAILED_NOT_STANDING = 0x3E,
        SPELL_FAILED_NOT_TRADEABLE = 0x3F,     // rogues trying "enchant" other's weapon with poison
        SPELL_FAILED_NOT_TRADING = 0x40,     // CAST_FAIL_CANT_ENCHANT_TRADE_ITEM
        SPELL_FAILED_NOT_UNSHEATHED = 0x41,     // yellow text
        SPELL_FAILED_NOT_WHILE_GHOST = 0x42,
        SPELL_FAILED_NO_AMMO = 0x43,
        SPELL_FAILED_NO_CHARGES_REMAIN = 0x44,
        SPELL_FAILED_NO_CHAMPION = 0x45,     // CAST_FAIL_NOT_SELECT
        SPELL_FAILED_NO_COMBO_POINTS = 0x46,
        SPELL_FAILED_NO_DUELING = 0x47,
        SPELL_FAILED_NO_ENDURANCE = 0x48,
        SPELL_FAILED_NO_FISH = 0x49,
        SPELL_FAILED_NO_ITEMS_WHILE_SHAPESHIFTED = 0x4A,
        SPELL_FAILED_NO_MOUNTS_ALLOWED = 0x4B,
        SPELL_FAILED_NO_PET = 0x4C,
        SPELL_FAILED_NO_POWER = 0x4D,     // CAST_FAIL_NOT_ENOUGH_MANA
        SPELL_FAILED_NOTHING_TO_DISPEL = 0x4E,
        SPELL_FAILED_NOTHING_TO_STEAL = 0x4F,
        SPELL_FAILED_ONLY_ABOVEWATER = 0x50,     // CAST_FAIL_CANT_USE_WHILE_SWIMMING
        SPELL_FAILED_ONLY_DAYTIME = 0x51,
        SPELL_FAILED_ONLY_INDOORS = 0x52,
        SPELL_FAILED_ONLY_MOUNTED = 0x53,
        SPELL_FAILED_ONLY_NIGHTTIME = 0x54,
        SPELL_FAILED_ONLY_OUTDOORS = 0x55,
        SPELL_FAILED_ONLY_SHAPESHIFT = 0x56,
        SPELL_FAILED_ONLY_STEALTHED = 0x57,
        SPELL_FAILED_ONLY_UNDERWATER = 0x58,     // CAST_FAIL_CAN_ONLY_USE_WHILE_SWIMMING
        SPELL_FAILED_OUT_OF_RANGE = 0x59,
        SPELL_FAILED_PACIFIED = 0x5A,
        SPELL_FAILED_POSSESSED = 0x5B,
        // SPELL_FAILED_REAGENTS                       = 0x5C, [-ZERO] not in 1.12
        SPELL_FAILED_REQUIRES_AREA = 0x5D,     // CAST_FAIL_YOU_NEED_TO_BE_IN_XXX
        SPELL_FAILED_REQUIRES_SPELL_FOCUS = 0x5E,     // CAST_FAIL_REQUIRES_XXX
        SPELL_FAILED_ROOTED = 0x5F,     // CAST_FAIL_UNABLE_TO_MOVE
        SPELL_FAILED_SILENCED = 0x60,
        SPELL_FAILED_SPELL_IN_PROGRESS = 0x61,
        SPELL_FAILED_SPELL_LEARNED = 0x62,
        SPELL_FAILED_SPELL_UNAVAILABLE = 0x63,
        SPELL_FAILED_STUNNED = 0x64,
        SPELL_FAILED_TARGETS_DEAD = 0x65,
        SPELL_FAILED_TARGET_AFFECTING_COMBAT = 0x66,
        SPELL_FAILED_TARGET_AURASTATE = 0x67,     // CAST_FAIL_CANT_DO_THAT_YET_2
        SPELL_FAILED_TARGET_DUELING = 0x68,
        SPELL_FAILED_TARGET_ENEMY = 0x69,
        SPELL_FAILED_TARGET_ENRAGED = 0x6A,     // CAST_FAIL_TARGET_IS_TOO_ENRAGED_TO_CHARM
        SPELL_FAILED_TARGET_FRIENDLY = 0x6B,
        SPELL_FAILED_TARGET_IN_COMBAT = 0x6C,
        SPELL_FAILED_TARGET_IS_PLAYER = 0x6D,
        SPELL_FAILED_TARGET_NOT_DEAD = 0x6E,
        SPELL_FAILED_TARGET_NOT_IN_PARTY = 0x6F,
        SPELL_FAILED_TARGET_NOT_LOOTED = 0x70,     // CAST_FAIL_CREATURE_MUST_BE_LOOTED_FIRST
        SPELL_FAILED_TARGET_NOT_PLAYER = 0x71,
        SPELL_FAILED_TARGET_NO_POCKETS = 0x72,     // CAST_FAIL_NOT_ITEM_TO_STEAL
        SPELL_FAILED_TARGET_NO_WEAPONS = 0x73,
        SPELL_FAILED_TARGET_UNSKINNABLE = 0x74,
        SPELL_FAILED_THIRST_SATIATED = 0x75,
        SPELL_FAILED_TOO_CLOSE = 0x76,
        SPELL_FAILED_TOO_MANY_OF_ITEM = 0x77,
        // SPELL_FAILED_TOTEMS                         = 0x78,  // [-ZERO] not in 1.12
        SPELL_FAILED_TRAINING_POINTS = 0x79,
        SPELL_FAILED_TRY_AGAIN = 0x7A,     // CAST_FAIL_FAILED_ATTEMPT
        SPELL_FAILED_UNIT_NOT_BEHIND = 0x7B,
        SPELL_FAILED_UNIT_NOT_INFRONT = 0x7C,
        SPELL_FAILED_WRONG_PET_FOOD = 0x7D,
        SPELL_FAILED_NOT_WHILE_FATIGUED = 0x7E,
        SPELL_FAILED_TARGET_NOT_IN_INSTANCE = 0x7F,     // CAST_FAIL_TARGET_MUST_BE_IN_THIS_INSTANCE
        SPELL_FAILED_NOT_WHILE_TRADING = 0x80,
        SPELL_FAILED_TARGET_NOT_IN_RAID = 0x81,
        SPELL_FAILED_DISENCHANT_WHILE_LOOTING = 0x82,
        SPELL_FAILED_PROSPECT_WHILE_LOOTING = 0x83,
        //  SPELL_FAILED_PROSPECT_NEED_MORE             = 0x85,
        SPELL_FAILED_TARGET_FREEFORALL = 0x85,
        SPELL_FAILED_NO_EDIBLE_CORPSES = 0x86,
        SPELL_FAILED_ONLY_BATTLEGROUNDS = 0x87,
        SPELL_FAILED_TARGET_NOT_GHOST = 0x88,
        SPELL_FAILED_TOO_MANY_SKILLS = 0x89,     // CAST_FAIL_YOUR_PET_CANT_LEARN_MORE_SKILLS
        SPELL_FAILED_CANT_USE_NEW_ITEM = 0x8A,
        SPELL_FAILED_WRONG_WEATHER = 0x8B,     // CAST_FAIL_CANT_DO_IN_THIS_WEATHER
        SPELL_FAILED_DAMAGE_IMMUNE = 0x8C,     // CAST_FAIL_CANT_DO_IN_IMMUNE
        SPELL_FAILED_PREVENTED_BY_MECHANIC = 0x8D,     // CAST_FAIL_CANT_DO_IN_XXX
        SPELL_FAILED_PLAY_TIME = 0x8E,     // CAST_FAIL_GAME_TIME_OVER
        SPELL_FAILED_REPUTATION = 0x8F,
        SPELL_FAILED_MIN_SKILL = 0x90,
        SPELL_FAILED_UNKNOWN = 0x91,

        SPELL_CAST_OK = 0xFF      // custom value, don't must be send to client
    }

    [Flags]
    public enum SpellAttributes : uint
    {
        SPELL_ATTR_UNK0 = 0x00000001,            // 0
        SPELL_ATTR_RANGED = 0x00000002,            // 1 All ranged abilites have this flag
        SPELL_ATTR_ON_NEXT_SWING_1 = 0x00000004,            // 2 on next swing
        SPELL_ATTR_UNK3 = 0x00000008,            // 3 not set in 2.4.2
        SPELL_ATTR_UNK4 = 0x00000010,            // 4 isAbility
        SPELL_ATTR_TRADESPELL = 0x00000020,            // 5 trade spells, will be added by client to a sublist of profession spell
        SPELL_ATTR_PASSIVE = 0x00000040,            // 6 Passive spell
        SPELL_ATTR_UNK7 = 0x00000080,            // 7 can't be linked in chat?
        SPELL_ATTR_UNK8 = 0x00000100,            // 8 hide created item in tooltip (for effect=24)
        SPELL_ATTR_UNK9 = 0x00000200,            // 9
        SPELL_ATTR_ON_NEXT_SWING_2 = 0x00000400,            // 10 on next swing 2
        SPELL_ATTR_UNK11 = 0x00000800,            // 11
        SPELL_ATTR_DAYTIME_ONLY = 0x00001000,            // 12 only useable at daytime, not set in 2.4.2
        SPELL_ATTR_NIGHT_ONLY = 0x00002000,            // 13 only useable at night, not set in 2.4.2
        SPELL_ATTR_INDOORS_ONLY = 0x00004000,            // 14 only useable indoors, not set in 2.4.2
        SPELL_ATTR_OUTDOORS_ONLY = 0x00008000,            // 15 Only useable outdoors.
        SPELL_ATTR_NOT_SHAPESHIFT = 0x00010000,            // 16 Not while shapeshifted
        SPELL_ATTR_ONLY_STEALTHED = 0x00020000,            // 17 Must be in stealth
        SPELL_ATTR_UNK18 = 0x00040000,            // 18
        SPELL_ATTR_LEVEL_DAMAGE_CALCULATION = 0x00080000,            // 19 spelldamage depends on caster level
        SPELL_ATTR_STOP_ATTACK_TARGET = 0x00100000,            // 20 Stop attack after use this spell (and not begin attack if use)
        SPELL_ATTR_IMPOSSIBLE_DODGE_PARRY_BLOCK = 0x00200000,            // 21 Cannot be dodged/parried/blocked
        SPELL_ATTR_SET_TRACKING_TARGET = 0x00400000,            // 22 SetTrackingTarget
        SPELL_ATTR_UNK23 = 0x00800000,            // 23 castable while dead?
        SPELL_ATTR_CASTABLE_WHILE_MOUNTED = 0x01000000,            // 24 castable while mounted
        SPELL_ATTR_DISABLED_WHILE_ACTIVE = 0x02000000,            // 25 Activate and start cooldown after aura fade or remove summoned creature or go
        SPELL_ATTR_UNK26 = 0x04000000,            // 26
        SPELL_ATTR_CASTABLE_WHILE_SITTING = 0x08000000,            // 27 castable while sitting
        SPELL_ATTR_CANT_USED_IN_COMBAT = 0x10000000,            // 28 Cannot be used in combat
        SPELL_ATTR_UNAFFECTED_BY_INVULNERABILITY = 0x20000000,            // 29 unaffected by invulnerability (hmm possible not...)
        SPELL_ATTR_UNK30 = 0x40000000,            // 30 breakable by damage?
        SPELL_ATTR_CANT_CANCEL = 0x80000000            // 31 positive aura can't be canceled
    }

    [Flags]
    public enum SpellAttributesEx : uint
    {
        SPELL_ATTR_EX_UNK0 = 0x00000001,            // 0
        SPELL_ATTR_EX_DRAIN_ALL_POWER = 0x00000002,            // 1 use all power (Only paladin Lay of Hands and Bunyanize)
        SPELL_ATTR_EX_CHANNELED_1 = 0x00000004,            // 2 channeled 1
        SPELL_ATTR_EX_UNK3 = 0x00000008,            // 3
        SPELL_ATTR_EX_UNK4 = 0x00000010,            // 4
        SPELL_ATTR_EX_NOT_BREAK_STEALTH = 0x00000020,            // 5 Not break stealth
        SPELL_ATTR_EX_CHANNELED_2 = 0x00000040,            // 6 channeled 2
        SPELL_ATTR_EX_NEGATIVE = 0x00000080,            // 7
        SPELL_ATTR_EX_NOT_IN_COMBAT_TARGET = 0x00000100,            // 8 Spell req target not to be in combat state
        SPELL_ATTR_EX_UNK9 = 0x00000200,            // 9
        SPELL_ATTR_EX_NO_THREAT = 0x00000400,            // 10 no generates threat on cast 100%
        SPELL_ATTR_EX_UNK11 = 0x00000800,            // 11
        SPELL_ATTR_EX_UNK12 = 0x00001000,            // 12
        SPELL_ATTR_EX_FARSIGHT = 0x00002000,            // 13 related to farsight
        SPELL_ATTR_EX_UNK14 = 0x00004000,            // 14
        SPELL_ATTR_EX_DISPEL_AURAS_ON_IMMUNITY = 0x00008000,            // 15 remove auras on immunity
        SPELL_ATTR_EX_UNAFFECTED_BY_SCHOOL_IMMUNE = 0x00010000,            // 16 unaffected by school immunity
        SPELL_ATTR_EX_UNK17 = 0x00020000,            // 17 for auras SPELL_AURA_TRACK_CREATURES, SPELL_AURA_TRACK_RESOURCES and SPELL_AURA_TRACK_STEALTHED select non-stacking tracking spells
        SPELL_ATTR_EX_UNK18 = 0x00040000,            // 18
        SPELL_ATTR_EX_CANT_TARGET_SELF = 0x00080000,            // 19 spells with area effect or friendly targets that exclude the caster
        SPELL_ATTR_EX_REQ_TARGET_COMBO_POINTS = 0x00100000,            // 20 Req combo points on target
        SPELL_ATTR_EX_UNK21 = 0x00200000,            // 21
        SPELL_ATTR_EX_REQ_COMBO_POINTS = 0x00400000,            // 22 Use combo points (in 4.x not required combo point target selected)
        SPELL_ATTR_EX_UNK23 = 0x00800000,            // 23
        SPELL_ATTR_EX_UNK24 = 0x01000000,            // 24 Req fishing pole??
        SPELL_ATTR_EX_UNK25 = 0x02000000,            // 25 not set in 2.4.2
        SPELL_ATTR_EX_UNK26 = 0x04000000,            // 26
        SPELL_ATTR_EX_UNK27 = 0x08000000,            // 27
        SPELL_ATTR_EX_UNK28 = 0x10000000,            // 28
        SPELL_ATTR_EX_UNK29 = 0x20000000,            // 29
        SPELL_ATTR_EX_UNK30 = 0x40000000,            // 30 overpower
        SPELL_ATTR_EX_UNK31 = 0x80000000            // 31
    };

    [Flags]
    public enum SpellAttributesEx2 : uint
    {
        SPELL_ATTR_EX2_UNK0 = 0x00000001,            // 0
        SPELL_ATTR_EX2_UNK1 = 0x00000002,            // 1
        SPELL_ATTR_EX2_CANT_REFLECTED = 0x00000004,            // 2 ? used for detect can or not spell reflected // do not need LOS (e.g. 18220 since 3.3.3)
        SPELL_ATTR_EX2_UNK3 = 0x00000008,            // 3 auto targeting? (e.g. fishing skill enhancement items since 3.3.3)
        SPELL_ATTR_EX2_UNK4 = 0x00000010,            // 4
        SPELL_ATTR_EX2_AUTOREPEAT_FLAG = 0x00000020,            // 5
        SPELL_ATTR_EX2_UNK6 = 0x00000040,            // 6 only usable on tabbed by yourself
        SPELL_ATTR_EX2_UNK7 = 0x00000080,            // 7
        SPELL_ATTR_EX2_UNK8 = 0x00000100,            // 8 not set in 2.4.2
        SPELL_ATTR_EX2_UNK9 = 0x00000200,            // 9
        SPELL_ATTR_EX2_UNK10 = 0x00000400,            // 10
        SPELL_ATTR_EX2_HEALTH_FUNNEL = 0x00000800,            // 11
        SPELL_ATTR_EX2_UNK12 = 0x00001000,            // 12
        SPELL_ATTR_EX2_UNK13 = 0x00002000,            // 13
        SPELL_ATTR_EX2_UNK14 = 0x00004000,            // 14
        SPELL_ATTR_EX2_UNK15 = 0x00008000,            // 15 not set in 2.4.2
        SPELL_ATTR_EX2_UNK16 = 0x00010000,            // 16
        SPELL_ATTR_EX2_UNK17 = 0x00020000,            // 17 suspend weapon timer instead of resetting it, (?Hunters Shot and Stings only have this flag?)
        SPELL_ATTR_EX2_UNK18 = 0x00040000,            // 18 Only Revive pet - possible req dead pet
        SPELL_ATTR_EX2_NOT_NEED_SHAPESHIFT = 0x00080000,            // 19 does not necessary need shapeshift (pre-3.x not have passive spells with this attribute)
        SPELL_ATTR_EX2_UNK20 = 0x00100000,            // 20
        SPELL_ATTR_EX2_DAMAGE_REDUCED_SHIELD = 0x00200000,            // 21 for ice blocks, pala immunity buffs, priest absorb shields, but used also for other spells -> not sure!
        SPELL_ATTR_EX2_UNK22 = 0x00400000,            // 22
        SPELL_ATTR_EX2_UNK23 = 0x00800000,            // 23 Only mage Arcane Concentration have this flag
        SPELL_ATTR_EX2_UNK24 = 0x01000000,            // 24
        SPELL_ATTR_EX2_UNK25 = 0x02000000,            // 25
        SPELL_ATTR_EX2_UNK26 = 0x04000000,            // 26 unaffected by school immunity
        SPELL_ATTR_EX2_UNK27 = 0x08000000,            // 27
        SPELL_ATTR_EX2_UNK28 = 0x10000000,            // 28 no breaks stealth if it fails??
        SPELL_ATTR_EX2_CANT_CRIT = 0x20000000,            // 29 Spell can't crit
        SPELL_ATTR_EX2_UNK30 = 0x40000000,            // 30
        SPELL_ATTR_EX2_FOOD_BUFF = 0x80000000            // 31 Food or Drink Buff (like Well Fed)
    }

    [Flags]
    public enum SpellAttributesEx3 : uint
    {
        SPELL_ATTR_EX3_UNK0 = 0x00000001,            // 0
        SPELL_ATTR_EX3_UNK1 = 0x00000002,            // 1
        SPELL_ATTR_EX3_UNK2 = 0x00000004,            // 2
        SPELL_ATTR_EX3_UNK3 = 0x00000008,            // 3
        SPELL_ATTR_EX3_UNK4 = 0x00000010,            // 4 Druid Rebirth only this spell have this flag
        SPELL_ATTR_EX3_UNK5 = 0x00000020,            // 5
        SPELL_ATTR_EX3_UNK6 = 0x00000040,            // 6
        SPELL_ATTR_EX3_UNK7 = 0x00000080,            // 7 create a separate (de)buff stack for each caster
        SPELL_ATTR_EX3_TARGET_ONLY_PLAYER = 0x00000100,            // 8 Can target only player
        SPELL_ATTR_EX3_UNK9 = 0x00000200,            // 9
        SPELL_ATTR_EX3_MAIN_HAND = 0x00000400,            // 10 Main hand weapon required
        SPELL_ATTR_EX3_BATTLEGROUND = 0x00000800,            // 11 Can casted only on battleground
        SPELL_ATTR_EX3_CAST_ON_DEAD = 0x00001000,            // 12 target is a dead player (not every spell has this flag)
        SPELL_ATTR_EX3_UNK13 = 0x00002000,            // 13
        SPELL_ATTR_EX3_UNK14 = 0x00004000,            // 14 "Honorless Target" only this spells have this flag
        SPELL_ATTR_EX3_UNK15 = 0x00008000,            // 15 Auto Shoot, Shoot, Throw,  - this is autoshot flag
        SPELL_ATTR_EX3_UNK16 = 0x00010000,            // 16 no triggers effects that trigger on casting a spell??
        SPELL_ATTR_EX3_NO_INITIAL_AGGRO = 0x00020000,            // 17 Causes no aggro if not missed
        SPELL_ATTR_EX3_CANT_MISS = 0x00040000,            // 18 Spell should always hit its target
        SPELL_ATTR_EX3_UNK19 = 0x00080000,            // 19
        SPELL_ATTR_EX3_DEATH_PERSISTENT = 0x00100000,            // 20 Death persistent spells
        SPELL_ATTR_EX3_UNK21 = 0x00200000,            // 21
        SPELL_ATTR_EX3_REQ_WAND = 0x00400000,            // 22 Req wand
        SPELL_ATTR_EX3_UNK23 = 0x00800000,            // 23
        SPELL_ATTR_EX3_REQ_OFFHAND = 0x01000000,            // 24 Req offhand weapon
        SPELL_ATTR_EX3_UNK25 = 0x02000000,            // 25 no cause spell pushback ?
        SPELL_ATTR_EX3_UNK26 = 0x04000000,            // 26
        SPELL_ATTR_EX3_UNK27 = 0x08000000,            // 27
        SPELL_ATTR_EX3_UNK28 = 0x10000000,            // 28 always cast ok ? (requires more research)
        SPELL_ATTR_EX3_UNK29 = 0x20000000,            // 29
        SPELL_ATTR_EX3_UNK30 = 0x40000000,            // 30
        SPELL_ATTR_EX3_UNK31 = 0x80000000            // 31
    }

    [Flags]
    public enum SpellAttributesEx4 : uint
    {
        SPELL_ATTR_EX4_UNK0 = 0x00000001,            // 0
        SPELL_ATTR_EX4_UNK1 = 0x00000002,            // 1 proc on finishing move?
        SPELL_ATTR_EX4_UNK2 = 0x00000004,            // 2
        SPELL_ATTR_EX4_UNK3 = 0x00000008,            // 3
        SPELL_ATTR_EX4_UNK4 = 0x00000010,            // 4 This will no longer cause guards to attack on use??
        SPELL_ATTR_EX4_UNK5 = 0x00000020,            // 5
        SPELL_ATTR_EX4_NOT_STEALABLE = 0x00000040,            // 6 although such auras might be dispellable, they cannot be stolen
        SPELL_ATTR_EX4_UNK7 = 0x00000080,            // 7
        SPELL_ATTR_EX4_UNK8 = 0x00000100,            // 8
        SPELL_ATTR_EX4_UNK9 = 0x00000200,            // 9
        SPELL_ATTR_EX4_SPELL_VS_EXTEND_COST = 0x00000400,            // 10
        SPELL_ATTR_EX4_UNK11 = 0x00000800,            // 11
        SPELL_ATTR_EX4_UNK12 = 0x00001000,            // 12
        SPELL_ATTR_EX4_UNK13 = 0x00002000,            // 13
        SPELL_ATTR_EX4_UNK14 = 0x00004000,            // 14
        SPELL_ATTR_EX4_UNK15 = 0x00008000,            // 15
        SPELL_ATTR_EX4_NOT_USABLE_IN_ARENA = 0x00010000,            // 16 not usable in arena
        SPELL_ATTR_EX4_USABLE_IN_ARENA = 0x00020000,            // 17 usable in arena
        SPELL_ATTR_EX4_UNK18 = 0x00040000,            // 18
        SPELL_ATTR_EX4_UNK19 = 0x00080000,            // 19
        SPELL_ATTR_EX4_UNK20 = 0x00100000,            // 20 do not give "more powerful spell" error message
        SPELL_ATTR_EX4_UNK21 = 0x00200000,            // 21
        SPELL_ATTR_EX4_UNK22 = 0x00400000,            // 22
        SPELL_ATTR_EX4_UNK23 = 0x00800000,            // 23
        SPELL_ATTR_EX4_UNK24 = 0x01000000,            // 24
        SPELL_ATTR_EX4_UNK25 = 0x02000000,            // 25 pet scaling auras
        SPELL_ATTR_EX4_CAST_ONLY_IN_OUTLAND = 0x04000000,            // 26 Can only be used in Outland.
        SPELL_ATTR_EX4_UNK27 = 0x08000000,            // 27
        SPELL_ATTR_EX4_UNK28 = 0x10000000,            // 28
        SPELL_ATTR_EX4_UNK29 = 0x20000000,            // 29
        SPELL_ATTR_EX4_UNK30 = 0x40000000,            // 30
        SPELL_ATTR_EX4_UNK31 = 0x80000000            // 31
    }

    public enum SpellEffects
    {
        SPELL_EFFECT_NONE = 0,
        SPELL_EFFECT_INSTAKILL = 1,
        SPELL_EFFECT_SCHOOL_DAMAGE = 2,
        SPELL_EFFECT_DUMMY = 3,
        SPELL_EFFECT_PORTAL_TELEPORT = 4,
        SPELL_EFFECT_TELEPORT_UNITS = 5,
        SPELL_EFFECT_APPLY_AURA = 6,
        SPELL_EFFECT_ENVIRONMENTAL_DAMAGE = 7,
        SPELL_EFFECT_POWER_DRAIN = 8,
        SPELL_EFFECT_HEALTH_LEECH = 9,
        SPELL_EFFECT_HEAL = 10,
        SPELL_EFFECT_BIND = 11,
        SPELL_EFFECT_PORTAL = 12,
        SPELL_EFFECT_RITUAL_BASE = 13,
        SPELL_EFFECT_RITUAL_SPECIALIZE = 14,
        SPELL_EFFECT_RITUAL_ACTIVATE_PORTAL = 15,
        SPELL_EFFECT_QUEST_COMPLETE = 16,
        SPELL_EFFECT_WEAPON_DAMAGE_NOSCHOOL = 17,
        SPELL_EFFECT_RESURRECT = 18,
        SPELL_EFFECT_ADD_EXTRA_ATTACKS = 19,
        SPELL_EFFECT_DODGE = 20,
        SPELL_EFFECT_EVADE = 21,
        SPELL_EFFECT_PARRY = 22,
        SPELL_EFFECT_BLOCK = 23,
        SPELL_EFFECT_CREATE_ITEM = 24,
        SPELL_EFFECT_WEAPON = 25,
        SPELL_EFFECT_DEFENSE = 26,
        SPELL_EFFECT_PERSISTENT_AREA_AURA = 27,
        SPELL_EFFECT_SUMMON = 28,
        SPELL_EFFECT_LEAP = 29,
        SPELL_EFFECT_ENERGIZE = 30,
        SPELL_EFFECT_WEAPON_PERCENT_DAMAGE = 31,
        SPELL_EFFECT_TRIGGER_MISSILE = 32,
        SPELL_EFFECT_OPEN_LOCK = 33,
        SPELL_EFFECT_SUMMON_CHANGE_ITEM = 34,
        SPELL_EFFECT_APPLY_AREA_AURA_PARTY = 35,
        SPELL_EFFECT_LEARN_SPELL = 36,
        SPELL_EFFECT_SPELL_DEFENSE = 37,
        SPELL_EFFECT_DISPEL = 38,
        SPELL_EFFECT_LANGUAGE = 39,
        SPELL_EFFECT_DUAL_WIELD = 40,
        SPELL_EFFECT_SUMMON_WILD = 41,
        SPELL_EFFECT_SUMMON_GUARDIAN = 42,
        SPELL_EFFECT_TELEPORT_UNITS_FACE_CASTER = 43,
        SPELL_EFFECT_SKILL_STEP = 44,
        SPELL_EFFECT_ADD_HONOR = 45,
        SPELL_EFFECT_SPAWN = 46,
        SPELL_EFFECT_TRADE_SKILL = 47,
        SPELL_EFFECT_STEALTH = 48,
        SPELL_EFFECT_DETECT = 49,
        SPELL_EFFECT_TRANS_DOOR = 50,
        SPELL_EFFECT_FORCE_CRITICAL_HIT = 51,
        SPELL_EFFECT_GUARANTEE_HIT = 52,
        SPELL_EFFECT_ENCHANT_ITEM = 53,
        SPELL_EFFECT_ENCHANT_ITEM_TEMPORARY = 54,
        SPELL_EFFECT_TAMECREATURE = 55,
        SPELL_EFFECT_SUMMON_PET = 56,
        SPELL_EFFECT_LEARN_PET_SPELL = 57,
        SPELL_EFFECT_WEAPON_DAMAGE = 58,
        SPELL_EFFECT_OPEN_LOCK_ITEM = 59,
        SPELL_EFFECT_PROFICIENCY = 60,
        SPELL_EFFECT_SEND_EVENT = 61,
        SPELL_EFFECT_POWER_BURN = 62,
        SPELL_EFFECT_THREAT = 63,
        SPELL_EFFECT_TRIGGER_SPELL = 64,
        SPELL_EFFECT_HEALTH_FUNNEL = 65,
        SPELL_EFFECT_POWER_FUNNEL = 66,
        SPELL_EFFECT_HEAL_MAX_HEALTH = 67,
        SPELL_EFFECT_INTERRUPT_CAST = 68,
        SPELL_EFFECT_DISTRACT = 69,
        SPELL_EFFECT_PULL = 70,
        SPELL_EFFECT_PICKPOCKET = 71,
        SPELL_EFFECT_ADD_FARSIGHT = 72,
        SPELL_EFFECT_SUMMON_POSSESSED = 73,
        SPELL_EFFECT_SUMMON_TOTEM = 74,
        SPELL_EFFECT_HEAL_MECHANICAL = 75,
        SPELL_EFFECT_SUMMON_OBJECT_WILD = 76,
        SPELL_EFFECT_SCRIPT_EFFECT = 77,
        SPELL_EFFECT_ATTACK = 78,
        SPELL_EFFECT_SANCTUARY = 79,
        SPELL_EFFECT_ADD_COMBO_POINTS = 80,
        SPELL_EFFECT_CREATE_HOUSE = 81,
        SPELL_EFFECT_BIND_SIGHT = 82,
        SPELL_EFFECT_DUEL = 83,
        SPELL_EFFECT_STUCK = 84,
        SPELL_EFFECT_SUMMON_PLAYER = 85,
        SPELL_EFFECT_ACTIVATE_OBJECT = 86,
        SPELL_EFFECT_SUMMON_TOTEM_SLOT1 = 87,
        SPELL_EFFECT_SUMMON_TOTEM_SLOT2 = 88,
        SPELL_EFFECT_SUMMON_TOTEM_SLOT3 = 89,
        SPELL_EFFECT_SUMMON_TOTEM_SLOT4 = 90,
        SPELL_EFFECT_THREAT_ALL = 91,
        SPELL_EFFECT_ENCHANT_HELD_ITEM = 92,
        SPELL_EFFECT_SUMMON_PHANTASM = 93,
        SPELL_EFFECT_SELF_RESURRECT = 94,
        SPELL_EFFECT_SKINNING = 95,
        SPELL_EFFECT_CHARGE = 96,
        SPELL_EFFECT_SUMMON_CRITTER = 97,
        SPELL_EFFECT_KNOCK_BACK = 98,
        SPELL_EFFECT_DISENCHANT = 99,
        SPELL_EFFECT_INEBRIATE = 100,
        SPELL_EFFECT_FEED_PET = 101,
        SPELL_EFFECT_DISMISS_PET = 102,
        SPELL_EFFECT_REPUTATION = 103,
        SPELL_EFFECT_SUMMON_OBJECT_SLOT1 = 104,
        SPELL_EFFECT_SUMMON_OBJECT_SLOT2 = 105,
        SPELL_EFFECT_SUMMON_OBJECT_SLOT3 = 106,
        SPELL_EFFECT_SUMMON_OBJECT_SLOT4 = 107,
        SPELL_EFFECT_DISPEL_MECHANIC = 108,
        SPELL_EFFECT_SUMMON_DEAD_PET = 109,
        SPELL_EFFECT_DESTROY_ALL_TOTEMS = 110,
        SPELL_EFFECT_DURABILITY_DAMAGE = 111,
        SPELL_EFFECT_SUMMON_DEMON = 112,
        SPELL_EFFECT_RESURRECT_NEW = 113,
        SPELL_EFFECT_ATTACK_ME = 114,
        SPELL_EFFECT_DURABILITY_DAMAGE_PCT = 115,
        SPELL_EFFECT_SKIN_PLAYER_CORPSE = 116,
        SPELL_EFFECT_SPIRIT_HEAL = 117,
        SPELL_EFFECT_SKILL = 118,
        SPELL_EFFECT_APPLY_AREA_AURA_PET = 119,
        SPELL_EFFECT_TELEPORT_GRAVEYARD = 120,
        SPELL_EFFECT_NORMALIZED_WEAPON_DMG = 121,
        SPELL_EFFECT_122 = 122,
        SPELL_EFFECT_SEND_TAXI = 123,
        SPELL_EFFECT_PLAYER_PULL = 124,
        SPELL_EFFECT_MODIFY_THREAT_PERCENT = 125,
        SPELL_EFFECT_126 = 126,
        SPELL_EFFECT_127 = 127,
        SPELL_EFFECT_128 = 128,
        SPELL_EFFECT_129 = 129,
        TOTAL_SPELL_EFFECTS = 130
    }

    /// <summary>
    /// Targets for spells
    /// </summary>
    public enum Targets
    {
        TARGET_NONE = 0,
        TARGET_SELF = 1,
        TARGET_RANDOM_ENEMY_CHAIN_IN_AREA = 2,                 // only one spell has that, but regardless, it's a target type after all
        TARGET_RANDOM_FRIEND_CHAIN_IN_AREA = 3,
        TARGET_RANDOM_UNIT_CHAIN_IN_AREA = 4,                 // some plague spells that are infectious - maybe targets not-infected friends inrange
        TARGET_PET = 5,
        TARGET_CHAIN_DAMAGE = 6,
        TARGET_AREAEFFECT_INSTANT = 7,                 // targets around provided destination point
        TARGET_AREAEFFECT_CUSTOM = 8,
        TARGET_INNKEEPER_COORDINATES = 9,                 // uses in teleport to innkeeper spells
        TARGET_11 = 11,                // used by spell 4 'Word of Recall Other'
        TARGET_ALL_ENEMY_IN_AREA = 15,
        TARGET_ALL_ENEMY_IN_AREA_INSTANT = 16,
        TARGET_TABLE_X_Y_Z_COORDINATES = 17,                // uses in teleport spells and some other
        TARGET_EFFECT_SELECT = 18,                // highly depends on the spell effect
        TARGET_ALL_PARTY_AROUND_CASTER = 20,
        TARGET_SINGLE_FRIEND = 21,
        TARGET_CASTER_COORDINATES = 22,                // used only in TargetA, target selection dependent from TargetB
        TARGET_GAMEOBJECT = 23,
        TARGET_IN_FRONT_OF_CASTER = 24,
        TARGET_DUELVSPLAYER = 25,
        TARGET_GAMEOBJECT_ITEM = 26,
        TARGET_MASTER = 27,
        TARGET_ALL_ENEMY_IN_AREA_CHANNELED = 28,
        TARGET_29 = 29,
        TARGET_ALL_FRIENDLY_UNITS_AROUND_CASTER = 30,           // select friendly for caster object faction (in different original caster faction) in TargetB used only with TARGET_ALL_AROUND_CASTER and in self casting range in TargetA
        TARGET_ALL_FRIENDLY_UNITS_IN_AREA = 31,
        TARGET_MINION = 32,
        TARGET_ALL_PARTY = 33,
        TARGET_ALL_PARTY_AROUND_CASTER_2 = 34,                // used in Tranquility
        TARGET_SINGLE_PARTY = 35,
        TARGET_ALL_HOSTILE_UNITS_AROUND_CASTER = 36,
        TARGET_AREAEFFECT_PARTY = 37,
        TARGET_SCRIPT = 38,
        TARGET_SELF_FISHING = 39,
        TARGET_FOCUS_OR_SCRIPTED_GAMEOBJECT = 40,
        TARGET_TOTEM_EARTH = 41,
        TARGET_TOTEM_WATER = 42,
        TARGET_TOTEM_AIR = 43,
        TARGET_TOTEM_FIRE = 44,
        TARGET_CHAIN_HEAL = 45,
        TARGET_SCRIPT_COORDINATES = 46,
        TARGET_DYNAMIC_OBJECT_FRONT = 47,
        TARGET_DYNAMIC_OBJECT_BEHIND = 48,
        TARGET_DYNAMIC_OBJECT_LEFT_SIDE = 49,
        TARGET_DYNAMIC_OBJECT_RIGHT_SIDE = 50,
        TARGET_AREAEFFECT_GO_AROUND_SOURCE = 51,
        TARGET_AREAEFFECT_GO_AROUND_DEST = 52,                // gameobject around destination, select by spell_script_target
        TARGET_CURRENT_ENEMY_COORDINATES = 53,                // set unit coordinates as dest, only 16 target B imlemented
        TARGET_LARGE_FRONTAL_CONE = 54,
        TARGET_ALL_RAID_AROUND_CASTER = 56,
        TARGET_SINGLE_FRIEND_2 = 57,
        TARGET_58 = 58,
        TARGET_NARROW_FRONTAL_CONE = 60,
        TARGET_AREAEFFECT_PARTY_AND_CLASS = 61,
        TARGET_DUELVSPLAYER_COORDINATES = 63
    }

    /**
 * This is what's used in a Modifier by the Aura class
 * to tell what the Aura should modify.
 */
    public enum AuraType
    {
        SPELL_AURA_NONE = 0,
        SPELL_AURA_BIND_SIGHT = 1,
        SPELL_AURA_MOD_POSSESS = 2,
        /**
         * The aura should do periodic damage, the function that handles
         * this is Aura::HandlePeriodicDamage, the amount is usually decided
         * by the Unit::SpellDamageBonusDone or Unit::MeleeDamageBonusDone
         * which increases/decreases the Modifier::m_amount
         */
        SPELL_AURA_PERIODIC_DAMAGE = 3,
        /**
         * Used by Aura::HandleAuraDummy
         */
        SPELL_AURA_DUMMY = 4,
        /**
         * Used by Aura::HandleModConfuse, will either confuse or unconfuse
         * the target depending on whether the apply flag is set
         */
        SPELL_AURA_MOD_CONFUSE = 5,
        SPELL_AURA_MOD_CHARM = 6,
        SPELL_AURA_MOD_FEAR = 7,
        /**
         * The aura will do periodic heals of a target, handled by
         * Aura::HandlePeriodicHeal, uses Unit::SpellHealingBonusDone
         * to calculate whether to increase or decrease Modifier::m_amount
         */
        SPELL_AURA_PERIODIC_HEAL = 8,
        /**
         * Changes the attackspeed, the Modifier::m_amount decides
         * how much we change in percent, ie, if the m_amount is
         * 50 the attackspeed will increase by 50%
         */
        SPELL_AURA_MOD_ATTACKSPEED = 9,
        /**
         * Modifies the threat that the Aura does in percent,
         * the Modifier::m_miscvalue decides which of the SpellSchools
         * it should affect threat for.
         * \see SpellSchoolMask
         */
        SPELL_AURA_MOD_THREAT = 10,
        /**
         * Just applies a taunt which will change the threat a mob has
         * Taken care of in Aura::HandleModThreat
         */
        SPELL_AURA_MOD_TAUNT = 11,
        /**
         * Stuns targets in different ways, taken care of in
         * Aura::HandleAuraModStun
         */
        SPELL_AURA_MOD_STUN = 12,
        /**
         * Changes the damage done by a weapon in any hand, the Modifier::m_miscvalue
         * will tell what school the damage is from, it's used as a bitmask
         * \see SpellSchoolMask
         */
        SPELL_AURA_MOD_DAMAGE_DONE = 13,
        /**
         * Not handled by the Aura class but instead this is implemented in
         * Unit::MeleeDamageBonusTaken and Unit::SpellBaseDamageBonusTaken
         */
        SPELL_AURA_MOD_DAMAGE_TAKEN = 14,
        /**
         * Not handled by the Aura class, implemented in Unit::DealMeleeDamage
         */
        SPELL_AURA_DAMAGE_SHIELD = 15,
        /**
         * Taken care of in Aura::HandleModStealth, take note that this
         * is not the same thing as invisibility
         */
        SPELL_AURA_MOD_STEALTH = 16,
        /**
         * Not handled by the Aura class, implemented in Unit::isVisibleForOrDetect
         * which does a lot of checks to determine whether the person is visible or not,
         * the SPELL_AURA_MOD_STEALTH seems to determine how in/visible ie a rogue is.
         */
        SPELL_AURA_MOD_STEALTH_DETECT = 17,
        /**
         * Handled by Aura::HandleInvisibility, the Modifier::m_miscvalue in the struct
         * seems to decide what kind of invisibility it is with a bitflag. the miscvalue
         * decides which bit is set, ie: 3 would make the 3rd bit be set.
         */
        SPELL_AURA_MOD_INVISIBILITY = 18,
        /**
         * Adds one of the kinds of detections to the possible detections.
         * As in SPEALL_AURA_MOD_INVISIBILITY the Modifier::m_miscvalue seems to decide
         * what kind of invisibility the Unit should be able to detect.
         */
        SPELL_AURA_MOD_INVISIBILITY_DETECTION = 19,
        SPELL_AURA_OBS_MOD_HEALTH = 20,                         // 20,21 unofficial
        SPELL_AURA_OBS_MOD_MANA = 21,
        /**
         * Handled by Aura::HandleAuraModResistance, changes the resistance for a unit
         * the field Modifier::m_miscvalue decides which kind of resistance that should
         * be changed, for possible values see SpellSchools.
         * \see SpellSchools
         */
        SPELL_AURA_MOD_RESISTANCE = 22,
        /**
         * Currently just sets Aura::m_isPeriodic to apply and has a special case
         * for Curse of the Plaguebringer.
         */
        SPELL_AURA_PERIODIC_TRIGGER_SPELL = 23,
        /**
         * Just sets Aura::m_isPeriodic to apply
         */
        SPELL_AURA_PERIODIC_ENERGIZE = 24,
        /**
         * Changes whether the target is pacified or not depending on the apply flag.
         * Pacify makes the target silenced and have all it's attack skill disabled.
         * See: http://www.wowhead.com/spell=6462/pacified
         */
        SPELL_AURA_MOD_PACIFY = 25,
        /**
         * Roots or unroots the target
         */
        SPELL_AURA_MOD_ROOT = 26,
        /**
         * Silences the target and stops and spell casts that should be stopped,
         * they have the flag SpellPreventionType::SPELL_PREVENTION_TYPE_SILENCE
         */
        SPELL_AURA_MOD_SILENCE = 27,
        SPELL_AURA_REFLECT_SPELLS = 28,
        SPELL_AURA_MOD_STAT = 29,
        SPELL_AURA_MOD_SKILL = 30,
        SPELL_AURA_MOD_INCREASE_SPEED = 31,
        SPELL_AURA_MOD_INCREASE_MOUNTED_SPEED = 32,
        SPELL_AURA_MOD_DECREASE_SPEED = 33,
        SPELL_AURA_MOD_INCREASE_HEALTH = 34,
        SPELL_AURA_MOD_INCREASE_ENERGY = 35,
        SPELL_AURA_MOD_SHAPESHIFT = 36,
        SPELL_AURA_EFFECT_IMMUNITY = 37,
        SPELL_AURA_STATE_IMMUNITY = 38,
        SPELL_AURA_SCHOOL_IMMUNITY = 39,
        SPELL_AURA_DAMAGE_IMMUNITY = 40,
        SPELL_AURA_DISPEL_IMMUNITY = 41,
        SPELL_AURA_PROC_TRIGGER_SPELL = 42,
        SPELL_AURA_PROC_TRIGGER_DAMAGE = 43,
        SPELL_AURA_TRACK_CREATURES = 44,
        SPELL_AURA_TRACK_RESOURCES = 45,
        SPELL_AURA_46 = 46,                                     // Ignore all Gear test spells
        SPELL_AURA_MOD_PARRY_PERCENT = 47,
        SPELL_AURA_48 = 48,                                     // One periodic spell
        SPELL_AURA_MOD_DODGE_PERCENT = 49,
        SPELL_AURA_MOD_BLOCK_SKILL = 50,
        SPELL_AURA_MOD_BLOCK_PERCENT = 51,
        SPELL_AURA_MOD_CRIT_PERCENT = 52,
        SPELL_AURA_PERIODIC_LEECH = 53,
        SPELL_AURA_MOD_HIT_CHANCE = 54,
        SPELL_AURA_MOD_SPELL_HIT_CHANCE = 55,
        SPELL_AURA_TRANSFORM = 56,
        SPELL_AURA_MOD_SPELL_CRIT_CHANCE = 57,
        SPELL_AURA_MOD_INCREASE_SWIM_SPEED = 58,
        SPELL_AURA_MOD_DAMAGE_DONE_CREATURE = 59,
        SPELL_AURA_MOD_PACIFY_SILENCE = 60,
        SPELL_AURA_MOD_SCALE = 61,
        SPELL_AURA_PERIODIC_HEALTH_FUNNEL = 62,
        SPELL_AURA_PERIODIC_MANA_FUNNEL = 63,
        SPELL_AURA_PERIODIC_MANA_LEECH = 64,
        SPELL_AURA_MOD_CASTING_SPEED_NOT_STACK = 65,
        SPELL_AURA_FEIGN_DEATH = 66,
        SPELL_AURA_MOD_DISARM = 67,
        SPELL_AURA_MOD_STALKED = 68,
        SPELL_AURA_SCHOOL_ABSORB = 69,
        SPELL_AURA_EXTRA_ATTACKS = 70,
        SPELL_AURA_MOD_SPELL_CRIT_CHANCE_SCHOOL = 71,
        SPELL_AURA_MOD_POWER_COST_SCHOOL_PCT = 72,
        SPELL_AURA_MOD_POWER_COST_SCHOOL = 73,
        SPELL_AURA_REFLECT_SPELLS_SCHOOL = 74,
        SPELL_AURA_MOD_LANGUAGE = 75,
        SPELL_AURA_FAR_SIGHT = 76,
        SPELL_AURA_MECHANIC_IMMUNITY = 77,
        SPELL_AURA_MOUNTED = 78,
        SPELL_AURA_MOD_DAMAGE_PERCENT_DONE = 79,
        SPELL_AURA_MOD_PERCENT_STAT = 80,
        SPELL_AURA_SPLIT_DAMAGE_PCT = 81,
        SPELL_AURA_WATER_BREATHING = 82,
        SPELL_AURA_MOD_BASE_RESISTANCE = 83,
        SPELL_AURA_MOD_REGEN = 84,
        SPELL_AURA_MOD_POWER_REGEN = 85,
        SPELL_AURA_CHANNEL_DEATH_ITEM = 86,
        SPELL_AURA_MOD_DAMAGE_PERCENT_TAKEN = 87,
        SPELL_AURA_MOD_HEALTH_REGEN_PERCENT = 88,
        SPELL_AURA_PERIODIC_DAMAGE_PERCENT = 89,
        SPELL_AURA_MOD_RESIST_CHANCE = 90,
        SPELL_AURA_MOD_DETECT_RANGE = 91,
        SPELL_AURA_PREVENTS_FLEEING = 92,
        SPELL_AURA_MOD_UNATTACKABLE = 93,
        SPELL_AURA_INTERRUPT_REGEN = 94,
        SPELL_AURA_GHOST = 95,
        SPELL_AURA_SPELL_MAGNET = 96,
        SPELL_AURA_MANA_SHIELD = 97,
        SPELL_AURA_MOD_SKILL_TALENT = 98,
        SPELL_AURA_MOD_ATTACK_POWER = 99,
        SPELL_AURA_AURAS_VISIBLE = 100,
        SPELL_AURA_MOD_RESISTANCE_PCT = 101,
        SPELL_AURA_MOD_MELEE_ATTACK_POWER_VERSUS = 102,
        SPELL_AURA_MOD_TOTAL_THREAT = 103,
        SPELL_AURA_WATER_WALK = 104,
        SPELL_AURA_FEATHER_FALL = 105,
        SPELL_AURA_HOVER = 106,
        SPELL_AURA_ADD_FLAT_MODIFIER = 107,
        SPELL_AURA_ADD_PCT_MODIFIER = 108,
        SPELL_AURA_ADD_TARGET_TRIGGER = 109,
        SPELL_AURA_MOD_POWER_REGEN_PERCENT = 110,
        SPELL_AURA_ADD_CASTER_HIT_TRIGGER = 111,
        SPELL_AURA_OVERRIDE_CLASS_SCRIPTS = 112,
        SPELL_AURA_MOD_RANGED_DAMAGE_TAKEN = 113,
        SPELL_AURA_MOD_RANGED_DAMAGE_TAKEN_PCT = 114,
        SPELL_AURA_MOD_HEALING = 115,
        SPELL_AURA_MOD_REGEN_DURING_COMBAT = 116,
        SPELL_AURA_MOD_MECHANIC_RESISTANCE = 117,
        SPELL_AURA_MOD_HEALING_PCT = 118,
        SPELL_AURA_SHARE_PET_TRACKING = 119,
        SPELL_AURA_UNTRACKABLE = 120,
        SPELL_AURA_EMPATHY = 121,
        SPELL_AURA_MOD_OFFHAND_DAMAGE_PCT = 122,
        SPELL_AURA_MOD_TARGET_RESISTANCE = 123,
        SPELL_AURA_MOD_RANGED_ATTACK_POWER = 124,
        SPELL_AURA_MOD_MELEE_DAMAGE_TAKEN = 125,
        SPELL_AURA_MOD_MELEE_DAMAGE_TAKEN_PCT = 126,
        SPELL_AURA_RANGED_ATTACK_POWER_ATTACKER_BONUS = 127,
        SPELL_AURA_MOD_POSSESS_PET = 128,
        SPELL_AURA_MOD_SPEED_ALWAYS = 129,
        SPELL_AURA_MOD_MOUNTED_SPEED_ALWAYS = 130,
        SPELL_AURA_MOD_RANGED_ATTACK_POWER_VERSUS = 131,
        SPELL_AURA_MOD_INCREASE_ENERGY_PERCENT = 132,
        SPELL_AURA_MOD_INCREASE_HEALTH_PERCENT = 133,
        SPELL_AURA_MOD_MANA_REGEN_INTERRUPT = 134,
        SPELL_AURA_MOD_HEALING_DONE = 135,
        SPELL_AURA_MOD_HEALING_DONE_PERCENT = 136,
        SPELL_AURA_MOD_TOTAL_STAT_PERCENTAGE = 137,
        SPELL_AURA_MOD_MELEE_HASTE = 138,
        SPELL_AURA_FORCE_REACTION = 139,
        SPELL_AURA_MOD_RANGED_HASTE = 140,
        SPELL_AURA_MOD_RANGED_AMMO_HASTE = 141,
        SPELL_AURA_MOD_BASE_RESISTANCE_PCT = 142,
        SPELL_AURA_MOD_RESISTANCE_EXCLUSIVE = 143,
        SPELL_AURA_SAFE_FALL = 144,
        SPELL_AURA_CHARISMA = 145,
        SPELL_AURA_PERSUADED = 146,
        SPELL_AURA_MECHANIC_IMMUNITY_MASK = 147,
        SPELL_AURA_RETAIN_COMBO_POINTS = 148,
        SPELL_AURA_RESIST_PUSHBACK = 149,                      //    Resist Pushback
        SPELL_AURA_MOD_SHIELD_BLOCKVALUE_PCT = 150,
        SPELL_AURA_TRACK_STEALTHED = 151,                      //    Track Stealthed
        SPELL_AURA_MOD_DETECTED_RANGE = 152,                    //    Mod Detected Range
        SPELL_AURA_SPLIT_DAMAGE_FLAT = 153,                     //    Split Damage Flat
        SPELL_AURA_MOD_STEALTH_LEVEL = 154,                     //    Stealth Level Modifier
        SPELL_AURA_MOD_WATER_BREATHING = 155,                   //    Mod Water Breathing
        SPELL_AURA_MOD_REPUTATION_GAIN = 156,                   //    Mod Reputation Gain
        SPELL_AURA_PET_DAMAGE_MULTI = 157,                      //    Mod Pet Damage
        SPELL_AURA_MOD_SHIELD_BLOCKVALUE = 158,
        SPELL_AURA_NO_PVP_CREDIT = 159,
        SPELL_AURA_MOD_AOE_AVOIDANCE = 160,
        SPELL_AURA_MOD_HEALTH_REGEN_IN_COMBAT = 161,
        SPELL_AURA_POWER_BURN_MANA = 162,
        SPELL_AURA_MOD_CRIT_DAMAGE_BONUS = 163,
        SPELL_AURA_164 = 164,
        SPELL_AURA_MELEE_ATTACK_POWER_ATTACKER_BONUS = 165,
        SPELL_AURA_MOD_ATTACK_POWER_PCT = 166,
        SPELL_AURA_MOD_RANGED_ATTACK_POWER_PCT = 167,
        SPELL_AURA_MOD_DAMAGE_DONE_VERSUS = 168,
        SPELL_AURA_MOD_CRIT_PERCENT_VERSUS = 169,
        SPELL_AURA_DETECT_AMORE = 170,
        SPELL_AURA_MOD_SPEED_NOT_STACK = 171,
        SPELL_AURA_MOD_MOUNTED_SPEED_NOT_STACK = 172,
        SPELL_AURA_ALLOW_CHAMPION_SPELLS = 173,
        SPELL_AURA_MOD_SPELL_DAMAGE_OF_STAT_PERCENT = 174,      // in 1.12.1 only dependent spirit case
        SPELL_AURA_MOD_SPELL_HEALING_OF_STAT_PERCENT = 175,
        SPELL_AURA_SPIRIT_OF_REDEMPTION = 176,
        SPELL_AURA_AOE_CHARM = 177,
        SPELL_AURA_MOD_DEBUFF_RESISTANCE = 178,
        SPELL_AURA_MOD_ATTACKER_SPELL_CRIT_CHANCE = 179,
        SPELL_AURA_MOD_FLAT_SPELL_DAMAGE_VERSUS = 180,
        SPELL_AURA_MOD_FLAT_SPELL_CRIT_DAMAGE_VERSUS = 181,     // unused - possible flat spell crit damage versus
        SPELL_AURA_MOD_RESISTANCE_OF_STAT_PERCENT = 182,
        SPELL_AURA_MOD_CRITICAL_THREAT = 183,
        SPELL_AURA_MOD_ATTACKER_MELEE_HIT_CHANCE = 184,
        SPELL_AURA_MOD_ATTACKER_RANGED_HIT_CHANCE = 185,
        SPELL_AURA_MOD_ATTACKER_SPELL_HIT_CHANCE = 186,
        SPELL_AURA_MOD_ATTACKER_MELEE_CRIT_CHANCE = 187,
        SPELL_AURA_MOD_ATTACKER_RANGED_CRIT_CHANCE = 188,
        SPELL_AURA_MOD_RATING = 189,
        SPELL_AURA_MOD_FACTION_REPUTATION_GAIN = 190,
        SPELL_AURA_USE_NORMAL_MOVEMENT_SPEED = 191,
        TOTAL_AURAS = 192
    }

    public enum AreaAuraType
    {
        AREA_AURA_PARTY,
        AREA_AURA_PET
    }

    public enum SpellMissInfo
    {
        SPELL_MISS_NONE = 0,
        SPELL_MISS_MISS = 1,
        SPELL_MISS_RESIST = 2,
        SPELL_MISS_DODGE = 3,
        SPELL_MISS_PARRY = 4,
        SPELL_MISS_BLOCK = 5,
        SPELL_MISS_EVADE = 6,
        SPELL_MISS_IMMUNE = 7,
        SPELL_MISS_IMMUNE2 = 8,
        SPELL_MISS_DEFLECT = 9,
        SPELL_MISS_ABSORB = 10,
        SPELL_MISS_REFLECT = 11
    }
}
