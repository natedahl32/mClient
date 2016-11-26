using mClient.Clients;
using mClient.Constants;
using System;

namespace mClient.World.AI.Activity.Item
{
    public class RepairAllItems : BaseActivity
    {
        #region Declarations

        private Clients.Unit mRepairUnit;

        #endregion

        #region Constructors

        public RepairAllItems(Clients.Unit repairUnit, PlayerAI ai) : base(ai)
        {
            if (repairUnit == null) throw new ArgumentNullException("repairUnit");
            mRepairUnit = repairUnit;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Repairing Items"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();
            PlayerAI.Client.SendChatMsg(ChatMsg.Party, Languages.Universal, "I'm repairing my items real quick.");
        }

        public override void Process()
        {
            // Make sure we are within distance of the repair man
            if (PlayerAI.Client.movementMgr.CalculateDistance(mRepairUnit.Position) > MovementMgr.MINIMUM_FOLLOW_DISTANCE)
            {
                // TODO: Blindly setting the quest giver as follow target is dangerous. We could run
                // right into a pack of hostiles. Should fix this!
                PlayerAI.SetFollowTarget(mRepairUnit);
                return;
            }
            else
            {
                PlayerAI.Client.RepairAllItems(mRepairUnit.Guid.GetOldGuid());
                PlayerAI.CompleteActivity();
                return;
            }
        }

        #endregion
    }
}
