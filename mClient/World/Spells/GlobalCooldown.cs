using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Spells
{
    /// <summary>
    /// Mimics the global cooldown for classes when using an ability
    /// </summary>
    public class GlobalCooldown
    {
        [DllImport("winmm.dll", EntryPoint = "timeGetTime")]
        public static extern uint MM_GetTime();

        #region Declarations

        // Start time the GCD was triggerd
        private uint mStartTime = 0;

        private Player mOwner;

        #endregion

        #region Constructors

        public GlobalCooldown(Player player)
        {
            if (player == null) throw new ArgumentNullException("player");
            mOwner = player;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the duration of the GCD
        /// </summary>
        public uint Duration { get; private set; }

        /// <summary>
        /// Gets whether or not there is currently a GCD. Returns true if the GCD is currently "casting" (meaning nothing else can be casted)
        /// </summary>
        public bool HasGCD
        {
            get
            {
                Update();
                if (Duration == 0 && mStartTime == 0)
                    return false;
                return true;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Triggers the GCD for a spell
        /// </summary>
        /// <param name="spell"></param>
        public void TriggerGCD(SpellEntry spell)
        {
            if (spell == null) throw new ArgumentNullException("spell");

            // Get the gcd value for the spell. If the spell doesn't have one, don't trigger the GCD
            uint gcd = spell.StartRecoveryTime;
            if (gcd == 0)
                return;

            if (gcd >= 1000 && gcd <= 1500)
            {
                // apply haste rating for the player
                gcd = (uint)(gcd * mOwner.PlayerObject.CastSpeedMod);
                if (gcd < 1000)
                    gcd = 1000;
                else if (gcd > 1500)
                    gcd = 1500;
            }

            // Start the GCD
            Duration = gcd;
            mStartTime = MM_GetTime();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the GCD and whether or not it has lapsed
        /// </summary>
        private void Update()
        {
            // If we don't have a duration there is nothing to do
            if (Duration == 0) return;
            // If we don't have a start time there is nothing to do
            if (mStartTime == 0) return;

            // Check if the GCD has elapsed
            if (MM_GetTime() > (mStartTime + Duration))
            {
                Duration = 0;
                mStartTime = 0;
            }
        }

        #endregion
    }
}
