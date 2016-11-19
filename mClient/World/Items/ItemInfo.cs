using mClient.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        public uint SubClass { get; set; }

        /// <summary>
        /// Gets or sets the item quality
        /// </summary>
        public ItemQualities Quality { get; set; }

        /// <summary>
        /// Gets or sets the item flags
        /// </summary>
        public ItemPrototypeFlags ItemFlags { get; set; }

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

        /// <summary>
        /// Gets the in-game link for this item
        /// </summary>
        public string ItemGameLink
        {
            get
            {
                return " |" + string.Format("c{0:X8}", ItemConstants.ItemQualityColors[(int)Quality]).ToLower() + "|Hitem:" + ItemId.ToString() + ":0:0:0:0:0:0:0|h[" + ItemName + "]|h|r";
            }
        }

        #region Public Methods

        public string DumpInfo()
        {
            var dump = string.Empty;
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                if (descriptor.PropertyType != typeof(IList<ItemSpellEffect>))
                {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(this);
                    dump += string.Format("{0}: {1} {2}", name, value, Environment.NewLine);
                }
            }

            // Now dump all item spell effects and other arrays/lists
            var i = 1;
            foreach (var effect in SpellEffects)
            {
                dump += string.Format("Item Spell Effect {0}: {1}", i, Environment.NewLine);
                dump += string.Format("  Spell ID: {0} {1}", effect.SpellId, Environment.NewLine);
                dump += string.Format("  Spell Trigger: {0} {1}", effect.SpellTrigger, Environment.NewLine);
                dump += string.Format("  Spell Charges: {0} {1}", effect.SpellCharges, Environment.NewLine);
                i++;
            }

            return dump;
        }

        #endregion

            #region Static Methods

            /// <summary>
            /// Extracts an item id from a message where an item was linked. If multiple items are linked in the message
            /// only the first item will be extracted.
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
        public static uint ExtractItemId(string message)
        {
            if (string.IsNullOrEmpty(message)) return 0;

            var index = message.IndexOf("|Hitem:");
            var startId = message.IndexOf(":", index);
            if (startId > -1)
            {
                var endId = message.IndexOf(":", startId + 1);
                var itemId = message.Substring(startId + 1, endId - (startId + 1));
                return Convert.ToUInt32(itemId);
            }

            // Could not find item id in the message
            return 0;
        }

        /// <summary>
        /// Extracts a list item ids from a message where an item or multiple items were linked. 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static List<uint> ExtractItemIds(string message)
        {
            if (string.IsNullOrEmpty(message)) return new List<uint>();

            var itemIds = new List<uint>();
            var index = message.IndexOf("|Hitem:");
            while (index > -1)
            {
                var startId = message.IndexOf(":", index);
                var endId = message.IndexOf(":", startId + 1);
                var itemId = message.Substring(startId + 1, endId - (startId + 1));
                itemIds.Add(Convert.ToUInt32(itemId));

                // Get the next index starting from the last end index
                index = message.IndexOf("|Hitem:", endId);
            }

            return itemIds;
        }

        #endregion
    }
}
