using mClient.BotServer;
using mClient.BotServer.Views;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Linq;

namespace BotServer.SignalRServer
{
    [HubName("BotServerHub")]
    public class BotServerHub : Hub
    {
        // static reference to our server
        public static mClient.BotServer.BotServer Server = new mClient.BotServer.BotServer();

        // Hub context used in server pushes
        private readonly static IHubContext mHubContext = GlobalHost.ConnectionManager.GetHubContext<BotServerHub>();

        #region Constructors

        public BotServerHub() : base()
        {
            // Subscribe to server events
            Server.BotAccountViewChange += Server_BotAccountViewChange;
        }

        #endregion

        #region Client Methods

        /// <summary>
        /// Clients request existing bot accounts
        /// </summary>
        public void RequestExistingBotAccounts()
        {
            SendExistingBotAccounts(Context.ConnectionId);
        }

        /// <summary>
        /// Client adds an existing bot account
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountPassword"></param>
        public void AddExistingBotAccount(string accountName, string accountPassword)
        {
            var account = new BotAccount(accountName, accountPassword);
            Server.AddExistingBotAccount(account);
            SendExistingBotAccounts(Context.ConnectionId);
        }

        /// <summary>
        /// Connect all clients to the world server
        /// </summary>
        public void ConnectAllClients()
        {
            Server.LogInAllClients();
        }

        /// <summary>
        /// Connects and logins a specific account
        /// </summary>
        /// <param name="id"></param>
        public void ConnectAccount(string id)
        {
            Server.LogInClient(id);
        }

        #endregion

        #region Server Methods

        /// <summary>
        /// Sends existing bot accounts to the requesting client
        /// </summary>
        /// <param name="connectionId"></param>
        public static void SendExistingBotAccounts(string connectionId)
        {
            var accounts = Server.BotAccounts.ToList();
            mHubContext.Clients.Client(connectionId).existingBotAccounts(accounts);
        }

        /// <summary>
        /// Sends an update for a bot account to all clients
        /// </summary>
        /// <param name="account"></param>
        public static void SendBotAccountUpdate(BotAccountView account)
        {
            mHubContext.Clients.All.botAccountUpdate(account);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Event handler for bot account changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Server_BotAccountViewChange(object sender, mClient.BotServer.EventArgs.BotAccountViewChangeEventArgs e)
        {
            SendBotAccountUpdate(e.BotAccount);
        }

        #endregion
    }
}
