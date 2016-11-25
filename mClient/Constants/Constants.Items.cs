using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Constants
{
    public static class ItemConstants
    {
        public const int MAX_ITEM_PROTO_STATS = 10;
        public const int MAX_ITEM_PROTO_DAMAGES = 5;
        public const int MAX_ITEM_PROTO_SPELLS = 5;

        public const int MAX_NUMBER_OF_EQUIPPABLE_BAGS = ((int)InventorySlots.INVENTORY_SLOT_BAG_END - (int)InventorySlots.INVENTORY_SLOT_BAG_START);

        public const int INVENTORY_SLOT_BAG_0 = 255;

        public const int MAX_ITEM_SUBCLASS_WEAPON = 21;
        public const int MAX_ITEM_SUBCLASS_ARMOR = 10;

        public const int MAX_ITEM_MOD = 8;

        public const int MAX_ITEM_QUALITY = 7;
        public static uint[] ItemQualityColors = new uint[MAX_ITEM_QUALITY]
        {
            0xff9d9d9d,        // GREY
            0xffffffff,        // WHITE
            0xff1eff00,        // GREEN
            0xff0070dd,        // BLUE
            0xffa335ee,        // PURPLE
            0xffff8000,        // ORANGE
            0xffe6cc80         // LIGHT YELLOW
        };

        public static uint[] ItemWeaponSkills = new uint[MAX_ITEM_SUBCLASS_WEAPON]
        {
            (uint)SkillType.SKILL_AXES,
            (uint)SkillType.SKILL_2H_AXES,
            (uint)SkillType.SKILL_BOWS,
            (uint)SkillType.SKILL_GUNS,
            (uint)SkillType.SKILL_MACES,
            (uint)SkillType.SKILL_2H_MACES,
            (uint)SkillType.SKILL_POLEARMS,
            (uint)SkillType.SKILL_SWORDS,
            (uint)SkillType.SKILL_2H_SWORDS,
            0,
            (uint)SkillType.SKILL_STAVES,
            0,
            0,
            (uint)SkillType.SKILL_UNARMED,
            0,
            (uint)SkillType.SKILL_DAGGERS,
            (uint)SkillType.SKILL_THROWN,
            (uint)SkillType.SKILL_ASSASSINATION,
            (uint)SkillType.SKILL_CROSSBOWS,
            (uint)SkillType.SKILL_WANDS,
            (uint)SkillType.SKILL_FISHING
        };

        public static uint[] ItemArmorSkills = new uint[MAX_ITEM_SUBCLASS_ARMOR]
        {
            0,
            (uint)SkillType.SKILL_CLOTH,
            (uint)SkillType.SKILL_LEATHER,
            (uint)SkillType.SKILL_MAIL,
            (uint)SkillType.SKILL_PLATE_MAIL,
            0,
            (uint)SkillType.SKILL_SHIELD,
            0,
            0,
            0
        };
    }

    public enum EquipmentSlots
    {
        EQUIPMENT_SLOT_START        = 0,
        EQUIPMENT_SLOT_HEAD         = 0,
        EQUIPMENT_SLOT_NECK         = 1,
        EQUIPMENT_SLOT_SHOULDERS    = 2,
        EQUIPMENT_SLOT_BODY         = 3,
        EQUIPMENT_SLOT_CHEST        = 4,
        EQUIPMENT_SLOT_WAIST        = 5,
        EQUIPMENT_SLOT_LEGS         = 6,
        EQUIPMENT_SLOT_FEET         = 7,
        EQUIPMENT_SLOT_WRISTS       = 8,
        EQUIPMENT_SLOT_HANDS        = 9,
        EQUIPMENT_SLOT_FINGER1      = 10,
        EQUIPMENT_SLOT_FINGER2      = 11,
        EQUIPMENT_SLOT_TRINKET1     = 12,
        EQUIPMENT_SLOT_TRINKET2     = 13,
        EQUIPMENT_SLOT_BACK         = 14,
        EQUIPMENT_SLOT_MAINHAND     = 15,
        EQUIPMENT_SLOT_OFFHAND      = 16,
        EQUIPMENT_SLOT_RANGED       = 17,
        EQUIPMENT_SLOT_TABARD       = 18,
        EQUIPMENT_SLOT_END          = 19
    }
    public enum ItemEnchantmentType
    {
        ITEM_ENCHANTMENT_TYPE_NONE = 0,
        ITEM_ENCHANTMENT_TYPE_COMBAT_SPELL = 1,
        ITEM_ENCHANTMENT_TYPE_DAMAGE = 2,
        ITEM_ENCHANTMENT_TYPE_EQUIP_SPELL = 3,
        ITEM_ENCHANTMENT_TYPE_RESISTANCE = 4,
        ITEM_ENCHANTMENT_TYPE_STAT = 5,
        ITEM_ENCHANTMENT_TYPE_TOTEM = 6
    }

    public enum InventorySlots                                         // 4 slots
    {
        INVENTORY_SLOT_BAG_START = 19,
        INVENTORY_SLOT_BAG_END = 23
    }

    public enum InventoryPackSlots                                     // 16 slots
    {
        INVENTORY_SLOT_ITEM_START = 23,
        INVENTORY_SLOT_ITEM_END = 39
    }

    public enum BankItemSlots                                          // 28 slots
    {
        BANK_SLOT_ITEM_START = 39,
        BANK_SLOT_ITEM_END = 63
    }

    public enum BankBagSlots                                           // 7 slots
    {
        BANK_SLOT_BAG_START = 63,
        BANK_SLOT_BAG_END = 69
    }

    public enum BuyBackSlots                                           // 12 slots
    {
        // stored in m_buybackitems
        BUYBACK_SLOT_START = 69,
        BUYBACK_SLOT_END = 81
    }

    public enum KeyRingSlots                                           // 32 slots
    {
        KEYRING_SLOT_START = 81,
        KEYRING_SLOT_END = 97
    }

    public enum TradeSlots
    {
        TRADE_SLOT_COUNT = 7,
        TRADE_SLOT_TRADED_COUNT = 6,
        TRADE_SLOT_NONTRADED = 6
    }

    public enum ItemModType
    {
        ITEM_MOD_MANA               = 0,
        ITEM_MOD_HEALTH             = 1,
        ITEM_MOD_AGILITY            = 3,
        ITEM_MOD_STRENGTH           = 4,
        ITEM_MOD_INTELLECT          = 5,
        ITEM_MOD_SPIRIT             = 6,
        ITEM_MOD_STAMINA            = 7
    }

    public enum ItemSpelltriggerType
    {
        ITEM_SPELLTRIGGER_ON_USE    = 0,                  // use after equip cooldown
        ITEM_SPELLTRIGGER_ON_EQUIP  = 1,
        ITEM_SPELLTRIGGER_CHANCE_ON_HIT = 2,
        ITEM_SPELLTRIGGER_SOULSTONE     = 4,
        /*
         * ItemSpelltriggerType 5 might have changed on 2.4.3/3.0.3: Such auras
         * will be applied on item pickup and removed on item loss - maybe on the
         * other hand the item is destroyed if the aura is removed ("removed on
         * death" of spell 57348 makes me think so)
         */
        ITEM_SPELLTRIGGER_ON_NO_DELAY_USE = 5                  // no equip cooldown
    }

    public enum ItemBondingType
    {
        NO_BIND                     = 0,
        BIND_WHEN_PICKED_UP         = 1,
        BIND_WHEN_EQUIPPED          = 2,
        BIND_WHEN_USE               = 3,
        BIND_QUEST_ITEM             = 4,
        BIND_QUEST_ITEM1            = 5         // not used in game
    }

    [Flags]
    public enum ItemPrototypeFlags
    {
        ITEM_FLAG_UNK0              = 0x00000001, // not used
        ITEM_FLAG_CONJURED          = 0x00000002,
        ITEM_FLAG_LOOTABLE          = 0x00000004, // affect only non container items that can be "open" for loot. It or lockid set enable for client show "Right click to open". See also ITEM_DYNFLAG_UNLOCKED
        ITEM_FLAG_UNK3              = 0x00000008, // not used in pre-3.x
        ITEM_FLAG_UNK4              = 0x00000010, // can't repeat old note: appears red icon (like when item durability==0)
        ITEM_FLAG_INDESTRUCTIBLE    = 0x00000020, // used for totem. Item can not be destroyed, except by using spell (item can be reagent for spell and then allowed)
        ITEM_FLAG_UNK6              = 0x00000040, // ? old note: usable
        ITEM_FLAG_NO_EQUIP_COOLDOWN = 0x00000080,
        ITEM_FLAG_UNK8              = 0x00000100,
        ITEM_FLAG_WRAPPER           = 0x00000200, // used or not used wrapper
        ITEM_FLAG_IGNORE_BAG_SPACE  = 0x00000400, // ignore bag space at new item creation?
        ITEM_FLAG_PARTY_LOOT        = 0x00000800, // determines if item is party loot or not
        ITEM_FLAG_UNK12             = 0x00001000, // not used in pre-3.x
        ITEM_FLAG_CHARTER           = 0x00002000, // guild charter
        ITEM_FLAG_UNK14             = 0x00004000,
        ITEM_FLAG_UNK15             = 0x00008000, // a lot of items have this
        ITEM_FLAG_UNK16             = 0x00010000, // a lot of items have this
        ITEM_FLAG_UNK17             = 0x00020000, // last used flag in 1.12.1

        ITEM_FLAG_UNIQUE_EQUIPPED   = 0x00080000 // custom server side check, in client added in 2.x
    }

    public enum BagFamily
    {
        BAG_FAMILY_NONE             = 0,
        BAG_FAMILY_ARROWS           = 1,
        BAG_FAMILY_BULLETS          = 2,
        BAG_FAMILY_SOUL_SHARDS      = 3,
        BAG_FAMILY_UNKNOWN1         = 4,
        BAG_FAMILY_UNKNOWN2         = 5,
        BAG_FAMILY_HERBS            = 6,
        BAG_FAMILY_ENCHANTING_SUPP  = 7,
        BAG_FAMILY_ENGINEERING_SUPP = 8,
        BAG_FAMILY_KEYS             = 9
    }

    public enum InventoryType
    {
        INVTYPE_NON_EQUIP           = 0,
        INVTYPE_HEAD                = 1,
        INVTYPE_NECK                = 2,
        INVTYPE_SHOULDERS           = 3,
        INVTYPE_BODY                = 4,
        INVTYPE_CHEST               = 5,
        INVTYPE_WAIST               = 6,
        INVTYPE_LEGS                = 7,
        INVTYPE_FEET                = 8,
        INVTYPE_WRISTS              = 9,
        INVTYPE_HANDS               = 10,
        INVTYPE_FINGER              = 11,
        INVTYPE_TRINKET             = 12,
        INVTYPE_WEAPON              = 13,
        INVTYPE_SHIELD              = 14,
        INVTYPE_RANGED              = 15,
        INVTYPE_CLOAK               = 16,
        INVTYPE_2HWEAPON            = 17,
        INVTYPE_BAG                 = 18,
        INVTYPE_TABARD              = 19,
        INVTYPE_ROBE                = 20,
        INVTYPE_WEAPONMAINHAND      = 21,
        INVTYPE_WEAPONOFFHAND       = 22,
        INVTYPE_HOLDABLE            = 23,
        INVTYPE_AMMO                = 24,
        INVTYPE_THROWN              = 25,
        INVTYPE_RANGEDRIGHT         = 26,
        INVTYPE_QUIVER              = 27,
        INVTYPE_RELIC               = 28
    }

    public enum ItemClass
    {
        ITEM_CLASS_CONSUMABLE       = 0,
        ITEM_CLASS_CONTAINER        = 1,
        ITEM_CLASS_WEAPON           = 2,
        ITEM_CLASS_GEM              = 3,
        ITEM_CLASS_ARMOR            = 4,
        ITEM_CLASS_REAGENT          = 5,
        ITEM_CLASS_PROJECTILE       = 6,
        ITEM_CLASS_TRADE_GOODS      = 7,
        ITEM_CLASS_GENERIC          = 8,
        ITEM_CLASS_RECIPE           = 9,
        ITEM_CLASS_MONEY            = 10,
        ITEM_CLASS_QUIVER           = 11,
        ITEM_CLASS_QUEST            = 12,
        ITEM_CLASS_KEY              = 13,
        ITEM_CLASS_PERMANENT        = 14,
        ITEM_CLASS_MISC             = 15
    }

    public enum ItemSubclassConsumable
    {
        ITEM_SUBCLASS_CONSUMABLE    = 0,
        ITEM_SUBCLASS_POTION        = 1,
        ITEM_SUBCLASS_ELIXIR        = 2,
        ITEM_SUBCLASS_FLASK         = 3,
        ITEM_SUBCLASS_SCROLL        = 4,
        ITEM_SUBCLASS_FOOD          = 5,
        ITEM_SUBCLASS_ITEM_ENHANCEMENT = 6,
        ITEM_SUBCLASS_BANDAGE       = 7,
        ITEM_SUBCLASS_CONSUMABLE_OTHER = 8
    }

    public enum ItemSubclassContainer
    {
        ITEM_SUBCLASS_CONTAINER = 0,
        ITEM_SUBCLASS_SOUL_CONTAINER = 1,
        ITEM_SUBCLASS_HERB_CONTAINER = 2,
        ITEM_SUBCLASS_ENCHANTING_CONTAINER = 3,
        ITEM_SUBCLASS_ENGINEERING_CONTAINER = 4,
        ITEM_SUBCLASS_GEM_CONTAINER = 5,
        ITEM_SUBCLASS_MINING_CONTAINER = 6,
        ITEM_SUBCLASS_LEATHERWORKING_CONTAINER = 7
    }

    public enum ItemSubclassWeapon
    {
        ITEM_SUBCLASS_WEAPON_AXE = 0,
        ITEM_SUBCLASS_WEAPON_AXE2 = 1,
        ITEM_SUBCLASS_WEAPON_BOW = 2,
        ITEM_SUBCLASS_WEAPON_GUN = 3,
        ITEM_SUBCLASS_WEAPON_MACE = 4,
        ITEM_SUBCLASS_WEAPON_MACE2 = 5,
        ITEM_SUBCLASS_WEAPON_POLEARM = 6,
        ITEM_SUBCLASS_WEAPON_SWORD = 7,
        ITEM_SUBCLASS_WEAPON_SWORD2 = 8,
        ITEM_SUBCLASS_WEAPON_obsolete = 9,
        ITEM_SUBCLASS_WEAPON_STAFF = 10,
        ITEM_SUBCLASS_WEAPON_EXOTIC = 11,
        ITEM_SUBCLASS_WEAPON_EXOTIC2 = 12,
        ITEM_SUBCLASS_WEAPON_FIST = 13,
        ITEM_SUBCLASS_WEAPON_MISC = 14,
        ITEM_SUBCLASS_WEAPON_DAGGER = 15,
        ITEM_SUBCLASS_WEAPON_THROWN = 16,
        ITEM_SUBCLASS_WEAPON_SPEAR = 17,
        ITEM_SUBCLASS_WEAPON_CROSSBOW = 18,
        ITEM_SUBCLASS_WEAPON_WAND = 19,
        ITEM_SUBCLASS_WEAPON_FISHING_POLE = 20
    }

    public enum ItemSubclassArmor
    {
        ITEM_SUBCLASS_ARMOR_MISC = 0,
        ITEM_SUBCLASS_ARMOR_CLOTH = 1,
        ITEM_SUBCLASS_ARMOR_LEATHER = 2,
        ITEM_SUBCLASS_ARMOR_MAIL = 3,
        ITEM_SUBCLASS_ARMOR_PLATE = 4,
        ITEM_SUBCLASS_ARMOR_BUCKLER = 5,
        ITEM_SUBCLASS_ARMOR_SHIELD = 6,
        ITEM_SUBCLASS_ARMOR_LIBRAM = 7,
        ITEM_SUBCLASS_ARMOR_IDOL = 8,
        ITEM_SUBCLASS_ARMOR_TOTEM = 9
    }

    public enum ItemSubclassReagent
    {
        ITEM_SUBCLASS_REAGENT = 0
    }

    public enum ItemSubclassProjectile
    {
        ITEM_SUBCLASS_WAND = 0,        // ABS
        ITEM_SUBCLASS_BOLT = 1,        // ABS
        ITEM_SUBCLASS_ARROW = 2,
        ITEM_SUBCLASS_BULLET = 3,
        ITEM_SUBCLASS_THROWN = 4         // ABS
    }

    public enum ItemSubclassTradeGoods
    {
        ITEM_SUBCLASS_TRADE_GOODS = 0,
        ITEM_SUBCLASS_PARTS = 1,
        ITEM_SUBCLASS_EXPLOSIVES = 2,
        ITEM_SUBCLASS_DEVICES = 3,
        // ITEM_SUBCLASS_JEWELCRAFTING                 = 4,
        ITEM_SUBCLASS_CLOTH = 5,
        ITEM_SUBCLASS_LEATHER = 6,
        ITEM_SUBCLASS_METAL_STONE = 7,
        ITEM_SUBCLASS_MEAT = 8,
        ITEM_SUBCLASS_HERB = 9,
        ITEM_SUBCLASS_ELEMENTAL = 10,
        ITEM_SUBCLASS_TRADE_GOODS_OTHER = 11,
        ITEM_SUBCLASS_ENCHANTING = 12
    }

    public enum ItemSubclassGeneric
    {
        ITEM_SUBCLASS_GENERIC = 0
    }

    public enum ItemSubclassRecipe
    {
        ITEM_SUBCLASS_BOOK = 0,
        ITEM_SUBCLASS_LEATHERWORKING_PATTERN = 1,
        ITEM_SUBCLASS_TAILORING_PATTERN = 2,
        ITEM_SUBCLASS_ENGINEERING_SCHEMATIC = 3,
        ITEM_SUBCLASS_BLACKSMITHING = 4,
        ITEM_SUBCLASS_COOKING_RECIPE = 5,
        ITEM_SUBCLASS_ALCHEMY_RECIPE = 6,
        ITEM_SUBCLASS_FIRST_AID_MANUAL = 7,
        ITEM_SUBCLASS_ENCHANTING_FORMULA = 8,
        ITEM_SUBCLASS_FISHING_MANUAL = 9,
    }

    public enum ItemSubclassMoney
    {
        ITEM_SUBCLASS_MONEY = 0
    }

    public enum ItemSubclassQuiver
    {
        ITEM_SUBCLASS_QUIVER0 = 0,        // ABS
        ITEM_SUBCLASS_QUIVER1 = 1,        // ABS
        ITEM_SUBCLASS_QUIVER = 2,
        ITEM_SUBCLASS_AMMO_POUCH = 3
    }

    public enum ItemSubclassQuest
    {
        ITEM_SUBCLASS_QUEST = 0
    }

    public enum ItemSubclassKey
    {
        ITEM_SUBCLASS_KEY = 0,
        ITEM_SUBCLASS_LOCKPICK = 1
    }

    public enum ItemSubclassPermanent
    {
        ITEM_SUBCLASS_PERMANENT = 0
    }

    public enum ItemSubclassJunk
    {
        ITEM_SUBCLASS_JUNK = 0,
        ITEM_SUBCLASS_JUNK_REAGENT = 1,
        ITEM_SUBCLASS_JUNK_PET = 2,
        ITEM_SUBCLASS_JUNK_HOLIDAY = 3,
        ITEM_SUBCLASS_JUNK_OTHER = 4,
        ITEM_SUBCLASS_JUNK_MOUNT = 5
    }

    public enum ItemQualities
    {
        ITEM_QUALITY_POOR = 0,                 // GREY
        ITEM_QUALITY_NORMAL = 1,                 // WHITE
        ITEM_QUALITY_UNCOMMON = 2,                 // GREEN
        ITEM_QUALITY_RARE = 3,                 // BLUE
        ITEM_QUALITY_EPIC = 4,                 // PURPLE
        ITEM_QUALITY_LEGENDARY = 5,                 // ORANGE
        ITEM_QUALITY_ARTIFACT = 6                  // LIGHT YELLOW
    }

    public enum TradeStatus
    {
        TRADE_STATUS_BUSY = 0,
        TRADE_STATUS_BEGIN_TRADE = 1,
        TRADE_STATUS_OPEN_WINDOW = 2,
        TRADE_STATUS_TRADE_CANCELED = 3,
        TRADE_STATUS_TRADE_ACCEPT = 4,
        TRADE_STATUS_BUSY_2 = 5,
        TRADE_STATUS_NO_TARGET = 6,
        TRADE_STATUS_BACK_TO_TRADE = 7,
        TRADE_STATUS_TRADE_COMPLETE = 8,
        TRADE_STATUS_TRADE_REJECTED = 9,
        TRADE_STATUS_TARGET_TO_FAR = 10,
        TRADE_STATUS_WRONG_FACTION = 11,
        TRADE_STATUS_CLOSE_WINDOW = 12,
        // 13?
        TRADE_STATUS_IGNORE_YOU = 14,
        TRADE_STATUS_YOU_STUNNED = 15,
        TRADE_STATUS_TARGET_STUNNED = 16,
        TRADE_STATUS_YOU_DEAD = 17,
        TRADE_STATUS_TARGET_DEAD = 18,
        TRADE_STATUS_YOU_LOGOUT = 19,
        TRADE_STATUS_TARGET_LOGOUT = 20,
        TRADE_STATUS_TRIAL_ACCOUNT = 21,                       // Trial accounts can not perform that action
        TRADE_STATUS_WRONG_REALM = 22,                       // You can only trade conjured items... (cross realm BG related).
        TRADE_STATUS_NOT_ON_TAPLIST = 23                        // Related to trading soulbound loot items
    }

    public enum InventoryResult
    {
        EQUIP_ERR_OK = 0,
        EQUIP_ERR_CANT_EQUIP_LEVEL_I = 1,       // ERR_CANT_EQUIP_LEVEL_I
        EQUIP_ERR_CANT_EQUIP_SKILL = 2,       // ERR_CANT_EQUIP_SKILL
        EQUIP_ERR_ITEM_DOESNT_GO_TO_SLOT = 3,       // ERR_WRONG_SLOT
        EQUIP_ERR_BAG_FULL = 4,       // ERR_BAG_FULL
        EQUIP_ERR_NONEMPTY_BAG_OVER_OTHER_BAG = 5,       // ERR_BAG_IN_BAG
        EQUIP_ERR_CANT_TRADE_EQUIP_BAGS = 6,       // ERR_TRADE_EQUIPPED_BAG
        EQUIP_ERR_ONLY_AMMO_CAN_GO_HERE = 7,       // ERR_AMMO_ONLY
        EQUIP_ERR_NO_REQUIRED_PROFICIENCY = 8,       // ERR_PROFICIENCY_NEEDED
        EQUIP_ERR_NO_EQUIPMENT_SLOT_AVAILABLE = 9,       // ERR_NO_SLOT_AVAILABLE
        EQUIP_ERR_YOU_CAN_NEVER_USE_THAT_ITEM = 10,      // ERR_CANT_EQUIP_EVER
        EQUIP_ERR_YOU_CAN_NEVER_USE_THAT_ITEM2 = 11,      // ERR_CANT_EQUIP_EVER
        EQUIP_ERR_NO_EQUIPMENT_SLOT_AVAILABLE2 = 12,      // ERR_NO_SLOT_AVAILABLE
        EQUIP_ERR_CANT_EQUIP_WITH_TWOHANDED = 13,      // ERR_2HANDED_EQUIPPED
        EQUIP_ERR_CANT_DUAL_WIELD = 14,      // ERR_2HSKILLNOTFOUND
        EQUIP_ERR_ITEM_DOESNT_GO_INTO_BAG = 15,      // ERR_WRONG_BAG_TYPE
        EQUIP_ERR_ITEM_DOESNT_GO_INTO_BAG2 = 16,      // ERR_WRONG_BAG_TYPE
        EQUIP_ERR_CANT_CARRY_MORE_OF_THIS = 17,      // ERR_ITEM_MAX_COUNT
        EQUIP_ERR_NO_EQUIPMENT_SLOT_AVAILABLE3 = 18,      // ERR_NO_SLOT_AVAILABLE
        EQUIP_ERR_ITEM_CANT_STACK = 19,      // ERR_CANT_STACK
        EQUIP_ERR_ITEM_CANT_BE_EQUIPPED = 20,      // ERR_NOT_EQUIPPABLE
        EQUIP_ERR_ITEMS_CANT_BE_SWAPPED = 21,      // ERR_CANT_SWAP
        EQUIP_ERR_SLOT_IS_EMPTY = 22,      // ERR_SLOT_EMPTY
        EQUIP_ERR_ITEM_NOT_FOUND = 23,      // ERR_ITEM_NOT_FOUND
        EQUIP_ERR_CANT_DROP_SOULBOUND = 24,      // ERR_DROP_BOUND_ITEM
        EQUIP_ERR_OUT_OF_RANGE = 25,      // ERR_OUT_OF_RANGE
        EQUIP_ERR_TRIED_TO_SPLIT_MORE_THAN_COUNT = 26,      // ERR_TOO_FEW_TO_SPLIT
        EQUIP_ERR_COULDNT_SPLIT_ITEMS = 27,      // ERR_SPLIT_FAILED
        EQUIP_ERR_MISSING_REAGENT = 28,      // ERR_SPELL_FAILED_REAGENTS_GENERIC
        EQUIP_ERR_NOT_ENOUGH_MONEY = 29,      // ERR_NOT_ENOUGH_MONEY
        EQUIP_ERR_NOT_A_BAG = 30,      // ERR_NOT_A_BAG
        EQUIP_ERR_CAN_ONLY_DO_WITH_EMPTY_BAGS = 31,      // ERR_DESTROY_NONEMPTY_BAG
        EQUIP_ERR_DONT_OWN_THAT_ITEM = 32,      // ERR_NOT_OWNER
        EQUIP_ERR_CAN_EQUIP_ONLY1_QUIVER = 33,      // ERR_ONLY_ONE_QUIVER
        EQUIP_ERR_MUST_PURCHASE_THAT_BAG_SLOT = 34,      // ERR_NO_BANK_SLOT
        EQUIP_ERR_TOO_FAR_AWAY_FROM_BANK = 35,      // ERR_NO_BANK_HERE
        EQUIP_ERR_ITEM_LOCKED = 36,      // ERR_ITEM_LOCKED
        EQUIP_ERR_YOU_ARE_STUNNED = 37,      // ERR_GENERIC_STUNNED
        EQUIP_ERR_YOU_ARE_DEAD = 38,      // ERR_PLAYER_DEAD
        EQUIP_ERR_CANT_DO_RIGHT_NOW = 39,      // ERR_CLIENT_LOCKED_OUT
        EQUIP_ERR_INT_BAG_ERROR = 40,      // ERR_INTERNAL_BAG_ERROR
        EQUIP_ERR_CAN_EQUIP_ONLY1_BOLT = 41,      // ERR_ONLY_ONE_BOLT
        EQUIP_ERR_CAN_EQUIP_ONLY1_AMMOPOUCH = 42,      // ERR_ONLY_ONE_AMMO
        EQUIP_ERR_STACKABLE_CANT_BE_WRAPPED = 43,      // ERR_CANT_WRAP_STACKABLE
        EQUIP_ERR_EQUIPPED_CANT_BE_WRAPPED = 44,      // ERR_CANT_WRAP_EQUIPPED
        EQUIP_ERR_WRAPPED_CANT_BE_WRAPPED = 45,      // ERR_CANT_WRAP_WRAPPED
        EQUIP_ERR_BOUND_CANT_BE_WRAPPED = 46,      // ERR_CANT_WRAP_BOUND
        EQUIP_ERR_UNIQUE_CANT_BE_WRAPPED = 47,      // ERR_CANT_WRAP_UNIQUE
        EQUIP_ERR_BAGS_CANT_BE_WRAPPED = 48,      // ERR_CANT_WRAP_BAGS
        EQUIP_ERR_ALREADY_LOOTED = 49,      // ERR_LOOT_GONE
        EQUIP_ERR_INVENTORY_FULL = 50,      // ERR_INV_FULL
        EQUIP_ERR_BANK_FULL = 51,      // ERR_BAG_FULL
        EQUIP_ERR_ITEM_IS_CURRENTLY_SOLD_OUT = 52,      // ERR_VENDOR_SOLD_OUT
        EQUIP_ERR_BAG_FULL3 = 53,      // ERR_BAG_FULL
        EQUIP_ERR_ITEM_NOT_FOUND2 = 54,      // ERR_ITEM_NOT_FOUND
        EQUIP_ERR_ITEM_CANT_STACK2 = 55,      // ERR_CANT_STACK
        EQUIP_ERR_BAG_FULL4 = 56,      // ERR_BAG_FULL
        EQUIP_ERR_ITEM_SOLD_OUT = 57,      // ERR_VENDOR_SOLD_OUT
        EQUIP_ERR_OBJECT_IS_BUSY = 58,      // ERR_OBJECT_IS_BUSY
        EQUIP_ERR_NONE = 59,      // ERR_CANT_BE_DISENCHANTED
        EQUIP_ERR_NOT_IN_COMBAT = 60,      // ERR_NOT_IN_COMBAT
        EQUIP_ERR_NOT_WHILE_DISARMED = 61,      // ERR_NOT_WHILE_DISARMED
        EQUIP_ERR_BAG_FULL6 = 62,      // ERR_BAG_FULL
        EQUIP_ERR_CANT_EQUIP_RANK = 63,      // ERR_CANT_EQUIP_RANK
        EQUIP_ERR_CANT_EQUIP_REPUTATION = 64,      // ERR_CANT_EQUIP_REPUTATION
        EQUIP_ERR_TOO_MANY_SPECIAL_BAGS = 65,      // ERR_TOO_MANY_SPECIAL_BAGS
        EQUIP_ERR_LOOT_CANT_LOOT_THAT_NOW = 66,      // ERR_LOOT_CANT_LOOT_THAT_NOW
                                                     // any greater values show as "bag full"
    }


    public enum LootSlotType
    {
        LOOT_SLOT_NORMAL = 0,                                  // can be looted
        LOOT_SLOT_VIEW = 1,                                  // can be only view (ignore any loot attempts)
        LOOT_SLOT_MASTER = 2,                                  // can be looted only master (error message)
        LOOT_SLOT_REQS = 3,                                  // can't be looted (error message about missing reqs)
        MAX_LOOT_SLOT_TYPE                                      // custom, use for mark skipped from show items
    }

    public enum LootItemType
    {
        LOOTITEM_TYPE_NORMAL = 1,
        LOOTITEM_TYPE_QUEST = 2,
        LOOTITEM_TYPE_CONDITIONNAL = 3
    }


    // masks for ITEM_FIELD_FLAGS field
    public enum ItemDynFlags
    {
        ITEM_DYNFLAG_BINDED = 0x00000001, // set in game at binding
        ITEM_DYNFLAG_UNK1 = 0x00000002,
        ITEM_DYNFLAG_UNLOCKED = 0x00000004, // have meaning only for item with proto->LockId, if not set show as "Locked, req. lockpicking N"
        ITEM_DYNFLAG_WRAPPED = 0x00000008, // mark item as wrapped into wrapper container
        ITEM_DYNFLAG_UNK4 = 0x00000010, // can't repeat old note: appears red icon (like when item durability==0)
        ITEM_DYNFLAG_UNK5 = 0x00000020,
        ITEM_DYNFLAG_UNK6 = 0x00000040, // ? old note: usable
        ITEM_DYNFLAG_UNK7 = 0x00000080,
        ITEM_DYNFLAG_UNK8 = 0x00000100,
        ITEM_DYNFLAG_READABLE = 0x00000200, // can be open for read, it or item proto pagetText make show "Right click to read"
        ITEM_DYNFLAG_UNK10 = 0x00000400,
        ITEM_DYNFLAG_UNK11 = 0x00000800,
        ITEM_DYNFLAG_UNK12 = 0x00001000,
        ITEM_DYNFLAG_UNK13 = 0x00002000,
        ITEM_DYNFLAG_UNK14 = 0x00004000,
        ITEM_DYNFLAG_UNK15 = 0x00008000,
        ITEM_DYNFLAG_UNK16 = 0x00010000,
        ITEM_DYNFLAG_UNK17 = 0x00020000,
    }

    public enum EnchantmentSlot
    {
        PERM_ENCHANTMENT_SLOT = 0,
        TEMP_ENCHANTMENT_SLOT = 1,
        MAX_INSPECTED_ENCHANTMENT_SLOT = 2,

        PROP_ENCHANTMENT_SLOT_0 = 3,                        // used with RandomSuffix
        PROP_ENCHANTMENT_SLOT_1 = 4,                        // used with RandomSuffix
        PROP_ENCHANTMENT_SLOT_2 = 5,                        // used with RandomSuffix
        PROP_ENCHANTMENT_SLOT_3 = 6,
        MAX_ENCHANTMENT_SLOT = 7
    }

    public enum EnchantmentOffset
    {
        ENCHANTMENT_ID_OFFSET = 0,
        ENCHANTMENT_DURATION_OFFSET = 1,
        ENCHANTMENT_CHARGES_OFFSET = 2
    }
}
