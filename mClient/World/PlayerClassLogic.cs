﻿using mClient.Constants;
using mClient.DBC;
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

        public const uint RECENTLY_BANDAGED = 11196;

        private Player mPlayer;

        // racial
        protected uint STONEFORM,
            ESCAPE_ARTIST,
            PERCEPTION,
            SHADOWMELD,
            BLOOD_FURY,
            WAR_STOMP,
            BERSERKING,
            WILL_OF_THE_FORSAKEN;

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

        #region Public Methods

        /// <summary>
        /// Initializes all spells the player currently has.
        /// </summary>
        public virtual void InitializeSpells()
        {
            // Racial abilities
            STONEFORM = InitSpell(RacialTraits.STONEFORM_ALL);
            ESCAPE_ARTIST = InitSpell(RacialTraits.ESCAPE_ARTIST_ALL);
            PERCEPTION = InitSpell(RacialTraits.PERCEPTION_ALL);
            SHADOWMELD = InitSpell(RacialTraits.SHADOWMELD_ALL);
            BLOOD_FURY = InitSpell(RacialTraits.BLOOD_FURY_ALL);
            WAR_STOMP = InitSpell(RacialTraits.WAR_STOMP_ALL);
            BERSERKING = InitSpell(RacialTraits.BERSERKING_ALL);
            WILL_OF_THE_FORSAKEN = InitSpell(RacialTraits.WILL_OF_THE_FORSAKEN_ALL);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes a spell by getting the current rank of the spell the player currently has
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        protected uint InitSpell(uint spellId)
        {
            // If the player does not have the spell
            if (!Player.HasSpell((ushort)spellId))
                return 0;

            var spell = spellId;
            uint nextSpell = 0;
            do
            {
                nextSpell = SkillLineAbilityTable.Instance.getParentForSpell(spell);
                if (nextSpell > 0)
                {
                    if (Player.HasSpell((ushort)nextSpell))
                        spell = nextSpell;
                    else
                        return spell;
                }
            } while (nextSpell != 0);
            return spell;
        }

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