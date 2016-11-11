using mClient.World.Quest;
using mClient.Clients;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mClient.World.AI.Activity.Messages;
using mClient.Constants;

namespace mClient.World.AI.Activity.Quest
{
    public class TurnInQuests : BaseActivity
    {
        #region Declarations

        private Object mTurningInToQuestGiver;
        private bool mRetrievedQuestsFromGiver = false;

        // Holds all quests retrieved from the quest giver
        private List<System.UInt32> mQuestsRetrieved;
        // Holds all quests we currently have in our log that were retrieved from the quest giver
        private List<System.UInt32> mQuestsWeHaveInLog;

        #endregion

        #region Constructors

        public TurnInQuests(Object questGiver, PlayerAI ai) : base(ai)
        {
            if (questGiver == null) throw new System.ArgumentNullException("questGiver");
            mTurningInToQuestGiver = questGiver;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Turn In Quests"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();
            PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, "I'm turning in some quests now.");
        }

        public override void Complete()
        {
            base.Complete();

            // Remove the quest giver as well so we don't keep trying to get quests from this entity immediately until
            // we can update the quest giver statuses
            PlayerAI.Player.RemoveQuestGiver(mTurningInToQuestGiver.Guid.GetOldGuid());

            // Finally update any quest giver statuses
            PlayerAI.Client.GetQuestGiverStatuses();
        }

        public override void Process()
        {
            // If our expectation for quests has elapsed, complete this activity
            if (ExpectationHasElapsed) PlayerAI.CompleteActivity();

            // Are we in range to accept the questgiver?
            if (PlayerAI.Client.movementMgr.CalculateDistance(mTurningInToQuestGiver.Position) > MovementMgr.MINIMUM_FOLLOW_DISTANCE)
            {
                // TODO: Blindly setting the quest giver as follow target is dangerous. We could run
                // right into a pack of hostiles. Should fix this!
                PlayerAI.SetFollowTarget(mTurningInToQuestGiver);
                return;
            }

            // If we've already retrieved quests from the quest giver
            if (mRetrievedQuestsFromGiver)
            {
                // Try to complete all the quests we received from the quest giver one at a time
                if (mQuestsWeHaveInLog == null) return;

                // If we don't have anymore quests we can finish this activity
                if (mQuestsWeHaveInLog.Count == 0)
                {
                    PlayerAI.CompleteActivity();
                    return;
                }

                // Get a quest to try and complete
                var selectedQuest = mQuestsWeHaveInLog[0];
                mQuestsWeHaveInLog.RemoveAt(0);

                // Complete the quest
                PlayerAI.StartActivity(new CompleteQuest(mTurningInToQuestGiver.Guid.GetOldGuid(), selectedQuest, PlayerAI));

                return;
            }

            // Get the quest list from the quest giver and turn in all quests they have to offer us
            PlayerAI.Client.GetQuestListFromQuestGiver(mTurningInToQuestGiver.Guid.GetOldGuid());
            mRetrievedQuestsFromGiver = true;
            // Start an expectation that we get quests back from the server
            Expect(() => mQuestsWeHaveInLog != null);
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

            // Handles a quest list returned from an npc
            if (message.MessageType == Constants.WorldServerOpCode.SMSG_QUESTGIVER_QUEST_LIST)
            {
                // Get the quest id list returned from this quest giver
                var questListMessage = message as QuestListMessage;
                if (questListMessage != null)
                {
                    if (questListMessage.FromEntityGuid == mTurningInToQuestGiver.Guid.GetOldGuid())
                    {
                        mQuestsRetrieved = questListMessage.QuestIdList.ToList();
                        mQuestsWeHaveInLog = mQuestsRetrieved.Where(q => PlayerAI.Player.PlayerObject.GetQuestSlot(q) < QuestConstants.MAX_QUEST_LOG_SIZE).ToList();
                    }
                }
            }

            // Handles a single quest returned from an npc as opposed to a list
            if (message.MessageType == WorldServerOpCode.SMSG_QUESTGIVER_REQUEST_ITEMS)
            {
                var requestItemsMessage = message as QuestGiverRequestItemsMessage;
                if (requestItemsMessage != null)
                {
                    // Make sure it's the correct npc
                    if (requestItemsMessage.NpcId == mTurningInToQuestGiver.Guid.GetOldGuid())
                    {
                        // If the quest is completable
                        if (requestItemsMessage.IsCompletable)
                        {
                            // Make sure we have the quest in our log
                            if (PlayerAI.Player.PlayerObject.GetQuestSlot(requestItemsMessage.QuestId) < QuestConstants.MAX_QUEST_LOG_SIZE)
                            {
                                mQuestsWeHaveInLog = new List<uint>() { requestItemsMessage.QuestId };
                            }
                            else
                            {
                                PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, string.Format("I received quest {0} from Npc but it isn't in my log!", requestItemsMessage.QuestId));
                            }
                        }
                        else
                        {
                            // TODO: Finish this up by sending a message via chat as to what items are still missing
                            // from the quest requirements and for what quest. Not certain under what circumstances we would
                            // receive this op code.
                            PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, string.Format("I received quest {0} from Npc but it isn't completable. I must be missing some items!", requestItemsMessage.QuestId));
                            mQuestsWeHaveInLog = new List<uint>();
                        }
                    }
                }
            }
        }

        #endregion
    }
}
