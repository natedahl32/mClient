using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity
{
    public abstract class ActivityMessage
    {
        #region Constructors

        public ActivityMessage(WorldServerOpCode messageType)
        {
            this.MessageType = messageType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the message type to send to activity
        /// </summary>
        public WorldServerOpCode MessageType { get; private set; }

        #endregion
    }
}
