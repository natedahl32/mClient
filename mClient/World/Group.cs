using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.World
{
    /// <summary>
    /// Holds group data for a particular member of a group
    /// </summary>
    public class GroupMemberData
    {
        /// <summary>
        /// Gets or sets the Online state of the group member
        /// </summary>
        public byte OnlineState { get; set; }

        /// <summary>
        /// Gets or sets the group flags of the group member
        /// </summary>
        public byte GroupFlags { get; set; }
    }

    /// <summary>
    /// Represents a group of players
    /// </summary>
    public class Group
    {
        #region Declarations

        private List<Player> mPlayersInGroup = new List<Player>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets all players in the group
        /// </summary>
        public IEnumerable<Player> PlayersInGroup { get { return mPlayersInGroup; } }

        /// <summary>
        /// Gets the Group Type
        /// </summary>
        public byte GroupType { get; private set; }

        /// <summary>
        /// Gets or sets the guid of the leader of the group
        /// </summary>
        private UInt64 LeaderGuid { get; set; }

        /// <summary>
        /// Gets or sets the guid of the master lotter of the group
        /// </summary>
        private UInt64 MasterLooterGuid { get; set; }

        /// <summary>
        /// Gets the loot method of the group
        /// </summary>
        public byte LootMethod { get; private set; }

        /// <summary>
        /// Gets the loot threshold of the group
        /// </summary>
        public byte LootThreshold { get; private set; }

        /// <summary>
        /// Gets the leader of the group
        /// </summary>
        public Player Leader
        {
            get
            {
                return mPlayersInGroup.Where(p => p.Guid.GetOldGuid() == LeaderGuid).SingleOrDefault();
            }
        }

        /// <summary>
        /// Gets the master looter of the group
        /// </summary>
        public Player MasterLooter
        {
            get
            {
                return mPlayersInGroup.Where(p => p.Guid.GetOldGuid() == MasterLooterGuid).SingleOrDefault();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a player to the group
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayerToGroup(Player player)
        {
            // Make sure the group is not full already
            if (mPlayersInGroup.Count >= ConstantValues.MAXIMUM_PLAYERS_IN_GROUP)
                return;

            // Make sure the player is not already in the group before adding them
            if (!mPlayersInGroup.Any(p => p.Guid == player.Guid))
                mPlayersInGroup.Add(player);
        }

        /// <summary>
        /// Sets the leader of the group
        /// </summary>
        /// <param name="leaderGuid"></param>
        public void SetLeader(UInt64 leaderGuid)
        {
            this.LeaderGuid = leaderGuid;
        }

        /// <summary>
        /// Sets the leader of the group by their name
        /// </summary>
        /// <param name="name"></param>
        public void SetLeader(string name)
        {
            var leaderByName = mPlayersInGroup.Where(p => p.Name == name).SingleOrDefault();
            if (leaderByName != null)
                LeaderGuid = leaderByName.Guid.GetOldGuid();
        }

        /// <summary>
        /// Updates group data
        /// </summary>
        /// <param name="masterLooterGuid"></param>
        /// <param name="lootMethod"></param>
        /// <param name="lootThreshold"></param>
        public void UpdateGroupData(UInt64 masterLooterGuid, byte lootMethod, byte lootThreshold)
        {
            this.MasterLooterGuid = masterLooterGuid;
            this.LootMethod = lootMethod;
            this.LootThreshold = lootThreshold;
        }

        /// <summary>
        /// Checks if a player guid is in the group
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool IsInGroup(UInt64 guid)
        {
            return mPlayersInGroup.Any(p => p.Guid.GetOldGuid() == guid);
        }

        /// <summary>
        /// Gets a player in the group by guid. Returns nothing if the player does not exist in the group
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public Player GetGroupMember(UInt64 guid)
        {
            return mPlayersInGroup.Where(p => p.Guid.GetOldGuid() == guid).SingleOrDefault();
        }

        #endregion
    }
}
