using mClient.Constants;
using mClient.Shared;
using mClient.World.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Quest
{
    /// <summary>
    /// Quest template information received from the server
    /// </summary>
    public class QuestInfo
    {
        #region Constructors

        public QuestInfo()
        {
            QuestObjectives = new List<QuestObjective>();
        }

        #endregion

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

        /// <summary>
        /// Gets or sets all objectives for this quest
        /// </summary>
        public List<QuestObjective> QuestObjectives { get; set; }

        /// <summary>
        /// Gets or sets the source item id
        /// </summary>
        public uint SourceItemId { get; set; }

        /// <summary>
        /// Gets the source item for the quest
        /// </summary>
        public ItemInfo SourceItem
        {
            get
            {
                if (SourceItemId > 0)
                    return ItemManager.Instance.Get(SourceItemId);
                return null;
            }
        }

        #endregion

        #region Public Methods

        public string DumpInfo()
        {
            var dump = string.Empty;
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                if (descriptor.PropertyType != typeof(List<QuestObjective>) &&
                    descriptor.PropertyType != typeof(ItemInfo))
                {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(this);
                    dump += string.Format("{0}: {1} {2}", name, value, Environment.NewLine);
                }
            }

            // Now dump all quest objectives and other arrays/lists
            var i = 1;
            foreach (var q in QuestObjectives)
            {
                dump += string.Format("Quest Objective {0}: {1}", i, Environment.NewLine);
                dump += string.Format("  Required Creature or GO Id: {0} {1}", q.RequiredCreatureOrGameObjectId, Environment.NewLine);
                dump += string.Format("  Required Creature or GO Count: {0} {1}", q.RequiredCreatureOrGameObjectCount, Environment.NewLine);
                dump += string.Format("  Required Item Id: {0} {1}", q.RequiredItemId, Environment.NewLine);
                dump += string.Format("  Required Item Count: {0} {1}", q.RequiredItemCount, Environment.NewLine);
                i++;
            }

            return dump;
        }

        #endregion
    }
}
