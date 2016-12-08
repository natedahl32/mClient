using mClient.World.Talents;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace mClient.World
{
    /// <summary>
    /// This class contains settings that pertain to a specific player. These settings are saved each time a character disconnects and loaded when a player
    /// logs in so they persist with the player. It is serialized to json when the character logs our or disconnects.
    /// </summary>
    public class PlayerSettings
    {
        #region Declarations

        private string mPlayerName;
        private object mWriteLock = new object();

        // Member variables
        private int mSpecId;
        private List<uint> mIgnoredQuests = new List<uint>();

        #endregion

        #region Constructors

        // Used by serializer
        public PlayerSettings() { }

        public PlayerSettings(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            mPlayerName = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the id of the spec that is assigned to this player
        /// </summary>
        public int SpecId
        {
            get { return mSpecId; }
            set
            {
                mSpecId = value;
                Serialize();
            }
        }

        /// <summary>
        /// Gets or sets quests that are ignored by the player
        /// </summary>
        public List<uint> IgnoredQuests
        {
            get { return mIgnoredQuests; }
            set { mIgnoredQuests = value; }
        }

        /// <summary>
        /// Gets the spec information that is assigned to this player. Null if the player has not been assigned a spec yet.
        /// </summary>
        [JsonIgnore]
        public Spec Spec
        {
            get
            {
                return SpecManager.Instance.Get((uint)SpecId);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a quest to the ignored list
        /// </summary>
        /// <param name="questId"></param>
        public void AddIgnoredQuest(uint questId)
        {
            if (!mIgnoredQuests.Contains(questId))
            {
                mIgnoredQuests.Add(questId);
                Serialize();
            }
        }

        /// <summary>
        /// Serializes quests to file
        /// </summary>
        public void Serialize()
        {
            // if we don't have player name yet don't serialize (might occur when deserializing)
            if (string.IsNullOrEmpty(mPlayerName)) return;

            // Lock and start a new thread for the write
            lock (mWriteLock)
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;

                var filePath = @"data\players\" + mPlayerName.ToLower() + ".json";
                using (StreamWriter sw = new StreamWriter(filePath))
                using (JsonWriter writer = new JsonTextWriter(sw))
                    serializer.Serialize(writer, this);
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Loads existing player settings for a player
        /// </summary>
        public static PlayerSettings Load(string playerName)
        {
            if (string.IsNullOrEmpty(playerName)) throw new ArgumentNullException("playerName");
            if (!Directory.Exists("data")) Directory.CreateDirectory("data");
            if (!Directory.Exists(@"data\players")) Directory.CreateDirectory(@"data\players");
            var filePath = @"data\players\" + playerName.ToLower() + ".json";
            if (!File.Exists(filePath)) return new PlayerSettings(playerName);

            var data = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(data.Trim())) return new PlayerSettings(playerName);
            var settings = JsonConvert.DeserializeObject<PlayerSettings>(data);
            settings.mPlayerName = playerName;
            return settings;
        }

        #endregion
    }
}
