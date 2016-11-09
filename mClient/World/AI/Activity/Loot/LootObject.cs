using mClient.Clients;
using mClient.World.AI.Activity.Messages;
using mClient.World.Items;
using System.Collections.Generic;

namespace mClient.World.AI.Activity.Loot
{
    public class LootObject : BaseActivity
    {
        #region Declarations

        private Object mLootableObject;
        private bool mIsLooting;
        private List<LootItem> mItemsToLoot;
        private LootItem mCurrentlyLootingItem;

        #endregion

        #region Constructors

        public LootObject(Object lootable, PlayerAI ai) : base(ai)
        {
            if (lootable == null) throw new System.ArgumentNullException("lootable");
            mLootableObject = lootable;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Looting Object"; }
        }

        #endregion

        #region Public Methods

        public override void Process()
        {
            // We are close enough, if we aren't looting start looting 
            if (!mIsLooting)
            {
                // Are we close enough to our lootable
                var distance = PlayerAI.Client.movementMgr.CalculateDistance(mLootableObject.Position);
                if (distance > MovementMgr.MINIMUM_FOLLOW_DISTANCE)
                {
                    // Set our follow target and then exit out as running
                    PlayerAI.SetFollowTarget(mLootableObject);
                    return;
                }

                // We are close enough, now loot it
                PlayerAI.Client.Loot(mLootableObject.Guid);
                mIsLooting = true;
                return;
            }

            // If we don't have items to loot yet keep waiting for them
            if (mItemsToLoot == null) return;
            
            // If we have items to loot still do that now
            if (mItemsToLoot.Count > 0)
            {
                // If we are already looting an item don't try to loot another one until we are done
                if (mCurrentlyLootingItem != null)
                    return;

                mCurrentlyLootingItem = mItemsToLoot[0];
                mItemsToLoot.RemoveAt(0);

                // Loot the item
                if (mCurrentlyLootingItem != null)
                    if (mCurrentlyLootingItem.LootSlotType == 0)
                        PlayerAI.Client.LootItem(mCurrentlyLootingItem.LootSlot);

                return;
            }

            // We aren't done until we have looted the last item, meaning our mCurrentlyLootingItem would be null
            if (mCurrentlyLootingItem == null)
            {
                // No more items to loot. Remove the lootable and end the activity
                PlayerAI.Player.RemoveLootable(mLootableObject.Guid);
                PlayerAI.CompleteActivity();
            }
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

            // Handle loot response message
            if (message.MessageType == Constants.WorldServerOpCode.SMSG_LOOT_RESPONSE)
            {
                var lootMessage = message as LootMessage;
                if (lootMessage != null)
                {
                    // If we have gold to loot, do that now
                    if (lootMessage.CoinAmount > 0)
                        PlayerAI.Client.LootMoney();

                    // Set the items to loot
                    mItemsToLoot = lootMessage.Items;
                }
            }

            // Handle inventory change result message
            if (message.MessageType == Constants.WorldServerOpCode.SMSG_INVENTORY_CHANGE_FAILURE)
            {
                var inventoryMessage = message as InventoryChangeMessage;
                if (inventoryMessage != null)
                    HandleInventoryMessage(inventoryMessage);
            }

            // Handle item push
            if (message.MessageType == Constants.WorldServerOpCode.SMSG_ITEM_PUSH_RESULT)
            {
                // TODO: Should we do anything else with this message? Add it to our inventory
                // or try to equip it if it is an item
                var itemPushMessage = message as ItemPushResultMessage;
                if (itemPushMessage != null)
                    if (itemPushMessage.ItemId == mCurrentlyLootingItem.ItemId)
                        mCurrentlyLootingItem = null;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles different message types from an inventory change message
        /// </summary>
        /// <param name="message"></param>
        private void HandleInventoryMessage(InventoryChangeMessage message)
        {
            // if inventory is full, we can't loot anymore
            if (message.ResultMessage == Constants.InventoryResult.EQUIP_ERR_INVENTORY_FULL)
            {
                // Clear the current item we are looting and all other items
                mCurrentlyLootingItem = null;
                mItemsToLoot.Clear();

                // Send release loot
                PlayerAI.Client.ReleaseLoot(mLootableObject.Guid.GetOldGuid());
                return;
            }

            if (message.ResultMessage != Constants.InventoryResult.EQUIP_ERR_OK)
                mCurrentlyLootingItem = null;
        }

        #endregion
    }
}
