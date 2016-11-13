using mClient.Constants;
using mClient.Shared;
using mClient.World.AI.Activity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Trade
{
    /// <summary>
    /// Starts a trade with another PC or Bot
    /// </summary>
    public class BeginTrade : BaseActivity
    {
        #region Declarations

        private WoWGuid mTradingWithGuid;
        private List<uint> mItemsTradingAway;
        private bool mTradeIsCanceled = false;
        private bool mCanStartTrading = false;
        private bool mIsTrading = false;

        #endregion

        #region Constructors

        public BeginTrade(WoWGuid tradingWith, IList<uint> itemsTrading, PlayerAI ai) : base(ai)
        {
            if (tradingWith == null) throw new ArgumentNullException("tradingWith");
            if (itemsTrading == null) throw new ArgumentNullException("itemsTrading");

            mTradingWithGuid = tradingWith;
            mItemsTradingAway = itemsTrading.ToList();
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Start a Trade"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            // If we got no items to trade than complete this activity
            if (mItemsTradingAway.Count <= 0)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // If we got more items to trade than we can trade in the window then
            // complete the activity and send a message to the sender.
            if (mItemsTradingAway.Count > (int)Constants.TradeSlots.TRADE_SLOT_TRADED_COUNT)
            {
                // Get the sender object so we can grab their name
                var senderObject = PlayerAI.Client.objectMgr.getObject(mTradingWithGuid);
                if (senderObject != null)
                    PlayerAI.Client.SendChatMsg(ChatMsg.Whisper, Languages.Universal, "I'm turning in some quests now.", senderObject.Name);
                
                PlayerAI.CompleteActivity();
                return;
            }

            // Send the initiate trade message right away
            PlayerAI.Client.InitiateTrade(mTradingWithGuid);
        }

        public override void Process()
        {
            // If the trade is canceled complete the activity
            if (mTradeIsCanceled) PlayerAI.CompleteActivity();

            // If we are trading and we are back here we can complete this activity, it means the trade is done
            if (mIsTrading)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // If we can start trading, push the activity
            if (mCanStartTrading && !mIsTrading)
            {
                PlayerAI.StartActivity(new TradeItems(mItemsTradingAway, PlayerAI));
                mIsTrading = true;
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
                    // Only handle if we haven't started trading yet or we will have issues when we recieve these
                    // messages with the trade window open
                    if (!mCanStartTrading)
                    {
                        // We are looking for trade window open message
                        if (tradeStatusMessage.TradeStatus != Constants.TradeStatus.TRADE_STATUS_OPEN_WINDOW)
                        {
                            mTradeIsCanceled = true;
                            return;
                        }

                        // Trade is good to go, we can add items to the trade window now. We can't add activities
                        // here though so set a flag that says we can start trading items.
                        mCanStartTrading = true;
                    }
                }
            }
        }

        #endregion
    }
}
