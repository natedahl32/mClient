using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Spells
{
    public class TrainerSpellData
    {
        #region Properties

        public uint SpellId { get; set; }

        public byte State { get; set; }

        public uint Cost { get; set; }

        public uint RequiredLevel { get; set; }

        public uint RequiredSkill { get; set; }

        public uint RequiredSkillValue { get; set; }

        public uint SpellChainNode { get; set; }

        public uint SpellChainNode2 { get; set; }

        #endregion
    }
}
