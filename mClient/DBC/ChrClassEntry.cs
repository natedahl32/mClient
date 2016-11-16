using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class ChrClassEntry
    {
        public uint ClassID { get; set; }
        public Powers PowerType { get; set; }
        public string Name { get; set; }
        public uint SpellFamily { get; set; }
    }
}
