using mClient.Clients;
using mClient.Constants;
using mClient.World.AI.Activity.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Quest
{
    public class AcceptQuest : BaseActivity
    {
        #region Declarations

        private Object mAcceptingFromQuestGiver;
        private bool mRetrievedQuestsFromGiver = false;

        // Holds all quests we currently do not have in our log that were retrieved from the quest giver
        private List<System.UInt32> mQuestsWeDoNotHave;

        #endregion

        #region Constructors

        public AcceptQuest(Object questGiver, PlayerAI ai) : base(ai)
        {
            if (questGiver == null) throw new System.ArgumentNullException("questGiver");
            mAcceptingFromQuestGiver = questGiver;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Accept Quests"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();
            PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, "I'm getting an available quest.");
        }

        public override void Complete()
        {
            base.Complete();

            // Remove the quest giver as well so we don't keep trying to get quests from this entity immediately until
            // we can update the quest giver statuses
            PlayerAI.Player.RemoveQuestGiver(mAcceptingFromQuestGiver.Guid.GetOldGuid());

            // Finally update any quest giver statuses
            PlayerAI.Client.GetQuestGiverStatuses();
        }

        public override void Process()
        {
            // If our expectation for a quest has elapsed, then complete the activity
            if (ExpectationHasElapsed)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // Are we in range to accept the questgiver?
            if (PlayerAI.Client.movementMgr.CalculateDistance(mAcceptingFromQuestGiver.Position) > MovementMgr.MINIMUM_FOLLOW_DISTANCE)
            {
                // TODO: Blindly setting the quest giver as follow target is dangerous. We could run
                // right into a pack of hostiles. Should fix this!
                PlayerAI.SetFollowTarget(mAcceptingFromQuestGiver);
                return;
            }

            // If we've already retrieved quests from the quest giver
            if (mRetrievedQuestsFromGiver)
            {
                // Try to complete all the quests we received from the quest giver one at a time
                if (mQuestsWeDoNotHave == null) return;

                // If we don't have anymore quests we can finish this activity
                if (mQuestsWeDoNotHave.Count == 0)
                {
                    PlayerAI.CompleteActivity();
                    return;
                }

                // Get a quest to try and accept
                var selectedQuest = mQuestsWeDoNotHave[0];
                mQuestsWeDoNotHave.RemoveAt(0);

                // If the selected quest is on our ignore list, ignore it
                if (PlayerAI.Player.IgnoredQuests.Contains(selectedQuest)) return;

                // Accept the quest
                PlayerAI.Client.AcceptQuest(mAcceptingFromQuestGiver.Guid.GetOldGuid(), selectedQuest);

                return;
            }

            // Get the quest list from the quest giver and accept all quests they have to offer us
            PlayerAI.Client.GetQuestListFromQuestGiver(mAcceptingFromQuestGiver.Guid.GetOldGuid());
            mRetrievedQuestsFromGiver = true;
            // Start an expectation that we get an available quest we don't already have from the server
            Expect(() => mQuestsWeDoNotHave != null);
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

            if (message.MessageType == Constants.WorldServerOpCode.SMSG_QUESTGIVER_QUEST_LIST)
            {
                // Get the quest id list returned from this quest giver
                var questListMessage = message as QuestListMessage;
                if (questListMessage != null)
                {
                    if (questListMessage.FromEntityGuid == mAcceptingFromQuestGiver.Guid.GetOldGuid())
                    {
                        var questsRetrieved = questListMessage.QuestIdList.ToList();
                        mQuestsWeDoNotHave = questsRetrieved.Where(q => PlayerAI.Player.PlayerObject.GetQuestSlot(q) == QuestConstants.MAX_QUEST_LOG_SIZE).ToList();
                    }
                }
            }
        }

        #endregion
    }
}
