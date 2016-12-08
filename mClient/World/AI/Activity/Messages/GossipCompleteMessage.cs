using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class GossipCompleteMessage : ActivityMessage
    {
        #region Constructors

        protected GossipCompleteMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public GossipCompleteMessage() : this(WorldServerOpCode.SMSG_GOSSIP_COMPLETE)
        { }

        #endregion
    }
}
