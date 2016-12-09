using mClient.DBC;

namespace mClient.World.ClassLogic.Warlock
{
    public class AfflictionLogic : WarlockLogic
    {
        #region Constructors

        public AfflictionLogic(Player player) : base(player)
        {
        }

        #endregion

        #region Properties

        public override SpellEntry NextSpellInRotation
        {
            get
            {
                var currentTarget = Player.PlayerAI.TargetSelection;
                if (currentTarget == null)
                    return null;

                // Corruption
                if (HasSpellAndCanCast(CORRUPTION) && !currentTarget.HasAura(CORRUPTION)) return Spell(CORRUPTION);
                // Immolate
                if (HasSpellAndCanCast(IMMOLATE) && !currentTarget.HasAura(IMMOLATE)) return Spell(IMMOLATE);
                // Siphon Life
                if (HasSpellAndCanCast(SIPHON_LIFE) && !currentTarget.HasAura(SIPHON_LIFE)) return Spell(SIPHON_LIFE);
                // Shadow Bolt
                if (HasSpellAndCanCast(SHADOW_BOLT)) return Spell(SHADOW_BOLT);

                return null;
            }
        }

        #endregion
    }
}
