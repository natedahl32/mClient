using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class SpellItemEnchantmentEntry
    {
        #region Properties

        public uint ID { get; set; }
        public uint[] EnchantmentType { get; set; }
        public uint[] EnchantmentAmount { get; set; }
        public uint[] SpellId { get; set; }
        public string Description { get; set; }
        public uint AuraId { get; set; }
        public uint Slot { get; set; }

        #endregion
    }
}
