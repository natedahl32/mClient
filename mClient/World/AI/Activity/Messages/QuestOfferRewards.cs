using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class QuestOfferRewards : ActivityMessage
    {
        #region Constructors

        protected QuestOfferRewards(WorldServerOpCode messageType) : base(messageType)
        {
            this.RewardItems = new List<RewardItem>();
        }

        public QuestOfferRewards() : this(WorldServerOpCode.SMSG_QUESTGIVER_OFFER_REWARD)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Id of the quest offering rewards
        /// </summary>
        public uint QuestId { get; set; }

        /// <summary>
        /// Auto complete the quest?
        /// </summary>
        public bool AutoComplete { get; set; }

        /// <summary>
        /// List of items offered as a reward
        /// </summary>
        public IList<RewardItem> RewardItems { get; set; }

        #endregion

        #region Classes

        public class RewardItem
        {
            public UInt32 ItemId { get; set; }
            public UInt32 ItemCount { get; set; }
            public UInt32 ItemChoiceIndex { get; set; }
        }

        #endregion
    }
}
