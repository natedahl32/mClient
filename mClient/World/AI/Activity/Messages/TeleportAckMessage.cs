using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mClient.Constants;
using mClient.Shared;

namespace mClient.World.AI.Activity.Messages
{
    public class TeleportAckMessage : ActivityMessage
    {
        #region Constructors

        protected TeleportAckMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public TeleportAckMessage() : this(WorldServerOpCode.MSG_MOVE_TELEPORT_ACK)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unit that is teleporting
        /// </summary>
        public WoWGuid Teleporter { get; set; }

        /// <summary>
        /// Gets or sets the location teleporting to
        /// </summary>
        public Coordinate Location { get; set; }

        #endregion
    }
}
