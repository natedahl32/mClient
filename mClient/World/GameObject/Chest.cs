using mClient.Constants;

namespace mClient.World.GameObject
{
    public class Chest : GameObjectInfo
    {
        public Chest(uint id, GameObjectType type, string name, int[] data) : base(id, type, name, data)
        {
        }

        /// <summary>
        /// Gets the lock id for this chest
        /// </summary>
        public uint LockId
        {
            get { return (uint)Data[0]; }
        }
    }
}
