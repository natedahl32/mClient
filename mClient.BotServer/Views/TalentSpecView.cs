using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.BotServer.Views
{
    public class TalentSpecView
    {
        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ForClass { get; set; }

        public string PrimarySpec { get; set; }

        #endregion
    }
}
