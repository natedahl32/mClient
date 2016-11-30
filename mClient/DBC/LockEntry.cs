using mClient.Constants;

namespace mClient.DBC
{
    public class LockEntry
    {
        #region Declarations

        public const int MAX_LOCK_CASE = 8;

        #endregion

        #region Properties

        public uint ID { get; set; }

        public LockKeyType[] Type { get; set; }

        public uint[] LockTypeIndex { get; set; }

        public uint[] RequiredSkillValue { get; set; }

        #endregion
    }
}
