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

        /// <summary>
        /// Gets the position of the corpse
        /// </summary>
        public override Coordinate Position
        {
            get
            {
                var x = GetFieldValue((int)CorpseFields.CORPSE_FIELD_POS_X);
                var y = GetFieldValue((int)CorpseFields.CORPSE_FIELD_POS_Y);
                var z = GetFieldValue((int)CorpseFields.CORPSE_FIELD_POS_Z);
                return new Coordinate(x, y, z);
            }
        }

        #endregion
    }
}
