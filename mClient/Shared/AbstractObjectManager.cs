using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Shared
{
    public abstract class AbstractObjectManager<T>
    {
        #region Declarations

        private const int ITEMS_ADDED_TO_SERIALIZE = 1;

        protected List<T> mObjects = new List<T>();
        protected Object mLock = new Object();
        private Object mWriteLock = new Object();
        private int addedSinceSerialize = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the file this manager serializes it's objects to
        /// </summary>
        protected abstract string SerializeToFile { get; }

        /// <summary>
        /// Gets the entire file path we serialize to
        /// </summary>
        private string FilePath { get { return @"data\" + SerializeToFile; } }

        /// <summary>
        /// Default serializer implementation
        /// </summary>
        protected virtual JsonSerializer Serializer
        {
            get
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                return serializer;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serializes quests to file
        /// </summary>
        public void Serialize()
        {
            // if there is nothing to save we don't need to serialize
            if (addedSinceSerialize == 0) return;

            Task task = Task.Factory.StartNew(() =>
            {
                // Lock and start a new thread for the write
                lock (mLock)
                lock (mWriteLock)
                {
                    JsonSerializer serializer = Serializer;

                    using (StreamWriter sw = new StreamWriter(FilePath))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                        serializer.Serialize(writer, mObjects);

                    // reset back to 0
                    addedSinceSerialize = 0;
                }
            });
        }

        /// <summary>
        /// Loads existing quests saved to file
        /// </summary>
        public void Load()
        {
            if (!Directory.Exists("data")) Directory.CreateDirectory("data");
            if (!File.Exists(FilePath)) return;

            var data = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(data.Trim())) return;
            mObjects = JsonConvert.DeserializeObject<List<T>>(data);
        }

        /// <summary>
        /// Gets object info based on the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract T Get(UInt32 id);

        /// <summary>
        /// Gets whether or not the object exists in the manager
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract bool Exists(T obj);

        /// <summary>
        /// Adds an object to the manager
        /// </summary>
        /// <param name="obj"></param>
        public void Add(T obj)
        {
            if (!Exists(obj))
            {
                lock (mLock)
                {
                    mObjects.Add(obj);
                    addedSinceSerialize++;
                }

                if (addedSinceSerialize >= ITEMS_ADDED_TO_SERIALIZE)
                    Serialize();
            }
        }

        #endregion
    }
}
