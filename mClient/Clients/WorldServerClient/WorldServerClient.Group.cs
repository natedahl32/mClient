﻿using mClient.Constants;
using mClient.Network;
using mClient.Shared;
using mClient.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Clients
{
    partial class WorldServerClient
    {
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_PARTY_MEMBER_STATS_FULL)]
        public void HandlePartyMemberStatsFull(PacketIn packet)
        {
            // Get guid
            WoWGuid guid = packet.ReadPackedGuidToWoWGuid();

            // Check if player is online or offline
            var playerMask = packet.ReadUInt32();
            var isOnline = (packet.ReadByte() == 1 ? true : false);
            if (!isOnline) return;

            var currentHP = packet.ReadUInt16();
            var maxHP = packet.ReadUInt16();
            var powerType = packet.ReadByte();
            var currentPower = packet.ReadUInt16();
            var maxPower = packet.ReadUInt16();
            var level = packet.ReadUInt16();

            var zoneId = packet.ReadUInt16();
            var x = packet.ReadUInt16();
            var y = packet.ReadUInt16();

            // Find the party member and update their stats
            var member = player.CurrentGroup.GetPlayer(guid);
            if (member != null && member.PlayerObject != null)
                member.PlayerObject.Update(currentHP, maxHP, level, currentPower, maxPower);
        }

        [PacketHandlerAtribute(WorldServerOpCode.SMSG_GROUP_INVITE)]
        public void HandleGroupInvite(PacketIn inpacket)
        {
            // Retrieve the name of the player that offered the invite
            var playerName = inpacket.ReadString();

            // Automatically accept group invite
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_GROUP_ACCEPT);
            Send(packet);

            // Add the player to a new group
            player.AddToGroup();
        }

        [PacketHandlerAtribute(WorldServerOpCode.SMSG_GROUP_UNINVITE)]
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_GROUP_DESTROYED)]
        public void HandleGroupUninvite(PacketIn inpacket)
        {
            // Remove the player from any group they are in
            player.RemoveFromGroup();
        }

        [PacketHandlerAtribute(WorldServerOpCode.SMSG_GROUP_SET_LEADER)]
        public void HandleGroupSetLeader(PacketIn inpacket)
        {
            var leaderName = inpacket.ReadString();

            // Set the leader of the group
            if (player.CurrentGroup != null)
                player.CurrentGroup.SetLeader(leaderName);
        }

        [PacketHandlerAtribute(WorldServerOpCode.SMSG_GROUP_LIST)]
        public void HandleGroupList(PacketIn inpacket)
        {
            // Update group information
            var groupType = inpacket.ReadByte();
            var groupFlags = inpacket.ReadByte();
            var memberCount = inpacket.ReadUInt32();

            // If I have group members but I'm not in a group yet, add myself to a group
            if (memberCount > 0 && player.CurrentGroup == null)
                player.AddToGroup();

            // get group member information
            for (var i = 0; i < memberCount; i++)
            {
                var name = inpacket.ReadString();
                var guid = inpacket.ReadUInt64();
                var onlineState = inpacket.ReadByte(); // 1 = online, 0 = offline
                var flags = inpacket.ReadByte(); // group flags for this member

                // Create new group member data
                var data = new GroupMemberData() { OnlineState = onlineState, GroupFlags = flags };

                // Create a new query for the player
                var query = new QueryQueue(QueryQueueType.Name, guid) { ExtraData = data };
                query.AddCallback((o) => HandleNewGroupMemberQuery(o));
                var obj = GetOrQueueObject(query) as PlayerObj;
                if (obj != null)
                    AddObjectToGroup(obj, data);
            }

            // If there is no one in the group, remove the player from it
            if (memberCount == 0)
            {
                player.RemoveFromGroup();
                return;
            }

            // If we don't have a group yet, create one and add ourselves to it. This can occur
            // when we log in to an already formed group (ie. you logged out in a group)
            if (player.CurrentGroup == null)
                player.AddToGroup();                

            // Get the group leader guid and set the leader of the group
            var leaderGuid = inpacket.ReadUInt64();
            if (leaderGuid > 0)
                player.CurrentGroup.SetLeader(leaderGuid);

            if (memberCount > 0)
            {
                var lootMethod = inpacket.ReadByte();
                var masterLooterGuid = inpacket.ReadUInt64();
                var lootThreshold = inpacket.ReadByte();
                player.CurrentGroup.UpdateGroupData(masterLooterGuid, (LootMethod)lootMethod, (ItemQualities)lootThreshold);
            }
        }

        /// <summary>
        /// Requests stats for all party members in the group
        /// </summary>
        private void RequestEntirePartyStats()
        {
            foreach (var p in player.CurrentGroup.PlayersInGroup)
            {
                if (p == null) continue;
                RequestPartyStats(p.Guid.GetOldGuid());
            }
        }

        /// <summary>
        /// Request party stats for a member of the party
        /// </summary>
        /// <param name="guid"></param>
        private void RequestPartyStats(UInt64 guid)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_REQUEST_PARTY_MEMBER_STATS);
            packet.Write(guid);
            Send(packet);
        }

        /// <summary>
        /// Adds the player to the group once the query has came back
        /// </summary>
        /// <param name="obj"></param>
        private void HandleNewGroupMemberQuery(Object obj)
        {
            // Get the query for this object
            var query = mQueryQueue.Where(q => q.Guid == obj.Guid.GetOldGuid() && q.QueryType == QueryQueueType.Name).SingleOrDefault();

            // Get the group member data stored in the query
            var data = query.ExtraData as GroupMemberData;

            AddObjectToGroup((PlayerObj)obj, data);
        }

        /// <summary>
        /// Adds an object to the current group of the player
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="data"></param>
        private void AddObjectToGroup(PlayerObj obj, GroupMemberData data)
        {
            var newMember = new Player(obj) { GroupData = data };
            player.CurrentGroup.AddPlayerToGroup(newMember);
            newMember.SetGroup(player.CurrentGroup);
            RequestPartyStats(obj.Guid.GetOldGuid());
        }
    }
}