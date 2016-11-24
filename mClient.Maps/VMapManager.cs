using mClient.Maps.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Maps
{
    public class VMapManager
    {
        #region Singleton

        static readonly VMapManager instance = new VMapManager();

        static VMapManager() { }

        VMapManager()
        {
        }

        public static VMapManager Instance { get { return instance; } }

        #endregion

        #region Declarations

        // Tree to check collision
        protected Dictionary<string, ManagedModel> iLoadedMOdelFiles;
        protected Dictionary<uint, StaticMapTree> iInstanceMapTrees;

        #endregion
    }
}
