using mClient.DBC;

namespace mClient.World.ClassLogic.Warrior
{
    public class ArmsLogic : WarriorLogic
    {
        #region Constructors

        public ArmsLogic(Player player) : base(player)
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
                // Rend
                if (HasSpellAndCanCast(REND) && !currentTarget.HasAura(REND)) return Spell(REND);
                // Mortal Strike
                if (HasSpellAndCanCast(MORTAL_STRIKE)) return Spell(MORTAL_STRIKE);
                // TOOD: Overpower, need to find a way to determine if it is active or not
                // Thunder Clap
                if (HasSpellAndCanCast(THUNDER_CLAP) && !currentTarget.HasAura(THUNDER_CLAP)) return Spell(THUNDER_CLAP);
                // Heroic Strike
                if (HasSpellAndCanCast(HEROIC_STRIKE)) return Spell(HEROIC_STRIKE);
                // Slam
                // TODO: Slam has a cast time, we need to take that into account
                if (HasSpellAndCanCast(SLAM)) return Spell(SLAM);

                return null;
            }
        }

        #endregion

        #region Private Methods

        protected override uint Stance()
        {
            return BATTLE_STANCE;
        }

        #endregion
    }
}
