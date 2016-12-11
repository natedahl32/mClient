using mClient.Constants;
using mClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Spells
{
    public class SpellDamageInfo
    {
        #region Properties

        public WoWGuid Attacker { get; set; }

        public WoWGuid Victim { get; set; }

        public uint SpellId { get; set; }

        public uint Damage { get; set; }

        public byte DamageSchool { get; set; }

        public uint Absorb { get; set; }

        public uint Resist { get; set; }

        public uint Blocked { get; set; }

        public SpellHitType HitInfo { get; set; }

        #endregion
    }
}
