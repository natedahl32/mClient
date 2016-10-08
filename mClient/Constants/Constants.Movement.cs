﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Constants
{
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
        //MOVEMENTFLAG_ONTRANSPORT = 0x00000200,
        MOVEMENTFLAG_LEVITATING = 0x00000400,
        MOVEMENTFLAG_FLYING = 0x00000800,
        MOVEMENTFLAG_JUMPING = 0x00001000,
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

    public enum SplineFlags
    {
        NONE = 0x00000000,
        POINT = 0x00010000,
        TARGET = 0x00020000,
        ORIENT = 0x00040000
    };
}
