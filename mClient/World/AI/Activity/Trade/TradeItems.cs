using mClient.Clients;
using mClient.World.AI.Activity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Trade
{
    /// <summary>
    /// Activity that handles trading of items. This activity is started only when the trade window is open. Use
    /// the BeginTrade activity to initiate a trade.
    /// </summary>
    public class TradeItems : BaseActivity
    {
        #region Declarations

        private List<uint> mTradingItems;
        private byte mCurrentTradeSlot;
        private bool mTradeAccepted = false;
        private bool mIsTradeCompleted = false;
        private List<InventoryItemSlot> mItemsToRemove = new List<InventoryItemSlot>();

        #endregion

        #region Constructors

        public TradeItems(IList<uint> tradingItems, PlayerAI ai) : base(ai)
        {
            if (tradingItems == null) throw new ArgumentNullException("tradingItems");
            mTradingItems = tradingItems.ToList();
            mCurrentTradeSlot = 0;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Trading Items"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            // If we don't have any items to trade complete the activity
            if (mTradingItems.Count <= 0) PlayerAI.CompleteActivity();
        }
        public override void Process()
        {
            // When the trade is completed, then complete the activity
            if (mIsTradeCompleted) PlayerAI.CompleteActivity();

            // While we have items in our list to trade, add them to the trade window
            if (mTradingItems.Count > 0 && mCurrentTradeSlot < (int)Constants.TradeSlots.TRADE_SLOT_TRADED_COUNT)
            {
                // Get the first item in the list and add it to the trade window
                var itemId = mTradingItems[0];
                mTradingItems.RemoveAt(0);
                var inventoryItemSlot = PlayerAI.Player.PlayerObject.GetInventoryItem(itemId);
                if (inventoryItemSlot == null)
                    return;

                mItemsToRemove.Add(inventoryItemSlot);
                PlayerAI.Client.AddItemToTradeWindow(mCurrentTradeSlot, inventoryItemSlot);
                mCurrentTradeSlot++;
                return;
            }

            // Trade items have been added to the trade window, now accept the trade
            if (!mTradeAccepted)
            {
                mTradeAccepted = true;
                PlayerAI.Client.AcceptTrade();
            }
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

            if (message.MessageType == Constants.WorldServerOpCode.SMSG_TRADE_STATUS)
            {
                var tradeStatusMessage = message as TradeStatusMessage;
                if (tradeStatusMessage != null)
                {
                    // If our trade goes back to trade status, let's accept again. The PC probably added something to the window
                    if (tradeStatusMessage.TradeStatus == Constants.TradeStatus.TRADE_STATUS_BACK_TO_TRADE)
                    {
                        mTradeAccepted = false;
                        return;
                    }

                    // If the trade is canceled, then the trade is completed, albeit unsuccessfully
                    if (tradeStatusMessage.TradeStatus == Constants.TradeStatus.TRADE_STATUS_TRADE_CANCELED)
                    {
                        mIsTradeCompleted = true;
                        return;
                    }

                    // If the trade was completed, remove the items from inventory and mark as completed
                    if (tradeStatusMessage.TradeStatus == Constants.TradeStatus.TRADE_STATUS_TRADE_COMPLETE)
                    {
                        foreach (var item in mItemsToRemove)
                            PlayerAI.Player.PlayerObject.RemoveInventoryItemSlot(item);
                        mIsTradeCompleted = true;
                        return;
                    }

                    // We will get an accept message from the other party, we'll just ignore that
                    if (tradeStatusMessage.TradeStatus == Constants.TradeStatus.TRADE_STATUS_TRADE_ACCEPT)
                        return;

                    // Anything else just cancel the trade
                    PlayerAI.Client.CancelTrade();
                    mIsTradeCompleted = true;
                }
            }
        }

        #endregion
    }
}
