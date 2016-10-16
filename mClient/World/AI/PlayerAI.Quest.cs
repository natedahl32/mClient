using FluentBehaviourTree;
using mClient.Shared;
using System;
using System.Linq;

namespace mClient.World.AI
{
    public partial class PlayerAI
    {
        // Holds the last coordinate that we did a quest status update check
        private Coordinate mLastQuestStatusCheckCoordinate;
        // The object guid we are currently accepting a quest from
        private UInt64 mAcceptingQuestFrom;

        protected IBehaviourTreeNode CreateQuestAITree()
        {
            // TODO: Need to code in a wait for rez if we are in combat
            var builder = new BehaviourTreeBuilder();
            return builder
                .Sequence("quest-sequence")
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
                    .Do("Do I have a quest to turn in?", t =>
                    {
                        return BehaviourTreeStatus.Success;
                    })
                    .Do("Do I have a quest available?", t =>
                    {
                        // If we are currently accepting a quest.
                        if (mAcceptingQuestFrom > 0)
                        {
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

                            // TODO: Actually go through the quest dialogs to accept all quests available
                            // from this quest giver.
                            return BehaviourTreeStatus.Success;
                        }

                        // So if we have any quests available to get and select the closest.
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

                        // If we have found a chosen target, start moving towards them
                        // TODO: Blindly setting the quest giver as follow target is dangerous. We could run
                        // right into a pack of hostiles. Should fix this!
                        if (chosenObject != null)
                            SetFollowTarget(chosenObject);


                        return BehaviourTreeStatus.Success;
                    })
                 .End()
                 .Build();
        }
    }
}
