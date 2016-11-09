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
    public partial class ServerInfo : Form
    {
        public ServerInfo()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            mClient.BotServer.Data.ServerInfo.Instance.Host = txtHost.Text;
            mClient.BotServer.Data.ServerInfo.Instance.Port = Convert.ToInt32(txtPort.Text);
        }

        private void ServerInfo_Load(object sender, EventArgs e)
        {
            txtHost.Text = mClient.BotServer.Data.ServerInfo.Instance.Host;
            txtPort.Text = mClient.BotServer.Data.ServerInfo.Instance.Port.ToString();
        }
    }
}
