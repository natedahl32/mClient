using mClient.DBC;

namespace mClient.World.ClassLogic.Warrior
{
    public class FuryLogic : WarriorLogic
    {
        #region Constructors

        public FuryLogic(Player player) : base(player)
        {

        }

        #endregion

        #region Properties

        public override SpellEntry NextSpellInRotation
        {
            get
            {
                // Buffs we should have before going into combat but maybe we don't because we didn't have enough rage
                if (HasSpellAndCanCast(BATTLE_SHOUT) && !Player.HasAura(BATTLE_SHOUT)) return Spell(BATTLE_SHOUT);

                var currentTarget = Player.PlayerAI.TargetSelection;
                if (currentTarget == null)
                    return null;

                // Execute
                if (HasSpellAndCanCast(EXECUTE) && currentTarget.HealthPercentage < 20) return Spell(EXECUTE);
                // Bloodthirst
                if (HasSpellAndCanCast(BLOODTHIRST)) return Spell(BLOODTHIRST);
                // Whirlwind
                if (HasSpellAndCanCast(WHIRLWIND)) return Spell(WHIRLWIND);
                // Heroic Strike
                if (HasSpellAndCanCast(HEROIC_STRIKE)) return Spell(HEROIC_STRIKE);

                return null;
            }
        }

        #endregion

        #region Private Methods

        protected override uint Stance()
        {
            return BERSERKER_STANCE;
        }

        #endregion
    }
}
