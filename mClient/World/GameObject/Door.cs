using mClient.Constants;

namespace mClient.World.GameObject
{
    public class Door : GameObjectInfo
    {
        public Door(uint id, GameObjectType type, string name, int[] data) : base(id, type, name, data)
        {
        }

        #region Properties

        /// <summary>
        /// used client side to determine GO_ACTIVATED means open/closed
        /// </summary>
        public uint StartOpen { get { return (uint)Data[0]; } }

        /// <summary>
        /// Id used in Lock.dbc
        /// </summary>
        public uint LockId { get { return (uint)Data[1]; } }

        /// <summary>
        /// secs till autoclose = autoCloseTime / 0x10000
        /// </summary>
        public uint AutoCloseItem { get { return (uint)Data[2]; } }

        /// <summary>
        /// Does opening get interrupted when you receive damage
        /// </summary>
        public uint NoDamageImmune { get { return (uint)Data[3]; } }

        /// <summary>
        /// Text displayed when opening the item
        /// </summary>
        public uint OpenTextID { get { return (uint)Data[4]; } }

        /// <summary>
        /// Text displayed when closing the item
        /// </summary>
        public uint CloseTextID { get { return (uint)Data[4]; } }

        #endregion
    }
}
