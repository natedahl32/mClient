using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class SkillLineEntry
    {
        #region Properties

        public uint Id { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public uint SpellIcon { get; set; }

        #endregion
    }
}
