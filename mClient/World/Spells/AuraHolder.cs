using mClient.DBC;

namespace mClient.World.Spells
{
    public class AuraHolder
    {
        #region Properties

        /// <summary>
        /// Get the spell for this aura
        /// </summary>
        public SpellEntry Spell { get; set; }

        /// <summary>
        /// Gets the duration of the aura in seconds
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// Gets the number of stacks of this aura
        /// </summary>
        public int Stacks { get; set; }

        #endregion
    }
}
