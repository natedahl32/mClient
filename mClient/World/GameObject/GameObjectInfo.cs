using System;
using System.Collections.Generic;

namespace mClient.World.GameObject
{
    public class GameObjectInfo
    {
        #region Declarations

        public const int MAX_GAMEOBJECT_DATA_COUNT = 24;

        #endregion

        #region Constructors

        public GameObjectInfo()
        {
            Data = new List<uint>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the id of the game object
        /// </summary>
        public UInt32 GameObjectId { get; set; }


        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public UInt32 GameObjectType { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the game object data
        /// </summary>
        public List<UInt32> Data { get; set; }

        #endregion
    }
}
