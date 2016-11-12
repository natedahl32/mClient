using mClient.Constants;
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

        /// <summary>
        /// Gets or sets whether or not this game object has been looted (if it is lootable)
        /// </summary>
        public bool HasBeenLooted { get; set; }

        /// <summary>
        /// Gets or sets whether or not we can interact with this game object
        /// </summary>
        public bool CanInteract
        {
            get
            {
                var value = (GameObjectFlags)GetFieldValue((int)GameObjectFields.GAMEOBJECT_FLAGS);
                // If the value has the NO_INTERACT flag we cannot interact
                if (value.HasFlag(GameObjectFlags.GO_FLAG_NO_INTERACT) || value.HasFlag(GameObjectFlags.GO_FLAG_INTERACT_COND))
                    return false;

                // Questgivers and chests have a dynamic flag set that determines if we can interact
                if (BaseInfo.GameObjectType == GameObjectType.Chest || BaseInfo.GameObjectType == GameObjectType.QuestGiver)
                {
                    var dynValue = (GameObjectDynamicLowFlags)GetFieldValue((int)GameObjectFields.GAMEOBJECT_DYN_FLAGS);
                    return dynValue.HasFlag(GameObjectDynamicLowFlags.GO_DYNFLAG_LO_ACTIVATE) || dynValue.HasFlag(GameObjectDynamicLowFlags.GO_DYNFLAG_LO_SPARKLE);
                }
                // Goobers also have the dynamic flag set but just use the activate flag, not the sparkle flag
                if (BaseInfo.GameObjectType == GameObjectType.Goober)
                {
                    var dynValue = (GameObjectDynamicLowFlags)GetFieldValue((int)GameObjectFields.GAMEOBJECT_DYN_FLAGS);
                    return dynValue.HasFlag(GameObjectDynamicLowFlags.GO_DYNFLAG_LO_ACTIVATE);
                }

                // No non-interact flag, so we assume we can interact
                return true;
            }
        }

        /// <summary>
        /// Gets whether or not the game object is locked
        /// </summary>
        public bool IsLocked
        {
            get
            {
                var value = (GameObjectFlags)GetFieldValue((int)GameObjectFields.GAMEOBJECT_FLAGS);
                return value.HasFlag(GameObjectFlags.GO_FLAG_LOCKED);
            }
        }

        /// <summary>
        /// Gets whether or not the game object is triggered
        /// </summary>
        public bool IsTriggered
        {
            get
            {
                var value = (GameObjectFlags)GetFieldValue((int)GameObjectFields.GAMEOBJECT_FLAGS);
                return value.HasFlag(GameObjectFlags.GO_FLAG_TRIGGERED);
            }
        }

        /// <summary>
        /// Gets whether or not the game object is currently in use
        /// </summary>
        public bool IsInUse
        {
            get
            {
                var value = (GameObjectFlags)GetFieldValue((int)GameObjectFields.GAMEOBJECT_FLAGS);
                return value.HasFlag(GameObjectFlags.GO_FLAG_IN_USE);
            }
        }

        #endregion
    }
}
