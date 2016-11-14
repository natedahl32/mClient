using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mClient.Constants;

namespace mClient.World.AI.Activity.Messages
{
    public class VendorInventoryListMessage : ActivityMessage
    {
        #region Constructors

        protected VendorInventoryListMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public VendorInventoryListMessage() : this(WorldServerOpCode.SMSG_LIST_INVENTORY)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of items for sale
        /// </summary>
        public uint ItemCount { get; set; }

        // TODO: Add more properties for this message

        #endregion
    }
}
