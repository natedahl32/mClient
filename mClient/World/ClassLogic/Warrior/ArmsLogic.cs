using mClient.Constants;
using mClient.DBC;

namespace mClient.World.ClassLogic.Warrior
{
    public class ArmsLogic : WarriorLogic
    {
        #region Declarations

        // holds the last time our target dodged our attack
        private uint mLastDodgeFlag = 0;

        #endregion

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
                // Overpower (only after our target has dodged and within 3 seconds)
                if ((MM_GetTime() - mLastDodgeFlag) < 3000 && HasSpellAndCanCast(OVERPOWER))
                {
                    mLastDodgeFlag = 0;
                    return Spell(OVERPOWER);
                }
                // Thunder Clap
                if (HasSpellAndCanCast(THUNDER_CLAP) && !currentTarget.HasAura(THUNDER_CLAP)) return Spell(THUNDER_CLAP);
                // Heroic Strike
                if (!mHeroicStrikePrepared && HasSpellAndCanCast(HEROIC_STRIKE))
                {
                    mHeroicStrikePrepared = true;
                    return Spell(HEROIC_STRIKE);
                }
                // Slam
                // TODO: Slam has a cast time, we need to take that into account
                if (HasSpellAndCanCast(SLAM)) return Spell(SLAM);

                return null;
            }
        }

        #endregion

        #region Public Methods

        public override void AttackUpdate(DamageInfo damageInfo)
        {
            // call base
            base.AttackUpdate(damageInfo);

            // If our target dodged an attack set the last dodge time
            if (damageInfo.Attacker.GetOldGuid() == Player.Guid.GetOldGuid())
                if (damageInfo.HitInfo.HasFlag(HitInfo.HITINFO_MISS) && damageInfo.TargetState == VictimState.VICTIMSTATE_DODGE)
                    mLastDodgeFlag = MM_GetTime();
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
