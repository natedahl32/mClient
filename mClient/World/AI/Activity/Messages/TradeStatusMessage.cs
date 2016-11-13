using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mClient.Constants;

namespace mClient.World.AI.Activity.Messages
{
    public class TradeStatusMessage : ActivityMessage
    {
        #region Constructors

        protected TradeStatusMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public TradeStatusMessage() : this(WorldServerOpCode.SMSG_TRADE_STATUS)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the status of the trade
        /// </summary>
        public TradeStatus TradeStatus { get; set; }

        #endregion
    }
}
