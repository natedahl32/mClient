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

        /// <summary>
        /// Maximum number of quests that can be in the log
        /// </summary>
        public const int MAX_QUEST_LOG_SIZE = 20;
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

    public enum QuestFailedReasons
    {
        INVALIDREASON_DONT_HAVE_REQ = 0,
        INVALIDREASON_QUEST_FAILED_LOW_LEVEL = 1,        // You are not high enough level for that quest.
        INVALIDREASON_QUEST_FAILED_WRONG_RACE = 6,        // That quest is not available to your race.
        INVALIDREASON_QUEST_ONLY_ONE_TIMED = 12,       // You can only be on one timed quest at a time.
        INVALIDREASON_QUEST_ALREADY_ON = 13,       // You are already on that quest
        INVALIDREASON_QUEST_FAILED_MISSING_ITEMS = 21,       // You don't have the required items with you. Check storage.
        INVALIDREASON_QUEST_FAILED_NOT_ENOUGH_MONEY = 23,       // You don't have enough money for that quest.
                                                                //[-ZERO] tbc enumerations [?]
        INVALIDREASON_QUEST_ALREADY_ON2 = 18,       // You are already on that quest
        INVALIDREASON_QUEST_ALREADY_DONE = 7,        // You have completed that quest.
    }

    public enum QuestStatus
    {
        QUEST_STATUS_NONE = 0,
        QUEST_STATUS_COMPLETE = 1,
        QUEST_STATUS_UNAVAILABLE = 2,
        QUEST_STATUS_INCOMPLETE = 3,
        QUEST_STATUS_AVAILABLE = 4,                        // unused in fact
        QUEST_STATUS_FAILED = 5,
        MAX_QUEST_STATUS
    }

    public enum QuestGiverStatus
    {
        DIALOG_STATUS_NONE = 0,
        DIALOG_STATUS_UNAVAILABLE = 1,
        DIALOG_STATUS_CHAT = 2,
        DIALOG_STATUS_INCOMPLETE = 3,
        DIALOG_STATUS_REWARD_REP = 4,
        DIALOG_STATUS_AVAILABLE = 5,
        DIALOG_STATUS_REWARD_OLD = 6,             // red dot on minimap
        DIALOG_STATUS_REWARD2 = 7,             // yellow dot on minimap
                                               // [-ZERO] tbc?  DIALOG_STATUS_REWARD                   = 8              // yellow dot on minimap
        DIALOG_STATUS_UNDEFINED = 100            // Used as result for unassigned ScriptCall
    }

    // values based at QuestInfo.dbc
    public enum QuestTypes
    {
        QUEST_TYPE_ELITE = 1,
        QUEST_TYPE_LIFE = 21,
        QUEST_TYPE_PVP = 41,
        QUEST_TYPE_RAID = 62,
        QUEST_TYPE_DUNGEON = 81,
        // tbc?
        QUEST_TYPE_WORLD_EVENT = 82,
        QUEST_TYPE_LEGENDARY = 83,
        QUEST_TYPE_ESCORT = 84,
    }

    [Flags]
    public enum QuestFlags
    {
        // Flags used at server and sent to client
        QUEST_FLAGS_NONE = 0x00000000,
        QUEST_FLAGS_STAY_ALIVE = 0x00000001,                // Not used currently
        QUEST_FLAGS_PARTY_ACCEPT = 0x00000002,                // If player in party, all players that can accept this quest will receive confirmation box to accept quest CMSG_QUEST_CONFIRM_ACCEPT/SMSG_QUEST_CONFIRM_ACCEPT
        QUEST_FLAGS_EXPLORATION = 0x00000004,                // Not used currently
        QUEST_FLAGS_SHARABLE = 0x00000008,                // Can be shared: Player::CanShareQuest()
                                                          // QUEST_FLAGS_NONE2        = 0x00000010,               // Not used currently
        QUEST_FLAGS_EPIC = 0x00000020,                // Not used currently: Unsure of content
        QUEST_FLAGS_RAID = 0x00000040,                // Not used currently

        QUEST_FLAGS_UNK2 = 0x00000100,                // Not used currently: _DELIVER_MORE Quest needs more than normal _q-item_ drops from mobs
        QUEST_FLAGS_HIDDEN_REWARDS = 0x00000200,                // Items and money rewarded only sent in SMSG_QUESTGIVER_OFFER_REWARD (not in SMSG_QUESTGIVER_QUEST_DETAILS or in client quest log(SMSG_QUEST_QUERY_RESPONSE))
        QUEST_FLAGS_AUTO_REWARDED = 0x00000400,                // These quests are automatically rewarded on quest complete and they will never appear in quest log client side.
    }

    [Flags]
    public enum QuestSpecialFlags
    {
        // Mangos flags for set SpecialFlags in DB if required but used only at server
        QUEST_SPECIAL_FLAG_REPEATABLE = 0x001,        // |1 in SpecialFlags from DB
        QUEST_SPECIAL_FLAG_EXPLORATION_OR_EVENT = 0x002,        // |2 in SpecialFlags from DB (if required area explore, spell SPELL_EFFECT_QUEST_COMPLETE casting, table `*_script` command SCRIPT_COMMAND_QUEST_EXPLORED use, set from script DLL)
                                                                // reserved for future versions           0x004,        // |4 in SpecialFlags.

        // Mangos flags for internal use only
        QUEST_SPECIAL_FLAG_DELIVER = 0x008,        // Internal flag computed only
        QUEST_SPECIAL_FLAG_SPEAKTO = 0x010,        // Internal flag computed only
        QUEST_SPECIAL_FLAG_KILL_OR_CAST = 0x020,        // Internal flag computed only
        QUEST_SPECIAL_FLAG_TIMED = 0x040,        // Internal flag computed only
    }
}
