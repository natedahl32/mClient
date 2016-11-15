using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class WarriorLogic : PlayerClassLogic
    {
        #region Constructors

        public WarriorLogic(Player player) : base(player)
        {
        }

        #endregion

        #region Warrior Constants

        public static class Spells
        {
            public const uint BATTLE_SHOUT_1 = 6673;
            public const uint BATTLE_STANCE_1 = 2457;
            public const uint BERSERKER_RAGE_1 = 18499;
            public const uint BERSERKER_STANCE_1 = 2458;
            public const uint BLOODRAGE_1 = 2687;
            public const uint BLOODTHIRST_1 = 23881;
            public const uint CHALLENGING_SHOUT_1 = 1161;
            public const uint CHARGE_1 = 100;
            public const uint CLEAVE_1 = 845;
            public const uint CONCUSSION_BLOW_1 = 12809;
            public const uint DEATH_WISH_1 = 12292;
            public const uint DEFENSIVE_STANCE_1 = 71;
            public const uint DEMORALIZING_SHOUT_1 = 1160;
            public const uint DISARM_1 = 676;
            public const uint EXECUTE_1 = 5308;
            public const uint HAMSTRING_1 = 1715;
            public const uint HEROIC_STRIKE_1 = 78;
            public const uint INTERCEPT_1 = 20252;
            public const uint INTERVENE_1 = 3411;
            public const uint INTIMIDATING_SHOUT_1 = 5246;
            public const uint LAST_STAND_1 = 12975;
            public const uint MOCKING_BLOW_1 = 694;
            public const uint MORTAL_STRIKE_1 = 12294;
            public const uint OVERPOWER_1 = 7384;
            public const uint PIERCING_HOWL_1 = 12323;
            public const uint PUMMEL_1 = 6552;
            public const uint RECKLESSNESS_1 = 1719;
            public const uint REND_1 = 772;
            public const uint RETALIATION_1 = 20230;
            public const uint REVENGE_1 = 6572;
            public const uint SHIELD_BASH_1 = 72;
            public const uint SHIELD_BLOCK_1 = 2565;
            public const uint SHIELD_SLAM_1 = 23922;
            public const uint SHIELD_WALL_1 = 871;
            public const uint SHOOT_BOW_1 = 2480;
            public const uint SHOOT_GUN_1 = 7918;
            public const uint SHOOT_XBOW_1 = 7919;
            public const uint SLAM_1 = 1464;
            public const uint SUNDER_ARMOR_1 = 7386;
            public const uint SWEEPING_STRIKES_1 = 12328;
            public const uint TAUNT_1 = 355;
            public const uint THUNDER_CLAP_1 = 6343;
            public const uint WHIRLWIND_1 = 1680;
        }

        #endregion
    }
}
