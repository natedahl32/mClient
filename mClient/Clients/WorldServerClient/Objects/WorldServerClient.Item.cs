﻿using mClient.Constants;
using mClient.Shared;
using mClient.World.Items;
using System;
using System.Linq;

namespace mClient.Clients
{
    public class Item : Object
    {
        #region Constructors

        public Item(WoWGuid guid) : base(guid)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the base item info
        /// </summary>
        public ItemInfo BaseInfo
        {
            get { return ItemManager.Instance.Get(this.ObjectFieldEntry); }
        }

        /// <summary>
        /// Gets the guid of the object that this item is contained in (if any)
        /// </summary>
        public WoWGuid ContainedInGuid
        {
            get
            {
                var guidLong = GetGuid(GetFieldValue((int)ItemFields.ITEM_FIELD_CONTAINED), GetFieldValue((int)ItemFields.ITEM_FIELD_CONTAINED + 1));
                return new WoWGuid(guidLong);
            }
        }

        /// <summary>
        /// Gets the current stack count of this item
        /// </summary>
        public uint StackCount
        {
            get { return GetFieldValue((int)ItemFields.ITEM_FIELD_STACK_COUNT); }
        }

        /// <summary>
        /// Gets the current durability of the item
        /// </summary>
        public uint Durability
        {
            get { return GetFieldValue((int)ItemFields.ITEM_FIELD_DURABILITY); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the equipment slot by this items inventory type
        /// </summary>
        /// <returns></returns>
        public EquipmentSlots GetEquipSlotByInventoryType()
        {
            switch (BaseInfo.InventoryType)
            {
                case InventoryType.INVTYPE_2HWEAPON:
                    return EquipmentSlots.EQUIPMENT_SLOT_MAINHAND;
                case InventoryType.INVTYPE_BODY:
                    return EquipmentSlots.EQUIPMENT_SLOT_TABARD;
                case InventoryType.INVTYPE_CHEST:
                    return EquipmentSlots.EQUIPMENT_SLOT_CHEST;
                case InventoryType.INVTYPE_CLOAK:
                    return EquipmentSlots.EQUIPMENT_SLOT_BACK;
                case InventoryType.INVTYPE_FEET:
                    return EquipmentSlots.EQUIPMENT_SLOT_FEET;
                case InventoryType.INVTYPE_FINGER:
                    return EquipmentSlots.EQUIPMENT_SLOT_FINGER1;
                case InventoryType.INVTYPE_HANDS:
                    return EquipmentSlots.EQUIPMENT_SLOT_HANDS;
                case InventoryType.INVTYPE_HEAD:
                    return EquipmentSlots.EQUIPMENT_SLOT_HEAD;
                case InventoryType.INVTYPE_LEGS:
                    return EquipmentSlots.EQUIPMENT_SLOT_LEGS;
                case InventoryType.INVTYPE_NECK:
                    return EquipmentSlots.EQUIPMENT_SLOT_NECK;
                case InventoryType.INVTYPE_RANGED:
                    return EquipmentSlots.EQUIPMENT_SLOT_RANGED;
                case InventoryType.INVTYPE_RELIC:
                    return EquipmentSlots.EQUIPMENT_SLOT_RANGED;
                case InventoryType.INVTYPE_ROBE:
                    return EquipmentSlots.EQUIPMENT_SLOT_CHEST;
                case InventoryType.INVTYPE_SHIELD:
                    return EquipmentSlots.EQUIPMENT_SLOT_OFFHAND;
                case InventoryType.INVTYPE_SHOULDERS:
                    return EquipmentSlots.EQUIPMENT_SLOT_SHOULDERS;
                case InventoryType.INVTYPE_TABARD:
                    return EquipmentSlots.EQUIPMENT_SLOT_TABARD;
                case InventoryType.INVTYPE_THROWN:
                    return EquipmentSlots.EQUIPMENT_SLOT_RANGED;
                case InventoryType.INVTYPE_TRINKET:
                    return EquipmentSlots.EQUIPMENT_SLOT_TRINKET1;
                case InventoryType.INVTYPE_WAIST:
                    return EquipmentSlots.EQUIPMENT_SLOT_WAIST;
                case InventoryType.INVTYPE_WEAPON:
                    return EquipmentSlots.EQUIPMENT_SLOT_MAINHAND;
                case InventoryType.INVTYPE_WEAPONMAINHAND:
                    return EquipmentSlots.EQUIPMENT_SLOT_MAINHAND;
                case InventoryType.INVTYPE_WEAPONOFFHAND:
                    return EquipmentSlots.EQUIPMENT_SLOT_OFFHAND;
                case InventoryType.INVTYPE_WRISTS:
                    return EquipmentSlots.EQUIPMENT_SLOT_WRISTS;
                default:
                    return EquipmentSlots.EQUIPMENT_SLOT_END;
            }
        }

        #endregion
    }

    public class InventoryItemSlot
    {
        public int Bag { get; set; }
        public int Slot { get; set; }
        public Item Item { get; set; }
    }
}
