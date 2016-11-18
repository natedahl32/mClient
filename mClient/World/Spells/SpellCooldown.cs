using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Spells
{
    public class SpellCooldown
    {
        [DllImport("winmm.dll", EntryPoint = "timeGetTime")]
        public static extern uint MM_GetTime();

        #region Declarations

        private SpellEntry mCooldownForSpell;

        // Start time the GCD was triggerd
        private uint mStartTime = 0;

        #endregion

        #region Constructors

        public SpellCooldown(SpellEntry spell)
        {
            if (spell == null) throw new ArgumentNullException("spell");
            mCooldownForSpell = spell;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the duration of the cooldown
        /// </summary>
        public uint Duration { get; private set; }

        /// <summary>
        /// Gets the spell this cooldown is for
        /// </summary>
        public SpellEntry Spell { get { return mCooldownForSpell; } }

        #endregion

        #region Public Methods

        /// <summary>
        /// Triggers the cooldown for a spell
        /// </summary>
        /// <param name="spell"></param>
        public void TriggerCooldown(Player player)
        {
            // Get values from spell
            uint category = mCooldownForSpell.Category;
            uint recovery = mCooldownForSpell.RecoveryTime;
            uint categoryRecovery = mCooldownForSpell.CategoryRecoveryTime;

            // for shoot spells
            if (recovery <= 0 && categoryRecovery <= 0 && (category == 76 || category == 351))
                recovery = player.PlayerObject.GetAttackTime(Constants.WeaponAttackType.RANGED_ATTACK);

            // TODO: Spell mods?

            // If no cooldown for this specific spell, check the category cooldown for this spell and use that. If no cooldown at all, we are done.
            if (recovery < 0) recovery = 0;
            if (categoryRecovery < 0) categoryRecovery = 0;
            if (recovery == 0)
                recovery = categoryRecovery;
            if (recovery == 0)
                return;

            // Set duration of cooldown
            Duration = recovery;
            mStartTime = MM_GetTime();
        }

        /// <summary>
        /// Triggers a cooldown for a specific duration. Used for spell category cooldowns
        /// </summary>
        /// <param name="cooldown"></param>
        public void TriggerCooldown(uint cooldown)
        {
            Duration = cooldown;
            mStartTime = MM_GetTime();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the spell cooldown and whether or not it has lapsed
        /// </summary>
        internal protected void Update()
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
