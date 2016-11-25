using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class SpellItemEnchantmentTable : DBCFile
    {
        private Dictionary<uint, SpellItemEnchantmentEntry> mSpellItemEnchantmentEntries = new Dictionary<uint, SpellItemEnchantmentEntry>();

        #region Singleton

        static readonly SpellItemEnchantmentTable instance = new SpellItemEnchantmentTable();

        static SpellItemEnchantmentTable() { }

        SpellItemEnchantmentTable() : base(@"SpellItemEnchantment.dbc")
        { }

        public static SpellItemEnchantmentTable Instance { get { return instance; } }

        #endregion

        protected override void dataLoaded()
        {
            for (uint i = 0; i < Records; i++)
            {
                var entry = new SpellItemEnchantmentEntry();
                entry.ID = getFieldAsUint32(i, 0);

                entry.EnchantmentType = new uint[3];
                entry.EnchantmentType[0] = getFieldAsUint32(i, 1);
                entry.EnchantmentType[1] = getFieldAsUint32(i, 2);
                entry.EnchantmentType[2] = getFieldAsUint32(i, 3);

                entry.EnchantmentAmount = new uint[3];
                entry.EnchantmentAmount[0] = getFieldAsUint32(i, 4);
                entry.EnchantmentAmount[1] = getFieldAsUint32(i, 5);
                entry.EnchantmentAmount[2] = getFieldAsUint32(i, 6);

                entry.SpellId = new uint[3];
                entry.SpellId[0] = getFieldAsUint32(i, 10);
                entry.SpellId[1] = getFieldAsUint32(i, 11);
                entry.SpellId[2] = getFieldAsUint32(i, 12);

                entry.Description = getStringForField(i, 13);
                entry.AuraId = getFieldAsUint32(i, 22);
                entry.Slot = getFieldAsUint32(i, 23);

                mSpellItemEnchantmentEntries.Add(entry.ID, entry);
            }
        }

        public SpellItemEnchantmentEntry getById(uint Id)
        {
            if (mSpellItemEnchantmentEntries.ContainsKey(Id))
                return mSpellItemEnchantmentEntries[Id];
            return null;
        }
    }
}
