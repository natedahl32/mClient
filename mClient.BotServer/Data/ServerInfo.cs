using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotServer.Data
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

        #endregion

        #region Properties

        /// <summary>
        /// Gets the host of the server, usually the IP address
        /// </summary>
        public string Host
        {
            get { return mServerHost; }
            set { mServerHost = value; }
        }

        #endregion
    }
}
