﻿using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        protected override JsonSerializer Serializer
        {
            get
            {
                var serializer = base.Serializer;
                serializer.TypeNameHandling = TypeNameHandling.All;
                return serializer;
            }
        }

        #endregion

        #region Public Methods

        public override bool Exists(GameObjectInfo obj)
        {
            if (obj == null) return false;
            lock (mLock)
                return mObjects.Any(i => i.GameObjectId == obj.GameObjectId);
        }

        public bool Exists(UInt32 gameObjectId)
        {
            lock (mLock)
                return mObjects.Any(i => i.GameObjectId == gameObjectId);
        }

        public override GameObjectInfo Get(uint id)
        {
            lock (mLock)
                return mObjects.Where(i => i != null && i.GameObjectId == id).SingleOrDefault();
        }

        public T GetSpecific<T>(uint id) 
            where T : GameObjectInfo
        {
            lock (mLock)
            {
                var obj = mObjects.Where(i => i != null && i.GameObjectId == id).SingleOrDefault();
                if (obj != null)
                    return obj as T;
            }
            return null;
        }

        #endregion

        #region Private Methods

        protected override void ObjectsLoaded()
        {
            // We need to recast our items to their appropriate types
            lock (mLock)
            {
                var GOs = mObjects.ToList();
                mObjects.Clear();

                // Loop through all the GameObjects loaded from file and recast them to their appropriate types
                foreach (var go in GOs)
                    mObjects.Add(GameObjectInfo.Create(go.GameObjectId, go.GameObjectType, go.Name, go.Data));
            }
        }

        #endregion
    }
}
