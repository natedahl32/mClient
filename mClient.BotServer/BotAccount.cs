using mClient.BotServer.Data;
using mClient.Clients;
using mClient.Constants;
using Newtonsoft.Json;
using System;

namespace mClient.BotServer
{
    public class BotAccount
    {
        #region Declarations

        private LogonServerClient mLogonClient;
        private WorldServerClient mWorldClient;

        // Flag that determines whether or not we are logged in to the game
        private bool mLoggedIn = false;
        private Guid mClientId;

        // Event handlers for bot accounts that let server respond to events for a client
        internal protected event EventHandler<BotAccountUpdateEventArgs> BotAccountUpdate;

        #endregion

        #region Constructors

        [JsonConstructor]
        public BotAccount()
        {
            mClientId = Guid.NewGuid();
        }

        /// <summary>
        /// Constructor used for existing accounts that have a character already created
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        public BotAccount(string accountName, string password) : this()
        {
            this.AccountName = accountName;
            this.Password = password;
            this.InitializeLoginServer();
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
            get { return mClientId; }
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
                if (mLogonClient != null)
                    return mLogonClient.Connected;
                return false;
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
        /// Gets the current activity of the bot
        /// </summary>
        [JsonIgnore]
        public string CurrentActivity { get; private set; }

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

        /// <summary>
        /// Gets or sets the level of the character
        /// </summary>
        public byte CharacterLevel { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Logs in the bot account to the server
        /// </summary>
        public void Login()
        {
            // If login client is null create it now
            if (mLogonClient == null)
            {
                mLogonClient = new LogonServerClient(ServerInfo.Instance.Host, this.AccountName, this.Password);
                mLogonClient.ReceivedRealmList += HandleRealmListEvent;
                mLogonClient.Disconnected += LoginDisconnectedHandler;
            }

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
            mWorldClient.ActivityChange += WorldClient_ActivityChange;
            mWorldClient.Connect();
        }

        /// <summary>
        /// Handles an activity change for the bot account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorldClient_ActivityChange(object sender, string e)
        {
            CurrentActivity = e;
            BotAccountUpdated();
        }

        /// <summary>
        /// Handles the bot logging in to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorldClient_LoggedIn(object sender, WorldServerClient.LoginEventArgs e)
        {
            mLoggedIn = true;
            BotAccountUpdated();
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
            BotAccountUpdated();
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

            // Update character data for the bot account
            var character = e.Characters[0];
            CharacterName = character.Name;
            CharacterClass = (Classname)character.Class;
            CharacterRace = (Race)character.Race;
            CharacterLevel = character.Level;
            BotAccountUpdated();

            // Login the first character in the list
            mWorldClient.LoginPlayer(e.Characters[0]);
        }

        private void LoginDisconnectedHandler(object sender, System.EventArgs e)
        {
            // TODO: What should we do on disconnect?
            mLoggedIn = false;
            BotAccountUpdated();
        }

        private void BotAccountUpdated()
        {
            BotAccountUpdate?.Invoke(this, new BotAccountUpdateEventArgs(this.ClientId));
        }

        internal protected void InitializeLoginServer()
        {
            mLogonClient = new LogonServerClient(ServerInfo.Instance.Host, this.AccountName, this.Password);
            mLogonClient.ReceivedRealmList += HandleRealmListEvent;
            mLogonClient.Disconnected += LoginDisconnectedHandler;
        }

        #endregion

        #region Event Arguments

        internal protected class BotAccountUpdateEventArgs : System.EventArgs
        {
            private Guid mClientId;

            public BotAccountUpdateEventArgs(Guid clientId)
            {
                mClientId = clientId;
            }

            public Guid ClientId { get { return mClientId; } }
        }

        #endregion
    }
}
