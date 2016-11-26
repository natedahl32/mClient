using mClient.Constants;
using mClient.Shared;

namespace mClient.Clients
{
    public class Corpse : Object
    {
        public Corpse(WoWGuid guid) : base(guid)
        {
        }

        #region Properties

        /// <summary>
        /// Gets the owner guid of the corpse
        /// </summary>
        public WoWGuid OwnerGuid
        {
            get { return GetWoWGuid(GetFieldValue((int)CorpseFields.CORPSE_FIELD_OWNER), GetFieldValue((int)CorpseFields.CORPSE_FIELD_OWNER + 1)); }
        }

        #endregion
    }
}
