using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Shared
{
    public abstract class Singleton<T> where T : class, new()
    {
        private static T mInstance;
        private static Object mLock = new System.Object();

        protected Singleton() { }

        public static T Instance
        {
            get
            {
                lock(mLock)
                {
                    if (mInstance == null)
                        mInstance = new T();
                }
                return mInstance;
            }
        }
    }
}
