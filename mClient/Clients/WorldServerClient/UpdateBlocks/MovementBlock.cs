using mClient.Constants;
using mClient.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Clients.UpdateBlocks
{
    public class MovementBlock
    {
        public ObjectUpdateFlags UpdateFlags { get; private set; }

        public MovementInfo Movement { get; private set; }

        public readonly float[] speeds = new float[6];

        public SplineInfo Spline { get; private set; }

        public uint LowGuid { get; private set; }

        public uint HighGuid { get; private set; }

        public ulong AttackingTarget { get; private set; }

        public uint TransportTime { get; private set; }

        public uint VehicleId { get; private set; }
        public float VehicleAimAdjustement { get; private set; }

        public ulong GoRotationULong { get; private set; }

        public MovementBlock()
        {
            Movement = new MovementInfo();
            Spline = new SplineInfo();
        }

        public static MovementBlock Read(PacketIn gr)
        {
            var movement = new MovementBlock();

            movement.UpdateFlags = (ObjectUpdateFlags)gr.ReadByte();

            if (movement.UpdateFlags.HasFlag(ObjectUpdateFlags.UPDATEFLAG_LIVING))
            {
                movement.Movement = MovementInfo.Read(gr);

                for (byte i = 0; i < movement.speeds.Length; ++i)
                    movement.speeds[i] = gr.ReadSingle();

                if (movement.Movement.Flags.HasFlag(MovementFlags.MOVEMENTFLAG_SPLINE_ENABLED))
                {
                    movement.Spline = SplineInfo.Read(gr);
                }
            }
            else
            {
                // Not in classic
                //if (movement.UpdateFlags.HasFlag(UpdateFlags.UPDATEFLAG_GO_POSITION))
                //{
                //    movement.Movement.Transport.Guid = gr.ReadPackedGuid();
                //    movement.Movement.Position = gr.ReadCoords3();
                //    movement.Movement.Transport.Position = gr.ReadCoords3();
                //    movement.Movement.Facing = gr.ReadSingle();
                //    movement.Movement.Transport.Facing = gr.ReadSingle();
                //}

                if (movement.UpdateFlags.HasFlag(ObjectUpdateFlags.UPDATEFLAG_HAS_POSITION))
                {
                    movement.Movement.Position = gr.ReadCoords3();
                    movement.Movement.Facing = gr.ReadSingle();
                }
            }

            // Not in classic
            //if (movement.UpdateFlags.HasFlag(ObjectUpdateFlags.UPDATEFLAG_LOWGUID))
            //{
            //    movement.LowGuid = gr.ReadUInt32();
            //}

            if (movement.UpdateFlags.HasFlag(ObjectUpdateFlags.UPDATEFLAG_HIGHGUID))
            {
                movement.HighGuid = gr.ReadUInt32();
            }

            if (movement.UpdateFlags.HasFlag(ObjectUpdateFlags.UPDATEFLAG_ALL))
            {
                gr.ReadUInt32();
            }

            // Not in classic
            if (movement.UpdateFlags.HasFlag(ObjectUpdateFlags.UPDATEFLAG_FULLGUID))
            {
                movement.AttackingTarget = gr.ReadPackedGuid();
            }

            if (movement.UpdateFlags.HasFlag(ObjectUpdateFlags.UPDATEFLAG_TRANSPORT))
            {
                movement.TransportTime = gr.ReadUInt32();
            }

            // WotLK
            //if (movement.UpdateFlags.HasFlag(UpdateFlags.UPDATEFLAG_VEHICLE))
            //{
            //    movement.VehicleId = gr.ReadUInt32();
            //    movement.VehicleAimAdjustement = gr.ReadSingle();
            //}

            // 3.1
            //if (movement.UpdateFlags.HasFlag(UpdateFlags.UPDATEFLAG_GO_ROTATION))
            //{
            //    movement.GoRotationULong = gr.ReadUInt64();
            //}
            return movement;
        }
    }
}
