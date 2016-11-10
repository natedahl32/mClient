using mClient.Shared;
using mClient.World.GameObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Clients
{
    public class GameObject : Object
    {
        public GameObject(WoWGuid guid) : base(guid)
        {
        }

        #region Properties

        /// <summary>
        /// Gets the base GameObjectInfo based on entry
        /// </summary>
        public GameObjectInfo BaseInfo
        {
            get { return GameObjectManager.Instance.Get(this.ObjectFieldEntry); }
        }

        #endregion
    }
}
