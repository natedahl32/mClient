using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.BotServer.Views
{
    public class BotAccountView
    {
        #region Declarations

        private readonly string mClientId;
        private bool mIsConnected = false;
        private bool mIsLoggedIn = false;

        #endregion

        #region Constructors

        public BotAccountView(BotAccount account)
        {
            mClientId = account.ClientId.ToString();
            mIsConnected = account.IsConnected;
            mIsLoggedIn = account.IsLoggedIn;
            AccountName = account.AccountName;
            CharacterName = account.CharacterName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current client id of the bot account
        /// </summary>
        public string ClientId
        {
            get { return mClientId; }
        }

        /// <summary>
        /// Gets the connection status of the bot to the world server
        /// </summary>
        public string ConnectionStatus
        {
            get
            {
                if (!mIsConnected) return "Not Connected";
                if (mIsConnected && !mIsLoggedIn) return "Connected";
                if (mIsConnected && mIsLoggedIn) return "Logged In";
                return "Uknown";
            }
        }

        /// <summary>
        /// Gets or sets the bots account name
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the characters name for this account
        /// </summary>
        public string CharacterName { get; set; }

        /// <summary>
        /// Gets or sets the class of the character
        /// </summary>
        public string CharacterClass { get; set; }

        /// <summary>
        /// Gets or sets the race of the character
        /// </summary>
        public string CharacterRace { get; set; }

        /// <summary>
        /// Gets or sets the level of the character
        /// </summary>
        public int CharacterLevel { get; set; }

        #endregion
    }
}
