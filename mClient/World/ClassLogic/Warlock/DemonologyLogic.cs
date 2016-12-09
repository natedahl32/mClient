using mClient.DBC;

namespace mClient.World.ClassLogic.Warlock
{
    public class DemonologyLogic : WarlockLogic
    {
        #region Constructors

        public DemonologyLogic(Player player) : base(player)
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
                // Shadow Bolt
                if (HasSpellAndCanCast(SHADOW_BOLT)) return Spell(SHADOW_BOLT);

                return null;
            }
        }

        #endregion
    }
}
