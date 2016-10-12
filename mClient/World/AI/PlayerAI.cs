using mClient.Clients;
using PObject = mClient.Clients.Object;
using System;
using System.Linq;
using mClient.Shared;
using System.Runtime.InteropServices;
using System.Threading;

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the AI thread
        /// </summary>
        public void StartAI()
        {
            try
            {
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
            if (target.Guid.GetOldGuid() != mTargetSelection.Guid.GetOldGuid())
                mIsAttackingTarget = false;

            mTargetSelection = target;
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
                    // TOOD: This will just be a tick on the behavior tree in the future

                    // If I'm in combat, handle the situation first before I do anything else
                    if (Player.IsInCombat)
                    {

                        // TODO: Use our target, NOT first enemy in the list
                        SetTargetSelection(Client.objectMgr.getObject(Player.EnemyList.FirstOrDefault()));

                        // If we are a melee player check melee range
                        // If we are not in melee range, set the target as the follow target
                        if (NotInMeleeRange && Player.IsMelee)
                        {
                            Client.movementMgr.FollowTarget = mTargetSelection;
                        }
                        else
                        {
                            // TOOD: Spell Casters should cast a spell here. BUT first we need to check range on the target
                            // TOOD: We need a state here. We can't keep sending this packet, we only need
                            // to send it once.
                            if (!mIsAttackingTarget && mTargetSelection != null)
                            {
                                Client.Attack(mTargetSelection.Guid.GetOldGuid());
                                mIsAttackingTarget = true;
                            }
                            
                            continue;
                        }
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

        #endregion
    }
}
