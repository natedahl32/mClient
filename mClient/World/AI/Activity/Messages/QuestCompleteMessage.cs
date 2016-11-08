using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class QuestCompleteMessage : ActivityMessage
    {
        #region Constructors

        protected QuestCompleteMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public QuestCompleteMessage() : this(WorldServerOpCode.SMSG_QUESTGIVER_QUEST_COMPLETE)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Id of the quest being completed
        /// </summary>
        public UInt32 QuestId { get; set; }

        #endregion
    }
}
