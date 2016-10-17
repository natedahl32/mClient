using mClient.Clients;
using PObject = mClient.Clients.Object;
using System;
using System.Linq;
using mClient.Shared;
using System.Runtime.InteropServices;
using System.Threading;
using FluentBehaviourTree;

namespace mClient.World.AI
{
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
                    .Splice(CreateQuestAITree())
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
                    this.mTree.Tick(new TimeData());
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
