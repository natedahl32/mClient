using mClient.Constants;
using mClient.Network;
using mClient.Shared;

namespace mClient.Clients
{
    partial class WorldServerClient
    {
        #region Packet Handlers

        /// <summary>
        /// Handles a guild petition show message where we can sign a petition
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_PETITION_SHOW_SIGNATURES)]
        public void HandleGuildPetitionShow(PacketIn packet)
        {
            // Get petition guid
            var petitionGuid = packet.ReadUInt64();
            var petitionOwnerGuid = packet.ReadUInt64();

            // If we are not in a guild already, sign the petition
            if (player.Guild == null)
                SignGuildPetition(petitionGuid);
        }

        #endregion

        #region Actions

        /// <summary>
        /// Signs a guild petition
        /// </summary>
        /// <param name="petitionGuid"></param>
        public void SignGuildPetition(ulong petitionGuid)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_PETITION_SIGN);
            packet.Write(petitionGuid);
            packet.Write((byte)0);
            Send(packet);
        }

        #endregion
    }
}
