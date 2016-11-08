using BotServer.Data;
using mClient.Clients;
using mClient.Constants;

namespace mClient.BotServer
{
    public class BotAccount
    {
        #region Declarations

        private LogonServerClient mLogonClient;
        private WorldServerClient mWorldClient;

        #endregion

        #region Constructors

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
            mLogonClient.Connect();
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
        /// Gets whether or not the bot account is connected to the server
        /// </summary>
        public bool IsConnected
        {
            get { return false; }
        }

        /// <summary>
        /// Gets whether or not the bot account is logged in to the game
        /// </summary>
        public bool IsLoggedIn
        {
            get { return false; }
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

    }
}
