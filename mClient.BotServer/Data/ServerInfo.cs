using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.BotServer.Data
{
    public class ServerInfo
    {
        #region Singleton

        static readonly ServerInfo instance = new ServerInfo();

        static ServerInfo() { }

        ServerInfo()
        { }

        public static ServerInfo Instance { get { return instance; } }

        #endregion

        #region Declarations

        private string mServerHost = "localhost";
        private int mPort = 3724;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the host of the server, usually the IP address
        /// </summary>
        public string Host
        {
            get { return mServerHost; }
            set { mServerHost = value; }
        }

        /// <summary>
        /// Gets or sets the port of the server
        /// </summary>
        public int Port
        {
            get { return mPort; }
            set { mPort = value; }
        }

        /// <summary>
        /// Gets the file path the information in this class is saved to
        /// </summary>
        private string FilePath { get { return @"serverinfo.json"; } }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serializes quests to file
        /// </summary>
        public void Serialize()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(FilePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
                serializer.Serialize(writer, this);
        }

        /// <summary>
        /// Loads existing quests saved to file
        /// </summary>
        public void Load()
        {
            if (!File.Exists(FilePath)) return;

            var data = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(data.Trim())) return;
            var info = JsonConvert.DeserializeObject<ServerInfo>(data);

            this.Host = info.Host;
            this.Port = info.Port;
        }

        #endregion
    }
}
