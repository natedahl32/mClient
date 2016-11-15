using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class MageLogic : PlayerClassLogic
    {
        #region Constructors

        public MageLogic(Player player) : base(player)
        {
        }

        #endregion

        #region Mage Constants

        public static class Spells
        {
            public const uint AMPLIFY_MAGIC_1 = 1008;
            public const uint ARCANE_BRILLIANCE_1 = 23028;
            public const uint ARCANE_EXPLOSION_1 = 1449;
            public const uint ARCANE_INTELLECT_1 = 1459;
            public const uint ARCANE_MISSILES_1 = 5143;
            public const uint ARCANE_POWER_1 = 12042;
            public const uint BLAST_WAVE_1 = 11113;
            public const uint BLINK_1 = 1953;
            public const uint BLIZZARD_1 = 10;
            public const uint COLD_SNAP_1 = 12472;
            public const uint COMBUSTION_1 = 11129;
            public const uint CONE_OF_COLD_1 = 120;
            public const uint CONJURE_FOOD_1 = 587;
            public const uint CONJURE_MANA_GEM_1 = 759;
            public const uint CONJURE_WATER_1 = 5504;
            public const uint COUNTERSPELL_1 = 2139;
            public const uint DAMPEN_MAGIC_1 = 604;
            public const uint EVOCATION_1 = 12051;
            public const uint FIRE_BLAST_1 = 2136;
            public const uint FIRE_WARD_1 = 543;
            public const uint FIREBALL_1 = 133;
            public const uint FLAMESTRIKE_1 = 2120;
            public const uint FROST_ARMOR_1 = 168;
            public const uint FROST_NOVA_1 = 122;
            public const uint FROST_WARD_1 = 6143;
            public const uint FROSTBOLT_1 = 116;
            public const uint ICE_ARMOR_1 = 7302;
            public const uint ICE_BARRIER_1 = 11426;
            public const uint ICE_BLOCK_1 = 27619;
            public const uint INVISIBILITY_1 = 66;
            public const uint MAGE_ARMOR_1 = 6117;
            public const uint MANA_SHIELD_1 = 1463;
            public const uint POLYMORPH_1 = 118;
            public const uint PRESENCE_OF_MIND_1 = 12043;
            public const uint PYROBLAST_1 = 11366;
            public const uint REMOVE_CURSE_MAGE_1 = 475;
            public const uint SCORCH_1 = 2948;
            public const uint SHOOT_2 = 5019;
            public const uint SLOW_FALL_1 = 130;
        }

        #endregion
    }
}
