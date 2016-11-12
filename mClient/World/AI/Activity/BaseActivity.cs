using mClient.Constants;
using mClient.Shared;
using System;
using System.Timers;

namespace mClient.World.AI.Activity
{
    /// <summary>
    /// Base activity AI class
    /// </summary>
    public abstract class BaseActivity
    {
        #region Declarations

        protected const int DEFAULT_EXPECTATION_ELAPSED_TIME = 2500; // 2.5 seconds elapsed time by default for expectations

        private PlayerAI mPlayerAI;
        private Func<bool> mExpectation;
        private Timer mExpectationTimer;
        private bool mExpecationIsElapsed;

        #endregion

        #region Constructors

        protected BaseActivity(PlayerAI ai)
        {
            if (ai == null) throw new ArgumentNullException("ai");
            mPlayerAI = ai;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the player AI for this activity
        /// </summary>
        public PlayerAI PlayerAI { get { return mPlayerAI; } }

        /// <summary>
        /// Gets the name of this activity
        /// </summary>
        public abstract string ActivityName { get; }

        /// <summary>
        /// Gets whether or not the current expectation has elapsed
        /// </summary>
        public bool ExpectationHasElapsed { get { return mExpecationIsElapsed; } }

        #endregion

        #region Public Methods

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
        /// Processes the activity. This method is only called if this activity is the current activity.
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

        #endregion

        #region Private Methods

        /// <summary>
        /// Starts an expectation for this activity
        /// </summary>
        /// <param name="requirement">Requirement to evalute</param>
        protected void Expect(Func<bool> requirement)
        {
            Expect(requirement, DEFAULT_EXPECTATION_ELAPSED_TIME);
        }

        /// <summary>
        /// Starts an expectation for this activity
        /// </summary>
        /// <param name="requirement">Requirement to evaluate</param>
        /// <param name="expectationElapsedTime">If expectation is not met within this time, the expectation will elapse.</param>
        protected void Expect(Func<bool> requirement, int expectationElapsedTime)
        {
            if (requirement == null) throw new ArgumentNullException("requirement");
            // Stop any currently runnning expectation
            if (mExpectationTimer != null)
                mExpectationTimer.Stop();
            mExpectation = requirement;

            // Create the timer
            mExpecationIsElapsed = false;
            mExpectationTimer = new Timer(expectationElapsedTime);
            mExpectationTimer.Elapsed += ExpectationTimer_Elapsed;
            mExpectationTimer.Start();
        }

        /// <summary>
        /// Stops the expectation from running
        /// </summary>
        protected void StopExpectation()
        {
            if (mExpectationTimer != null)
                mExpectationTimer.Stop();
            mExpecationIsElapsed = false;
        }

        /// <summary>
        /// Elapsed event for expectation timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpectationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Note that we don't call ExpectationElapsed from here because the expectation could lapse while
            // an activity is not the current activity. We want the ability for activities to complete state when
            // an expectation is lapsed so we need to process that in the main Process method, which guarantees
            // our activity is the current activity.
            if (!mExpectation.Invoke())
                mExpecationIsElapsed = true;
            else
            {
                if (mExpectationTimer != null)
                    mExpectationTimer.Stop();
                mExpecationIsElapsed = false;
            }
        }

        #endregion
    }
}
