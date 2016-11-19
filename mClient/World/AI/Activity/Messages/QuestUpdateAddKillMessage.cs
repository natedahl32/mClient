using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mClient.Constants;

namespace mClient.World.AI.Activity.Messages
{
    public class QuestUpdateAddKillMessage : ActivityMessage
    {
        #region Constructors

        protected QuestUpdateAddKillMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public QuestUpdateAddKillMessage() : this(WorldServerOpCode.SMSG_QUESTUPDATE_ADD_KILL)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the id of the quest that was updated
        /// </summary>
        public uint QuestId { get; set; }

        /// <summary>
        /// Gets or sets the entry id of the creature of GO that was updated
        /// </summary>
        public uint CreateOrGoId { get; set; }

        /// <summary>
        /// Gets or sets the amount of kills for the objective
        /// </summary>
        public uint KillCount { get; set; }

        /// <summary>
        /// Gets or sets the number of required kills to meet the objective
        /// </summary>
        public uint KillRequiredCount { get; set; }

        #endregion
    }
}
