using mClient.Constants;
using mClient.World.ClassLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World
{
    public abstract class PlayerClassLogic
    {
        #region Declarations

        private Player mPlayer;

        #endregion

        #region Constructors

        protected PlayerClassLogic(Player player)
        {
            if (player == null) throw new ArgumentNullException("player");
            mPlayer = player;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the player this logic belongs to
        /// </summary>
        public Player Player { get { return mPlayer; } }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creats a class logic instance based on class
        /// </summary>
        /// <param name="class"></param>
        /// <returns></returns>
        public static PlayerClassLogic CreateClassLogic(Classname @class, Player player)
        {
            switch (@class)
            {
                case Classname.Druid:
                    return new DruidLogic(player);
                case Classname.Hunter:
                    return new HunterLogic(player);
                case Classname.Mage:
                    return new MageLogic(player);
                case Classname.Paladin:
                    return new PaladinLogic(player);
                case Classname.Priest:
                    return new PriestLogic(player);
                case Classname.Rogue:
                    return new RogueLogic(player);
                case Classname.Shaman:
                    return new ShamanLogic(player);
                case Classname.Warlock:
                    return new WarlockLogic(player);
                case Classname.Warrior:
                    return new WarriorLogic(player);
                default:
                    break;
            }
            return null;
        }

        #endregion
    }
}
