using mClient.Constants;
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class ChrClassesTable : DBCFile
    {
        private Dictionary<uint, ChrClassEntry> mChrClassEntries = new Dictionary<uint, ChrClassEntry>();

        #region Singleton

        static readonly ChrClassesTable instance = new ChrClassesTable();

        static ChrClassesTable() { }

        ChrClassesTable() : base(@"ChrClasses.dbc")
        { }

        public static ChrClassesTable Instance { get { return instance; } }

        #endregion

        protected override void dataLoaded()
        {
            for (uint i = 0; i < Records; i++)
            {
                var entry = new ChrClassEntry();
                entry.ClassID = getFieldAsUint32(i, 0);
                entry.PowerType = (Powers)getFieldAsUint32(i, 3);
                entry.Name = getStringForField(i, 5);
                entry.SpellFamily = getFieldAsUint32(i, 15);

                mChrClassEntries.Add(entry.ClassID, entry);
            }
        }

        public ChrClassEntry getByClassId(uint classId)
        {
            return mChrClassEntries[classId];
        }
    }
}
