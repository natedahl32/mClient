using mClient.Constants;
using mClient.DBC;
using mClient.Network;
using mClient.Shared;
using mClient.World.AI.Activity.Messages;
using mClient.World.Spells;

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
            // Get available spells we do not have yet
            player.UpdateAvailableSpells();
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

            // Call player level up method to tell them they leveled
            player.LevelUp(level);
        }

        /// <summary>
        /// Handles a spell list from a trainer
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_TRAINER_LIST)]
        public void HandleTrainerList(PacketIn packet)
        {
            var message = new TrainerSpellListMessage();

            var trainerGuid = packet.ReadUInt64();
            var trainerType = packet.ReadUInt32();
            message.TrainerGuid = new WoWGuid(trainerGuid);

            var spellCount = packet.ReadUInt32();
            for (int i = 0; i < spellCount; i++)
            {
                var spellData = new TrainerSpellData();
                spellData.SpellId = packet.ReadUInt32();
                spellData.State = packet.ReadByte();
                spellData.Cost = packet.ReadUInt32();
                packet.ReadUInt32();  // something to do with primary profession first rank
                packet.ReadUInt32();  // another primary profession field
                spellData.RequiredLevel = packet.ReadByte();
                spellData.RequiredSkill = packet.ReadUInt32();
                spellData.RequiredSkillValue = packet.ReadUInt32();
                spellData.SpellChainNode = packet.ReadUInt32();
                spellData.SpellChainNode2 = packet.ReadUInt32();
                packet.ReadUInt32();

                if (spellData.State == 0)
                    message.CanLearnSpells.Add(spellData);
                else if (spellData.State == 1)
                    message.CanNotLearnSpells.Add(spellData);
                // I believe GRAY status means already purchased. We will ignore those spells.
            }

            var title = packet.ReadString();
            player.PlayerAI.SendMessageToAllActivities(message);
        }

        /// <summary>
        /// Handles successfully buying a spell from a trainer
        /// </summary>
        /// <param name="packet"></param>
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_TRAINER_BUY_SUCCEEDED)]
        public void HandleTrainerBuySucceeded(PacketIn packet)
        {
            var trainerGuid = packet.ReadUInt64();
            var spellId = packet.ReadUInt32();

            var message = new TrainerBuySucceededMessage()
            {
                TrainerGuid = new WoWGuid(trainerGuid),
                SpellId = spellId
            };
            player.PlayerAI.SendMessageToAllActivities(message);
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

        /// <summary>
        /// Requests a list of spells from a trainer
        /// </summary>
        /// <param name="guid"></param>
        public void RequestTrainerList(ulong guid)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_TRAINER_LIST);
            packet.Write(guid);
            Send(packet);
        }

        /// <summary>
        /// Purchases a spell from a trainer
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="spellId"></param>
        public void BuySpellFromTrainer(ulong guid, uint spellId)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_TRAINER_BUY_SPELL);
            packet.Write(guid);
            packet.Write(spellId);
            Send(packet);
        }

        #endregion
    }
}
