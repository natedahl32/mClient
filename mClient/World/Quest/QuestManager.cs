using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Quest
{
    public class QuestManager
    {
        #region Declarations

        private List<QuestInfo> mQuests = new List<QuestInfo>();
        private Object mLock = new Object();

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets the instance of quest manager
        /// </summary>
        public static QuestManager Instance
        {
            get { return Singleton<QuestManager>.Instance; }
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Gets quest info based on the id of the quest
        /// </summary>
        /// <param name="questId"></param>
        /// <returns></returns>
        public QuestInfo GetQuest(UInt32 questId)
        {            
            return mQuests.Where(q => q.QuestId == questId).SingleOrDefault();
        }

        /// <summary>
        /// Gets a quest info by title
        /// </summary>
        /// <param name="questTitle"></param>
        /// <returns></returns>
        public QuestInfo GetQuest(string questTitle)
        {
            if (string.IsNullOrEmpty(questTitle)) return null;
            return mQuests.Where(q => q.QuestName.ToLower() == questTitle.ToLower()).SingleOrDefault();
        }

        /// <summary>
        /// Adds a quest to the manager
        /// </summary>
        /// <param name="questInfo"></param>
        public void AddQuest(QuestInfo questInfo)
        {
            if (!mQuests.Any(q => q.QuestId == questInfo.QuestId))
                lock(mLock)
                    mQuests.Add(questInfo);
        }

        /// <summary>
        /// Adds or updates a quest in the manager
        /// </summary>
        /// <param name="questInfo"></param>
        public void AddOrUpdateQuest(QuestInfo questInfo)
        {
            if (questInfo == null) return;
            var existing = mQuests.Where(q => q.QuestId == questInfo.QuestId).SingleOrDefault();
            if (existing == null)
            {
                lock(mLock)
                    mQuests.Add(questInfo);
                return;
            }

            // Already exists, remove it and then re-add it
            lock(mLock)
            {
                mQuests.Remove(existing);
                mQuests.Add(questInfo);
            }
        }

        #endregion
    }
}
