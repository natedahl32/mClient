using mClient.Constants;
using mClient.Shared;
using System;

namespace mClient.World.AI.Activity
{
    /// <summary>
    /// Base activity AI class
    /// </summary>
    public abstract class BaseActivity
    {
        private PlayerAI mPlayerAI;

        protected BaseActivity(PlayerAI ai)
        {
            if (ai == null) throw new ArgumentNullException("ai");
            mPlayerAI = ai;
        }

        /// <summary>
        /// Gets the player AI for this activity
        /// </summary>
        public PlayerAI PlayerAI { get { return mPlayerAI; } }

        /// <summary>
        /// Gets the name of this activity
        /// </summary>
        public abstract string ActivityName { get; }

        /// <summary>
        /// Called when the activity is started
        /// </summary>
        public virtual void Start()
        {
            Log.WriteLine(LogType.Debug, "Starting activity {0}...", ActivityName);
        }

        /// <summary>
        /// Called when the activity is completed and removed from the queue
        /// </summary>
        public virtual void Complete()
        {
            Log.WriteLine(LogType.Debug, "Stopping activity {0}...", ActivityName);
        }

        /// <summary>
        /// Called when an activity is being paused because a new activity has taken priority
        /// </summary>
        public virtual void Pause()
        {
            Log.WriteLine(LogType.Debug, "Pausing activity {0}...", ActivityName);
        }

        /// <summary>
        /// Processes the activity. This method is only called if this activity is the current activity
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// Handles messages from the server. Don't complete activities from here because it isn't guaranteed
        /// this activity is the one that is active.
        /// </summary>
        /// <param name="message"></param>
        public virtual void HandleMessage(ActivityMessage message)
        {

        }
    }
}
