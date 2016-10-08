using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Constants
{
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
}
