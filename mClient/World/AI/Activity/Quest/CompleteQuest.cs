using mClient.World.AI.Activity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mClient.World.AI.Activity.Quest
{
    public class CompleteQuest : BaseActivity
    {
        #region Declarations

        private UInt64 mQuestGiverGuid;
        private UInt32 mCompletingQuestId;
        private bool mReceivedCompletion;

        private List<QuestOfferRewards.RewardItem> mQuestRewardOptions;

        #endregion

        #region Constructors

        public CompleteQuest(UInt64 questGiverGuid, UInt32 questId, PlayerAI ai) : base(ai)
        {
            mQuestGiverGuid = questGiverGuid;
            mCompletingQuestId = questId;
            mReceivedCompletion = false;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Complete Quest"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();
            PlayerAI.Client.CompleteQuest(mQuestGiverGuid, mCompletingQuestId);
        }

        public override void Process()
        {
            // if we have quest rewards we need to either choose a reward or just send back a response
            // to complete the quest
            if (mQuestRewardOptions != null)
            {
                // if we don't have any options or we only have one option, then send the response
                // we still need to send a response with 0 options
                if (mQuestRewardOptions.Count <= 1)
                {
                    PlayerAI.Client.ChooseQuestReward(mQuestGiverGuid, mCompletingQuestId, 0);
                    // Remove the options so we don't try choosing one again
                    mQuestRewardOptions = null;
                    return;
                }

                // TODO: We need to make a decision on which items to select for the quest reward
                PlayerAI.Client.ChooseQuestReward(mQuestGiverGuid, mCompletingQuestId, 0);
            }

            // if we have received completion of the quest then remove it from our quest log and end this activity
            if (mReceivedCompletion)
            {
                // Remove the quest from our log now that we have completed it. NOTE - call the one on the player object
                // otherwise will try to send the drop to the server (which we don't want to do because we just completed it)
                PlayerAI.Player.PlayerObject.DropQuest(mCompletingQuestId);
                PlayerAI.CompleteActivity();
            }
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

            // message for rewards
            if (message.MessageType == Constants.WorldServerOpCode.SMSG_QUESTGIVER_OFFER_REWARD)
            {
                var questOfferRewardMessage = message as QuestOfferRewards;
                if (questOfferRewardMessage != null)
                    mQuestRewardOptions = questOfferRewardMessage.RewardItems.ToList();
            }

            // message for quest complete
            if (message.MessageType == Constants.WorldServerOpCode.SMSG_QUESTGIVER_QUEST_COMPLETE)
            {
                var questCompleteMessage = message as QuestCompleteMessage;
                if (questCompleteMessage != null)
                {
                    if (questCompleteMessage.QuestId == mCompletingQuestId)
                        mReceivedCompletion = true;
                }
            }
        }

        #endregion
    }
}
