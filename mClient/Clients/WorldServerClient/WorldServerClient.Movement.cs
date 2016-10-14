using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

using System.Runtime.InteropServices;
using System.Resources;

using mClient.Shared;
using mClient.Network;
using mClient.Constants;

namespace mClient.Clients
{
    public partial class WorldServerClient
    {

        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_START_FORWARD)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_START_BACKWARD)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_STOP)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_START_STRAFE_LEFT)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_START_STRAFE_RIGHT)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_STOP_STRAFE)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_JUMP)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_START_TURN_LEFT)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_START_TURN_RIGHT)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_STOP_TURN)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_START_PITCH_UP)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_START_PITCH_DOWN)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_STOP_PITCH)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_SET_RUN_MODE)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_SET_WALK_MODE)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_TOGGLE_LOGGING)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_TELEPORT)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_TELEPORT_CHEAT)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_TELEPORT_ACK)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_TOGGLE_FALL_LOGGING)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_FALL_LAND)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_START_SWIM)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_STOP_SWIM)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_HEARTBEAT)]
        [PacketHandlerAtribute(WorldServerOpCode.MSG_MOVE_SET_FACING)]
        public void HandleAnyMove(PacketIn packet)
        {
            byte mask = packet.ReadByte();

            WoWGuid guid = new WoWGuid(mask, packet.ReadBytes(WoWGuid.BitCount8(mask)));

            Object obj = objectMgr.getObject(guid);
            if (obj != null)
            {
                packet.ReadUInt32();    // MoveFlags
                packet.ReadUInt32();    // Time
                var x = packet.ReadFloat();
                var y = packet.ReadFloat();
                var z = packet.ReadFloat();
                var o = packet.ReadFloat();
                obj.Position= new Coordinate(x, y, z, o);
                //if (guid.GetOldGuid() == player.PlayerObject.Guid.GetOldGuid())
                //    Log.WriteLine(LogType.Debug, "Received position: {0} {1} {2} {3}");
            }
        }

        [PacketHandlerAtribute(WorldServerOpCode.SMSG_MONSTER_MOVE)]
        public void HandleMonsterMove(PacketIn packet)
        {
            byte mask = packet.ReadByte();

            WoWGuid guid = new WoWGuid(mask, packet.ReadBytes(WoWGuid.BitCount8(mask)));

            Object obj = objectMgr.getObject(guid);
            if (obj != null)
            {
                obj.Position = new Coordinate(packet.ReadFloat(), packet.ReadFloat(), packet.ReadFloat());
            }
            else
            {
                obj = Object.CreateObjectByType(guid, ObjectType.Unit);
                obj.Position = new Coordinate(packet.ReadFloat(), packet.ReadFloat(), packet.ReadFloat());
                objectMgr.addObject(obj);
            }
        }

        void Heartbeat(object source, ElapsedEventArgs e)
        {
            if (objectMgr.getPlayerObject().Position == null)
                return;

            SendMovementPacket(WorldServerOpCode.MSG_MOVE_HEARTBEAT);
        }

        public void SendMovementPacket(WorldServerOpCode movementOpCode, UInt32 time = 0)
        {
            PacketOut packet = new PacketOut(movementOpCode);
            packet.Write(movementMgr.Flag.MoveFlags);
            //packet.Write((byte)0);
            if (time == 0)
                packet.Write((UInt32)MM_GetTime());
            else
                packet.Write(time);
            var obj = objectMgr.getPlayerObject();
            var x = (float)obj.Position.X;
            var y = (float)obj.Position.Y;
            var z = (float)obj.Position.Z;
            var o = (float)obj.Position.O;
            //Log.WriteLine(LogType.Debug, "{4} Position: {0} {1} {2} {3}", x, y, z, o, movementOpCode);
            packet.Write(x);
            packet.Write(y);
            packet.Write(z);
            packet.Write(o);
            packet.Write((UInt32)0);
            Send(packet);
        }
    }
}

