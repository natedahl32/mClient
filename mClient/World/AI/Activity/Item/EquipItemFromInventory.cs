using mClient.Clients;
using mClient.Constants;
using mClient.World.AI.Activity.Messages;
using System;

namespace mClient.World.AI.Activity.Item
{
    public class EquipItemFromInventory : BaseActivity
    {
        #region Declarations

        private InventoryItemSlot mInventoryItemToEquip;
        private EquipmentSlots mDestinationSlot = EquipmentSlots.EQUIPMENT_SLOT_END;

        private bool mDone = false;
        private bool mSentEquip = false;
        private bool mIsSuccessful = true; // we assume success unless we get a failure

        #endregion

        #region Constructors

        public EquipItemFromInventory(InventoryItemSlot invSlot, PlayerAI ai) : base(ai)
        {
            if (invSlot == null) throw new ArgumentNullException("invSlot");
            mInventoryItemToEquip = invSlot;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Equipping Item From Inventory"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, $"I'm equipping the item {mInventoryItemToEquip.Item.BaseInfo.ItemGameLink}.");
        }

        public override void Process()
        {
            // Check for expectation elapsed. This is generally the case when equipping to an empty slot. We don't get a response back from the server.
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

            // On our first pass, find the equipment slot that we want to equip this item in
            if (mDestinationSlot == EquipmentSlots.EQUIPMENT_SLOT_END)
            {
                if (mInventoryItemToEquip.Item.BaseInfo.InventoryType == InventoryType.INVTYPE_WEAPON)
                {
                    // Check main hand slot
                    mDestinationSlot = CheckSlot(EquipmentSlots.EQUIPMENT_SLOT_MAINHAND);
                    if (mDestinationSlot != EquipmentSlots.EQUIPMENT_SLOT_END)
                        return;
                    // Check off hand slot
                    mDestinationSlot = CheckSlot(EquipmentSlots.EQUIPMENT_SLOT_OFFHAND);
                }
                else if (mInventoryItemToEquip.Item.BaseInfo.InventoryType == InventoryType.INVTYPE_TRINKET)
                {
                    mDestinationSlot = CheckSlot(EquipmentSlots.EQUIPMENT_SLOT_TRINKET1);
                    if (mDestinationSlot != EquipmentSlots.EQUIPMENT_SLOT_END)
                        return;
                    mDestinationSlot = CheckSlot(EquipmentSlots.EQUIPMENT_SLOT_TRINKET2);
                }
                else if (mInventoryItemToEquip.Item.BaseInfo.InventoryType == InventoryType.INVTYPE_FINGER)
                {
                    mDestinationSlot = CheckSlot(EquipmentSlots.EQUIPMENT_SLOT_FINGER1);
                    if (mDestinationSlot != EquipmentSlots.EQUIPMENT_SLOT_END)
                        return;
                    mDestinationSlot = CheckSlot(EquipmentSlots.EQUIPMENT_SLOT_FINGER2);
                }

                // If we can't find a slot to equip to, then complete the activity
                if (mDestinationSlot == EquipmentSlots.EQUIPMENT_SLOT_END)
                    PlayerAI.CompleteActivity();
                // Return regardless of whether we found a slot to equip to or not, we will actually equip next pass
                return;
            }

            // We have a destination slot, let's equip the item
            if (!mSentEquip)
            {
                PlayerAI.Client.EquipItem(mInventoryItemToEquip.Item.Guid.GetOldGuid(), (byte)mDestinationSlot);
                mSentEquip = true;
                Expect(() => mDone, 2000);
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

        #region Private Methods

        /// <summary>
        /// Checks an equipment slot to see if the equipped item exists or is worse than the item we want to equip
        /// </summary>
        /// <param name="slotToCheck"></param>
        /// <returns></returns>
        private EquipmentSlots CheckSlot(EquipmentSlots slotToCheck)
        {
            var equipped = PlayerAI.Player.PlayerObject.GetItemInEquipmentSlot(slotToCheck);
            if (equipped == null || PlayerAI.Player.ClassLogic.CompareItems(mInventoryItemToEquip.Item, equipped) > 0)
                return slotToCheck;
            return EquipmentSlots.EQUIPMENT_SLOT_END;
        }

        #endregion
    }
}
