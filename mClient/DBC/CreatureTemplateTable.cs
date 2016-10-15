using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class CreatureTemplateTable : DBCFile
    {
        public CreatureTemplateTable()
            : base(@"Spell.dbc")
        {
        }
    }
}
