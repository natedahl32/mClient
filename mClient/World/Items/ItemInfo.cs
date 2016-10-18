﻿using mClient.Constants;
using System;
using System.Collections.Generic;

namespace mClient.World.Items
{
    public class ItemInfo
    {
        #region Declarations

        private List<ItemStat> mItemStats = new List<ItemStat>();
        private List<ItemDamage> mItemDamages = new List<ItemDamage>();
        private Dictionary<SpellSchools, UInt32> mResistances = new Dictionary<SpellSchools, uint>();
        private List<ItemSpellEffect> mSpellEffects = new List<ItemSpellEffect>();

        #endregion

        /// <summary>
        /// Gets or sets the item id
        /// </summary>
        public UInt32 ItemId { get; set; }

        /// <summary>
        /// Gets or sets the item name
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the item class
        /// </summary>
        public ItemClass ItemClass { get; set; }

        /// <summary>
        /// Gets or sets the item sub class consumable
        /// </summary>
        public ItemSubclassConsumable SubClassConsumable { get; set; }

        /// <summary>
        /// Gets or sets the item quality
        /// </summary>
        public ItemQualities Quality { get; set; }

        /// <summary>
        /// Gets or sets the item flags
        /// </summary>
        public UInt32 ItemFlags { get; set; }

        /// <summary>
        /// Gets or sets the buy price
        /// </summary>
        public UInt32 BuyPrice { get; set; }

        /// <summary>
        /// Gets or sets the sell price
        /// </summary>
        public UInt32 SellPrice { get; set; }

        /// <summary>
        /// Gets or sets the inventory type
        /// </summary>
        public InventoryType InventoryType { get; set; }

        /// <summary>
        /// Gets or sets the allowable class
        /// </summary>
        public UInt32 AllowableClass { get; set; }

        /// <summary>
        /// Gets or sets the allowable race
        /// </summary>
        public UInt32 AllowableRace { get; set; }

        /// <summary>
        /// Gets or sets the item level
        /// </summary>
        public UInt32 ItemLevel { get; set; }

        /// <summary>
        /// Gets or sets the required level
        /// </summary>
        public UInt32 RequiredLevel { get; set; }

        /// <summary>
        /// Gets or sets the required skill
        /// </summary>
        public UInt32 RequiredSkill { get; set; }

        /// <summary>
        /// Gets or sets the required skill rank
        /// </summary>
        public UInt32 RequiredSkillRank { get; set; }

        /// <summary>
        /// Gets or sets the required spell
        /// </summary>
        public UInt32 RequiredSpell { get; set; }

        /// <summary>
        /// Gets or sets the required honor rank
        /// </summary>
        public UInt32 RequiredHonorRank { get; set; }

        /// <summary>
        /// Gets or sets the required city rank
        /// </summary>
        public UInt32 RequiredCityRank { get; set; }

        /// <summary>
        /// Gets or sets the required reputation faction
        /// </summary>
        public UInt32 RequiredReputationFaction { get; set; }

        /// <summary>
        /// Gets or sets the required reputation faction rank
        /// </summary>
        public UInt32 RequiredRepuationFactionRank { get; set; }

        /// <summary>
        /// Gets or sets the maximum count
        /// </summary>
        public UInt32 MaxCount { get; set; }

        /// <summary>
        /// Gets or sets whether or not stackable
        /// </summary>
        public UInt32 Stackable { get; set; }

        /// <summary>
        /// Gets or sets the number of container slots
        /// </summary>
        public UInt32 ContainerSlots { get; set; }

        /// <summary>
        /// Gets the stats for this item
        /// </summary>
        public IList<ItemStat> ItemStats
        {
            get { return mItemStats; }
        }

        /// <summary>
        /// Gets the item damages for this item
        /// </summary>
        public IList<ItemDamage> ItemDamages
        {
            get { return mItemDamages; }
        }

        /// <summary>
        /// Gets resistances
        /// </summary>
        public IDictionary<SpellSchools, UInt32> Resistances
        {
            get { return mResistances; }
        }

        /// <summary>
        /// Gets or sets the delay
        /// </summary>
        public UInt32 Delay { get; set; }

        /// <summary>
        /// Gets or sets the ammo type
        /// </summary>
        public UInt32 AmmoType { get; set; }

        /// <summary>
        /// Gets or sets the ranged mod range
        /// </summary>
        public float RangedModRange { get; set; }

        /// <summary>
        /// Gets all spell effects
        /// </summary>
        public IList<ItemSpellEffect> SpellEffects
        {
            get { return mSpellEffects; }
        }

        /// <summary>
        /// Gets or sets the bonding type
        /// </summary>
        public ItemBondingType Bonding { get; set; }

        /// <summary>
        /// Gets or sets the id of the quest this item starts
        /// </summary>
        public UInt32 StartsQuestId { get; set; }

        /// <summary>
        /// Gets or sets the random property
        /// </summary>
        public UInt32 RandomProperty { get; set; }

        /// <summary>
        /// Gets or sets the block
        /// </summary>
        public UInt32 Block { get; set; }

        /// <summary>
        /// Gets or sets the item set
        /// </summary>
        public UInt32 ItemSet { get; set; }

        /// <summary>
        /// Gets or sets the maximum durability
        /// </summary>
        public UInt32 MaxDurability { get; set; }

        /// <summary>
        /// Gets or sets the bag family
        /// </summary>
        public UInt32 BagFamily { get; set; }
    }
}
