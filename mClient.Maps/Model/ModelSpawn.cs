using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Maps.Model
{
    public class ModelSpawn
    {
        #region Declarations

        public uint flags;
        public ushort adtId;
        public uint ID;
        public Vector3 iPos;
        public Vector3 iRot;
        public float iScale;
        //public AABox iBound;
        public string name;

        //public AABox getBounds { get { return iBounds; } }

       
        #endregion
    }
}
