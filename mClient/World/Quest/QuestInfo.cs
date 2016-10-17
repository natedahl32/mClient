using mClient.Constants;
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Quest
{
    public class QuestInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets the id of the quest
        /// </summary>
        public UInt32 QuestId { get; set; }

        /// <summary>
        /// Gets or sets the name of the quest
        /// </summary>
        public string QuestName { get; set; }

        /// <summary>
        /// Gets or sets quest level
        /// </summary>
        public UInt32 QuestLevel { get; set; }

        /// <summary>
        /// Gets or sets the type of quest
        /// </summary>
        public QuestTypes QuestType { get; set; }

        /// <summary>
        /// Gets or sets the id of the next quest in the chain
        /// </summary>
        public UInt32 NextQuestInChain { get; set; }

        /// <summary>
        /// Gets or sets the quest flags
        /// </summary>
        public QuestFlags QuestFlags { get; set; }

        /// <summary>
        /// Gets or sets the map id the quest point is in
        /// </summary>
        public UInt32 QuestPointMapId { get; set; }

        /// <summary>
        /// Gets or sets the point of the quest on the map
        /// </summary>
        public Coords3 QuestPoint { get; set; }

        #endregion
    }
}
