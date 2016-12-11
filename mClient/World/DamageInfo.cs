using mClient.Constants;
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World
{
    /// <summary>
    /// Class that contains damage information on attack/spell hit or miss
    /// </summary>
    public class DamageInfo
    {
        #region Properties

        public WoWGuid Attacker { get; set; }

        public WoWGuid Victim { get; set; }

        public HitInfo HitInfo { get; set; }

        public uint FullDamage { get; set; }

        public uint Absorb { get; set; }

        public uint Resist { get; set; }

        public VictimState TargetState { get; set; }

        #endregion
    }
}
