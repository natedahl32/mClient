using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class SpellLearnedMessage : ActivityMessage
    {
        #region Constructors

        protected SpellLearnedMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public SpellLearnedMessage() : this(WorldServerOpCode.SMSG_LEARNED_SPELL)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the spell id that was learned
        /// </summary>
        public uint SpellId { get; set; }

        #endregion
    }
}
