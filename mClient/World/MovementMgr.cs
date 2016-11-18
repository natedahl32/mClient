using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

using System.Runtime.InteropServices;
using System.Resources;
using mClient.Network;
using mClient.Shared;
using mClient.Constants;
using mClient.Terrain;
using PObject = mClient.Clients.Object;

namespace mClient.Clients
{
    public class MovementMgr
    {
        #region Declarations

        [DllImport("winmm.dll", EntryPoint = "timeGetTime")]
        public static extern uint MM_GetTime();

        public const float MINIMUM_FOLLOW_DISTANCE = 2.0f;
        public const float MAXIMUM_FOLLOW_DISTANCE = 100.0f;

        private System.Timers.Timer aTimer = new System.Timers.Timer();
        Thread loop = null;
        public MovementFlag Flag = new MovementFlag();

        private System.Object mWaypointsLock = new System.Object();
        private List<Coordinate> mWaypoints = new List<Coordinate>();

        // Movement variables
        private PObject mFollowTarget = null;
        private System.Object mFollowTargetLock = new System.Object();

        Coordinate oldLocation;
        UInt32 lastUpdateTime;
        ObjectMgr objectMgr;
        TerrainMgr terrainMgr;
        WorldServerClient mClient;

        #endregion

        #region Constructors

        public MovementMgr(WorldServerClient Client)
        {
            mClient = Client;
            objectMgr = Client.objectMgr;
            terrainMgr = Client.terrainMgr;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the follow target currently set
        /// </summary>
        public PObject FollowTarget
        {
            get { return mFollowTarget; }
            set
            {
                lock(mFollowTargetLock)
                    mFollowTarget = value;
            }
        }

        /// <summary>
        /// Gets whether or not there the unit is moving
        /// </summary>
        public bool IsMoving
        {
            get { return Flag.IsMoveFlagSet(MovementFlags.MOVEMENTFLAG_FORWARD); }
        }

        #endregion

        public void Start()
        {
            try
            {
                Flag.SetMoveFlag(MovementFlags.MOVEMENTFLAG_NONE);
                lastUpdateTime = MM_GetTime();

                loop = new Thread(Loop);
                loop.IsBackground = true;
                loop.Start();
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogType.Error, "Exception Occured");
                Log.WriteLine(LogType.Error, "Message: {0}", ex.Message);
                Log.WriteLine(LogType.Error, "Stacktrace: {0}", ex.StackTrace);
            }
        }

        public void Stop()
        {   
            if (loop != null)
                loop.Abort();
        }

