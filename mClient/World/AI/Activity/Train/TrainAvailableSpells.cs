using mClient.Clients;
using mClient.Constants;
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
    public class TrainAvailableSpells : BaseActivity
    {
        #region Declarations

        private Clients.Unit mTrainer;
        private bool mRequestListFromTrainer = false;
        private List<TrainerSpellData> mCanLearnSpells;

        #endregion

        #region Constructors

        public TrainAvailableSpells(Clients.Unit trainer, PlayerAI ai) : base(ai)
        {
            if (trainer == null) throw new ArgumentNullException("trainer");
            mTrainer = trainer;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Train Available Spells"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();
            PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, "I'm visiting my class trainer to get my new spells.");
        }

        public override void Complete()
        {
            base.Complete();

            // When we are done at a trainer, re-initialize our spells in class logic. We most likely got some upgrades so 
            // we want to take advantage of that right away.
            PlayerAI.Player.ClassLogic.InitializeSpells();
        }

        public override void Process()
        {
            // Are we in range of the trainer?
            if (PlayerAI.Client.movementMgr.CalculateDistance(mTrainer.Position) > MovementMgr.MINIMUM_FOLLOW_DISTANCE)
            {
                // TODO: Blindly setting the trainer as follow target is dangerous. We could run
                // right into a pack of hostiles. Should fix this!
                PlayerAI.SetFollowTarget(mTrainer);
                return;
            }

            // If we've already requested the list from the trainer
            if (mRequestListFromTrainer)
            {
                // We haven't received the list yet, keep waiting for it
                if (mCanLearnSpells == null) return;

                // If we don't have anymore spells that we can learn
                if (mCanLearnSpells.Count == 0)
                {
                    PlayerAI.CompleteActivity();
                    return;
                }

                // Get a spell and try to learn it
                var selectedSpell = mCanLearnSpells[0];
                mCanLearnSpells.RemoveAt(0);

                // Learn the spell from the trainer
                PlayerAI.StartActivity(new LearnSpellFromTrainer(mTrainer, selectedSpell, PlayerAI));

                return;
            }

            // Get the spell list from the trainer and learn any new spells
            PlayerAI.Client.RequestTrainerList(mTrainer.Guid.GetOldGuid());
            mRequestListFromTrainer = true;
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

            if (message.MessageType == Constants.WorldServerOpCode.SMSG_TRAINER_LIST)
            {
                // Get the quest id list returned from this quest giver
                var trainerListMessage = message as TrainerSpellListMessage;
                if (trainerListMessage != null)
                {
                    if (trainerListMessage.TrainerGuid.GetOldGuid() == mTrainer.Guid.GetOldGuid())
                    {
                        // Use different variable so we don't modify the enumeration in Process while we are getting our data
                        var learnSpells = trainerListMessage.CanLearnSpells;
                        // Set the cost of all spells we can learn so we know how much they are in the future and don't keep trying to buy them if we don't have enough Ca$h
                        foreach (var trainerSpell in learnSpells)
                        {
                            var spell = SpellTable.Instance.getSpell(trainerSpell.SpellId);
                            if (spell != null)
                            {
                                // Now get the triggered spell
                                var triggeredSpell = SpellTable.Instance.getSpell(spell.EffectTriggerSpell[0]);
                                if (triggeredSpell != null)
                                    triggeredSpell.MoneyCost = trainerSpell.Cost;
                            }
                        }
                        // Remove all spells that we can't afford
                        learnSpells.RemoveAll(s => s.Cost > PlayerAI.Player.PlayerObject.Money);

                        mCanLearnSpells = learnSpells;
                    }
                }
            }
        }

        #endregion
    }
}
