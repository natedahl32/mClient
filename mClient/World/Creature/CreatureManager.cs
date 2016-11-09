using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Creature
{
    public class CreatureManager : AbstractObjectManager<CreatureInfo>
    {
        #region Singleton

        static readonly CreatureManager instance = new CreatureManager();

        static CreatureManager() { }

        CreatureManager()
        { }

        public static CreatureManager Instance { get { return instance; } }

        #endregion

        #region Properties

        protected override string SerializeToFile
        {
            get
            {
                return "creatures.json";
            }
        }

        #endregion

        #region Public Methods

        public override bool Exists(CreatureInfo obj)
        {
            if (obj == null) return false;
            return mObjects.Any(i => i.CreatureId == obj.CreatureId);
        }

        public bool Exists(UInt32 creatureId)
        {
            return mObjects.Any(i => i.CreatureId == creatureId);
        }

        public override CreatureInfo Get(uint id)
        {
            return mObjects.Where(i => i != null && i.CreatureId == id).SingleOrDefault();
        }

        #endregion
    }
}
