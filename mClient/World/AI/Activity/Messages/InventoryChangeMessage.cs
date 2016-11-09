using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class InventoryChangeMessage : ActivityMessage
    {
        #region Constructors

        protected InventoryChangeMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public InventoryChangeMessage() : this(WorldServerOpCode.SMSG_INVENTORY_CHANGE_FAILURE)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the result message for the inventory change
        /// </summary>
        public InventoryResult ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets the required level that was part of the message
        /// </summary>
        public uint? RequiredLevel { get; set; }

        /// <summary>
        /// Gets or sets the item guid that was part of the message
        /// </summary>
        public ulong? ItemGuid { get; set; }

        /// <summary>
        /// Gets or sets the item 2 guid that was part of the message
        /// </summary>
        public ulong? Item2Guid { get; set; }

        #endregion
    }
}
