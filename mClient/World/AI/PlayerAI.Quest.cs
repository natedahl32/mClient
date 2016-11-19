using FluentBehaviourTree;
using mClient.Clients;
using mClient.Constants;
using mClient.Shared;
using mClient.Terrain;
using mClient.World.AI.Activity.Quest;
using mClient.World.Quest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mClient.World.AI
{
    public partial class PlayerAI
    {
        #region Declarations

        private const float DISTANCE_TRAVELED_TO_UPDATE_QUEST_GIVERS = 20.0f;
        private const float MAXIMUM_DISTANCE_FOR_SOURCE_POINT_OBJECTIVE = 20.0f;
        private const float MAXIMUM_DISTANCE_FOR_GO_OBJECTIVE = 10.0f;

        // Holds the last coordinate that we did a quest status update check
        private Coordinate mLastQuestStatusCheckCoordinate;
        // The object guid we are currently accepting a quest from
        private QuestGiver mAcceptingQuestFrom;
        // The object guid we are currently turning in a quest to
        private QuestGiver mTurningInQuestTo;

        // Wait variables
        private bool mWaitingToAcceptQuests = false;
        private bool mWaitingToTurnInQuests = false;

        private List<QuestGiver> mTurninQuestsGivers;
        private List<QuestGiver> mHasAvailableQuestsGivers;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether or not this player is waiting to accept quests from an entity.
        /// </summary>
        public bool WaitingToAcceptQuests { get { return mWaitingToAcceptQuests; } set { mWaitingToAcceptQuests = value; } }

        /// <summary>
        /// Gets or sets wehther or not this player is waiting to turn in quests from an entity
        /// </summary>
        public bool WaitingToTurnInQuests { get { return mWaitingToTurnInQuests; } set { mWaitingToTurnInQuests = value; } }

        #endregion

        protected IBehaviourTreeNode CreateQuestAITree()
        {
            var builder = new BehaviourTreeBuilder();
            return builder
                .Sequence("quest-sequence")
                    .Sequence("Find Quests to Get or Turn In")
                        .Do("Need Quest Status Update?", t => QuestGiverStatusUpdate())
                        .Do("Has Current Activity?", t =>            // quest logic doesn't run if we have an activity we are currently doing
                                {
                                    if (HasCurrentActivity)
                                        return BehaviourTreeStatus.Failure;
                                    return BehaviourTreeStatus.Success;
                                })
                        .Selector("Quest Turn In or Get Quests")
                            .Sequence("Turn In Quests")
                                .Do("Has Quests To Turn In", t => HasQuestsToTurnIn())
                                .Do("Find Quest To Turn In", t => FindQuestToTurnIn())
                                .Do("Turn In Quests", t => TurnInQuest())
                            .End()
                            .Sequence("Get Available Quests")
                                .Do("Has Quests Available To Accept", t => HasQuestsAvailableToGet())
                                .Do("Find Quest To Accept", t => FindQuestToAccept())
                                .Do("Accept Quetss", t => AcceptQuest())
                            .End()
                        .End()
                    .End()
                 .End()
                 .Build();
        }

        /// <summary>
        /// Does an update for quest give statuses
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus QuestGiverStatusUpdate()
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
            if (TerrainMgr.CalculateDistance(Player.Position, mLastQuestStatusCheckCoordinate) >= DISTANCE_TRAVELED_TO_UPDATE_QUEST_GIVERS)
            {
                mLastQuestStatusCheckCoordinate = Player.Position;
                Client.GetQuestGiverStatuses();
                return BehaviourTreeStatus.Failure;
            }

            return BehaviourTreeStatus.Success;
        }

        /// <summary>
        /// Checks if we have any quest givers that we can turn in a quest to
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus HasQuestsToTurnIn()
        {
            // See if we have any quests available to turn in and select the closest.
            mTurninQuestsGivers = Player.QuestGivers.Where(q => q.Status == Constants.QuestGiverStatus.DIALOG_STATUS_REWARD_REP ||
                                                                q.Status == Constants.QuestGiverStatus.DIALOG_STATUS_REWARD_OLD ||
                                                                q.Status == Constants.QuestGiverStatus.DIALOG_STATUS_REWARD2).ToList();
            if (mTurninQuestsGivers.Count > 0)
                return BehaviourTreeStatus.Success;
            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Checks if we have any quest givers that we can get quests from
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus HasQuestsAvailableToGet()
        {
            // See if we have any quests available to accept
            mHasAvailableQuestsGivers = Player.QuestGivers.Where(q => q.Status == Constants.QuestGiverStatus.DIALOG_STATUS_AVAILABLE).ToList();
            if (mHasAvailableQuestsGivers.Count > 0)
                return BehaviourTreeStatus.Success;

            //if (gameObjects.Any(go => go.))
            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Finds a quest within distance to turn in
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus FindQuestToTurnIn()
        {
            // If we already found a quest to turn in, use that one
            if (mTurningInQuestTo != null) return BehaviourTreeStatus.Success;

            // Find a quest to turn in
            var closestDistance = 1000000.0f;
            mClient.Clients.Object chosenObject = null;
            foreach (var q in mTurninQuestsGivers)
            {
                var obj = Client.objectMgr.getObject(new WoWGuid(q.Guid));
                if (obj != null)
                {
                    var distance = Client.movementMgr.CalculateDistance(obj.Position);
                    if (distance < closestDistance && distance < QuestConstants.MAX_TURNIN_DISTANCE)
                    {
                        closestDistance = distance;
                        mTurningInQuestTo = q;
                        chosenObject = obj;
                    }
                }
            }

            // If we found one return success
            if (mTurningInQuestTo != null)
                return BehaviourTreeStatus.Success;
            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Finds a quest within distance to accept
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus FindQuestToAccept()
        {
            // If we already found a quest to accept, use that one
            if (mAcceptingQuestFrom != null) return BehaviourTreeStatus.Success;

            // Find a quest to turn in
            var closestDistance = 1000000.0f;
            mClient.Clients.Object chosenObject = null;
            foreach (var q in mHasAvailableQuestsGivers)
            {
                var obj = Client.objectMgr.getObject(new WoWGuid(q.Guid));
                if (obj != null)
                {
                    var distance = Client.movementMgr.CalculateDistance(obj.Position);
                    if (distance < closestDistance && distance < QuestConstants.MAX_TURNIN_DISTANCE)
                    {
                        closestDistance = distance;
                        mAcceptingQuestFrom = q;
                        chosenObject = obj;
                    }
                }
            }

            // If we found one return success
            if (mAcceptingQuestFrom != null)
                return BehaviourTreeStatus.Success;
            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Turns in the quest we have chosen
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus TurnInQuest()
        {
            // Make sure we have a quest giver that we are turning in quests to
            if (mTurningInQuestTo == null) return BehaviourTreeStatus.Failure;

            // Make sure the questgiver object currently exists
            var obj = Client.objectMgr.getObject(new WoWGuid(mTurningInQuestTo.Guid));
            if (obj == null)
            {
                mTurningInQuestTo = null;
                return BehaviourTreeStatus.Failure;
            }

            // Start the turn in quest activity
            StartActivity(new TurnInQuests(obj, this));
            mTurningInQuestTo = null;
            return BehaviourTreeStatus.Success;
        }

        /// <summary>
        /// Accepts the quest we have chosen
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus AcceptQuest()
        {
            // Make sure we have a quest giver that we are accepting quests from
            if (mAcceptingQuestFrom == null) return BehaviourTreeStatus.Failure;

            // Make sure the questgiver object currently exists
            var obj = Client.objectMgr.getObject(new WoWGuid(mAcceptingQuestFrom.Guid));
            if (obj == null)
            {
                mAcceptingQuestFrom = null;
                return BehaviourTreeStatus.Failure;
            }

            // Start the accept quest activity
            StartActivity(new AcceptQuest(obj, this));
            mAcceptingQuestFrom = null;
            return BehaviourTreeStatus.Success;
        }
    }
}
