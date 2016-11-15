using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class RogueLogic : PlayerClassLogic
    {
        #region Constructors

        public RogueLogic(Player player) : base(player)
        {
        }

        #endregion

        #region Public Methods

        public override void InitializeSpells()
        {

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
