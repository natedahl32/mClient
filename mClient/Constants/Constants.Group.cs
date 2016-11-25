using System;

namespace mClient.Constants
{
    public enum LootMethod : byte
    {
        FREE_FOR_ALL = 0,
        ROUND_ROBIN = 1,
        MASTER_LOOT = 2,
        GROUP_LOOT = 3,
        NEED_BEFORE_GREED = 4,

        NOT_GROUP_TYPE_LOOT = 5                                 // internal use only
    }

    [Flags]
    public enum RollVoteMask
    {
        ROLL_VOTE_MASK_PASS = 0x01,
        ROLL_VOTE_MASK_NEED = 0x02,
        ROLL_VOTE_MASK_GREED = 0x04,
        ROLL_VOTE_MASK_DISENCHANT = 0x08,

        ROLL_VOTE_MASK_ALL = 0x0F,
    }

    public enum RollVote
    {
        ROLL_PASS = 0,
        ROLL_NEED = 1,
        ROLL_GREED = 2,
        ROLL_DISENCHANT = 3,
        ROLL_NOT_EMITED_YET = 4,                             // send to client
        ROLL_NOT_VALID = 5                                   // not send to client
    }
}
