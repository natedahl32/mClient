using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Constants
{
    public enum SpellTargetFlags : uint
    {
        Self = 0,
        SpellTargetFlag_Dynamic_0x1 = 0x1,
        Unit = 0x0002,
        SpellTargetFlag_Dynamic_0x4 = 0x4,
        SpellTargetFlag_Dynamic_0x8 = 0x8,
        Item = 0x10,
        SourceLocation = 0x20,
        DestinationLocation = 0x40,
        UnkObject_0x80 = 0x80,
        UnkUnit_0x100 = 0x100,
        PvPCorpse = 0x200,
        UnitCorpse = 0x400,
        Object = 0x800,
        TradeItem = 0x1000,
        String = 0x2000,
        /// <summary>
        /// For spells that open an object
        /// </summary>
        OpenObject = 0x4000,
        Corpse = 0x8000,
        SpellTargetFlag_Dynamic_0x10000 = 0x10000,
        Glyph = 0x20000,

        Flag_0x200000 = 0x200000,
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
