using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mClient.Constants;
using mClient.Shared;

namespace mClient.World.AI.Activity.Messages
{
    public class CorpseQueryMessage : ActivityMessage
    {
        #region Constructors

        protected CorpseQueryMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public CorpseQueryMessage() : this(WorldServerOpCode.MSG_CORPSE_QUERY)
        {   }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the map id the corpse is in
        /// </summary>
        public UInt32 MapId { get; set; }

        /// <summary>
        /// Gets or sets the location of the corpse in the map
        /// </summary>
        public Coordinate Location { get; set; }

        #endregion
    }
}
