using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class QuestGiverRequestItemsMessage : ActivityMessage
    {
        #region Constructors

        protected QuestGiverRequestItemsMessage(WorldServerOpCode messageType) : base(messageType)
        {
        }

        public QuestGiverRequestItemsMessage() : this(WorldServerOpCode.SMSG_QUESTGIVER_REQUEST_ITEMS)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the NPC Id
        /// </summary>
        public UInt64 NpcId { get; set; }

        /// <summary>
        /// Gets or sets the quest id
        /// </summary>
        public uint QuestId { get; set; }

        /// <summary>
        /// Gets or sets whether or not the quest is completable
        /// </summary>
        public bool IsCompletable { get; set; }

        #endregion
    }
}
