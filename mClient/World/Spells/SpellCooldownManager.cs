using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Spells
{
    public class SpellCooldownManager
    {
        #region Declarations

        // Holds all our spell cooldowns
        private Dictionary<uint, SpellCooldown> mSpellCooldowns = new Dictionary<uint, SpellCooldown>();
        private Object mSpellCooldownLock = new System.Object();

        // Player that owns this spell cooldown manager
        private Player mOwner;

        #endregion

        #region Constructors

        public SpellCooldownManager(Player player)
        {
            if (player == null) throw new ArgumentNullException("player");
            mOwner = player;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts a cooldown for a spell. If a cooldown already exists for the spell this will overwrite it.
        /// </summary>
        /// <param name="spell"></param>
        public void StartCooldown(SpellEntry spell)
        {
            if (spell == null) throw new ArgumentNullException("spell");

            // Create the cooldown and trigger it
            var cooldown = new SpellCooldown(spell);
            cooldown.TriggerCooldown(mOwner);

            // If the spell has a spell category cooldown, we need to apply the cooldown for all spells the player owns
            // with that category.
            if (spell.Category > 0 && spell.CategoryRecoveryTime > 0)
                ApplyCategoryCooldowns(spell);

            // If the cooldown was triggered (spell might now have a cooldown) then add it to the cooldowns
            if (cooldown.Duration > 0)
            {
                // Add it to our cooldowns
                AddCooldown(cooldown);
            }
        }

        /// <summary>
        /// Checks if a spell is currently on cooldown
        /// </summary>
        /// <param name="spellId"></param>
        public bool HasCooldown(uint spellId)
        {
            lock (mSpellCooldownLock)
            {
                if (mSpellCooldowns.ContainsKey(spellId))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Updates cooldowns, removes all that have elapsed
        /// </summary>
        public void UpdateCooldowns()
        {
            var toRemoveList = new List<uint>();
            lock (mSpellCooldownLock)
            {
                // Update cooldowns and add them to be removed if the duration has elapsed
                foreach (var kvp in mSpellCooldowns)
                {
                    kvp.Value.Update();
                    if (kvp.Value.Duration == 0)
                        toRemoveList.Add(kvp.Key);
                }

                foreach (var toRemove in toRemoveList)
                    mSpellCooldowns.Remove(toRemove);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds or updates a cooldown
        /// </summary>
        /// <param name="cooldown"></param>
        private void AddCooldown(SpellCooldown cooldown)
        {
            // Add it to our cooldowns
            lock (mSpellCooldownLock)
                if (mSpellCooldowns.ContainsKey(cooldown.Spell.SpellId))
                    mSpellCooldowns[cooldown.Spell.SpellId] = cooldown;
                else
                    mSpellCooldowns.Add(cooldown.Spell.SpellId, cooldown);
        }

        /// <summary>
        /// Applies cooldowns for all spells in this category
        /// </summary>
        /// <param name="spell"></param>
        private void ApplyCategoryCooldowns(SpellEntry spell)
        {
            var category = spell.Category;
            var duration = spell.CategoryRecoveryTime;

            // If no category or category duration there is nothing to do
            if (category == 0) return;
            if (duration == 0) return;

            // Go through all spells the player has, excluding the spell that was passed
            foreach (var spellId in mOwner.Spells)
            {
                // Skip the spell that was passed in, we already handled that
                if (spellId == spell.SpellId) continue;

                var s = SpellTable.Instance.getSpell(spellId);
                if (s != null && s.Category == category)
                {
                    var cooldown = new SpellCooldown(s);
                    cooldown.TriggerCooldown(duration);
                    AddCooldown(cooldown);
                }
            }
        }

        #endregion
    }
}
