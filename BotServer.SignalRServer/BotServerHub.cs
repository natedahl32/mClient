using mClient.BotServer;
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

        #endregion
    }
}
