using mClient.Clients;
using mClient.Shared;
using mClient.World.AI.Activity.Messages;
using mClient.World.AI.Activity.Movement;

namespace mClient.World.AI.Activity.Death
{
    public class FindCorpse : BaseActivity
    {
        #region Declarations

        private uint mCorpseMap;
        private Coordinate mCorpseLocation;
        private bool mHasTeleportedToCorpse;

        #endregion

        #region Constructors

        public FindCorpse(PlayerAI ai) : base(ai)
        {
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Find Corpse"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();
            // Send a query for our corpse
            PlayerAI.Client.SendCorpseQuery();

            // Expect a query response in 2 seconds
            Expect(() => PlayerAI.Player.PlayerCorpse != null, 10000);
        }

        public override void Process()
        {
            // if our expectation elapses, send another query request
            if (ExpectationHasElapsed)
                PlayerAI.Client.SendCorpseQuery();

            // if we have our corpse we can complete this activity
            if (PlayerAI.Player.PlayerCorpse != null)
            {
                PlayerAI.CompleteActivity();
                return;
            }
            else
            {
                // We technically don't need the corpse object, so if we are close to the position just set the corpse object
                if (PlayerAI.Client.movementMgr.CalculateDistance(mCorpseLocation) <= MovementMgr.MINIMUM_FOLLOW_DISTANCE)
                {
                    var corpse = new Clients.Corpse(PlayerAI.Player.Guid) { Position = mCorpseLocation };
                    PlayerAI.Player.ObjectAdded(corpse);

                    PlayerAI.CompleteActivity();
                    return;
                }
            }

            // if we have a corpse location and we have not teleported to it yet, go to it
            if (mCorpseLocation != null && !mHasTeleportedToCorpse)
            {
                mHasTeleportedToCorpse = true;
                PlayerAI.StartActivity(new TeleportToCoordinate(mCorpseMap, mCorpseLocation, PlayerAI));
                return;
            }
        }

        public override void HandleMessage(ActivityMessage message)
        {
            base.HandleMessage(message);

            if (message.MessageType == Constants.WorldServerOpCode.MSG_CORPSE_QUERY)
            {
                var corpseMessage = message as CorpseQueryMessage;
                if (corpseMessage == null) return;

                // Push a new activity that allows us to teleport to our corpse
                mCorpseMap = corpseMessage.MapId;
                mCorpseLocation = corpseMessage.Location;
                mHasTeleportedToCorpse = false;
            }
        }

        #endregion
    }
}
