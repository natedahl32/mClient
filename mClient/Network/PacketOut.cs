﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mClient.Constants;
using System.Net.Sockets;
using System.IO;

namespace mClient.Network
{
    public class PacketOut : BinaryWriter
    {
        public readonly PacketId packetId;
        public readonly ServiceType Service;


        public PacketOut(WorldServerOpCode opcode) 
            : base (new MemoryStream())
        {
            packetId = new PacketId(opcode);
            Service = ServiceType.World;
            this.Write((uint)opcode);
        }

        public PacketOut(LogonServerOpCode opcode)
            : base(new MemoryStream())
        {
            packetId = new PacketId(opcode);
            Service = ServiceType.Logon;
            this.Write((byte)opcode);
        }

		public override void Write(string Text)
		{
			if (Text != null) Write(Encoding.Default.GetBytes(Text));
			Write((byte)0); // String terminator
		}

        public int WritePackedUInt64(ulong number)
        {
            var buffer = BitConverter.GetBytes(number);

            byte mask = 0;
            var startPos = this.BaseStream.Position;

            this.Write(mask);

            for (var i = 0; i < 8; i++)
            {
                if (buffer[i] != 0)
                {
                    mask |= (byte)(1 << i);
                    this.Write(buffer[i]);
                }
            }

            var endPos = this.BaseStream.Position;

            this.BaseStream.Position = startPos;
            this.Write(mask);
            this.BaseStream.Position = endPos;

            return (int)(endPos - startPos);
        }

        public byte[] ToArray()
		{
			return ((MemoryStream)BaseStream).ToArray();
		}

        public long Lenght()
        {
            return base.BaseStream.Length;
        }

		public static implicit operator byte[] (PacketOut packet)
		{
			return packet.ToArray();
		}

        public string ToHex()
        {
            StringBuilder hexDump = new StringBuilder();
            byte[] packetData = ToArray();

            hexDump.Append('\n');
            hexDump.Append("{Client->Server} " + string.Format("Packet: ({0}) {1} PacketSize = {2}\n" +
                                                       "|------------------------------------------------|----------------|\n" +
                                                       "|00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F |0123456789ABCDEF|\n" +
                                                       "|------------------------------------------------|----------------|\n",
                                                       "0x" + ((short)packetId.RawId).ToString("X4"),packetId, packetData.Length));

            int end = 0 + packetData.Length;
            for (int i = 0; i < end; i += 16)
            {
                StringBuilder text = new StringBuilder();
                StringBuilder hex = new StringBuilder();
                hex.Append("|");

                for (int j = 0; j < 16; j++)
                {
                    if (j + i < end)
                    {
                        byte val = packetData[j + i];
                        hex.Append(packetData[j + i].ToString("X2"));
                        hex.Append(" ");
                        if (val >= 32 && val <= 127)
                        {
                            text.Append((char)val);
                        }
                        else
                        {
                            text.Append(".");
                        }
                    }
                    else
                    {
                        hex.Append("   ");
                        text.Append(" ");
                    }
                }
                hex.Append("|");
                hex.Append(text + "|");
                hex.Append('\n');
                hexDump.Append(hex.ToString());
            }

            hexDump.Append("-------------------------------------------------------------------");

            return hexDump.ToString();
        }
    }
}
