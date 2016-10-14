using mClient.Network;
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Clients.UpdateBlocks
{
    public class TransportInfo
    {
        public ulong Guid { get; set; }
        public Coords3 Position { get; set; }
        public float Facing { get; set; }
        public uint Time { get; set; }
        public byte Seat { get; set; }
        public uint Time2 { get; set; }

        public static TransportInfo Read(PacketIn reader)
        {
            var tInfo = new TransportInfo();
            tInfo.Guid = reader.ReadUInt64();
            tInfo.Position = reader.ReadCoords3();
            tInfo.Facing = reader.ReadSingle();
            return tInfo;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Transport Guid: 0x{0:X16}", Guid).AppendLine();
            sb.AppendFormat("Transport Position: {0}", Position).AppendLine();
            sb.AppendFormat("Transport Facing: {0}", Facing).AppendLine();
            return sb.ToString();
        }
    }
}
