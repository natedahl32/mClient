using mClient.Constants;
using System;

namespace mClient.World.Quest
{
    /// <summary>
    /// Quest giver data received from the server
    /// </summary>
    public class QuestGiver
    {
        #region Properties

        /// <summary>
        /// Gets or sets guid of the quest giver
        /// </summary>
        public UInt64 Guid { get; set; }

        /// <summary>
        /// Gets or sets the status of the quest giver
        /// </summary>
        public QuestGiverStatus Status { get; set; }

        #endregion
    }
}
