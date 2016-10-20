using mClient.Shared;
using System.Collections.Generic;
using System.Linq;

namespace mClient.World.Quest
{
    public class QuestManager : AbstractObjectManager<QuestInfo>
    {
        #region Singleton

        static readonly QuestManager instance = new QuestManager();

        static QuestManager() { }

        QuestManager()
        { }

        public static QuestManager Instance { get { return instance; } }

        #endregion

        #region Properties

        protected override string SerializeToFile
        {
            get
            {
                return "quests.json";
            }
        }

        #endregion

        #region Public Methods

        public override QuestInfo Get(uint id)
        {
            return mObjects.Where(q => q != null && q.QuestId == id).SingleOrDefault();
        }

        public override bool Exists(QuestInfo obj)
        {
            if (obj == null) return false;
            return mObjects.Any(q => q.QuestId == obj.QuestId);
        }

        /// <summary>
        /// Gets a quest info by title
        /// </summary>
        /// <param name="questTitle"></param>
        /// <returns></returns>
        public QuestInfo GetQuest(string questTitle)
        {
            if (string.IsNullOrEmpty(questTitle)) return null;
            return mObjects.Where(q => q.QuestName.ToLower() == questTitle.ToLower()).SingleOrDefault();
        }

        #endregion
    }
}
