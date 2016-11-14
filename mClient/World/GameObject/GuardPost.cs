using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.GameObject
{
    public class GuardPost : GameObjectInfo
    {
        public GuardPost(uint id, GameObjectType type, string name, int[] data) : base(id, type, name, data)
        {
        }
    }
}