        void Loop()
        {
            while (true)
            {
                try
                {
                    // If we have a follow target, do our best to follow them
                    lock (mFollowTargetLock)
                        if (mFollowTarget != null)
                        {
                            HandleFollowTarget();
                            continue;
                        }

                    // Otherwise follow any waypoints set for us
                    Coordinate Waypoint;
                    float angle, dist;

                    UInt32 timeNow = MM_GetTime();
                    UInt32 diff = (timeNow - lastUpdateTime);
                    lastUpdateTime = timeNow;
                    if (mWaypoints.Count != 0)
                    {
                        Waypoint = mWaypoints.FirstOrDefault();

                        if (Waypoint != null)
                        {
                            angle = TerrainMgr.CalculateAngle(objectMgr.getPlayerObject().Position, Waypoint);
                            dist = TerrainMgr.CalculateDistance(objectMgr.getPlayerObject().Position, Waypoint);
                            if (angle == objectMgr.getPlayerObject().Position.O)
                            {
                                if (dist > MINIMUM_FOLLOW_DISTANCE)
                                {
                                    bool isMoving = Flag.IsMoveFlagSet(MovementFlags.MOVEMENTFLAG_FORWARD);
                                    Flag.SetMoveFlag(MovementFlags.MOVEMENTFLAG_FORWARD);
                                    UpdatePosition(diff);
                                    lastUpdateTime = timeNow;
                                    if (!isMoving)
                                        mClient.SendMovementPacket(WorldServerOpCode.MSG_MOVE_START_FORWARD, timeNow);
                                }
                                else
                                {
                                    Flag.SetMoveFlag(MovementFlags.MOVEMENTFLAG_NONE);
                                    RemoveWaypoint(Waypoint);
                                    UpdatePosition(diff);
                                    if (mWaypoints.Count == 0)
                                        mClient.SendMovementPacket(WorldServerOpCode.MSG_MOVE_STOP, timeNow);
                                }
                            }
                            else
                            {
                                Flag.SetMoveFlag(MovementFlags.MOVEMENTFLAG_NONE);
                                objectMgr.getPlayerObject().Position.O = angle;
                            }
                        }
                        else
                        {
                            RemoveWaypoint(Waypoint);
                        }
                    }
                    else
                    {
                        Flag.Clear();
                        if (!Flag.IsMoveFlagSet(MovementFlags.MOVEMENTFLAG_NONE))
                            mClient.SendMovementPacket(WorldServerOpCode.MSG_MOVE_STOP, timeNow);
                        Flag.SetMoveFlag(MovementFlags.MOVEMENTFLAG_NONE);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLine(LogType.Error, "Exception Occured");
                    Log.WriteLine(LogType.Error, "Message: {0}", ex.Message);
                    Log.WriteLine(LogType.Error, "Stacktrace: {0}", ex.StackTrace);
                }
            }
        }


        public void UpdatePosition(UInt32 diff)
        {
            double h; double speed;

            if (objectMgr.getPlayerObject() == null)
                return;

            if (Flag.IsMoveFlagSet(MovementFlags.MOVEMENTFLAG_FORWARD))
            {
                speed = 7.0;
                
            }
            else
                return;

            float predictedDX = 0;
            float predictedDY = 0;

            if (oldLocation == null)
                oldLocation = objectMgr.getPlayerObject().Position;


            h = objectMgr.getPlayerObject().Position.O;

            float dt = (float)diff / 1000f;
            float dx = (float)Math.Cos(h) * (float)speed * dt;
            float dy = (float)Math.Sin(h) * (float)speed * dt;

            predictedDX = dx;
            predictedDY = dy;

            Coordinate loc = objectMgr.getPlayerObject().Position;
            float realDX = loc.X - oldLocation.X;
            float realDY = loc.Y - oldLocation.Y;

            float predictDist = (float)Math.Sqrt(predictedDX * predictedDX + predictedDY * predictedDY);
            float realDist = (float)Math.Sqrt(realDX * realDX + realDY * realDY);

            if (predictDist > 0.0)
            {

                Coordinate expected = new Coordinate(loc.X + predictedDX, loc.Y + predictedDY, objectMgr.getPlayerObject().Position.Z, objectMgr.getPlayerObject().Position.O);
                expected = terrainMgr.getZ(expected);
                objectMgr.getPlayerObject().Position = expected;

            }

            oldLocation = loc;
        }

        public float CalculateDistance(Coordinate c1)
        {
            return TerrainMgr.CalculateDistance(objectMgr.getPlayerObject().Position, c1);
        }

        #region Private Methods

        /// <summary>
        /// Handles following a target movement
        /// </summary>
        private void HandleFollowTarget()
        {
            if (mFollowTarget == null) return;
            if ((mFollowTarget as Unit) != null && (mFollowTarget as Unit).IsDead)
            {
                mFollowTarget = null;
                return;
            }

            // Make sure the follow target has a coordinate
            if (mFollowTarget == null || mFollowTarget.Position == null)
                return;

            var targetPosition = mFollowTarget.Position;
            var angle = TerrainMgr.CalculateAngle(objectMgr.getPlayerObject().Position, targetPosition);
            var dist = TerrainMgr.CalculateDistance(objectMgr.getPlayerObject().Position, targetPosition);

            UInt32 timeNow = MM_GetTime();
            UInt32 diff = (timeNow - lastUpdateTime);
            lastUpdateTime = timeNow;

            // if the angle is not correct, send a set facing packet and face the client in the right direction
            if (angle != objectMgr.getPlayerObject().Position.O)
                objectMgr.getPlayerObject().Position.O = angle;


            // check if we are within distance of the target position or not
            if (dist > MINIMUM_FOLLOW_DISTANCE && dist < MAXIMUM_FOLLOW_DISTANCE)
            {
                bool isMoving = IsMoving;
                Flag.SetMoveFlag(MovementFlags.MOVEMENTFLAG_FORWARD);
                UpdatePosition(diff);
                lastUpdateTime = timeNow;
                if (!isMoving)
                    mClient.SendMovementPacket(WorldServerOpCode.MSG_MOVE_START_FORWARD, timeNow);
            }
            else
            {
                bool isMoving = IsMoving;
                Flag.SetMoveFlag(MovementFlags.MOVEMENTFLAG_NONE);
                UpdatePosition(diff);
                if (isMoving)
                    mClient.SendMovementPacket(WorldServerOpCode.MSG_MOVE_STOP, timeNow);
            }
        }

        /// <summary>
        /// Clears all waypoints
        /// </summary>
        private void ClearWaypoints()
        {
            lock (mWaypointsLock)
                mWaypoints.Clear();
        }


        /// <summary>
        /// Adds a new waypoint
        /// </summary>
        /// <param name="waypoint"></param>
        private void AddWaypoint(Coordinate waypoint)
        {
            if (waypoint != null)
                lock (mWaypoints)
                    mWaypoints.Add(waypoint);
        }

        /// <summary>
        /// Removes an existing waypoint
        /// </summary>
        /// <param name="waypoint"></param>
        private void RemoveWaypoint(Coordinate waypoint)
        {
            if (mWaypoints.Contains(waypoint))
            {
                lock (mWaypointsLock)
                    mWaypoints.Remove(waypoint);
            }
        }

        #endregion
    }


    public class MovementFlag
    {
        public UInt32 MoveFlags;

        public void Clear()
        {
            MoveFlags = new uint();
        }

        public void SetMoveFlag(MovementFlags flag)
        {
            if (flag == 0)
            {
                MoveFlags = (uint)flag;
                return;
            }

            MoveFlags |= (uint)flag;
        }

        public void UnSetMoveFlag(MovementFlags flag)
        {
            MoveFlags &= ~(uint)flag;
        }

        public bool IsMoveFlagSet(MovementFlags flag)
        {
            if (MoveFlags == 0 && (uint)flag == 0) return true;
            return ((MoveFlags & (uint)flag) >= 1) ? true : false;
        }
    }
}
