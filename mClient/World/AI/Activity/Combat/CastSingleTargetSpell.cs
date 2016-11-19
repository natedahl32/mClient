using mClient.Constants;
using mClient.DBC;
using mClient.Shared;
using mClient.World.AI.Activity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Combat
{
    /// <summary>
    /// Casts a spell by a PLAYER at the currently selected target
    /// </summary>
    public class CastSingleTargetSpell : BaseActivity
    {
        #region Declarations

        private SpellEntry mCastingSpell;
        private bool mDoneCasting = false;

        #endregion

        #region Constructors

        public CastSingleTargetSpell(SpellEntry spell, PlayerAI ai) : base(ai)
        {
            if (spell == null) throw new ArgumentNullException("spell");
            mCastingSpell = spell;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Casting Spell"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();
            PlayerAI.Player.CastSpell(mCastingSpell, PlayerAI.TargetSelection);
            PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, $"I'm casting {mCastingSpell.SpellName} {mCastingSpell.Rank}.");
        }

        public override void Process()
        {
            if (mDoneCasting)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // If the current target selection is an NPC unit and dead we can safely complete this activity
            if (PlayerAI.TargetSelection == null || (PlayerAI.TargetSelection.IsNPC && PlayerAI.TargetSelection.IsDead))
            {
                PlayerAI.CompleteActivity();
                return;
            }
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

            // Catches spell cast failure do to any number of reasons, we don't really care what the reason is though
            if (message.MessageType == Constants.WorldServerOpCode.SMSG_CAST_FAILED)
            {
                var castFailedMessage = message as SpellCastFailedMessage;
                if (castFailedMessage != null)
                {
                    if (castFailedMessage.SpellId == mCastingSpell.SpellId)
                    {
                        mDoneCasting = true;
                        Log.WriteLine(LogType.Debug, $"Cast of spell {mCastingSpell.SpellName} failed because reason {castFailedMessage.Result}.");
                    }
                }
            }

            // Catches spell that was interrupted
            if (message.MessageType == Constants.WorldServerOpCode.SMSG_SPELL_FAILURE)
            {
                var castInterruptedMessage = message as SpellCastInterruptedMessage;
                if (castInterruptedMessage != null)
                {
                    if (castInterruptedMessage.SpellId == mCastingSpell.SpellId && castInterruptedMessage.CasterGuid.GetOldGuid() == PlayerAI.Player.Guid.GetOldGuid())
                    {
                        mDoneCasting = true;
                    }
                }
            }

            // Catches spell that was cast successfully
            if (message.MessageType == Constants.WorldServerOpCode.SMSG_SPELL_GO)
            {
                var spellGoMessage = message as SpellCastGoMessage;
                if (spellGoMessage != null)
                {
                    if (spellGoMessage.SpellId == mCastingSpell.SpellId && spellGoMessage.CasterGuid.GetOldGuid() == PlayerAI.Player.Guid.GetOldGuid())
                    {
                        mDoneCasting = true;
                    }
                }
            }
        }

        #endregion
    }
}
