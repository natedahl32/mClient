using mClient.World.Items;
using mClient.World.Quest;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Load all other data
            mClient.BotServer.Data.ServerInfo.Instance.Load();
            QuestManager.Instance.Load();
            ItemManager.Instance.Load();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save any data elements before we close
            Program.Server.Serialize();
            mClient.BotServer.Data.ServerInfo.Instance.Serialize();
            QuestManager.Instance.Serialize();
            ItemManager.Instance.Serialize();
        }

        private void serverInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var serverInfoForm = new ServerInfo();
            serverInfoForm.ShowDialog();
        }

        private void addExistingBotAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var botAccount = new BotAccount();
            botAccount.Display(BotAccount.BotAccountType.AddExisting);
        }
    }
}
