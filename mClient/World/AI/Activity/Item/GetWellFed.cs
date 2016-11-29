using mClient.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Item
{
    public class GetWellFed : BaseActivity
    {
        #region Declarations

        private InventoryItemSlot mInventoryItem;
        private bool mHasWellFed = false;

        #endregion

        #region Constructors

        public GetWellFed(InventoryItemSlot invItem, PlayerAI ai) : base(ai)
        {
            if (invItem == null) throw new ArgumentNullException("invItem");
            mInventoryItem = invItem;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Getting Well Fed!"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            // Use the item on ourself
            PlayerAI.Client.UseItemInInventoryOnSelf((byte)mInventoryItem.Bag, (byte)mInventoryItem.Slot);

            // Set an expectation that we wait at least 15 seconds for the well fed buff
            Expect(() => mHasWellFed, 15000);
        }

        public override void Process()
        {
            // If our expectation for a quest has elapsed, then complete the activity
            if (ExpectationHasElapsed)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // If we have the well fed buff we are done
            if (mHasWellFed)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // Check for the well fed buff
            mHasWellFed = PlayerAI.Player.IsWellFed;
        }

        #endregion
    }
}
