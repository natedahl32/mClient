using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Quest
{
    /// <summary>
    /// An item in a players quest log. Data specific to a quest for a player
    /// </summary>
    public class QuestLogItem
    {
        #region Declarations

        private uint mQuestId;
        private uint mCountersAndState;
        private uint mTime;

        #endregion

        #region Constructors

        public QuestLogItem(uint questId, uint countersAndState, uint time)
        {
            mQuestId = questId;
            mCountersAndState = countersAndState;
            mTime = time;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the quest id of this quest
        /// </summary>
        public uint QuestId { get { return mQuestId; } }

        /// <summary>
        /// Gets the state of the quest
        /// </summary>
        public QuestSlotStateMask QuestState
        {
            get
            {
                // State is the final bit in the counters and state value
                // Counters are the first 24 bits, the state is the last 8 bits
                var bytes = BitConverter.GetBytes(mCountersAndState);
                if (bytes.Length != 4) throw new ApplicationException("Converted uint does not equal 4 bytes!");
                return (QuestSlotStateMask)bytes[3];
            }
        }

        /// <summary>
        /// Gets whether or not the quest is complete
        /// </summary>
        public bool IsComplete
        {
            get
            {
                return QuestState.HasFlag(QuestSlotStateMask.QUEST_STATE_COMPLETE);
            }
        }

        #endregion
    }
}
