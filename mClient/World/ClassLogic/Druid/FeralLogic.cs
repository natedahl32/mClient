using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic.Druid
{
    public class FeralLogic : DruidLogic
    {
        #region Constructors

        public FeralLogic(Player player) : base(player)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether or not this player is a melee combatant
        /// </summary>
        public override bool IsMelee
        {
            get
            {
                return true;
            }
        }

        #endregion
    }
}
