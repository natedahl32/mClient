using mClient.DBC;

namespace mClient.World.ClassLogic.Warlock
{
    public class DestructionLogic : WarlockLogic
    {
        #region Constructors

        public DestructionLogic(Player player) : base(player)
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

                // Shadowburn
                if (HasSpellAndCanCast(CORRUPTION) && !currentTarget.HasAura(CORRUPTION) && currentTarget.HealthPercentage <= 10.0f) return Spell(CORRUPTION);
                // Corruption
                if (HasSpellAndCanCast(CORRUPTION) && !currentTarget.HasAura(CORRUPTION)) return Spell(CORRUPTION);
                // Immolate
                if (HasSpellAndCanCast(IMMOLATE) && !currentTarget.HasAura(IMMOLATE)) return Spell(IMMOLATE);
                // Conflagarate
                if (HasSpellAndCanCast(CONFLAGRATE) && currentTarget.HasAura(IMMOLATE)) return Spell(CONFLAGRATE);
                // Shadow Bolt
                if (HasSpellAndCanCast(SHADOW_BOLT)) return Spell(SHADOW_BOLT);

                return null;
            }
        }

        #endregion
    }
}
