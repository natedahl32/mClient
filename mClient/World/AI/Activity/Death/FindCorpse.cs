using mClient.World.AI.Activity.Messages;
using mClient.World.AI.Activity.Movement;

namespace mClient.World.AI.Activity.Death
{
    public class FindCorpse : BaseActivity
    {
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
        }

        public override void Process()
        {
            // if we have our corpse we can complete this activity
            if (PlayerAI.Player.PlayerCorpse != null)
            {
                PlayerAI.CompleteActivity();
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
                PlayerAI.StartActivity(new TeleportToCoordinate(corpseMessage.MapId, corpseMessage.Location, PlayerAI));
            }
        }

        #endregion
    }
}
