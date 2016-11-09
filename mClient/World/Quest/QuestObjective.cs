using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Quest
{
    public class QuestObjective
    {
        #region Properties

        /// <summary>
        /// Gets or sets the id of the creature or game object required for this objective
        /// </summary>
        public UInt32 RequiredCreatureOrGameObjectId { get; set; }

        /// <summary>
        /// Gets or sets the count of creatures or game objects required for this objective
        /// </summary>
        public UInt32 RequiredCreatureOrGameObjectCount { get; set; }

        /// <summary>
        /// Gets or sets the id of the item required for this objective
        /// </summary>
        public UInt32 RequiredItemId { get; set; }

        /// <summary>
        /// Gets or sets the count of the required items for this objective
        /// </summary>
        public UInt32 RequiredItemCount { get; set; }

        #endregion
    }
}
