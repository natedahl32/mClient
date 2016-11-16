using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class MageLogic : PlayerClassLogic
    {
        #region Declarations

        // ARCANE
        protected uint ARCANE_MISSILES,
               ARCANE_EXPLOSION,
               COUNTERSPELL,
               SLOW,
               POLYMORPH,
               ARCANE_POWER;

        // RANGED
        protected uint SHOOT;

        // FIRE
        protected uint FIREBALL,
               FIRE_BLAST,
               FLAMESTRIKE,
               SCORCH,
               PYROBLAST,
               BLAST_WAVE,
               COMBUSTION,
               FIRE_WARD;

        // FROST
        protected uint FROSTBOLT,
               FROST_NOVA,
               BLIZZARD,
               CONE_OF_COLD,
               ICE_BARRIER,
               FROST_WARD,
               ICE_BLOCK,
               COLD_SNAP;

        // buffs
        protected uint FROST_ARMOR,
               ICE_ARMOR,
               MAGE_ARMOR,
               ARCANE_INTELLECT,
               ARCANE_BRILLIANCE,
               MANA_SHIELD,
               DAMPEN_MAGIC,
               AMPLIFY_MAGIC;

        protected uint CONJURE_WATER,
               CONJURE_FOOD,
               CONJURE_MANA_GEM,
               BLINK,
               EVOCATION,
               INVISIBILITY,
               PRESENCE_OF_MIND,
               REMOVE_CURSE,
               SLOW_FALL;

        #endregion

        #region Constructors

        public MageLogic(Player player) : base(player)
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
        public override Dictionary<SpellEntry, Player> GroupMembersNeedingOOCBuffs
        {
            get
            {
                return new Dictionary<SpellEntry, Player>();
            }
        }

        /// <summary>
        /// Gets whether or not this player is a melee combatant
        /// </summary>
        public override bool IsMelee
        {
            get
            {
                return false;
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
            AMPLIFY_MAGIC = InitSpell(Spells.AMPLIFY_MAGIC_1);
            ARCANE_BRILLIANCE = InitSpell(Spells.ARCANE_BRILLIANCE_1);
            ARCANE_EXPLOSION = InitSpell(Spells.ARCANE_EXPLOSION_1);
            ARCANE_INTELLECT = InitSpell(Spells.ARCANE_INTELLECT_1);
            ARCANE_MISSILES = InitSpell(Spells.ARCANE_MISSILES_1);
            ARCANE_POWER = InitSpell(Spells.ARCANE_POWER_1);
            BLAST_WAVE = InitSpell(Spells.BLAST_WAVE_1);
            BLINK = InitSpell(Spells.BLINK_1);
            BLIZZARD = InitSpell(Spells.BLIZZARD_1);
            COLD_SNAP = InitSpell(Spells.COLD_SNAP_1);
            COMBUSTION = InitSpell(Spells.COMBUSTION_1);
            CONE_OF_COLD = InitSpell(Spells.CONE_OF_COLD_1);
            CONJURE_FOOD = InitSpell(Spells.CONJURE_FOOD_1);
            CONJURE_WATER = InitSpell(Spells.CONJURE_WATER_1);
            CONJURE_MANA_GEM = InitSpell(Spells.CONJURE_MANA_GEM_1);
            COUNTERSPELL = InitSpell(Spells.COUNTERSPELL_1);
            DAMPEN_MAGIC = InitSpell(Spells.DAMPEN_MAGIC_1);
            EVOCATION = InitSpell(Spells.EVOCATION_1);
            FIRE_BLAST = InitSpell(Spells.FIRE_BLAST_1);
            FIRE_WARD = InitSpell(Spells.FIRE_WARD_1);
            FIREBALL = InitSpell(Spells.FIREBALL_1);
            FLAMESTRIKE = InitSpell(Spells.FLAMESTRIKE_1);
            FROST_ARMOR = InitSpell(Spells.FROST_ARMOR_1);
            FROST_NOVA = InitSpell(Spells.FROST_NOVA_1);
            FROST_WARD = InitSpell(Spells.FROST_WARD_1);
            FROSTBOLT = InitSpell(Spells.FROSTBOLT_1);
            ICE_ARMOR = InitSpell(Spells.ICE_ARMOR_1);
            ICE_BARRIER = InitSpell(Spells.ICE_BARRIER_1);
            ICE_BLOCK = InitSpell(Spells.ICE_BLOCK_1);
            INVISIBILITY = InitSpell(Spells.INVISIBILITY_1);
            MAGE_ARMOR = InitSpell(Spells.MAGE_ARMOR_1);
            MANA_SHIELD = InitSpell(Spells.MANA_SHIELD_1);
            POLYMORPH = InitSpell(Spells.POLYMORPH_1);
            PRESENCE_OF_MIND = InitSpell(Spells.PRESENCE_OF_MIND_1);
            PYROBLAST = InitSpell(Spells.PYROBLAST_1);
            REMOVE_CURSE = InitSpell(Spells.REMOVE_CURSE_MAGE_1);
            SCORCH = InitSpell(Spells.SCORCH_1);
            SHOOT = InitSpell(Spells.SHOOT_2);
            SLOW_FALL = InitSpell(Spells.SLOW_FALL_1);
            SLOW = InitSpell(Spells.SLOW_1);
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
            public const uint SLOW_1 = 246;
        }

        #endregion
    }
}
