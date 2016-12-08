using mClient.BotServer.Data;
using mClient.Clients;
using mClient.Constants;
using Newtonsoft.Json;

namespace mClient.BotServer
{
    public class BotAccount
    {
        #region Declarations

        private LogonServerClient mLogonClient;
        private WorldServerClient mWorldClient;

        // Flag that determines whether or not we are logged in to the game
        private bool mLoggedIn = false;

        #endregion

        #region Constructors

        [JsonConstructor]
        public BotAccount() { }

        /// <summary>
        /// Constructor used for existing accounts that have a character already created
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        public BotAccount(string accountName, string password)
        {
            this.AccountName = accountName;
            this.Password = password;

            mLogonClient = new LogonServerClient(ServerInfo.Instance.Host, this.AccountName, this.Password);
            mLogonClient.ReceivedRealmList += HandleRealmListEvent;
            mLogonClient.Disconnected += LoginDisconnectedHandler;
        }

        /// <summary>
        /// Constructor used for new bot accounts
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        public BotAccount(string accountName, string password, string characterName, Classname characterClass, Race characterRace) : this(accountName, password)
        {
            this.CharacterName = characterName;
            this.CharacterClass = characterClass;
            this.CharacterRace = characterRace;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current client id of the bot account
        /// </summary>
        [JsonIgnore]
        public System.Guid ClientId
        {
            get
            {
                if (mWorldClient != null && mWorldClient.Connected)
                    return mWorldClient.Id;
                if (mLogonClient != null)
                    return mLogonClient.Id;
                
                return System.Guid.Empty;
            }
        }

        /// <summary>
        /// Gets whether or not the bot account is connected to the server
        /// </summary>
        [JsonIgnore]
        public bool IsConnected
        {
            get
            {
                if (mWorldClient != null)
                    return mWorldClient.Connected;
                return mLogonClient.Connected;
            }
        }

        /// <summary>
        /// Gets whether or not the bot account is logged in to the game
        /// </summary>
        [JsonIgnore]
        public bool IsLoggedIn
        {
            get { return mLoggedIn; }
        }

        /// <summary>
        /// Gets or sets the bots account name
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the accounts password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the characters name for this account
        /// </summary>
        public string CharacterName { get; set; }

        /// <summary>
        /// Gets or sets the class of the character
        /// </summary>
        public Classname CharacterClass { get; set; }

        /// <summary>
        /// Gets or sets the race of the character
        /// </summary>
        public Race CharacterRace { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Logs in the bot account to the server
        /// </summary>
        public void Login()
        {
            // Don't login if we already are
            if (IsLoggedIn) return;

            // If we are not connected then connect now
            if (!IsConnected)
                mLogonClient.Connect();

            // Now we wait for the RealmList event
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles event receiving realm list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleRealmListEvent(object sender, RealmListEventArgs e)
        {
            if (e.Realms.Count == 0)
            {
                // TODO: What to do when no realms are available?
                return;
            }

            mLogonClient.HardDisconnect();
            mWorldClient = new WorldServerClient(Config.Login, e.Realms[0], mLogonClient.mKey);
            mWorldClient.ReceivedCharacterList += WorldClient_ReceivedCharacterList;
            mWorldClient.Disconnected += WorldClient_Disconnected;
            mWorldClient.LoggedIn += WorldClient_LoggedIn;
            mWorldClient.Connect();
        }

        /// <summary>
        /// Handles the bot logging in to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorldClient_LoggedIn(object sender, WorldServerClient.LoginEventArgs e)
        {
            mLoggedIn = true;
        }

        /// <summary>
        /// Handles disconnect from world server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorldClient_Disconnected(object sender, System.EventArgs e)
        {
            mLoggedIn = false;
            mWorldClient = null;
        }

        /// <summary>
        /// Handles receiving the character list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorldClient_ReceivedCharacterList(object sender, WorldServerClient.CharacterListEventArgs e)
        {
            if (e.Characters.Count == 0)
            {
                // TODO: What to do if we don't have any characters?
                return;
            }

            // Login the first character in the list
            mWorldClient.LoginPlayer(e.Characters[0]);
        }

        private void LoginDisconnectedHandler(object sender, System.EventArgs e)
        {
            // TODO: What should we do on disconnect?
        }

        #endregion
    }
}
