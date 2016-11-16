using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class RogueLogic : PlayerClassLogic
    {
        #region Declarations

        // COMBAT
        protected uint ADRENALINE_RUSH,
               SINISTER_STRIKE,
               BACKSTAB,
               GOUGE,
               EVASION,
               SPRINT,
               KICK,
               FEINT;

        // SUBTLETY
        protected uint STEALTH,
               VANISH,
               HEMORRHAGE,
               BLIND,
               PICK_POCKET,
               CRIPPLING_POISON,
               DEADLY_POISON,
               MIND_NUMBING_POISON,
               GHOSTLY_STRIKE,
               DISTRACT,
               PREPARATION,
               PREMEDITATION;

        // ASSASSINATION
        protected uint COLD_BLOOD,
               EVISCERATE,
               SLICE_DICE,
               GARROTE,
               EXPOSE_ARMOR,
               AMBUSH,
               RUPTURE,
               CHEAP_SHOT,
               KIDNEY_SHOT,
               BLADE_FURRY,
               DISARM_TRAP,
               PICK_LOCK,
               RIPOSTE,
               SAP;

        #endregion

        #region Constructors

        public RogueLogic(Player player) : base(player)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether or not the player has any buffs to give out (including self buffs)
        /// </summary>
        public override bool HasOOCBuffs
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all group members that need a buff
        /// </summary>
        public override Dictionary<SpellEntry, IList<Player>> GroupMembersNeedingOOCBuffs
        {
            get
            {
                return new Dictionary<SpellEntry, IList<Player>>();
            }
        }

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

        /// <summary>
        /// Gets the next spell to cast in a DPS rotation for the class
        /// </summary>
        public override SpellEntry NextSpellInRotation
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Ignores spells that we think we should learn. These are generally broken spells in the DBC files that we can't weed out using normal methods
        /// </summary>
        public override IEnumerable<uint> IgnoreLearningSpells
        {
            get
            {
                return new List<uint>();
            }
        }

        #endregion

        #region Public Methods

        public override void InitializeSpells()
        {
            base.InitializeSpells();

            // Spells
            ADRENALINE_RUSH = InitSpell(Spells.ADRENALINE_RUSH_1);
            AMBUSH = InitSpell(Spells.AMBUSH_1);
            BACKSTAB = InitSpell(Spells.BACKSTAB_1);
            BLADE_FURRY = InitSpell(Spells.BLADE_FLURRY_1);
            BLIND = InitSpell(Spells.BLIND_1);
            CHEAP_SHOT = InitSpell(Spells.CHEAP_SHOT_1);
            COLD_BLOOD = InitSpell(Spells.COLD_BLOOD_1);
            DISARM_TRAP = InitSpell(Spells.DISARM_TRAP_1);
            DISTRACT = InitSpell(Spells.DISTRACT_1);
            EVASION = InitSpell(Spells.EVASION_1);
            EVISCERATE = InitSpell(Spells.EVISCERATE_1);
            EXPOSE_ARMOR = InitSpell(Spells.EXPOSE_ARMOR_1);
            FEINT = InitSpell(Spells.FEINT_1);
            GARROTE = InitSpell(Spells.GARROTE_1);
            GHOSTLY_STRIKE = InitSpell(Spells.GHOSTLY_STRIKE_1);
            GOUGE = InitSpell(Spells.GOUGE_1);
            HEMORRHAGE = InitSpell(Spells.HEMORRHAGE_1);
            KICK = InitSpell(Spells.KICK_1);
            KIDNEY_SHOT = InitSpell(Spells.KIDNEY_SHOT_1);
            PICK_LOCK = InitSpell(Spells.PICK_LOCK_1);
            PICK_POCKET = InitSpell(Spells.PICK_POCKET_1);
            PREMEDITATION = InitSpell(Spells.PREMEDITATION_1);
            PREPARATION = InitSpell(Spells.PREPARATION_1);
            RIPOSTE = InitSpell(Spells.RIPOSTE_1);
            RUPTURE = InitSpell(Spells.RUPTURE_1);
            SAP = InitSpell(Spells.SAP_1);
            SINISTER_STRIKE = InitSpell(Spells.SINISTER_STRIKE_1);
            SLICE_DICE = InitSpell(Spells.SLICE_AND_DICE_1);
            SPRINT = InitSpell(Spells.SPRINT_1);
            STEALTH = InitSpell(Spells.STEALTH_1);
            VANISH = InitSpell(Spells.VANISH_1);
        }

        #endregion

        #region Rogue Constants

        public static class Spells
        {
            public const uint ADRENALINE_RUSH_1 = 13750;
            public const uint AMBUSH_1 = 8676;
            public const uint BACKSTAB_1 = 53;
            public const uint BLADE_FLURRY_1 = 13877;
            public const uint BLIND_1 = 2094;
            public const uint CHEAP_SHOT_1 = 1833;
            public const uint COLD_BLOOD_1 = 14177;
            public const uint DISARM_TRAP_1 = 1842;
            public const uint DISTRACT_1 = 1725;
            public const uint EVASION_1 = 5277;
            public const uint EVISCERATE_1 = 2098;
            public const uint EXPOSE_ARMOR_1 = 8647;
            public const uint FEINT_1 = 1966;
            public const uint GARROTE_1 = 703;
            public const uint GHOSTLY_STRIKE_1 = 14278;
            public const uint GOUGE_1 = 1776;
            public const uint HEMORRHAGE_1 = 16511;
            public const uint KICK_1 = 1766;
            public const uint KIDNEY_SHOT_1 = 408;
            public const uint PICK_LOCK_1 = 1804;
            public const uint PICK_POCKET_1 = 921;
            public const uint PREMEDITATION_1 = 14183;
            public const uint PREPARATION_1 = 14185;
            public const uint RIPOSTE_1 = 14251;
            public const uint RUPTURE_1 = 1943;
            public const uint SAP_1 = 6770;
            public const uint SINISTER_STRIKE_1 = 1752;
            public const uint SLICE_AND_DICE_1 = 5171;
            public const uint SPRINT_1 = 2983;
            public const uint STEALTH_1 = 1784;
            public const uint VANISH_1 = 1856;
        }

        #endregion
    }
}
