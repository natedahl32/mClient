using mClient.Constants;
using mClient.Network;
using mClient.Shared;
using System;
using System.Text;

namespace mClient.Clients.UpdateBlocks
{
    public class MovementInfo
    {
        public MovementFlags Flags { get; private set; }
        public uint TimeStamp { get; private set; }
        public Coords3 Position { get; set; }
        public float Facing { get; set; }
        public TransportInfo Transport { get; private set; }
        public float Pitch { get; private set; }
        public uint FallTime { get; private set; }
        public float FallVelocity { get; private set; }
        public float FallCosAngle { get; private set; }
        public float FallSinAngle { get; private set; }
        public float FallSpeed { get; private set; }
        public float SplineElevation { get; private set; }

        public MovementInfo()
        {
            Transport = new TransportInfo();
        }

        public static MovementInfo Read(PacketIn gr)
        {
            var movementInfo = new MovementInfo();

            movementInfo.Flags = (MovementFlags)gr.ReadUInt32();
            movementInfo.TimeStamp = gr.ReadUInt32();

            movementInfo.Position = gr.ReadCoords3();
            movementInfo.Facing = gr.ReadSingle();

            if (movementInfo.Flags.HasFlag(MovementFlags.MOVEMENTFLAG_ONTRANSPORT))
            {
                movementInfo.Transport = TransportInfo.Read(gr);
            }

            if (movementInfo.Flags.HasFlag(MovementFlags.MOVEMENTFLAG_SWIMMING))
            {
                movementInfo.Pitch = gr.ReadSingle();
            }

            movementInfo.FallTime = gr.ReadUInt32();

            if (movementInfo.Flags.HasFlag(MovementFlags.MOVEMENTFLAG_FALLING))
            {
                movementInfo.FallVelocity = gr.ReadSingle();
                movementInfo.FallCosAngle = gr.ReadSingle();
                movementInfo.FallSinAngle = gr.ReadSingle();
                movementInfo.FallSpeed = gr.ReadSingle();
            }

            if (movementInfo.Flags.HasFlag(MovementFlags.MOVEMENTFLAG_SPLINE_ELEVATION))
            {
                movementInfo.SplineElevation = gr.ReadSingle();
            }

            return movementInfo;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Movement Flags: {0}", Flags).AppendLine();
            sb.AppendFormat("Movement TimeStamp: {0}", TimeStamp).AppendLine();
            sb.AppendFormat("Movement Position: {0}", Position).AppendLine();
            sb.AppendFormat("Movement Facing: {0}", Facing).AppendLine();

            if (Flags.HasFlag(MovementFlags.MOVEMENTFLAG_ONTRANSPORT))
                sb.AppendFormat("Movement Transport: {0}", Transport).AppendLine();

            if (Flags.HasFlag(MovementFlags.MOVEMENTFLAG_SWIMMING))
                sb.AppendFormat("Movement Pitch: {0}", Pitch).AppendLine();

            sb.AppendFormat("Movement FallTime: {0}", FallTime).AppendLine();

            if (Flags.HasFlag(MovementFlags.MOVEMENTFLAG_FALLING))
            {
                sb.AppendFormat("Movement FallVelocity: {0}", FallVelocity).AppendLine();
                sb.AppendFormat("Movement FallCosAngle: {0}", FallCosAngle).AppendLine();
                sb.AppendFormat("Movement FallSinAngle: {0}", FallSinAngle).AppendLine();
                sb.AppendFormat("Movement FallSpeed: {0}", FallSpeed).AppendLine();
            }

            if (Flags.HasFlag(MovementFlags.MOVEMENTFLAG_SPLINE_ELEVATION))
                sb.AppendFormat("Movement SplineElevation: {0}", SplineElevation).AppendLine();

            return sb.ToString();
        }
    }
}
