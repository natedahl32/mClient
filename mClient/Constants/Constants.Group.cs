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

    [Flags]
    public enum GroupUpdateFlags
    {
        GROUP_UPDATE_FLAG_NONE = 0x00000000,       // nothing
        GROUP_UPDATE_FLAG_STATUS = 0x00000001,       // uint8, flags
        GROUP_UPDATE_FLAG_CUR_HP = 0x00000002,       // uint16
        GROUP_UPDATE_FLAG_MAX_HP = 0x00000004,       // uint16
        GROUP_UPDATE_FLAG_POWER_TYPE = 0x00000008,       // uint8
        GROUP_UPDATE_FLAG_CUR_POWER = 0x00000010,       // uint16
        GROUP_UPDATE_FLAG_MAX_POWER = 0x00000020,       // uint16
        GROUP_UPDATE_FLAG_LEVEL = 0x00000040,       // uint16
        GROUP_UPDATE_FLAG_ZONE = 0x00000080,       // uint16
        GROUP_UPDATE_FLAG_POSITION = 0x00000100,       // uint16, uint16
        GROUP_UPDATE_FLAG_AURAS = 0x00000200,       // uint32 mask, for each bit set uint16 spellid
        GROUP_UPDATE_FLAG_PET_GUID = 0x00000400,       // uint64 pet guid
        GROUP_UPDATE_FLAG_PET_NAME = 0x00000800,       // pet name, nullptr terminated string
        GROUP_UPDATE_FLAG_PET_MODEL_ID = 0x00001000,       // uint16, model id
        GROUP_UPDATE_FLAG_PET_CUR_HP = 0x00002000,       // uint16 pet cur health
        GROUP_UPDATE_FLAG_PET_MAX_HP = 0x00004000,       // uint16 pet max health
        GROUP_UPDATE_FLAG_PET_POWER_TYPE = 0x00008000,       // uint8 pet power type
        GROUP_UPDATE_FLAG_PET_CUR_POWER = 0x00010000,       // uint16 pet cur power
        GROUP_UPDATE_FLAG_PET_MAX_POWER = 0x00020000,       // uint16 pet max power
        GROUP_UPDATE_FLAG_PET_AURAS = 0x00040000,       // uint32 mask, for each bit set uint16 spellid, pet auras...

        GROUP_UPDATE_PET = 0x0007FC00,       // all pet flags
        GROUP_UPDATE_FULL = 0x0007FFFF,       // all known flags
    }
}
