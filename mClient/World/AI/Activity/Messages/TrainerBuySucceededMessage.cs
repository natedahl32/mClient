using mClient.Constants;
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class TrainerBuySucceededMessage : ActivityMessage
    {
        #region Constructors

        protected TrainerBuySucceededMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public TrainerBuySucceededMessage() : this(WorldServerOpCode.SMSG_TRAINER_BUY_SUCCEEDED)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the trainer guid the buy was from
        /// </summary>
        public WoWGuid TrainerGuid { get; set; }

        /// <summary>
        /// Gets or sets the id of the bought spell
        /// </summary>
        public uint SpellId { get; set; }

        #endregion
    }
}
