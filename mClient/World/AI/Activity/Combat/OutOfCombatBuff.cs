using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Combat
{
    public class OutOfCombatBuff : BaseActivity
    {
        #region Declarations

        private SpellEntry mBuffSpell;
        private List<Player> mReceivingBuff;
        private bool mIsGroupBuff = false;

        #endregion

        #region Constructors

        public OutOfCombatBuff(SpellEntry buff, IList<Player> receivingBuffs, PlayerAI ai) : base(ai)
        {
            if (buff == null) throw new ArgumentNullException("buff");
            if (receivingBuffs == null || receivingBuffs.Count == 0) throw new ArgumentNullException("receivingBuffs");

            mBuffSpell = buff;
            mReceivingBuff = receivingBuffs.ToList();
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Out of Combat Buffs"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            // Determine if the spell we are buffing with is a single target or group buff spell
            mIsGroupBuff = mBuffSpell.IsGroupSpell;
        }
        public override void Process()
        {
            // If not more players need the buff we are done
            if (mReceivingBuff.Count == 0)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // TODO: If this spell requires a reagent and we are out of it now, complete the activity

            // Handle buffing differently depending on if this is a single target spell or group spell
            if (mIsGroupBuff)
                HandleGroupBuff();
            else
                HandleSingleTargetBuff();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles group buff spells
        /// </summary>
        private void HandleGroupBuff()
        {
            if (mReceivingBuff.Count == 0) return;

            // First remove all targets that have had the buff applied since the last cast. By nature of being a group spell
            // a cast on one target may apply the aura to other targets as well.
            mReceivingBuff.RemoveAll(p => p.HasAura(mBuffSpell.SpellId));

            // Now just handle as though it were a single target spell cast
            HandleSingleTargetBuff();
        }

        /// <summary>
        /// Handles single target buff spells
        /// </summary>
        private void HandleSingleTargetBuff()
        {
            if (mReceivingBuff.Count == 0) return;

            // Remove next target in buff list and cast on them
            var nextTarget = mReceivingBuff[0];
            mReceivingBuff.RemoveAt(0);

            // Target the player first
            PlayerAI.SetTargetSelection(nextTarget.PlayerObject);

            // Case on the target
            PlayerAI.StartActivity(new CastSingleTargetSpell(mBuffSpell, PlayerAI));
        }

        #endregion
    }
}
