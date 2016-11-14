using mClient.World.AI.Activity.Messages;
using mClient.World.Items;
using System;

namespace mClient.World.AI.Activity.Loot
{
    public class LootItemFromObject : BaseActivity
    {
        #region Declarations

        private LootItem mLootingItem;
        private bool mDoneLooting;

        #endregion

        #region Constructors

        public LootItemFromObject(LootItem itemLooting, PlayerAI ai) : base(ai)
        {
            if (itemLooting == null) throw new ArgumentNullException("itemLooting");
            mLootingItem = itemLooting;
            mDoneLooting = false;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Looting Item from Object"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            // Loot the item if we can
            if (mLootingItem.LootSlotType == 0)
                PlayerAI.Client.LootItem(mLootingItem.LootSlot);
            // We can't loot it
            else
                mDoneLooting = true;
        }

        public override void Process()
        {
            // If we are done looting complete this activity
            if (mDoneLooting)
            {
                PlayerAI.CompleteActivity();
                return;
            }
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

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
                    if (itemPushMessage.ItemId == mLootingItem.ItemId)
                        mDoneLooting = true;
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
            // if we get any error while looting we are done with the item
            if (message.ResultMessage != Constants.InventoryResult.EQUIP_ERR_OK)
                mDoneLooting = true;
        }

        #endregion
    }
}
