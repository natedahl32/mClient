using mClient.Clients;
using mClient.World.AI.Activity.Messages;
using mClient.World.Items;
using System.Collections.Generic;

namespace mClient.World.AI.Activity.Loot
{
    public class LootObject : BaseActivity
    {
        #region Declarations

        private Object mLootableObject;
        private bool mIsLooting;
        private List<LootItem> mItemsToLoot;

        #endregion

        #region Constructors

        public LootObject(Object lootable, PlayerAI ai) : base(ai)
        {
            if (lootable == null) throw new System.ArgumentNullException("lootable");
            mLootableObject = lootable;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Looting Object"; }
        }

        #endregion

        #region Public Methods

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
                PlayerAI.Client.Loot(mLootableObject.Guid);
                mIsLooting = true;
                return;
            }

            // If we don't have items to loot yet keep waiting for them
            if (mItemsToLoot == null) return;
            
            // If we have items to loot still do that now
            if (mItemsToLoot.Count > 0)
            {
                var item = mItemsToLoot[0];
                mItemsToLoot.RemoveAt(0);

                // Loot the item
                if (item != null)
                    if (item.LootSlotType == 0)
                        PlayerAI.Client.LootItem(item.LootSlot);

                return;
            }

            // No more items to loot. Remove the lootable and end the activity
            PlayerAI.Player.RemoveLootable(mLootableObject.Guid);
            PlayerAI.CompleteActivity();
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

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
