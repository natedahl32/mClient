using mClient.Constants;
using mClient.DBC;
using mClient.Network;
using mClient.Shared;

namespace mClient.Clients
{
    partial class WorldServerClient
    {
        #region Handlers

        /// <summary>
        /// Handles logging out a player
        /// </summary>
        /// <param name="inpacket"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_LOGOUT_COMPLETE)]
        public void HandleLogoutComplete(PacketIn inpacket)
        {
            var itemClass = (ItemClass)inpacket.ReadByte();
            var subClass = inpacket.ReadUInt32();

            player.AddProficiency(itemClass, subClass);
        }

        /// <summary>
        /// Add item proficiency to player
        /// </summary>
        /// <param name="inpacket"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_SET_PROFICIENCY)]
        public void HandleSetProficiency(PacketIn inpacket)
        {
            var itemClass = (ItemClass)inpacket.ReadByte();
            var subClass = inpacket.ReadUInt32();

            player.AddProficiency(itemClass, subClass);
        }

        /// <summary>
        /// Updates the players auras
        /// </summary>
        /// <param name="inpacket"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_UPDATE_AURA_DURATION)]
        public void HandleUpdateAuraDuration(PacketIn inpacket)
        {
            var auraSlot = inpacket.ReadByte();
            var auraDuration = inpacket.ReadUInt32();

            // TODO: Update the players auras
        }

        [PacketHandlerAtribute(WorldServerOpCode.SMSG_INITIAL_SPELLS)]
        public void HandleInitialSpells(PacketIn inpacket)
        {
            var dummy = inpacket.ReadByte();
            var spellCount = inpacket.ReadUInt16();

            for (int i = 0; i < spellCount; i++)
            {
                var spell = inpacket.ReadUInt16();
                var dummy2 = inpacket.ReadUInt16();
                // Add spell to the player
                player.AddSpell(spell);
            }

            var spellCooldownCount = inpacket.ReadUInt16();

            for (int i = 0; i < spellCooldownCount; i++)
            {
                var spellCooldown = inpacket.ReadUInt16();
                var castItemId = inpacket.ReadUInt16();
                var spellCategory = inpacket.ReadUInt16();

                var cooldown = inpacket.ReadUInt32();
                var cooldownCategory = inpacket.ReadUInt32();
            }

            // Initialize bot spells now that we've got our initial spells
            if (player.ClassLogic != null)
                player.ClassLogic.InitializeSpells();
        }

        /// <summary>
        /// Handles the player leveling up
        /// </summary>
        /// <param name="inpacket"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_LEVELUP_INFO)]
        public void HandleLevelUp(PacketIn packet)
        {
            var level = packet.ReadUInt32();
            var hpIncrease = packet.ReadUInt32();
            var manaIncrease = packet.ReadUInt32();
            packet.ReadUInt32();
            packet.ReadUInt32();
            packet.ReadUInt32();
            packet.ReadUInt32();

            // Stat increases
            for (int i = 0; i < 5; i++)
            {
                var stat = packet.ReadUInt32();
            }

            // TODO: Need to find whether or not we have spells/skills that need to be updated
            // via a trainer. If there are we need to talk to a trainer to learn them.

            // TODO: Need to handle unspent talent points. They should be spent based on the spec assigned to the bot
        }

        #endregion

        #region Actions

        /// <summary>
        /// Sends a request to the server to logout the player
        /// </summary>
        public void LogoutPlayer()
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_LOGOUT_REQUEST);
            Send(packet);
        }

        #endregion
    }
}
