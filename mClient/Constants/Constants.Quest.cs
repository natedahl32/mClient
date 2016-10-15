using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Constants
{
    public static class QuestConstants
    {
        /// <summary>
        /// Quest offset to use for quest flags
        /// </summary>
        public const int MAX_QUEST_OFFSET = 3;
    }

    public enum QuestSlotOffsets
    {
        QUEST_ID_OFFSET = 0,
        QUEST_COUNT_STATE_OFFSET = 1,                        // including counters 6bits+6bits+6bits+6bits + state 8bits
        QUEST_TIME_OFFSET = 2
    }

    [Flags]
    public enum QuestSlotStateMask
    {
        QUEST_STATE_NONE = 0x0000,
        QUEST_STATE_COMPLETE = 0x0001,
        QUEST_STATE_FAIL = 0x0002
    }
}
