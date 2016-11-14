using mClient.Clients;
using PObject = mClient.Clients.Object;
using System;
using System.Linq;
using mClient.Shared;
using System.Runtime.InteropServices;
using System.Threading;
using FluentBehaviourTree;
using mClient.World.AI.Activity;
using System.Collections.Generic;

namespace mClient.World.AI
{
    /// <summary>
    /// Player AI consists of the following:
    /// 1) A behavior tree that determines the current activity of the bot (ie. the state)
    /// 2) Activity stack that determines what the bot is doing at that moment in time. This is a FILO stack that processes
    ///    the top activity in the stack until it is complete or another activity is moved on to the stack.
    /// </summary>
    public partial class PlayerAI
    {
        [DllImport("winmm.dll", EntryPoint = "timeGetTime")]
        public static extern uint MM_GetTime();

        #region Declarations

        private Thread mAILoop = null;
        private UInt32 lastUpdateTime;

        private Player mPlayer;
        private WorldServerClient mClient;
        private IBehaviourTreeNode mTree;
        private Stack<BaseActivity> mActivityStack;
        private System.Object mActivityStackLock = new System.Object();

        // Combat variables
        private PObject mTargetSelection = null;
        private bool mIsAttackingTarget = false;

        #endregion

        #region Constructors

        public PlayerAI(Player player, WorldServerClient client)
        {
            if (player == null) throw new ArgumentNullException("player");
            if (client == null) throw new ArgumentNullException("client");
            mPlayer = player;
            mClient = client;
            mActivityStack = new Stack<BaseActivity>();

            // Defaults
            NotInMeleeRange = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the world server client associated with this Player AI
        /// </summary>
        public WorldServerClient Client { get { return mClient; } }

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

        /// <summary>
        /// Gets the target select of the player
        /// </summary>
        public Unit TargetSelection
        {
            get { return mTargetSelection as Unit; }
        }

        /// <summary>
        /// Gets whether or not the there is a current activity
        /// </summary>
        protected bool HasCurrentActivity
        {
            get { return mActivityStack.Count > 0; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the AI thread
        /// </summary>
        public void StartAI()
        {
            try
            {
                this.BuildBehavior();

                lastUpdateTime = MM_GetTime();

                mAILoop = new Thread(Loop);
                mAILoop.IsBackground = true;
                mAILoop.Start();
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogType.Error, "Exception Occured");
                Log.WriteLine(LogType.Error, "Message: {0}", ex.Message);
                Log.WriteLine(LogType.Error, "Stacktrace: {0}", ex.StackTrace);
            }
        }

        /// <summary>
        /// Clears the follow target for the AI
        /// </summary>
        public void ClearFollowTarget()
        {
            Client.movementMgr.FollowTarget = null;
        }

        /// <summary>
        /// Follow a particular target
        /// </summary>
        /// <param name="guid"></param>
        public void SetFollowTarget(PObject obj)
        {
            if (obj != null)
                Client.movementMgr.FollowTarget = obj;
        }

        /// <summary>
        /// Adds a new activity to the stack and starts it immediately
        /// </summary>
        /// <param name="activity">Activity to start</param>
        public void StartActivity(BaseActivity activity)
        {
            if (activity == null) throw new ArgumentNullException("activity");

            // If the stack already contains an activity of this type then don't add it. This prevents our
            // behavior tree from adding the same activity multiple times. Our game logic dictates the same activity
            // should never be in the stack multiple times.
            if (mActivityStack.Any(a => a.GetType() == activity.GetType())) return;

            // Pause the current activity
            if (mActivityStack.Count > 0)
                mActivityStack.Peek().Pause();

            // Start the new activity before adding to the stack otherwise we run the process method before
            // the start method, which isn't correct.
            activity.Start();
            lock (mActivityStackLock)
                mActivityStack.Push(activity);
        }

        /// <summary>
        /// Completes the current activity
        /// </summary>
        public void CompleteActivity()
        {
            lock (mActivityStackLock)
            {
                var currentActivity = mActivityStack.Pop();
                if (currentActivity != null)
                    currentActivity.Complete();
            }
        }

        /// <summary>
        /// Sends messages to all activities
        /// </summary>
        public void SendMessageToAllActivities(ActivityMessage message)
        {
            lock (mActivityStackLock)
                foreach (var activity in mActivityStack)
                    activity.HandleMessage(message);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set the target selection for any combat related AI actions
        /// </summary>
        /// <param name="target"></param>
        private void SetTargetSelection(PObject target)
        {
            // Either coming from a non existing target or going to a non existing target
            if (target == null || mTargetSelection == null)
                mIsAttackingTarget = false;
            // Switching targets
            else if (target.Guid.GetOldGuid() != mTargetSelection.Guid.GetOldGuid())
                mIsAttackingTarget = false;

            mTargetSelection = target;
        }

        /// <summary>
        /// Builds the behavior tree for this player
        /// </summary>
        private void BuildBehavior()
        {
            var builder = new BehaviourTreeBuilder();
            this.mTree = builder
                .Selector("root-selector")
                    .Splice(CreateDeathAITree())
                    .Splice(CreateCombatAITree())
                    .Splice(CreateLootAITree())
                    .Splice(CreateQuestAITree())
                    .Splice(CreateInventoryAITree())
                    .Splice(CreateIdleAITree())
                .End()
                .Build();
        }

        /// <summary>
        /// Handles the AI loop
        /// </summary>
        private void Loop()
        {
            while (true)
            {
                try
                {
                    // Wait 2 seconds before we start AI to make sure we are logged in to the game
                    if ((MM_GetTime() - lastUpdateTime) < 2000) continue;

                    // Update NPC movement
                    var npcUnits = Client.objectMgr.GetNpcUnits().ToList();
                    foreach (var npc in npcUnits)
                        if (npc.MonsterMovement != null)
                            npc.MonsterMovement.UpdatePosition();

                    // Update behavior tree
                    this.mTree.Tick(new TimeData());

                    // Process the activity stack
                    if (mActivityStack.Count > 0)
                        mActivityStack.Peek().Process();
                }
                catch (Exception ex)
                {
                    Log.WriteLine(LogType.Error, "Exception Occured");
                    Log.WriteLine(LogType.Error, "Message: {0}", ex.Message);
                    Log.WriteLine(LogType.Error, "Stacktrace: {0}", ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// Waits for the ref variable to become false. If that doesn't happen within 2 seconds a timeout will action
        /// will be called.
        /// </summary>
        /// <param name="checkValue"></param>
        /// <param name="timeOut"></param>
        /// <returns>Returns false if the timeout elapsed. Otherwise true.</returns>
        protected bool Wait(ref bool checkValue, Action timeOut)
        {
            return Wait(ref checkValue, 2000, timeOut);
        }

        /// <summary>
        /// Waits for the ref variable to become false. If that doesn't happen within the specified timeout a timeout action
        /// will be called.
        /// </summary>
        /// <param name="checkValue"></param>
        /// <param name="timeoutMilliseconds"></param>
        /// <param name="timeOut"></param>
        /// <returns>Returns false if the timeout elapsed. Otherwise true.</returns>
        protected bool Wait(ref bool checkValue, int timeoutMilliseconds, Action timeOut)
        {
            var startTime = MM_GetTime();
            while(checkValue)
            {
                if ((MM_GetTime() - startTime) >= timeoutMilliseconds)
                {
                    timeOut.Invoke();
                    return false;
                }
            }

            // Check value was returned to off state
            return true;
        }

        #endregion
    }
}
