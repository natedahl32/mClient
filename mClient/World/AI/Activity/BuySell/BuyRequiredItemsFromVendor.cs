using mClient.Clients;
using mClient.Constants;
using mClient.World.AI.Activity.Messages;
using mClient.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mClient.World.AI.Activity.BuySell
{
    public class BuyRequiredItemsFromVendor : BaseActivity
    {
        #region Declarations

        private Clients.Unit mVendor;
        private bool mCancelBuy = false;
        private bool mRequestInventory = false;
        private bool mVendorWindowOpen = false;
        private List<RequiredItemData> mRequiredItemsToBuy;

        #endregion

        #region Constructors

        public BuyRequiredItemsFromVendor(Clients.Unit vendor, PlayerAI ai) : base(ai)
        {
            if (vendor == null) throw new ArgumentNullException("vendor");
            mVendor = vendor;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Buying Required Items From Vendor"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();
            PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, "I'm checking if I can buy stuff from a vendor real quick.");
        }

        public override void Process()
        {
            // Are we in range of the vendor?
            if (PlayerAI.Client.movementMgr.CalculateDistance(mVendor.Position) > MovementMgr.MINIMUM_FOLLOW_DISTANCE)
            {
                // TODO: Blindly setting the vendor as follow target is dangerous. We could run
                // right into a pack of hostiles. Should fix this!
                PlayerAI.SetFollowTarget(mVendor);
                return;
            }

            if (mRequestInventory)
            {
                // Wait for the vendor window to be ope
                if (!mVendorWindowOpen) return;

                // If we don't have a list of items we need to buy from this vendor or we don't have any more items to buy from this vendor
                if (mRequiredItemsToBuy == null || mRequiredItemsToBuy.Count == 0)
                {
                    PlayerAI.CompleteActivity();
                    return;
                }

                // Get the next item in the list and buy it
                var item = mRequiredItemsToBuy[0];
                mRequiredItemsToBuy.RemoveAt(0);

                // Get the vendor item for this item
                var vendorItem = mVendor.VendorItemsAvailable.Where(vi => vi.ItemId == item.ItemId).FirstOrDefault();
                if (vendorItem != null)
                {
                    var numberToBuy = (item.ItemCount == 1 ? 1 : (vendorItem.CurrentVendorCount > 0 ? Math.Min(vendorItem.CurrentVendorCount, item.ItemCount) : item.ItemCount));
                    PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, $"Buying {vendorItem.Item.ItemGameLink}.");
                    PlayerAI.Client.BuyItem(mVendor.Guid.GetOldGuid(), vendorItem.ItemId, (byte)numberToBuy);
                }

                // TODO: Do we need to add the item to our inventory or will an update catch that?
                return;
            }

            // Get the inventory of the vendor
            PlayerAI.Client.ListVendorInventory(mVendor.Guid.GetOldGuid());
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
                        mCancelBuy = true;
                    else
                    {
                        mVendorWindowOpen = true;
                        // Get a specific list of required items that we can buy from this vendor
                        mRequiredItemsToBuy = PlayerAI.Player.RequiredItemsThatAreNeeded.Where(ri => mVendor.VendorItemsAvailable.Any(vi => vi.ItemId == ri.ItemId)).ToList();
                    }
                }
            }
        }

        #endregion
    }
}
