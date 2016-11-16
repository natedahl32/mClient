using mClient.Constants;
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class SpellCastGoMessage : ActivityMessage
    {
        #region Constructors

        protected SpellCastGoMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public SpellCastGoMessage() : this(WorldServerOpCode.SMSG_SPELL_GO)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the guid of the caster
        /// </summary>
        public WoWGuid CasterGuid { get; set; }

        /// <summary>
        /// Gets or sets the spell id that failed
        /// </summary>
        public uint SpellId { get; set; }

        /// <summary>
        /// Gets or sets the cast flags
        /// </summary>
        public ushort CastFlags { get; set; }

        #endregion
    }
}
