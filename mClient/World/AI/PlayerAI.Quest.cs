using FluentBehaviourTree;
using mClient.Shared;
using System;
using System.Linq;

namespace mClient.World.AI
{
    public partial class PlayerAI
    {
        #region Declarations

        // Holds the last coordinate that we did a quest status update check
        private Coordinate mLastQuestStatusCheckCoordinate;
        // The object guid we are currently accepting a quest from
        private UInt64 mAcceptingQuestFrom;
        // The object guid we are currently turning in a quest to
        private UInt64 mTurningInQuestTo;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether or not this player is waiting to accept quests from an entity.
        /// </summary>
        public bool WaitingToAcceptOrTurnInQuests { get; set; }

        #endregion

        protected IBehaviourTreeNode CreateQuestAITree()
        {
            // TODO: Need to code in a wait for rez if we are in combat
            var builder = new BehaviourTreeBuilder();
            return builder
                .Sequence("quest-sequence")
                    .Sequence("Find Quests to Get or Turn In")
                        .Do("Need Quest Status Update?", t =>
                        {
                            // If we don't have a last coordinate yet, we haven't done a quest status check.
                            // Do it now and return failure to skip this AI. We will revisit it once we get back
                            // quest statuses.
                            if (mLastQuestStatusCheckCoordinate == null)
                            {
                                mLastQuestStatusCheckCoordinate = Player.Position;
                                Client.GetQuestGiverStatuses();
                                return BehaviourTreeStatus.Failure;
                            }

                            // If we have moved more than 20 yards since the last update, do another update and
                            // return failure. We will revisit this once we get back quest statuses.
                            if (Client.movementMgr.CalculateDistance(mLastQuestStatusCheckCoordinate) >= 20.0f)
                            {
                                mLastQuestStatusCheckCoordinate = Player.Position;
                                Client.GetQuestGiverStatuses();
                                return BehaviourTreeStatus.Failure;
                            }

                            return BehaviourTreeStatus.Success;
                        })
                        .Do("Find Quest to Turn In", t =>
                        {
                            // See if we have any quests available to turn in and select the closest.
                            var questGivers = Player.QuestGivers.Where(q => q.Status == Constants.QuestGiverStatus.DIALOG_STATUS_REWARD_REP ||
                                                                            q.Status == Constants.QuestGiverStatus.DIALOG_STATUS_REWARD_OLD ||
                                                                            q.Status == Constants.QuestGiverStatus.DIALOG_STATUS_REWARD2).ToList();
                            var closestDistance = 1000000.0f;
                            mClient.Clients.Object chosenObject = null;
                            foreach (var q in questGivers)
                            {
                                var obj = Client.objectMgr.getObject(new WoWGuid(q.Guid));
                                if (obj != null)
                                {
                                    var distance = Client.movementMgr.CalculateDistance(obj.Position);
                                    if (distance < closestDistance)
                                    {
                                        closestDistance = distance;
                                        mTurningInQuestTo = q.Guid;
                                        chosenObject = obj;
                                    }
                                }
                            }

                            return BehaviourTreeStatus.Success;
                        })
                        .Do("Find Available Quest to Get", t =>
                        {
                            // See if we have any quests available to get and select the closest.
                            var questGivers = Player.QuestGivers.Where(q => q.Status == Constants.QuestGiverStatus.DIALOG_STATUS_AVAILABLE).ToList();
                            var closestDistance = 1000000.0f;
                            mClient.Clients.Object chosenObject = null;
                            foreach (var q in questGivers)
                            {
                                var obj = Client.objectMgr.getObject(new WoWGuid(q.Guid));
                                if (obj != null)
                                {
                                    var distance = Client.movementMgr.CalculateDistance(obj.Position);
                                    if (distance < closestDistance)
                                    {
                                        closestDistance = distance;
                                        mAcceptingQuestFrom = q.Guid;
                                        chosenObject = obj;
                                    }
                                }
                            }

                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                    .Selector("Get or Turn In Quests")
                        .Do("Do I have a quest to turn in?", t =>
                        {
                            // Try to turn in quests first to make room for new ones and open up new quest chains

                            // If I don't have a guid with a quest to turn in
                            if (mTurningInQuestTo == 0) return BehaviourTreeStatus.Failure;

                            // If we are waiting to turn in quests from a quest giver than exit out with success
                            if (WaitingToAcceptOrTurnInQuests) return BehaviourTreeStatus.Success;

                            // Does this object exist currently?
                            var obj = Client.objectMgr.getObject(new WoWGuid(mTurningInQuestTo));
                            if (obj == null)
                            {
                                mTurningInQuestTo = 0;
                                return BehaviourTreeStatus.Failure;
                            }

                            // Are we in range to accept the quest?
                            if (Client.movementMgr.CalculateDistance(obj.Position) > 3.0f)
                            {
                                // TODO: Blindly setting the quest giver as follow target is dangerous. We could run
                                // right into a pack of hostiles. Should fix this!
                                SetFollowTarget(obj);
                                return BehaviourTreeStatus.Success;
                            }

                            // Get the quest list from the quest giver and accept all quests they have to offer for us
                            Client.GetQuestListFromQuestGiver(mTurningInQuestTo);
                            // Clear the guid of the person we are accepting quests from
                            mTurningInQuestTo = 0;
                            // Set flag that we are waiting to accept quests from the quest giver
                            WaitingToAcceptOrTurnInQuests = true;

                            return BehaviourTreeStatus.Success;
                        })
                        .Do("Do I have a quest to accept?", t =>
                        {
                            // If I don't have a guid with a quest to accept
                            if (mAcceptingQuestFrom == 0) return BehaviourTreeStatus.Failure;

                            // If we are waiting to accept quests from a quest giver than exit out with success
                            if (WaitingToAcceptOrTurnInQuests) return BehaviourTreeStatus.Success;

                            // Does this object exist currently?
                            var obj = Client.objectMgr.getObject(new WoWGuid(mAcceptingQuestFrom));
                            if (obj == null)
                            {
                                mAcceptingQuestFrom = 0;
                                return BehaviourTreeStatus.Failure;
                            }

                            // Are we in range to accept the quest?
                            if (Client.movementMgr.CalculateDistance(obj.Position) > 3.0f)
                            {
                                // TODO: Blindly setting the quest giver as follow target is dangerous. We could run
                                // right into a pack of hostiles. Should fix this!
                                SetFollowTarget(obj);
                                return BehaviourTreeStatus.Success;
                            }

                            // Get the quest list from the quest giver and accept all quests they have to offer for us
                            Client.GetQuestListFromQuestGiver(mAcceptingQuestFrom);
                            // Clear the guid of the person we are accepting quests from
                            mAcceptingQuestFrom = 0;
                            // Set flag that we are waiting to accept quests from the quest giver
                            WaitingToAcceptOrTurnInQuests = true;

                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                 .End()
                 .Build();
        }
    }
}
