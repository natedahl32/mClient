using mClient.Clients;
using mClient.Constants;
using mClient.World.AI.Activity.Messages;
using mClient.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Loot
{
    public class LootGameObject : BaseActivity
    {
        #region Declarations

        private mClient.Clients.Object mLootableObject;
        private bool mIsLooting;
        private List<LootItem> mItemsToLoot;

        #endregion

        #region Constructors

        public LootGameObject(mClient.Clients.Object obj, PlayerAI ai) : base(ai)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            mLootableObject = obj;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Looting Game Object"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();
            PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, "I'm looting a chest real quick.");
        }

        public override void Complete()
        {
            base.Complete();

            // Set it as looted so we don't try to loot it again.
            if ((mLootableObject as Clients.GameObject) != null)
                (mLootableObject as Clients.GameObject).HasBeenLooted = true;

            // Send release loot
            PlayerAI.Client.ReleaseLoot(mLootableObject.Guid.GetOldGuid());
        }

        public override void Process()
        {
            // We are close enough, if we aren't looting start looting 
            if (!mIsLooting)
            {
                // Are we close enough to our lootable
                var distance = PlayerAI.Client.movementMgr.CalculateDistance(mLootableObject.Position);
                if (distance > MovementMgr.MINIMUM_FOLLOW_DISTANCE)
                {
                    // Set our follow target and then exit out as running
                    PlayerAI.SetFollowTarget(mLootableObject);
                    return;
                }

                // We are close enough, now loot it
                // TODO: I'm not sure this is the correct way to do it, I think what spell/how we open
                // the chest has to do with one of the data fields and the Lock.dbc file, but I can't find a connection.
                PlayerAI.Client.CastSpell(mLootableObject, PlayerAI.Player.GetSpellToOpenLockedObject(mLootableObject as Clients.GameObject));
                mIsLooting = true;
                // Set expectation that we get loot
                Expect(() => mItemsToLoot != null, 10000); // open spell is a 5 second cast, so wait at least double that
                return;
            }

            // If we don't have items to loot yet keep waiting for them
            if (mItemsToLoot == null)
            {
                // If our expecation that we got loot has lapsed, get out
                if (ExpectationHasElapsed)
                    PlayerAI.CompleteActivity();
                return;
            }

            // If we have items to loot still do that now
            if (mItemsToLoot.Count > 0)
            {
                var itemToLoot = mItemsToLoot[0];
                mItemsToLoot.RemoveAt(0);

                // Loot the item
                if (itemToLoot != null)
                    PlayerAI.StartActivity(new LootItemFromObject(itemToLoot, PlayerAI));

                return;
            }

            // Complete the activity
            PlayerAI.CompleteActivity();
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

            // Handle loot response message
            if (message.MessageType == Constants.WorldServerOpCode.SMSG_LOOT_RESPONSE)
            {
                var lootMessage = message as LootMessage;
                if (lootMessage != null)
                {
                    // If we have gold to loot, do that now
                    if (lootMessage.CoinAmount > 0)
                        PlayerAI.Client.LootMoney();

                    // Set the items to loot
                    mItemsToLoot = lootMessage.Items;
                }
            }
        }

        #endregion
    }
}
