using mClient.Constants;
using mClient.Shared;
using System.Collections.Generic;

namespace mClient.DBC
{
    public class LockTable : DBCFile
    {
        private Dictionary<uint, LockEntry> mLockEntries = new Dictionary<uint, LockEntry>();

        #region Singleton

        static readonly LockTable instance = new LockTable();

        static LockTable() { }

        LockTable() : base(@"Lock.dbc")
        { }

        public static LockTable Instance { get { return instance; } }

        #endregion

        protected override void dataLoaded()
        {
            for (uint i = 0; i < Records; i++)
            {
                var entry = new LockEntry();
                entry.ID = getFieldAsUint32(i, 0);

                entry.Type = new LockKeyType[LockEntry.MAX_LOCK_CASE];
                entry.Type[0] = (LockKeyType)getFieldAsUint32(i, 1);
                entry.Type[1] = (LockKeyType)getFieldAsUint32(i, 2);
                entry.Type[2] = (LockKeyType)getFieldAsUint32(i, 3);
                entry.Type[3] = (LockKeyType)getFieldAsUint32(i, 4);
                entry.Type[4] = (LockKeyType)getFieldAsUint32(i, 5);
                entry.Type[5] = (LockKeyType)getFieldAsUint32(i, 6);
                entry.Type[6] = (LockKeyType)getFieldAsUint32(i, 7);
                entry.Type[7] = (LockKeyType)getFieldAsUint32(i, 8);

                entry.LockTypeIndex = new uint[LockEntry.MAX_LOCK_CASE];
                entry.LockTypeIndex[0] = getFieldAsUint32(i, 9);
                entry.LockTypeIndex[1] = getFieldAsUint32(i, 10);
                entry.LockTypeIndex[2] = getFieldAsUint32(i, 11);
                entry.LockTypeIndex[3] = getFieldAsUint32(i, 12);
                entry.LockTypeIndex[4] = getFieldAsUint32(i, 13);
                entry.LockTypeIndex[5] = getFieldAsUint32(i, 14);
                entry.LockTypeIndex[6] = getFieldAsUint32(i, 15);
                entry.LockTypeIndex[7] = getFieldAsUint32(i, 16);

                entry.RequiredSkillValue = new uint[LockEntry.MAX_LOCK_CASE];
                entry.RequiredSkillValue[0] = getFieldAsUint32(i, 17);
                entry.RequiredSkillValue[1] = getFieldAsUint32(i, 18);
                entry.RequiredSkillValue[2] = getFieldAsUint32(i, 19);
                entry.RequiredSkillValue[3] = getFieldAsUint32(i, 20);
                entry.RequiredSkillValue[4] = getFieldAsUint32(i, 21);
                entry.RequiredSkillValue[5] = getFieldAsUint32(i, 22);
                entry.RequiredSkillValue[6] = getFieldAsUint32(i, 23);
                entry.RequiredSkillValue[7] = getFieldAsUint32(i, 24);

                mLockEntries.Add(entry.ID, entry);
            }
        }

        public LockEntry getById(uint lockId)
        {
            if (mLockEntries.ContainsKey(lockId))
                return mLockEntries[lockId];
            return null;
        }
    }
}
