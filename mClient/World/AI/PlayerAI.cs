using mClient.Clients;
using PObject = mClient.Clients.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using mClient.Shared;
using mClient.Terrain;
using mClient.Constants;
using System.Runtime.InteropServices;

namespace mClient.World.AI
{
    public partial class PlayerAI
    {
        [DllImport("winmm.dll", EntryPoint = "timeGetTime")]
        public static extern uint MM_GetTime();

        #region Declarations

        private Player mPlayer;

        // Movement variables
        private PObject mFollowTarget = null;

        // Combat variables
        private WoWGuid mTargetSelection = null;
        private bool mIsAttackingTarget = false;

        #endregion

        #region Constructors

        public PlayerAI(Player player)
        {
            if (player == null) throw new ArgumentNullException("player");
            mPlayer = player;

            // Defaults
            NotInMeleeRange = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the player this AI belongs to
        /// </summary>
        public Player Player
        {
            get { return mPlayer; }
        }

        /// <summary>
        /// Gets or sets whether or not the player is in melee range
        /// </summary>
        public bool NotInMeleeRange { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handle player logic
        /// </summary>
        /// <param name="client"></param>
        public void HandlePlayerLogic(WorldServerClient client)
        {
            // If I'm in combat, handle the situation first before I do anything else
            if (Player.IsInCombat)
            {
                // TOOD: Need a combat module

                // If we are not in melee range, set the target as the follow target
                // TODO: Use our target, NOT first enemy in the list
                if (NotInMeleeRange)
                {
                    mFollowTarget = client.objectMgr.getObject(Player.EnemyList.FirstOrDefault());
                    if (mFollowTarget != null)
                    {
                        FollowTarget(client);
                        return;
                    }
                }
                else
                {
                    client.Attack(Player.EnemyList.FirstOrDefault().GetOldGuid());
                    return;
                }
            }

            // If I am in a group set my follow target to be the group leader
            //if (Player.CurrentGroup != null && Player.CurrentGroup.Leader != null)
            //    mFollowTarget = Player.CurrentGroup.Leader.PlayerObject;


            FollowTarget(client);
        }

        /// <summary>
        /// Clears the follow target for the AI
        /// </summary>
        public void ClearFollowTarget()
        {
            mFollowTarget = null;
        }

        /// <summary>
        /// Follow a particular target
        /// </summary>
        /// <param name="guid"></param>
        public void FollowTarget(WoWGuid guid)
        {
            // TOOD: ObjectManager needs to be static so we can pull an object from here
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Makes the player follow their follow target
        /// </summary>
        private void FollowTarget(WorldServerClient client)
        {
            // If we have a follow target and we don't currently have a waypoint. Set a waypoint for the current targets position.
            if (mFollowTarget != null)
            {
                client.movementMgr.Waypoints.Clear();
                if (mFollowTarget.Position != null)
                {
                    // Only add the waypoint if we are within distance of the person we are following
                    var distance = client.movementMgr.CalculateDistance(mFollowTarget.Position);
                    if (distance > 1 && distance < 100)
                        client.movementMgr.Waypoints.Add(mFollowTarget.Position);
                    else
                    {
                        var angle = TerrainMgr.CalculateAngle(client.objectMgr.getPlayerObject().Position, mFollowTarget.Position);
                        if (client.objectMgr.getPlayerObject().Position.O != angle)
                        {
                            client.objectMgr.getPlayerObject().Position.O = angle;
                            UInt32 timeNow = MM_GetTime();
                            client.SendMovementPacket(WorldServerOpCode.MSG_MOVE_SET_FACING, timeNow);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
