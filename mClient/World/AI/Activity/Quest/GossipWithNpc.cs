using mClient.Clients;
using mClient.World.AI.Activity.Messages;
using System;

namespace mClient.World.AI.Activity.Quest
{
    public class GossipWithNpc : BaseActivity
    {
        #region Declarations

        private Clients.Unit mNpc;
        private bool mDone = false;
        private bool mStartedGossip = false;

        // Holds the gossip menu index to send to the npc
        private int mGossipMenuIndex = -1;

        #endregion

        #region Constructors

        public GossipWithNpc(Clients.Unit npc, PlayerAI ai) : base(ai)
        {
            if (npc == null) throw new ArgumentNullException("npc");
            mNpc = npc;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Gossiping with NPC"; }
        }

        #endregion

        #region Public Methods

        public override void Process()
        {
            // Check if we are done
            if (mDone)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // Are we in range to talk to the npc?
            if (PlayerAI.Client.movementMgr.CalculateDistance(mNpc.Position) > MovementMgr.MINIMUM_FOLLOW_DISTANCE)
            {
                // TODO: Blindly setting the npc as follow target is dangerous. We could run
                // right into a pack of hostiles. Should fix this!
                PlayerAI.SetFollowTarget(mNpc);
                return;
            }

            // If we have not started gossip yet do that now
            if (!mStartedGossip)
            {
                mStartedGossip = true;
                PlayerAI.Client.Gossip(mNpc.Guid.GetOldGuid());
                return;
            }

            // If we have a gossip menu index to send do that now
            if (mGossipMenuIndex > -1)
            {
                PlayerAI.Client.Gossip(mNpc.Guid.GetOldGuid(), (uint)mGossipMenuIndex);
                mGossipMenuIndex = -1;
                return;
            }
        }

        public override void Pause()
        {
            base.Pause();
            // Set flag to start gossip again when we come back
            mStartedGossip = false;
            mGossipMenuIndex = -1;
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);
            
            // If we get a gossip complete message we are done
            if (message.MessageType == Constants.WorldServerOpCode.SMSG_GOSSIP_COMPLETE)
            {
                mDone = true;
                return;
            }

            // If we get a gossip menu item message, retrieve the menu items so can traverse them
            if (message.MessageType == Constants.WorldServerOpCode.SMSG_GOSSIP_MESSAGE)
            {
                var gossipItemMessage = message as GossipItemListMessage;
                if (gossipItemMessage != null)
                {
                    // If there are no items in the gossip list, we are done
                    if (gossipItemMessage.GossipItems.Count == 0)
                    {
                        mDone = true;
                        return;
                    }

                    // If there is one item in the gossip list
                    if (gossipItemMessage.GossipItems.Count == 1)
                    {
                        mGossipMenuIndex = (int)gossipItemMessage.GossipItems[0].GossipMenuIndex;
                        return;
                    }

                    // TODO: Handle multiple options    
                    // If there is more than one item in the gossip list just choose the first one for now. We will need to figure a way to handle scenarios where we need to select a specific gossip option
                    mGossipMenuIndex = (int)gossipItemMessage.GossipItems[0].GossipMenuIndex;
                }
            }
        }

        #endregion
    }
}
