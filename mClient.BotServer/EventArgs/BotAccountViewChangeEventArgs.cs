using mClient.BotServer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.BotServer.EventArgs
{
    public class BotAccountViewChangeEventArgs : System.EventArgs
    {
        #region Declarations

        private readonly BotAccountView mBotAccount;

        #endregion

        #region Constructors

        public BotAccountViewChangeEventArgs(BotAccountView account)
        {
            mBotAccount = account;
        }

        #endregion

        #region Properties

        public BotAccountView BotAccount { get { return mBotAccount; } }

        #endregion
    }
}
