using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Linq;

using mClient.Constants;
using mClient.Terrain;
using mClient.Shared;

namespace mClient.Clients
{
    /// <summary>
    /// Keeps track of the world. 
    /// Keeps track of all Objects, including the player as well as providing methods to move (or warp) the player and provides collision detection and pathing.
    /// </summary>
    public class ObjectMgr
    {
        public UInt32 MapID;
        public WoWGuid playerGuid;

        private System.Object mObjectsLock = new System.Object();
        private List<Object> mObjects;

        public ObjectMgr()
        {
            mObjects = new List<Object>();
        }

        public Object getPlayerObject()
        {
            int index = getObjectIndex(playerGuid);
            if (index == -1)
            {
                Object obj = Object.CreateObjectByType(playerGuid, ObjectType.Player);
                lock (mObjectsLock)
                    addObject(obj);

                return obj;
                
            }
            else
                return mObjects[index];
        }

        public IList<Unit> GetNpcUnits()
        {
            List<Object> copy;
            // avoid collection was modified errors by making a copy of the list
            lock (mObjectsLock)
                copy = mObjects.ToList();
            return copy.Where(o => (o as Unit) != null && (o as Unit).IsNPC).Cast<Unit>().ToList();    
        }

        public void addObject(Object obj)
        {
            //Log.WriteLine(LogType.Debug, "Object created: {0}", obj.Guid.GetOldGuid());
            int index = getObjectIndex(obj.Guid);
            if (index != -1)
            {
                updateObject(obj);
            }
            else
            {
                lock(mObjectsLock)
                    mObjects.Add(obj);
            }
        }

        public void updateObject(Object obj)
        {
            //Log.WriteLine(LogType.Debug, "Object updated: {0}", obj.Guid.GetOldGuid());
            int index = getObjectIndex(obj.Guid);
            if (index != -1)
            {
                Object[] test;

                lock (mObjectsLock)
                {
                    mObjects[index] = obj;
                    test = new Object[1];
                    test[0] = obj;
                }
            }
            else
            {
                addObject(obj);
            }
        }

        public void delObject(WoWGuid guid)
        {
            int index = getObjectIndex(guid);
            if (index != -1)
            {
                lock(mObjectsLock)
                    mObjects.RemoveAt(index);
            }
        }

        public Object getObject(string name)
        {
            int index = getObjectIndex(name);
            if (index == -1)
            {
                return null;
            }
            else
                return mObjects[index];
        }

        public Object getObject(WoWGuid guid)
        {
            int index = getObjectIndex(guid);
            if (index == -1)
            {
                return null;
            }
            else
                return mObjects[index];

        }

        public Object getNearestObject(Object obj)
        {
            Object[] list = getObjectArray();
            Object closest = null;
            float dist;
            float mindist = 9999999999;

            if (list.Length < 1)
            {
                return null;
            }

            foreach (Object obj2 in list)
            {
                dist = TerrainMgr.CalculateDistance(obj.Position, obj2.Position);
                if (dist < mindist)
                {
                    mindist = dist;
                    closest = obj2;
                }
            }

            return closest;
        }

        public Object getNearestObject()
        {
            Object[] list = getObjectArray();
            Object closest = null;
            float dist;
            float mindist = 9999999999;

            if (list.Length < 1)
            {
                return null;
            }

            foreach (Object obj2 in list)
            {
                if (obj2.Guid.GetOldGuid() != playerGuid.GetOldGuid())
                {
                    dist = TerrainMgr.CalculateDistance(getPlayerObject().Position, obj2.Position);
                    if (dist < mindist)
                    {
                        mindist = dist;
                        closest = obj2;
                    }
                }
            }

            return closest;
        }

        public ObjectType getObjectType(WoWGuid guid)
        {
            int index = getObjectIndex(guid);
            if (index != -1)
            {
                return mObjects[index].Type;
            }
            else
                return new ObjectType();
        }

        public bool objectExists(WoWGuid guid)
        {

            int index = getObjectIndex(guid);
            if (index == -1)
            {
                return false;
            }
            else
                return true;
        }

        private int getObjectIndex(WoWGuid guid)
        {
            int index = mObjects.FindIndex(s => s.Guid.GetOldGuid() == guid.GetOldGuid());
            return index;
        }

        private int getObjectIndex(string name)
        {
            int index = mObjects.FindIndex(s => s.Name == name);
            return index;
        }

        public Object[] getObjectArray()
        {
            return mObjects.ToArray();
        }

        #region Specific Objects

        /// <summary>
        /// Gets all containers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Clients.Container> GetAllContainers()
        {
            return getObjectArray().Where(o => o != null && (o as Clients.Container) != null).Cast<Clients.Container>();
        }

        /// <summary>
        /// Gets all corpses
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Clients.Corpse> GetAllCorpses()
        {
            return getObjectArray().Where(o => o != null && (o as Clients.Corpse) != null).Cast<Clients.Corpse>();
        }

        /// <summary>
        /// Gets all dynamic objects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Clients.DynamicObject> GetAllDynamicObjects()
        {
            return getObjectArray().Where(o => o != null && (o as Clients.DynamicObject) != null).Cast<Clients.DynamicObject>();
        }

        /// <summary>
        /// Gets all items
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Clients.Item> GetAllItems()
        {
            return getObjectArray().Where(o => o != null && (o as Clients.Item) != null).Cast<Clients.Item>();
        }

        /// <summary>
        /// Gets all game objects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Clients.GameObject> GetAllGameObjects()
        {
            return getObjectArray().Where(o => o != null && (o as Clients.GameObject) != null).Cast<Clients.GameObject>();
        }

        /// <summary>
        /// Gets all units
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Clients.Unit> GetAllUnits()
        {
            return getObjectArray().Where(o => o != null && (o as Clients.Unit) != null).Cast<Clients.Unit>();
        }

        /// <summary>
        /// Gets all players
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Clients.PlayerObj> GetAllPlayers()
        {
            return getObjectArray().Where(o => o != null && (o as Clients.PlayerObj) != null).Cast<Clients.PlayerObj>();
        }

        #endregion
    }

}