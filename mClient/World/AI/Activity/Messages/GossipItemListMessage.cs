using mClient.Constants;
using mClient.World.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class GossipItemListMessage : ActivityMessage
    {
        #region Constructors

        protected GossipItemListMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public GossipItemListMessage() : this(WorldServerOpCode.SMSG_GOSSIP_MESSAGE)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the guid of the npc providing the gossip
        /// </summary>
        public ulong NpcGuid { get; set; }

        /// <summary>
        /// Gets or sets the list of gossip items provided by the Npc
        /// </summary>
        public List<GossipMenuItem> GossipItems { get; set; }

        #endregion
    }
}
