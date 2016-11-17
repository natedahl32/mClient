using mClient.DBC;
using mClient.World.AI.Activity.Messages;
using mClient.World.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Train
{
    public class LearnSpellFromTrainer : BaseActivity
    {
        #region Declarations

        private Clients.Unit mTrainer;
        private TrainerSpellData mSpellData;
        private bool mDone = false;

        #endregion

        #region Constructors

        public LearnSpellFromTrainer(Clients.Unit trainer, TrainerSpellData spell, PlayerAI ai) : base(ai)
        {
            if (trainer == null) throw new ArgumentNullException("trainer");
            if (spell == null) throw new ArgumentNullException("spell");

            mTrainer = trainer;
            mSpellData = spell;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Learn Spell From Trainer"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            // If we don't have enough money for this spell, not much we can do
            if (mSpellData.Cost > PlayerAI.Player.PlayerObject.Money)
            {
                mDone = true;
                return;
            }

            // Buy the spell
            PlayerAI.Client.BuySpellFromTrainer(mTrainer.Guid.GetOldGuid(), mSpellData.SpellId);
        }

        public override void Process()
        {
            if (mDone)
            {
                PlayerAI.CompleteActivity();
                return;
            }
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

            if (message.MessageType == Constants.WorldServerOpCode.SMSG_TRAINER_BUY_SUCCEEDED)
            {
                // Get the quest id list returned from this quest giver
                var trainerBuySucceededMessage = message as TrainerBuySucceededMessage;
                if (trainerBuySucceededMessage != null)
                {
                    if (trainerBuySucceededMessage.TrainerGuid.GetOldGuid() == mTrainer.Guid.GetOldGuid() && trainerBuySucceededMessage.SpellId == mSpellData.SpellId)
                    {
                        mDone = true;

                        // Remove the spell that this spell granted us from the available spell list and add the spell to our list
                        var boughtSpell = SpellTable.Instance.getSpell(mSpellData.SpellId);
                        if (boughtSpell == null)
                            return;

                        var triggeredSpellId = boughtSpell.EffectTriggerSpell[0];
                        PlayerAI.Player.AddSpell((ushort)triggeredSpellId);
                    }
                }
            }
        }

        #endregion
    }
}
