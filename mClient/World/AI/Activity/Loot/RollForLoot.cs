using mClient.Constants;
using mClient.World.Items;

namespace mClient.World.AI.Activity.Loot
{
    public class RollForLoot : BaseActivity
    {
        #region Declarations

        private ulong mLootSourceGuid;
        private uint mItemSlot;
        private uint mItemId;
        private uint mRandomSuffix;
        private uint mRandomPropertyId;
        private uint mRollTimeout;
        private byte mRollOptions;

        #endregion

        #region Constructors

        public RollForLoot(ulong lootSource, uint itemSlot, uint itemId, uint randomSuffix, uint randomPropertyId, uint rollTimeout, byte rollOptions, PlayerAI ai) : base(ai)
        {
            mLootSourceGuid = lootSource;
            mItemSlot = itemSlot;
            mItemId = itemId;
            mRandomSuffix = randomSuffix;
            mRandomPropertyId = randomPropertyId;
            mRollTimeout = rollTimeout;
            mRollOptions = rollOptions;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Rolling For Loot"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            // Start an expectation that we send a roll back before the roll tiemout elapses
            Expect(() => ItemManager.Instance.Get(mItemId) != null, (int)mRollTimeout - 1000);
        }

        public override void Process()
        {
            // If our expectation elapsed
            if (ExpectationHasElapsed)
            {
                RollPass();
                PlayerAI.CompleteActivity();
                return;
            }

            // Check if we have our base item yet
            var item = ItemManager.Instance.Get(mItemId);
            if (item != null)
            {
                bool isUseful = PlayerAI.Player.IsItemUseful(item);
                // If it's useful and it is a weapon or armor, check if it's an upgrade
                if (isUseful && (item.ItemClass == Constants.ItemClass.ITEM_CLASS_WEAPON || item.ItemClass == Constants.ItemClass.ITEM_CLASS_ARMOR))
                {
                    if (PlayerAI.Player.IsItemAnUpgrade(item))
                    {
                        RollNeed();
                        PlayerAI.CompleteActivity();
                        return;
                    }

                    // Was not an upgrade, roll greed for it
                    RollGreed();
                    PlayerAI.CompleteActivity();
                    return;
                }

                // Roll need for useful items that are not armor or weapons
                if (isUseful)
                {
                    RollNeed();
                    PlayerAI.CompleteActivity();
                    return;
                }

                // Roll greed for anything else
                RollGreed();
                PlayerAI.CompleteActivity();
                return;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Rolls need if we can, otherwise rolls next best (greed and then pass)
        /// </summary>
        private void RollNeed()
        {
            RollVoteMask mask = (RollVoteMask)mRollOptions;
            if (mask.HasFlag(RollVoteMask.ROLL_VOTE_MASK_NEED))
                PlayerAI.Client.RollForItem(mLootSourceGuid, mItemSlot, RollVote.ROLL_NEED);
            else
                RollGreed();
        }

        /// <summary>
        /// Rolls greed if we can, otherwise rolls next best (pass)
        /// </summary>
        private void RollGreed()
        {
            // TODO: If we are an enchanter, roll disenchant here
            RollVoteMask mask = (RollVoteMask)mRollOptions;
            if (mask.HasFlag(RollVoteMask.ROLL_VOTE_MASK_GREED))
                PlayerAI.Client.RollForItem(mLootSourceGuid, mItemSlot, RollVote.ROLL_GREED);
            else
                RollGreed();
        }

        /// <summary>
        /// Rolls pass for an item
        /// </summary>
        private void RollPass()
        {
            RollVoteMask mask = (RollVoteMask)mRollOptions;
            if (mask.HasFlag(RollVoteMask.ROLL_VOTE_MASK_DISENCHANT))
                PlayerAI.Client.RollForItem(mLootSourceGuid, mItemSlot, RollVote.ROLL_DISENCHANT);
            else
                PlayerAI.Client.RollForItem(mLootSourceGuid, mItemSlot, RollVote.ROLL_PASS);
        }

        #endregion
    }
}
