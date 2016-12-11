
using mClient.Constants;
using mClient.DBC;

namespace mClient.World.ClassLogic.Warrior
{
    public class ProtectionLogic : WarriorLogic
    {
        #region Declarations

        // Holds the last time we had a chance to use Revenge (block, dodge, parry)
        private uint mLastRevengeCounterChance = 0;

        #endregion

        #region Constructors

        public ProtectionLogic(Player player) : base(player)
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

                // If the target of my current is not me, then taunt it on to me
                if (currentTarget.TargetGuid.GetOldGuid() != Player.Guid.GetOldGuid())
                {
                    // Try taunt first
                    if (HasSpellAndCanCast(TAUNT)) return Spell(TAUNT);
                    // Try revenge next
                    if ((MM_GetTime() - mLastRevengeCounterChance) <= 3000 && HasSpellAndCanCast(REVENGE)) return Spell(REVENGE);
                    // Try shield slam
                    if (HasSpellAndCanCast(SHIELD_SLAM)) return Spell(SHIELD_SLAM);
                    // Try sunder armor
                    if (HasSpellAndCanCast(SUNDER_ARMOR)) return Spell(SUNDER_ARMOR);
                }

                // Below is single target dps abilities. We should have already checked targets that we do not have aggro on above.

                // Revenge (if chance occurred in the last 3 seconds)
                if ((MM_GetTime() - mLastRevengeCounterChance) <= 3000 && HasSpellAndCanCast(REVENGE)) return Spell(REVENGE);
                // Use Heroic Strike with large amounts of rage
                if (Player.PlayerObject.CurrentRage >= 80 && !mHeroicStrikePrepared && HasSpellAndCanCast(HEROIC_STRIKE)) return Spell(HEROIC_STRIKE);
                // Apply sunder armors
                var sunderAura = currentTarget.GetAuraForSpell(SUNDER_ARMOR);
                if ((sunderAura == null || sunderAura.Stacks < 3) && HasSpellAndCanCast(SUNDER_ARMOR)) return Spell(SUNDER_ARMOR);
                // Shield block (TODO: if Elite target)
                if (HasSpellAndCanCast(SHIELD_BLOCK)) return Spell(SHIELD_BLOCK);
                // Demoralizing shout
                if (HasSpellAndCanCast(DEMORALIZING_SHOUT) && !currentTarget.HasAura(DEMORALIZING_SHOUT)) return Spell(DEMORALIZING_SHOUT);

                return null;
            }
        }

        #endregion

        #region Public Methods

        public override void AttackUpdate(DamageInfo damageInfo)
        {
            // call base
            base.AttackUpdate(damageInfo);

            // If our target dodged, parried, or blocked an attack set the last revenge chance time
            if (damageInfo.Attacker.GetOldGuid() == Player.Guid.GetOldGuid())
                if (damageInfo.HitInfo.HasFlag(HitInfo.HITINFO_MISS) && 
                        (damageInfo.TargetState == VictimState.VICTIMSTATE_DODGE ||
                         damageInfo.TargetState == VictimState.VICTIMSTATE_BLOCKS ||
                         damageInfo.TargetState == VictimState.VICTIMSTATE_PARRY))
                    mLastRevengeCounterChance = MM_GetTime();
        }

        #endregion

        #region Private Methods

        protected override uint Stance()
        {
            return DEFENSIVE_STANCE;
        }

        #endregion
    }
}
