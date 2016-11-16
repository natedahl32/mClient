using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class SpellCastFailedMessage : ActivityMessage
    {
        #region Constructors

        protected SpellCastFailedMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public SpellCastFailedMessage() : this(WorldServerOpCode.SMSG_CAST_FAILED)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the spell id that failed
        /// </summary>
        public uint SpellId { get; set; }

        /// <summary>
        /// Gets or sets the spell cast result
        /// </summary>
        public SpellCastResult Result { get; set; }

        #endregion
    }
}
