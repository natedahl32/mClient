using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.GameObject
{
    public class GameObjectManager : AbstractObjectManager<GameObjectInfo>
    {
        #region Singleton

        static readonly GameObjectManager instance = new GameObjectManager();

        static GameObjectManager() { }

        GameObjectManager()
        { }

        public static GameObjectManager Instance { get { return instance; } }

        #endregion

        #region Properties

        protected override string SerializeToFile
        {
            get
            {
                return "gameobjects.json";
            }
        }

        #endregion

        #region Public Methods

        public override bool Exists(GameObjectInfo obj)
        {
            if (obj == null) return false;
            return mObjects.Any(i => i.GameObjectId == obj.GameObjectId);
        }

        public bool Exists(UInt32 gameObjectId)
        {
            return mObjects.Any(i => i.GameObjectId == gameObjectId);
        }

        public override GameObjectInfo Get(uint id)
        {
            return mObjects.Where(i => i != null && i.GameObjectId == id).SingleOrDefault();
        }

        #endregion
    }
}
