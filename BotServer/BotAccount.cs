using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BotServer
{
    public partial class BotAccount : Form
    {
        public enum BotAccountType
        {
            AddExisting,
            CreateNew,
            Update
        }

        private BotAccountType mAccountChangeType;

        public BotAccount()
        {
            InitializeComponent();
        }

        public void Display(BotAccountType type)
        {
            mAccountChangeType = type;

            txtAccountName.Text = string.Empty;
            txtAccountPassword.Text = string.Empty;
            txtCharacterName.Text = string.Empty;

            this.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (mAccountChangeType == BotAccountType.AddExisting)
            {
                var account = new mClient.BotServer.BotAccount(txtAccountName.Text, txtAccountPassword.Text);
                account.CharacterName = txtCharacterName.Text;
                Program.Server.AddExistingBotAccount(account);
                this.Close();
            }
        }
    }
}
