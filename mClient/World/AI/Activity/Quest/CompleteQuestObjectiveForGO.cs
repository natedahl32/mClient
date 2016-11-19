using mClient.Clients;
using mClient.Shared;
using mClient.World.AI.Activity.Messages;
using mClient.World.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Quest
{
    /// <summary>
    /// Activity that attempts to complete an object for a quest. This may not complete the quest, as it may require multiple objectives to be met.
    /// </summary>
    public class CompleteQuestObjectiveForGO : BaseActivity
    {
        #region Declarations

        private QuestInfo mQuestInfo;
        private Clients.Object mQuestObject;
        private bool mObjectiveComplete = false;

        private bool mUsedItem = false;

        #endregion

        #region Constructors

        public CompleteQuestObjectiveForGO(QuestInfo quest, Clients.Object questObject, PlayerAI ai) : base(ai)
        {
            if (quest == null) throw new ArgumentNullException("quest");
            if (questObject == null) throw new ArgumentNullException("questObject");
            mQuestInfo = quest;
            mQuestObject = questObject;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Complete Quest Objective for Game Object"; }
        }

        #endregion

        #region Public Methods

        public override void Process()
        {
            // If the objective was completed we can exit out
            if (mObjectiveComplete)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // TODO: Is the object hostile or are there hostiles around it? If so, maybe we don't want to run in right away? Maybe we want to rest to full health and/or clear hostiles around the target first.
            // Are we in range of the object?
            if (PlayerAI.Client.movementMgr.CalculateDistance(mQuestObject.Position) > MovementMgr.MINIMUM_FOLLOW_DISTANCE)
            {
                // TODO: Blindly setting the object as follow target is dangerous. We could run
                // right into a pack of hostiles. Should fix this!
                PlayerAI.SetFollowTarget(mQuestObject);
                return;
            }

            // We are in range, if the quest has a source item let's use it on the object now
            if (mQuestInfo.SourceItemId > 0)
            {
                // If we've used the item already
                if (mUsedItem)
                    return;

                // TODO: Check the range on the source items spell to make sure we are close enough

                // Get the item from our inventory
                var invSlot = PlayerAI.Player.PlayerObject.GetInventoryItem(mQuestInfo.SourceItemId);
                PlayerAI.Client.UseItemInInventoryOnTarget((byte)invSlot.Bag, (byte)invSlot.Slot, mQuestObject.Guid);
                mUsedItem = true;
                return;
            }
            else
            {
                // TODO: What to do here? Kill quest? Use/open the object? Talk to them if they are a unit?
            }
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

            if (message.MessageType == Constants.WorldServerOpCode.SMSG_QUESTUPDATE_ADD_KILL)
            {
                // Get the quest id list returned from this quest giver
                var questAddKillUpdateMessage = message as QuestUpdateAddKillMessage;
                if (questAddKillUpdateMessage != null)
                {
                    if (questAddKillUpdateMessage.QuestId == mQuestInfo.QuestId)
                    {
                        mObjectiveComplete = true;
                    }
                }
            }

            if (message.MessageType == Constants.WorldServerOpCode.SMSG_CAST_FAILED)
            {
                var spellCastFailedMessage = message as SpellCastFailedMessage;
                if (spellCastFailedMessage != null)
                {
                    Log.WriteLine(LogType.Debug, "Cast failed when trying to complete an objective. Spell id {0} and result is {1}", spellCastFailedMessage.SpellId, spellCastFailedMessage.Result);
                }
            }
        }

        #endregion
    }
}
