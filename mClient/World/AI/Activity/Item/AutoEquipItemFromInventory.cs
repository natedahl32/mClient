using mClient.Clients;
using mClient.Constants;
using mClient.World.AI.Activity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Item
{
    public class AutoEquipItemFromInventory : BaseActivity
    {
        #region Declarations

        private InventoryItemSlot mInventoryItemToEquip;

        private bool mDone = false;
        private bool mIsSuccessful = true; // we assume success unless we get a failure

        #endregion

        #region Constructors

        public AutoEquipItemFromInventory(InventoryItemSlot invSlot, PlayerAI ai) : base(ai)
        {
            if (invSlot == null) throw new ArgumentNullException("invSlot");
            mInventoryItemToEquip = invSlot;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Auto Equipping Item From Inventory"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, $"I'm equipping the item {mInventoryItemToEquip.Item.Name}.");

            // Send the auto equip message to the server
            PlayerAI.Client.AutoEquipItem((byte)mInventoryItemToEquip.Bag, (byte)mInventoryItemToEquip.Slot);

            // Set an expectation that we receive something in 3 seconds otherwise we assume it was successful
            Expect(() => mDone, 3000);
        }
        public override void Process()
        {
            // Check for expectation elapsed
            if (ExpectationHasElapsed)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            if (mDone)
            {
                // If we were not successful for some reason, do we need to do anyting?
                if (!mIsSuccessful)
                {
                    // TODO: Do what?
                }

                PlayerAI.CompleteActivity();
                return;
            }
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

            if (message.MessageType == Constants.WorldServerOpCode.SMSG_INVENTORY_CHANGE_FAILURE)
            {
                var invChangeMessage = message as InventoryChangeMessage;
                if (invChangeMessage != null)
                {
                    if (invChangeMessage.ResultMessage == Constants.InventoryResult.EQUIP_ERR_OK)
                    {
                        mDone = true;
                        return;
                    }

                    // Anyting else and it was an error
                    mIsSuccessful = false;
                    mDone = true;
                    return;
                }
            }
        }

        #endregion
    }
}
