using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class QuestListMessage : ActivityMessage
    {
        #region Constructors

        protected QuestListMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public QuestListMessage() : this(WorldServerOpCode.SMSG_QUESTGIVER_QUEST_LIST)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// The guid of the entity that sent the quest list
        /// </summary>
        public UInt64 FromEntityGuid { get; set; }

        /// <summary>
        /// List of quest ids returned from a quest giver
        /// </summary>
        public IList<UInt32> QuestIdList { get; set; }

        #endregion
    }
}
