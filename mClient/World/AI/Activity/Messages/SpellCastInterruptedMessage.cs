using mClient.Constants;
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class SpellCastInterruptedMessage : ActivityMessage
    {
        #region Constructors

        protected SpellCastInterruptedMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public SpellCastInterruptedMessage() : this(WorldServerOpCode.SMSG_SPELL_FAILURE)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the caster guid
        /// </summary>
        public WoWGuid CasterGuid { get; set; }

        /// <summary>
        /// Gets or sets the spell id that failed
        /// </summary>
        public uint SpellId { get; set; }

        #endregion
    }
}
