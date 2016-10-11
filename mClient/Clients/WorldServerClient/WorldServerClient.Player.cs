using mClient.Constants;
using mClient.DBC;
using mClient.Network;
using mClient.Shared;

namespace mClient.Clients
{
    partial class WorldServerClient
    {
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

            SpellTable spells = new SpellTable();

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
        }
    }
}
