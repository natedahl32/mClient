using mClient.Clients;
using mClient.Constants;
using mClient.World.AI.Activity.Messages;
using mClient.World.Spells;
using System;
using System.Collections.Generic;

namespace mClient.World.AI.Activity.Train
{
    public class TrainSkill : BaseActivity
    {
        #region Declarations

        private Clients.Unit mTrainer;
        private bool mRequestListFromTrainer = false;
        private List<TrainerSpellData> mCanLearnSpells;

        #endregion

        #region Constructors

        public TrainSkill(Clients.Unit trainer, PlayerAI ai) : base(ai)
        {
            if (trainer == null) throw new ArgumentNullException("trainer");
            mTrainer = trainer;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Training Skill"; }
        }

        #endregion

        #region Public Methods

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

            // Get the skill list from the trainer and learn any new skills available
            PlayerAI.Client.RequestTrainerList(mTrainer.Guid.GetOldGuid());
            mRequestListFromTrainer = true;
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

            if (message.MessageType == Constants.WorldServerOpCode.SMSG_TRAINER_LIST)
            {
                // Get the list of spells/skills returned from the trainer
                var trainerListMessage = message as TrainerSpellListMessage;
                if (trainerListMessage != null)
                {
                    if (trainerListMessage.TrainerGuid.GetOldGuid() == mTrainer.Guid.GetOldGuid())
                    {
                        // Get all spells we can learn from this trainer
                        mCanLearnSpells = trainerListMessage.CanLearnSpells;
                        if (mCanLearnSpells.Count == 0)
                            PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, "There are no skills for me to learn from this trainer.");
                    }
                }
            }
        }

        #endregion
    }
}
