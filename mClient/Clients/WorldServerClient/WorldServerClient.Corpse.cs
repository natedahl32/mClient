using mClient.Constants;
using mClient.Network;
using mClient.Shared;
using mClient.World.AI.Activity.Messages;

namespace mClient.Clients
{
    public partial class WorldServerClient
    {
        #region Handler Methods

        /// <summary>
        /// Handles corpse query
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.MSG_CORPSE_QUERY)]
        public void HandleCorpseQuery(PacketIn packet)
        {
            var isFound = packet.ReadByte();
            if (isFound == 0)
                return;

            // corpse was found
            var mapId = packet.ReadUInt32();
            var corpseLocation = packet.ReadCoords3();

            // send an activity message with the information
            var message = new CorpseQueryMessage()
            {
                MapId = mapId,
                Location = new Coordinate(corpseLocation.X, corpseLocation.Y, corpseLocation.Z)
            };
            player.PlayerAI.SendMessageToAllActivities(message);

            // teleport to our corpse
            var newPosition = new Coordinate(corpseLocation.X, corpseLocation.Y, corpseLocation.Z);
            Teleport(mapId, newPosition);
            player.PlayerObject.Position = newPosition;
        }

        /// <summary>
        /// Handles a resurrect request
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_RESURRECT_REQUEST)]
        public void HandleResurrectRequest(PacketIn packet)
        {
            var resserGuid = packet.ReadUInt64();
            // If the resser is in our party then accept it
            if (player.CurrentGroup.IsInGroup(resserGuid))
                AcceptResurrectRequest();
        }

        #endregion

        #region Send Methods

        /// <summary>
        /// Sends request to reclaim our corpse
        /// </summary>
        public void ReclaimCorpse()
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_RECLAIM_CORPSE);
            packet.Write(player.Guid.GetOldGuid());
            Send(packet);
        }

        /// <summary>
        /// Accepts a resurrect request
        /// </summary>
        public void AcceptResurrectRequest()
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_RESURRECT_RESPONSE);
            packet.Write(player.Guid.GetOldGuid());
            packet.Write(1);
            Send(packet);
        }

        /// <summary>
        /// Sends query for our corpse
        /// </summary>
        public void SendCorpseQuery()
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.MSG_CORPSE_QUERY);
            Send(packet);
        }

        /// <summary>
        /// Sends repop request to server after a death
        /// </summary>
        public void Repop()
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_REPOP_REQUEST);
            Send(packet);
        }

        #endregion
    }
}
