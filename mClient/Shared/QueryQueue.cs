using mClient.Constants;
using PObject = mClient.Clients.Object;
using System;
using System.Collections.Generic;

namespace mClient.Shared
{
    public class QueryQueue
    {
        #region Declarations

        // Holds on to callbacks for this query
        private List<Action<PObject>> mCallbacks = new List<Action<PObject>>();

        #endregion

        #region Constructors

        public QueryQueue(QueryQueueType type, UInt64 guid)
        {
            this.QueryType = type;
            this.Guid = guid;
        }

        public QueryQueue(QueryQueueType type, UInt64 guid, UInt32 entry) : this(type, guid)
        {
            this.QueryType = type;
            this.Guid = guid;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of query
        /// </summary>
        public QueryQueueType QueryType { get; private set; }

        /// <summary>
        /// Gets the Guid of the object to query
        /// </summary>
        public UInt64 Guid { get; private set; }

        /// <summary>
        /// Gets the Entry of the object to query
        /// </summary>
        public UInt32 Entry { get; private set; }

        /// <summary>
        /// Gets or sets any extra data that needs to be sent with the query
        /// </summary>
        public System.Object ExtraData { get; set; }

        /// <summary>
        /// Gets all callbacks for this query
        /// </summary>
        public IEnumerable<Action<PObject>> Callbacks { get { return mCallbacks; } }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a callback method for this query
        /// </summary>
        /// <param name="callback"></param>
        public void AddCallback(Action<PObject> callback)
        {
            this.mCallbacks.Add(callback);
        }

        #endregion
    }
}
