using mClient.Constants;
using mClient.Network;
using mClient.Shared;
using mClient.World.Items;
using System;

namespace mClient.Clients
{
    public partial class WorldServerClient
    {
        #region Packet Handlers

        /// <summary>
        /// Handles an item query from the server
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_ITEM_QUERY_SINGLE_RESPONSE)]
        public void HandleItemQueryResponse(PacketIn packet)
        {
            var itemId = packet.ReadUInt32();
            if (packet.Remaining == 0)
            {
                // No item info found for item!
                return;
            }

            var itemClass = packet.ReadUInt32();
            var itemSubClass = packet.ReadUInt32();
            var itemName = packet.ReadString();

            // If we already have the item we don't need to go any further
            if (ItemManager.Instance.ContainsItem(itemId)) return;

            ItemInfo item = new ItemInfo()
            {
                ItemId = itemId,
                ItemClass = (ItemClass)itemClass,
                SubClassConsumable = (ItemSubclassConsumable)itemSubClass,
                ItemName = itemName
            };

            // Bunk names
            packet.ReadByte();
            packet.ReadByte();
            packet.ReadByte();

            packet.ReadUInt32();    // display info ID
            item.Quality = (ItemQualities)packet.ReadUInt32();
            item.ItemFlags = packet.ReadUInt32();
            item.BuyPrice = packet.ReadUInt32();
            item.SellPrice = packet.ReadUInt32();
            item.InventoryType = (InventoryType)packet.ReadUInt32();
            item.AllowableClass = packet.ReadUInt32();
            item.AllowableRace = packet.ReadUInt32();
            item.ItemLevel = packet.ReadUInt32();
            item.RequiredLevel = packet.ReadUInt32();
            item.RequiredSkill = packet.ReadUInt32();
            item.RequiredSkillRank = packet.ReadUInt32();
            item.RequiredSpell = packet.ReadUInt32();
            item.RequiredHonorRank = packet.ReadUInt32();
            item.RequiredCityRank = packet.ReadUInt32();
            item.RequiredReputationFaction = packet.ReadUInt32();
            item.RequiredRepuationFactionRank = packet.ReadUInt32();
            item.MaxCount = packet.ReadUInt32();
            item.Stackable = packet.ReadUInt32();
            item.ContainerSlots = packet.ReadUInt32();

            // Item stats
            for (int i = 0; i < ItemConstants.MAX_ITEM_PROTO_STATS; i++)
            {
                var itemStat = new ItemStat();
                itemStat.StatType = (ItemModType)packet.ReadUInt32();
                itemStat.StatValue = packet.ReadUInt32();
                if (itemStat.StatValue > 0)
                    item.ItemStats.Add(itemStat);
            }

            // Item damages
            for (int i = 0; i < ItemConstants.MAX_ITEM_PROTO_DAMAGES; i++)
            {
                var itemDamage = new ItemDamage();
                itemDamage.MinDamage = packet.ReadFloat();
                itemDamage.MaxDamage = packet.ReadFloat();
                itemDamage.DamageType = (SpellSchools)packet.ReadUInt32();
                if (itemDamage.MaxDamage > 0)
                    item.ItemDamages.Add(itemDamage);
            }

            item.Resistances.Add(SpellSchools.SPELL_SCHOOL_NORMAL, packet.ReadUInt32());
            item.Resistances.Add(SpellSchools.SPELL_SCHOOL_HOLY, packet.ReadUInt32());
            item.Resistances.Add(SpellSchools.SPELL_SCHOOL_FIRE, packet.ReadUInt32());
            item.Resistances.Add(SpellSchools.SPELL_SCHOOL_NATURE, packet.ReadUInt32());
            item.Resistances.Add(SpellSchools.SPELL_SCHOOL_FROST, packet.ReadUInt32());
            item.Resistances.Add(SpellSchools.SPELL_SCHOOL_SHADOW, packet.ReadUInt32());
            item.Resistances.Add(SpellSchools.SPELL_SCHOOL_ARCANE, packet.ReadUInt32());

            item.Delay = packet.ReadUInt32();
            item.AmmoType = packet.ReadUInt32();
            item.RangedModRange = packet.ReadFloat();

            for (int i = 0; i < ItemConstants.MAX_ITEM_PROTO_SPELLS; i++)
            {
                var itemSpellEffect = new ItemSpellEffect();
                itemSpellEffect.SpellId = packet.ReadUInt32();
                itemSpellEffect.SpellTrigger = packet.ReadUInt32();
                itemSpellEffect.SpellCharges = packet.ReadUInt32();
                var cooldown = packet.ReadUInt32();
                var category = packet.ReadUInt32();
                var categoryCooldown = packet.ReadUInt32();

                if (itemSpellEffect.SpellId > 0)
                    item.SpellEffects.Add(itemSpellEffect);
            }

            item.Bonding = (ItemBondingType)packet.ReadUInt32();
            var description = packet.ReadString();
            var pageText = packet.ReadUInt32();
            var languageId = packet.ReadUInt32();
            var pageMaterial = packet.ReadUInt32();
            item.StartsQuestId = packet.ReadUInt32();
            var lockId = packet.ReadUInt32();
            var material = packet.ReadUInt32();
            var sheath = packet.ReadUInt32();
            item.RandomProperty = packet.ReadUInt32();
            item.Block = packet.ReadUInt32();
            item.ItemSet = packet.ReadUInt32();
            item.MaxDurability = packet.ReadUInt32();
            var area = packet.ReadUInt32();
            var mapId = packet.ReadUInt32();
            item.BagFamily = packet.ReadUInt32();

            ItemManager.Instance.AddItem(item);
        }

        #endregion

        #region Actions

        /// <summary>
        /// Queries the server for an item prototype
        /// </summary>
        /// <param name="itemId"></param>
        public void QueryItemPrototype(UInt32 itemId)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_ITEM_QUERY_SINGLE);
            packet.Write(itemId);
            packet.Write((UInt64)0);
            Send(packet);
        }

        /// <summary>
        /// Queries the server for an item prototype
        /// </summary>
        /// <param name="itemId"></param>
        public void QueryItemPrototype(UInt64 guid)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_ITEM_QUERY_SINGLE);
            packet.Write((UInt32)0);
            packet.Write(guid);
            Send(packet);
        }

        #endregion
    }
}
