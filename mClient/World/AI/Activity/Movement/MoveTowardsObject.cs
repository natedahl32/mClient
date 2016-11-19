using System;

namespace mClient.World.AI.Activity.Movement
{
    public class MoveTowardsObject : BaseActivity
    {
        #region Declarations

        private const float NON_MOVE_BUFFER = 0.5f;
        private const float MOVE_BUFFER = 2.0f;

        private Clients.Object mMoveTowardsObject;
        private float mMaxDistance;

        #endregion

        #region Constructors

        public MoveTowardsObject(Clients.Object obj, float maxDistance, PlayerAI ai) : base(ai)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            mMoveTowardsObject = obj;
            mMaxDistance = maxDistance;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Move Towards Object"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            // Set follow target to the object
            PlayerAI.SetFollowTarget(mMoveTowardsObject);
        }

        public override void Process()
        {
            // Once we are within range we can complete the activity
            var distance = PlayerAI.Client.movementMgr.CalculateDistance(mMoveTowardsObject.Position);
            if (distance < (mMaxDistance - NON_MOVE_BUFFER))
            {
                // If the object is a unit and they are moving, give ourselves a little leeway
                var unit = mMoveTowardsObject as Clients.Unit;
                if (unit != null && mMaxDistance > MOVE_BUFFER && unit.IsMoving)
                {
                    if (distance < (mMaxDistance - MOVE_BUFFER))
                    {
                        PlayerAI.CompleteActivity();
                        PlayerAI.StartActivity(new Wait(1000, PlayerAI));
                        return;
                    }
                }
                else
                {
                    PlayerAI.CompleteActivity();
                    PlayerAI.StartActivity(new Wait(1000, PlayerAI));
                    return;
                }
            }
        }

        #endregion
    }
}
