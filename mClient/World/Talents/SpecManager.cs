using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Talents
{
    public class SpecManager : AbstractObjectManager<Spec>
    {
        #region Singleton

        static readonly SpecManager instance = new SpecManager();

        static SpecManager() { }

        SpecManager()
        { }

        public static SpecManager Instance { get { return instance; } }

        #endregion

        #region Properties

        protected override string SerializeToFile
        {
            get
            {
                return "specs.json";
            }
        }

        #endregion

        #region Public Methods

        public override bool Exists(Spec obj)
        {
            if (obj == null) return false;
            lock (mLock)
                return mObjects.Any(i => i.Id == obj.Id);
        }

        public bool Exists(uint id)
        {
            lock (mLock)
                return mObjects.Any(i => i.Id == id);
        }

        public override Spec Get(uint id)
        {
            lock (mLock)
                return mObjects.Where(i => i != null && i.Id == id).SingleOrDefault();
        }

        public IEnumerable<Spec> GetAll()
        {
            lock (mLock)
                return mObjects.ToList();
        }

        public uint GetNextId()
        {
            lock (mLock)
                return (uint)mObjects.Select(s => s.Id).DefaultIfEmpty(0).Max() + 1;
        }

        #endregion
    }
}
