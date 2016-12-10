using mClient.BotServer.EventArgs;
using mClient.BotServer.Views;
using mClient.Constants;
using mClient.World.Creature;
using mClient.World.GameObject;
using mClient.World.Guild;
using mClient.World.Items;
using mClient.World.Quest;
using mClient.World.Talents;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.BotServer
{
    public class BotServer
    {
        #region Declarations

        // Holds all bot accounts currently registered with the server
        private List<BotAccount> mBotAccounts = new List<BotAccount>();
        private Object mLock = new Object();

        // Broadcasted events for server
        public event EventHandler<BotAccountViewChangeEventArgs> BotAccountViewChange;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the file path the information in this class is saved to
        /// </summary>
        private string FilePath { get { return @"botaccounts.json"; } }

        /// <summary>
        /// Gets IEnumerable of all bot accounts available on 
        /// </summary>
        public IEnumerable<BotAccountView> BotAccounts
        {
            get
            {
                return mBotAccounts.Select(a => new BotAccountView(a));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serializes bot accounts to file
        /// </summary>
        public void Serialize()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(FilePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
                serializer.Serialize(writer, mBotAccounts);
        }

        /// <summary>
        /// Loads existing bot accounts from file
        /// </summary>
        public void Load()
        {
            if (!File.Exists(FilePath)) return;

            var data = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(data.Trim())) return;
            mBotAccounts = JsonConvert.DeserializeObject<List<BotAccount>>(data);

            // Initialize login server for each bot account that was loaded
            foreach (var account in mBotAccounts)
                account.InitializeLoginServer();

            // Load our caches
            CreatureManager.Instance.Load();
            GameObjectManager.Instance.Load();
            QuestManager.Instance.Load();
            ItemManager.Instance.Load();
            GuildManager.Instance.Load();
            SpecManager.Instance.Load();
        }

        /// <summary>
        /// Adds an existing bot account.
        /// </summary>
        /// <param name="botAccount"></param>
        public void AddExistingBotAccount(BotAccount botAccount)
        {
            if (mBotAccounts.Any(a => a.AccountName == botAccount.AccountName))
                throw new ApplicationException(string.Format("Cannot add a duplicate bot account with account name {0}", botAccount.AccountName));
            // Only check character name if it was supplied, otherwise we get it when the character logs in
            if (!string.IsNullOrEmpty(botAccount.CharacterName) && mBotAccounts.Any(a => a.CharacterName == botAccount.CharacterName))
                throw new ApplicationException(string.Format("Cannot add a duplicate bot account with character name {0}", botAccount.CharacterName));

            lock(mLock)
            {
                mBotAccounts.Add(botAccount);
                Serialize();
            }
        }

        /// <summary>
        /// Connects and logs in all clients that are not already connected
        /// </summary>
        public void LogInAllClients()
        {
            foreach (var client in mBotAccounts)
                LogInBotAccount(client);
        }

        /// <summary>
        /// Connects and logs in a specific client that is not already logged in
        /// </summary>
        /// <param name="clientId"></param>
        public void LogInClient(string clientId)
        {
            var client = mBotAccounts.Where(a => a.ClientId.ToString() == clientId).SingleOrDefault();
            if (client != null)
                LogInBotAccount(client);
        }

        /// <summary>
        /// Saves a new talent spec to the spec manager
        /// </summary>
        /// <param name="specName"></param>
        /// <param name="specDescription"></param>
        /// <param name="className"></param>
        /// <param name="talents"></param>
        public void SaveTalentSpec(string specName, string specDescription, byte className, uint[] talents)
        {
            var spec = new Spec(specName)
            {
                Description = specDescription,
                ForClass = (Classname)className,
                Talents = talents
            };
            SpecManager.Instance.Add(spec);
        }

        /// <summary>
        /// Lists all talent specs available
        /// </summary>
        /// <returns></returns>
        public IList<TalentSpecView> ListTalentSpecs()
        {
            var specs = SpecManager.Instance.GetAll().ToList();
            return specs.Select(ts => new TalentSpecView()
            {
                Id = ts.Id,
                Name = ts.Name,
                Description = ts.Description,
                ForClass = ts.ForClass.GetDisplayName(),
                PrimarySpec = ts.TalentSpec.GetDisplayName()
            }).ToList();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Logs in a bot account to the world server
        /// </summary>
        /// <param name="account"></param>
        private void LogInBotAccount(BotAccount account)
        {
            if (account == null) throw new ArgumentNullException("account");

            // Logs in a bot account
            account.BotAccountUpdate += Account_BotAccountUpdate;
            account.Login();
        }

        /// <summary>
        /// Handles changes to a bot account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Account_BotAccountUpdate(object sender, BotAccount.BotAccountUpdateEventArgs e)
        {
            var account = mBotAccounts.Where(a => a.ClientId == e.ClientId).SingleOrDefault();
            if (account == null) return;

            // Broadcast changes and serialize
            BroadcastAccountViewChange(account);
            Serialize();
        }

        /// <summary>
        /// Broadcast bot account changes to listeners
        /// </summary>
        /// <param name="account"></param>
        private void BroadcastAccountViewChange(BotAccount account)
        {
            if (BotAccountViewChange != null)
                BotAccountViewChange(this, new BotAccountViewChangeEventArgs(new BotAccountView(account)));
        }

        #endregion
    }
}
