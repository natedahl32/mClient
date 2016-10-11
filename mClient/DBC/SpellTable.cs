using mClient.Shared;

namespace mClient.DBC
{
    public class SpellTable : DBCFile
    {
        public SpellTable()
            : base(@"Spell.dbc")
        {
        }

        public string getSpell(uint spellId)
        {
            for (uint x = 0; x < wdbc_header.nRecords; x++)
            {
                uint id = getFieldAsUint32(x, 0);

                if (id == spellId)
                    return getStringForField(x, 120);
            }

            return null;
        }
    }
}
