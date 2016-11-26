using mClient.Clients.UpdateBlocks;
using mClient.Constants;
using mClient.Network;
using mClient.Shared;
using mClient.World.AI.Activity.Loot;
using mClient.World.AI.Activity.Messages;
using mClient.World.Items;
using System;
using System.Collections.Generic;

namespace mClient.Clients
{
    public partial class WorldServerClient
    {
        #region Packet Handlers

        /// <summary>
        /// Handles inventory list from vendor
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_LIST_INVENTORY)]
        public void HandleListInventory(PacketIn packet)
        {
            var message = new VendorInventoryListMessage();
            var vendorGuid = packet.ReadUInt64();
            var itemCount = packet.ReadByte();

            if (itemCount > 0)
            {
                var itemIndex = packet.ReadUInt32(); // index in the inventory list, looks like this is 1-based
                var itemId = packet.ReadUInt32();
                packet.ReadUInt32();    // Display ID
                var currentVendorCount = packet.ReadUInt32();
                var price = packet.ReadUInt32();
                var maxDurability = packet.ReadUInt32();
                var buyCount = packet.ReadUInt32();
            }

            message.ItemCount = itemCount;
            player.PlayerAI.SendMessageToAllActivities(message);
        }

        /// <summary>
        /// Handles rolls for loot
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_LOOT_START_ROLL)]
        public void HandleLootRollStart(PacketIn packet)
        {
            RollOnLootMessage message = new RollOnLootMessage();
            message.LootSourceGuid = packet.ReadUInt64();
            message.ItemSlot = packet.ReadUInt32();
            message.ItemId = packet.ReadUInt32();
            message.RandomSuffix = packet.ReadUInt32();
            message.RandomPropertyId = packet.ReadUInt32();
            message.RollTimeout = packet.ReadUInt32();
            // Tells us what roll options we have
            message.RollOptionMask = packet.ReadByte();

            // TODO: Probably need to fix this. We don't want to sit and wait for a loot roll while we are in combat, which will happen if we loot and get this message during combat
            // Start the activity before we query for the item prototype
            player.PlayerAI.StartActivity(new RollForLoot(message.LootSourceGuid, message.ItemSlot, message.ItemId, message.RandomSuffix, message.RandomPropertyId, message.RollTimeout, message.RollOptionMask, player.PlayerAI));

            // Check for the item and if we don't have it yet send a request for it
            var item = ItemManager.Instance.Get(message.ItemId);
            if (item == null)
                QueryItemPrototype(message.ItemId);

            // Send the message in case activities are expecting the message
            player.PlayerAI.SendMessageToAllActivities(message);
        }

        /// <summary>
        /// Handles loot being released by a player
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_LOOT_RELEASE_RESPONSE)]
        public void HandleLootRelease(PacketIn packet)
        {
            var lootGuid = packet.ReadUInt64();
            // TODO: Figure out under what circumstances this is called?
        }

        /// <summary>
        /// Handles loot lists
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_LOOT_LIST)]
        public void HandleLootList(PacketIn packet)
        {
            // note - we don't get this opcode for free for all loot
            var lootGuid = packet.ReadUInt64();
            var masterLooterGuid = packet.ReadPackedGuidToWoWGuid();
            var currentLooterGuid = packet.ReadPackedGuidToWoWGuid();

            // We are now adding all killed enemies to our loot list so we at least try and loot items we might
            // need, such as quest items. Adding the below line while also adding each killed enemy to our lootable list
            // causes issues.

            // if this is our loot then add it to our Loot
            //if (currentLooterGuid.GetOldGuid() == player.Guid.GetOldGuid())
            //    player.AddLootable(new WoWGuid(lootGuid));
        }

        /// <summary>
        /// Handles a loot response
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_LOOT_RESPONSE)]
        public void HandleLootResponse(PacketIn packet)
        {
            var itemsToLoot = new List<LootItem>();

            var guidTarget = packet.ReadUInt64();   // Not sure what this is for
            var clientLootType = packet.ReadByte();
            var goldAmount = packet.ReadUInt32();
            var itemCount = packet.ReadByte();
            for (int i = 0; i < itemCount; i++)
            {
                var lootItem = new LootItem();
                lootItem.Read(packet);
                itemsToLoot.Add(lootItem);
            }

            var message = new LootMessage() { CoinAmount = goldAmount, Items = itemsToLoot };
            player.PlayerAI.SendMessageToAllActivities(message);
        }

        /// <summary>
        /// Handles equip errors
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_INVENTORY_CHANGE_FAILURE)]
        public void HandleInventoryChangeFailure(PacketIn packet)
        {
            var message = new InventoryChangeMessage();
            message.ResultMessage = (InventoryResult)packet.ReadByte();
            if (message.ResultMessage != InventoryResult.EQUIP_ERR_OK)
            {
                if (message.ResultMessage == InventoryResult.EQUIP_ERR_CANT_EQUIP_LEVEL_I)
                    message.RequiredLevel = packet.ReadUInt32();
                

                message.ItemGuid = packet.ReadUInt64();
                message.Item2Guid = packet.ReadUInt64();
                packet.ReadByte();
            }

            player.PlayerAI.SendMessageToAllActivities(message);
            SendChatMsg(ChatMsg.Party, Languages.Universal, string.Format("Inventory change failure with msg {0}", message.ResultMessage));
        }

        /// <summary>
        /// Handles a trade status from the server
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_TRADE_STATUS)]
        public void HandleTradeStatus(PacketIn packet)
        {
            var message = new TradeStatusMessage();
            var tradeStatus = (TradeStatus)packet.ReadUInt32();
            message.TradeStatus = tradeStatus;
            switch(tradeStatus)
            {
                case TradeStatus.TRADE_STATUS_BEGIN_TRADE:
                    var traderGuid = packet.ReadUInt64();
                    BeginTrade();
                    break;
                case TradeStatus.TRADE_STATUS_CLOSE_WINDOW:
                    var result = (InventoryResult)packet.ReadUInt32();
                    var isTargetResult = packet.ReadByte();
                    var itemLimitCategoryId = packet.ReadUInt32();  // From ItemLimitCategory.dbc
                    break;
                case TradeStatus.TRADE_STATUS_OPEN_WINDOW:
                    // TODO: window is open, we can put items into trade now
                    break;
                case TradeStatus.TRADE_STATUS_TRADE_ACCEPT:
                    AcceptTrade();
                    break;
                default:
                    break;
            }

            // Send the message
            player.PlayerAI.SendMessageToAllActivities(message);
        }

        /// <summary>
        /// Handles item pushed to us from server (usually after looting an item)
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_ITEM_PUSH_RESULT)]
        public void HandleItemPushResult(PacketIn packet)
        {
            var message = new ItemPushResultMessage();
            message.PlayerGuid = packet.ReadUInt64();
            message.Looted = (packet.ReadUInt32() == 0);
            message.Created = (packet.ReadUInt32() != 0);
            packet.ReadUInt32();
            message.BagSlot = packet.ReadByte();
            message.ItemSlot = packet.ReadUInt32();
            message.ItemId = packet.ReadUInt32();
            message.ItemSuffixFactor = packet.ReadUInt32();
            message.RandomPropertyId = packet.ReadUInt32();
            message.Count = packet.ReadUInt32();
            message.CountInInventory = packet.ReadUInt32();

            player.PlayerAI.SendMessageToAllActivities(message);
        }

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
            if (ItemManager.Instance.Exists(itemId)) return;

            ItemInfo item = new ItemInfo()
            {
                ItemId = itemId,
                ItemClass = (ItemClass)itemClass,
                SubClass = itemSubClass,
                ItemName = itemName
            };

            // Bunk names
            packet.ReadByte();
            packet.ReadByte();
            packet.ReadByte();

            packet.ReadUInt32();    // display info ID
            item.Quality = (ItemQualities)packet.ReadUInt32();
            item.ItemFlags = (ItemPrototypeFlags)packet.ReadUInt32();
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

            ItemManager.Instance.Add(item);
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
        /// Initiates a trade with another PC or Bot
        /// </summary>
        /// <param name="otherTrader"></param>
        public void InitiateTrade(WoWGuid otherTrader)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_INITIATE_TRADE);
            packet.Write(otherTrader.GetOldGuid());
            Send(packet);
        }

        /// <summary>
        /// Cancels the current trade
        /// </summary>
        public void CancelTrade()
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_CANCEL_TRADE);
            Send(packet);
        }

        /// <summary>
        /// Begins a trade when initiated by another player
        /// </summary>
        public void BeginTrade()
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_BEGIN_TRADE);
            Send(packet);
        }

        /// <summary>
        /// Accepts the current trade
        /// </summary>
        public void AcceptTrade()
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_ACCEPT_TRADE);
            packet.Write((UInt32)0);
            Send(packet);
        }

        /// <summary>
        /// Adds an item to the trade window
        /// </summary>
        /// <param name="tradeSlot">Slot in trade window to add</param>
        /// <param name="inventoryItemSlot">Inventory slot to add to trade window</param>
        public void AddItemToTradeWindow(byte tradeSlot, InventoryItemSlot inventoryItemSlot)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_SET_TRADE_ITEM);
            packet.Write(tradeSlot);
            packet.Write((byte)inventoryItemSlot.Bag);
            packet.Write((byte)inventoryItemSlot.Slot);
            Send(packet);
        }

        /// <summary>
        /// Auto equips the item 
        /// </summary>
        /// <param name="bagSlot"></param>
        /// <param name="slot"></param>
        public void AutoEquipItem(byte bagSlot, byte slot)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_AUTOEQUIP_ITEM);
            packet.Write(bagSlot);
            packet.Write(slot);
            Send(packet);
        }

        /// <summary>
        /// Starts the loot sequence for an object
        /// </summary>
        /// <param name="guid"></param>
        public void Loot(WoWGuid guid)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_LOOT);
            packet.Write(guid.GetOldGuid());
            Send(packet);
        }

        /// <summary>
        /// Loots money from lootable object
        /// </summary>
        public void LootMoney()
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_LOOT_MONEY);
            Send(packet);

        }

        /// <summary>
        /// Loots a slot from a lootable object
        /// </summary>
        /// <param name="itemSlot">Slot to loot</param>
        public void LootItem(byte itemSlot)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_AUTOSTORE_LOOT_ITEM);
            packet.Write(itemSlot);
            Send(packet);
        }

        /// <summary>
        /// Releases an item we are currently looting
        /// </summary>
        /// <param name="guid"></param>
        public void ReleaseLoot(ulong guid)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_LOOT_RELEASE);
            packet.Write(guid);
            Send(packet);
        }

        /// <summary>
        /// Uses a game object in the world
        /// </summary>
        /// <param name="guid"></param>
        public void UseGameObject(WoWGuid guid)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_GAMEOBJ_USE);
            packet.Write(guid.GetOldGuid());
            Send(packet);
        }

        /// <summary>
        /// Destroys an item in inventory
        /// </summary>
        /// <param name="itemSlot">Inventory slot to destroy</param>
        public void DestroyItem(InventoryItemSlot itemSlot)
        {
            DestroyItem(itemSlot, 1);
        }

        /// <summary>
        /// Destroys an item in inventory
        /// </summary>
        /// <param name="itemSlot">Inventory slot to destroy</param>
        /// <param name="count">Number of items to destroy</param>
        public void DestroyItem(InventoryItemSlot itemSlot, byte count)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_DESTROYITEM);
            packet.Write((byte)itemSlot.Bag);
            packet.Write((byte)itemSlot.Slot);
            packet.Write(count);
            packet.Write((byte)0);
            packet.Write((byte)0);
            packet.Write((byte)0);
            Send(packet);
        }

        /// <summary>
        /// Requests the inventory list for a vendor
        /// </summary>
        /// <param name="guid"></param>
        public void ListVendorInventory(ulong guid)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_LIST_INVENTORY);
            packet.Write(guid);
            Send(packet);
        }

        /// <summary>
        /// Sells an item to a vendor
        /// </summary>
        /// <param name="vendorGuid">Guid of vendor to sell to</param>
        /// <param name="itemGuid">Guid of item to sell</param>
        /// <param name="count">Number of items to sell</param>
        public void SellItem(ulong vendorGuid, ulong itemGuid, byte count)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_SELL_ITEM);
            packet.Write(vendorGuid);
            packet.Write(itemGuid);
            packet.Write(count);
            Send(packet);
        }

        /// <summary>
        /// Uses an item that is currently in inventory
        /// </summary>
        /// <param name="bagIndex"></param>
        /// <param name="slot"></param>
        /// <param name="target"></param>
        public void UseItemInInventoryOnTarget(byte bagIndex, byte slot, WoWGuid target)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_USE_ITEM);
            packet.Write(bagIndex);
            packet.Write(slot);
            packet.Write((byte)0);

            var spellCastTargets = new SpellCastTargets();
            spellCastTargets.TargetsMask = SpellTargetFlags.TARGET_FLAG_UNIT;
            spellCastTargets.UnitTargetGuid = target;
            spellCastTargets.WriteToPacket(ref packet);

            Send(packet);
        }

        /// <summary>
        /// Rolls for an item
        /// </summary>
        public void RollForItem(ulong guid, uint itemSlot, RollVote roll)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_LOOT_ROLL);
            packet.Write(guid);
            packet.Write(itemSlot);
            packet.Write((byte)roll);
            Send(packet);
        }

        /// <summary>
        /// Repairs all items in the players inventory
        /// </summary>
        /// <param name="mpcGuid"></param>
        public void RepairAllItems(ulong npcGuid)
        {
            RepairItem(npcGuid, 0);
        }

        /// <summary>
        /// Repairs a single item that belongs to the player
        /// </summary>
        /// <param name="mpcGuid"></param>
        /// <param name="itemGuid"></param>
        public void RepairItem(ulong npcGuid, ulong itemGuid)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_REPAIR_ITEM);
            packet.Write(npcGuid);
            packet.Write(itemGuid);
            Send(packet);
        }

        #endregion
    }
}
