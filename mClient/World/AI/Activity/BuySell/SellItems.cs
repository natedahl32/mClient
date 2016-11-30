using mClient.Clients;
using mClient.Constants;
using mClient.World.AI.Activity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mClient.World.AI.Activity.BuySell
{
    public class SellItems : BaseActivity
    {
        #region Declarations

        private Clients.Unit mSellToVendor;
        private bool mCancelSell = false;
        private bool mRequestInventory = false;
        private bool mVendorWindowOpen = false;
        private List<Clients.Item> mItemsToSell;

        #endregion

        #region Constructors

        public SellItems(Clients.Unit sellToVendor, PlayerAI ai) : base(ai)
        {
            if (sellToVendor == null) throw new ArgumentNullException("sellToVendor");
            mSellToVendor = sellToVendor;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Selling Items"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            // Find items we want to sell
            mItemsToSell = PlayerAI.Player.PlayerObject.InventoryItems.Where(i => i.Item != null && PlayerAI.Player.ShouldSellItem(i.Item)).Select(i => i.Item).ToList();
            if (mItemsToSell.Count <= 0)
                mCancelSell = true;
            else
                PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, "I'm selling items real quick.");
        }

        public override void Process()
        {
            // If we are canceling the sell then exit the activity
            if (mCancelSell)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // Are we in range of the vendor?
            if (PlayerAI.Client.movementMgr.CalculateDistance(mSellToVendor.Position) > MovementMgr.MINIMUM_FOLLOW_DISTANCE)
            {
                // TODO: Blindly setting the vendor as follow target is dangerous. We could run
                // right into a pack of hostiles. Should fix this!
                PlayerAI.SetFollowTarget(mSellToVendor);
                return;
            }

            if (mRequestInventory)
            {
                // Wait for the vendor window to be ope
                if (!mVendorWindowOpen) return;

                // If no more items to sell then complete the activity
                if (mItemsToSell.Count <= 0)
                {
                    PlayerAI.CompleteActivity();
                    return;
                }

                // Get the next item in the list and sell it
                var item = mItemsToSell[0];
                mItemsToSell.RemoveAt(0);

                PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, $"Selling {item.ItemGameLink}.");
                PlayerAI.Client.SellItem(mSellToVendor.Guid.GetOldGuid(), item.Guid.GetOldGuid(), (byte)item.StackCount);

                // Remove the item from our inventory
                PlayerAI.Player.PlayerObject.RemoveItemFromInventory(item);
            }

            // Get the inventory of the vendor
            PlayerAI.Client.ListVendorInventory(mSellToVendor.Guid.GetOldGuid());
            mRequestInventory = true;
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

            if (message.MessageType == Constants.WorldServerOpCode.SMSG_LIST_INVENTORY)
            {
                // Get the quest id list returned from this quest giver
                var vendorListMessage = message as VendorInventoryListMessage;
                if (vendorListMessage != null)
                {
                    if (vendorListMessage.ItemCount == 0)
                        mCancelSell = true;
                    else
                        mVendorWindowOpen = true;
                }
            }
        }

        #endregion
    }
}
