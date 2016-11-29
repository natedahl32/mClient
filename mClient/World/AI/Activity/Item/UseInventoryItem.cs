using mClient.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Item
{
    public class UseInventoryItem : BaseActivity
    {
        #region Declarations

        private uint mUseItemId;
        private bool mIsDone = false;

        // Inventory slot containing the item to use
        private InventoryItemSlot mInvSlot;

        #endregion

        #region Constructors

        public UseInventoryItem(uint itemId, PlayerAI ai) : base(ai)
        {
            mUseItemId = itemId;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Using Inventory Item"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            // Get the item we want to use and make sure we have it in inventory
            mInvSlot = PlayerAI.Player.PlayerObject.GetInventoryItem(mUseItemId);
            if (mInvSlot == null)
            {
                mIsDone = true;
                return;
            }
        }

        public override void Process()
        {
            // Check if we are done
            if (mIsDone)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // Use the item on the target now and complete the activity
            PlayerAI.Client.UseItemInInventoryOnSelf((byte)mInvSlot.Bag, (byte)mInvSlot.Slot);
            PlayerAI.CompleteActivity();
        }

        #endregion
    }
}
