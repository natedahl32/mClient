using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Constants
{
    /// <summary>
    /// Commands given to bot players regarding movement
    /// </summary>
    public enum MoveCommands
    {
        None,
        Stay,
        Follow
    }

    [Flags]
    public enum MovementFlags
    {
        MOVEMENTFLAG_NONE = 0x00000000,
        MOVEMENTFLAG_FORWARD = 0x00000001,
        MOVEMENTFLAG_BACKWARD = 0x00000002,
        MOVEMENTFLAG_STRAFE_LEFT = 0x00000004,
        MOVEMENTFLAG_STRAFE_RIGHT = 0x00000008,
        MOVEMENTFLAG_LEFT = 0x00000010,
        MOVEMENTFLAG_RIGHT = 0x00000020,
        MOVEMENTFLAG_PITCH_UP = 0x00000040,
        MOVEMENTFLAG_PITCH_DOWN = 0x00000080,
        MOVEMENTFLAG_WALK = 0x00000100,
        MOVEMENTFLAG_UNKNOWN = 0x00000200,
        MOVEMENTFLAG_LEVITATING = 0x00000400,
        MOVEMENTFLAG_FLYING = 0x00000800,
        MOVEMENTFLAG_FALLING = 0x00002000,
        MOVEMENTFLAG_FALLINGFAR = 0x00004000,
        MOVEMENTFLAG_SWIMMING = 0x00200000,               // appears with fly flag also
        MOVEMENTFLAG_SPLINE_ENABLED = 0x00400000,
        MOVEMENTFLAG_CAN_FLY = 0x00800000,
        MOVEMENTFLAG_FLYING_OLD = 0x01000000,
        MOVEMENTFLAG_ONTRANSPORT = 0x02000000,
        MOVEMENTFLAG_SPLINE_ELEVATION = 0x04000000,               // probably wrong name
        MOVEMENTFLAG_ROOT = 0x08000000,
        MOVEMENTFLAG_WATERWALKING = 0x10000000,
        MOVEMENTFLAG_SAFE_FALL = 0x20000000,               // active rogue safe fall spell (passive)
        MOVEMENTFLAG_HOVER = 0x40000000
    };

    [Flags]
    public enum SplineFlags : uint
    {
        None = 0x00000000,
        Done = 0x00000001,
        Falling = 0x00000002,           // Affects elevation computation
        Unknown3 = 0x00000004,
        Unknown4 = 0x00000008,
        Unknown5 = 0x00000010,
        Unknown6 = 0x00000020,
        Unknown7 = 0x00000040,
        Unknown8 = 0x00000080,
        Runmode = 0x00000100,
        Flying = 0x00000200,           // Smooth movement(Catmullrom interpolation mode), flying animation
        No_Spline = 0x00000400,
        Unknown12 = 0x00000800,
        Unknown13 = 0x00001000,
        Unknown14 = 0x00002000,
        Unknown15 = 0x00004000,
        Unknown16 = 0x00008000,
        Final_Point = 0x00010000,
        Final_Target = 0x00020000,
        Final_Angle = 0x00040000,
        Unknown19 = 0x00080000,           // exists, but unknown what it does
        Cyclic = 0x00100000,           // Movement by cycled spline
        Enter_Cycle = 0x00200000,           // Everytimes appears with cyclic flag in monster move packet, erases first spline vertex after first cycle done
        Frozen = 0x00400000,           // Will never arrive
        Unknown23 = 0x00800000,
        Unknown24 = 0x01000000,
        Unknown25 = 0x02000000,          // exists, but unknown what it does
        Unknown26 = 0x04000000,
        Unknown27 = 0x08000000,
        Unknown28 = 0x10000000,
        Unknown29 = 0x20000000,
        Unknown30 = 0x40000000,
        Unknown31 = 0x80000000,

        // Masks
        Mask_Final_Facing = Final_Point | Final_Target | Final_Angle,
        // flags that shouldn't be appended into SMSG_MONSTER_MOVE\SMSG_MONSTER_MOVE_TRANSPORT packet, should be more probably
        Mask_No_Monster_Move = Mask_Final_Facing | Done,
        // CatmullRom interpolation mode used
        Mask_CatmullRom = Flying,
    };

    public enum SplineMode : byte
    {
        Linear = 0,
        CatmullRom = 1,
        Bezier3 = 2
    }

    public enum SplineType : byte
    {
        Normal = 0,
        Stop = 1,
        FacingSpot = 2,
        FacingTarget = 3,
        FacingAngle = 4
    }
}
