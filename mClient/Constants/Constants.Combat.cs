using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Constants
{
    public enum SpellTargetFlags : uint
    {
        TARGET_FLAG_SELF = 0x00000000,
        TARGET_FLAG_UNUSED1 = 0x00000001,               // not used in any spells (can be set dynamically)
        TARGET_FLAG_UNIT = 0x00000002,                  // pguid
        TARGET_FLAG_UNUSED2 = 0x00000004,               // not used in any spells (can be set dynamically)
        TARGET_FLAG_UNUSED3 = 0x00000008,               // not used in any spells (can be set dynamically)
        TARGET_FLAG_ITEM = 0x00000010,                  // pguid
        TARGET_FLAG_SOURCE_LOCATION = 0x00000020,       // 3 float
        TARGET_FLAG_DEST_LOCATION = 0x00000040,         // 3 float
        TARGET_FLAG_OBJECT_UNK = 0x00000080,            // used in 7 spells only
        TARGET_FLAG_UNIT_UNK = 0x00000100,              // looks like self target (389 spells)
        TARGET_FLAG_PVP_CORPSE = 0x00000200,            // pguid
        TARGET_FLAG_UNIT_CORPSE = 0x00000400,           // 10 spells (gathering professions)
        TARGET_FLAG_OBJECT = 0x00000800,                // pguid, 0 spells
        TARGET_FLAG_TRADE_ITEM = 0x00001000,            // pguid, 0 spells
        TARGET_FLAG_STRING = 0x00002000,                // string, 0 spells
        TARGET_FLAG_UNK1 = 0x00004000,                  // 199 spells, opening object/lock
        TARGET_FLAG_CORPSE = 0x00008000,                // pguid, resurrection spells
        TARGET_FLAG_UNK2 = 0x00010000,                  // pguid, not used in any spells (can be set dynamically)
    }

    public static class SpellEnumExtensions
    {
        public static bool Has(this SpellTargetFlags flags, SpellTargetFlags toCheck)
        {
            return (flags & toCheck) != 0;
        }
    }

    public enum Swing
    {
        NOSWING = 0,
        SINGLEHANDEDSWING = 1,
        TWOHANDEDSWING = 2
    }

    public enum VictimState
    {
        VICTIMSTATE_UNAFFECTED = 0,                         // seen in relation with HITINFO_MISS
        VICTIMSTATE_NORMAL = 1,
        VICTIMSTATE_DODGE = 2,
        VICTIMSTATE_PARRY = 3,
        VICTIMSTATE_INTERRUPT = 4,
        VICTIMSTATE_BLOCKS = 5,
        VICTIMSTATE_EVADES = 6,
        VICTIMSTATE_IS_IMMUNE = 7,
        VICTIMSTATE_DEFLECTS = 8
    }

    [Flags]
    public enum HitInfo
    {
        HITINFO_NORMALSWING = 0x00000000,
        HITINFO_UNK0 = 0x00000001,               // req correct packet structure
        HITINFO_NORMALSWING2 = 0x00000002,
        HITINFO_LEFTSWING = 0x00000004,
        HITINFO_UNK3 = 0x00000008,
        HITINFO_MISS = 0x00000010,
        HITINFO_ABSORB = 0x00000020,               // plays absorb sound
        HITINFO_RESIST = 0x00000040,               // resisted atleast some damage
        HITINFO_CRITICALHIT = 0x00000080,
        HITINFO_UNK8 = 0x00000100,               // wotlk?
        HITINFO_UNK9 = 0x00002000,               // wotlk?
        HITINFO_GLANCING = 0x00004000,
        HITINFO_CRUSHING = 0x00008000,
        HITINFO_NOACTION = 0x00010000,
        HITINFO_SWINGNOHITSOUND = 0x00080000
    }
}
