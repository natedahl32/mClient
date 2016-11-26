using mClient.Constants;
using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mClient.Clients;
using mClient.World.Items;

namespace mClient.World.ClassLogic
{
    public class HunterLogic : PlayerClassLogic
    {
        #region Declarations

        protected uint PET_SUMMON,
           PET_DISMISS,
           PET_REVIVE,
           PET_MEND,
           PET_FEED,
           BESTIAL_WRATH,
           BAD_ATTITUDE,
           SONIC_BLAST,
           DEMORALIZING_SCREECH,
           INTIMIDATION;

        protected uint AUTO_SHOT,
               HUNTERS_MARK,
               ARCANE_SHOT,
               CONCUSSIVE_SHOT,
               DISTRACTING_SHOT,
               MULTI_SHOT,
               EXPLOSIVE_SHOT,
               SERPENT_STING,
               SCORPID_STING,
               VIPER_STING,
               WYVERN_STING,
               AIMED_SHOT,
               VOLLEY,
               BLACK_ARROW,
               TRANQ_SHOT,
               SCATTER_SHOT;

        protected uint RAPTOR_STRIKE,
               WING_CLIP,
               MONGOOSE_BITE,
               DISENGAGE,
               DETERRENCE;

        protected uint FREEZING_TRAP,
               IMMOLATION_TRAP,
               FROST_TRAP,
               EXPLOSIVE_TRAP;

        protected uint ASPECT_OF_THE_HAWK,
               ASPECT_OF_THE_MONKEY,
               ASPECT_OF_THE_BEAST,
               ASPECT_OF_THE_CHEETAH,
               ASPECT_OF_THE_PACK,
               ASPECT_OF_THE_WILD,
               RAPID_FIRE,
               TRUESHOT_AURA,
               BEAST_LORE,
               EAGLE_EYE,
               EYES_OF_THE_BEAST,
               FEIGN_DEATH,
               FLARE,
               SCARE_BEAST,
               TAME_BEAST,
               TRACK_BEASTS,
               TRACK_DEMONS,
               TRACK_DRAGONKIN,
               TRACK_ELEMENTALS,
               TRACK_GIANTS,
               TRACK_HIDDEN,
               TRACK_HUMANOIDS,
               TRACK_UNDEAD,
               COUNTERATTACK,
               READINESS;

        #endregion

        #region Constructors

        public HunterLogic(Player player) : base(player)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of this class
        /// </summary>
        public override string ClassName
        {
            get { return "Hunter"; }
        }

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
            ARCANE_SHOT = InitSpell(Spells.ARCANE_SHOT_1);
            ASPECT_OF_THE_BEAST = InitSpell(Spells.ASPECT_OF_THE_BEAST_1);
            ASPECT_OF_THE_CHEETAH = InitSpell(Spells.ASPECT_OF_THE_CHEETAH_1);
            ASPECT_OF_THE_HAWK = InitSpell(Spells.ASPECT_OF_THE_HAWK_1);
            ASPECT_OF_THE_MONKEY = InitSpell(Spells.ASPECT_OF_THE_MONKEY_1);
            ASPECT_OF_THE_PACK = InitSpell(Spells.ASPECT_OF_THE_PACK_1);
            ASPECT_OF_THE_WILD = InitSpell(Spells.ASPECT_OF_THE_WILD_1);
            AUTO_SHOT = InitSpell(Spells.AUTO_SHOT_1);
            BEAST_LORE = InitSpell(Spells.BEAST_LORE_1);
            PET_SUMMON = InitSpell(Spells.CALL_PET_1);
            CONCUSSIVE_SHOT = InitSpell(Spells.CONCUSSIVE_SHOT_1);
            DETERRENCE = InitSpell(Spells.DETERRENCE_1);
            DISENGAGE = InitSpell(Spells.DISENGAGE_1);
            PET_DISMISS = InitSpell(Spells.DISMISS_PET_1);
            DISTRACTING_SHOT = InitSpell(Spells.DISTRACTING_SHOT_1);
            EAGLE_EYE = InitSpell(Spells.EAGLE_EYE_1);
            EXPLOSIVE_TRAP = InitSpell(Spells.EXPLOSIVE_TRAP_1);
            EYES_OF_THE_BEAST = InitSpell(Spells.EYES_OF_THE_BEAST_1);
            PET_FEED = InitSpell(Spells.FEED_PET_1);
            FEIGN_DEATH = InitSpell(Spells.FEIGN_DEATH_1);
            FLARE = InitSpell(Spells.FLARE_1);
            FREEZING_TRAP = InitSpell(Spells.FREEZING_TRAP_1);
            FROST_TRAP = InitSpell(Spells.FROST_TRAP_1);
            HUNTERS_MARK = InitSpell(Spells.HUNTERS_MARK_1);
            IMMOLATION_TRAP = InitSpell(Spells.IMMOLATION_TRAP_1);
            PET_MEND = InitSpell(Spells.MEND_PET_1);
            MONGOOSE_BITE = InitSpell(Spells.MONGOOSE_BITE_1);
            MULTI_SHOT = InitSpell(Spells.MULTISHOT_1);
            RAPID_FIRE = InitSpell(Spells.RAPID_FIRE_1);
            RAPTOR_STRIKE = InitSpell(Spells.RAPTOR_STRIKE_1);
            PET_REVIVE = InitSpell(Spells.REVIVE_PET_1);
            SCARE_BEAST = InitSpell(Spells.SCARE_BEAST_1);
            SCORPID_STING = InitSpell(Spells.SCORPID_STING_1);
            SERPENT_STING = InitSpell(Spells.SERPENT_STING_1);
            TAME_BEAST = InitSpell(Spells.TAME_BEAST_1);
            TRACK_BEASTS = InitSpell(Spells.TRACK_BEASTS_1);
            TRACK_DEMONS = InitSpell(Spells.TRACK_DEMONS_1);
            TRACK_DRAGONKIN = InitSpell(Spells.TRACK_DRAGONKIN_1);
            TRACK_ELEMENTALS = InitSpell(Spells.TRACK_ELEMENTALS_1);
            TRACK_GIANTS = InitSpell(Spells.TRACK_GIANTS_1);
            TRACK_HIDDEN = InitSpell(Spells.TRACK_HIDDEN_1);
            TRACK_HUMANOIDS = InitSpell(Spells.TRACK_HUMANOIDS_1);
            TRACK_UNDEAD = InitSpell(Spells.TRACK_UNDEAD_1);
            TRANQ_SHOT = InitSpell(Spells.TRANQUILIZING_SHOT_1);
            VIPER_STING = InitSpell(Spells.VIPER_STING_1);
            VOLLEY = InitSpell(Spells.VOLLEY_1);
            WING_CLIP = InitSpell(Spells.WING_CLIP_1);
            AIMED_SHOT = InitSpell(Spells.AIMED_SHOT_1);
            BESTIAL_WRATH = InitSpell(Spells.BESTIAL_WRATH_1);
            BLACK_ARROW = InitSpell(Spells.BLACK_ARROW_1);
            COUNTERATTACK = InitSpell(Spells.COUNTERATTACK_1);
            INTIMIDATION = InitSpell(Spells.INTIMIDATION_1);
            READINESS = InitSpell(Spells.READINESS_1);
            SCATTER_SHOT = InitSpell(Spells.SCATTER_SHOT_1);
            TRUESHOT_AURA = InitSpell(Spells.TRUESHOT_AURA_1);
            WYVERN_STING = InitSpell(Spells.WYVERN_STING_1);
        }

        public override float CompareItems(ItemInfo item1, ItemInfo item2)
        {
            // Get the base value of the compare
            var baseCompare = base.CompareItems(item1, item2);

            float item1Score = 0f;
            float item2Score = 0f;

            if (item1.ItemClass == ItemClass.ITEM_CLASS_WEAPON && item2.ItemClass == ItemClass.ITEM_CLASS_WEAPON)
            {
                item1Score += (item1.DPS * 0.9f);
                item2Score += (item2.DPS * 0.9f);
            }

            // Reduce armor score so it isn't overvalued. There can be a lot on items
            float item1Armor = item1.Resistances[SpellSchools.SPELL_SCHOOL_NORMAL] / 20;
            float item2Armor = item2.Resistances[SpellSchools.SPELL_SCHOOL_NORMAL] / 20;

            item1Score += (item1Armor * 0.1f);
            item2Score += (item2Armor * 0.1f);

            var newCompare = item1Score - item2Score;
            return baseCompare + newCompare;
        }

        #endregion

        #region Private Methods

        protected override void SetStatWeights()
        {
            base.SetStatWeights();

            mStatWeights[ItemModType.ITEM_MOD_STAMINA] = 0.45f;
            mStatWeights[ItemModType.ITEM_MOD_SPIRIT] = 0.01f;
            mStatWeights[ItemModType.ITEM_MOD_INTELLECT] = 0.01f;
            mStatWeights[ItemModType.ITEM_MOD_STRENGTH] = 0.6f;
            mStatWeights[ItemModType.ITEM_MOD_AGILITY] = 0.9f;
            mStatWeights[ItemModType.ITEM_MOD_MANA] = 0.3f;
            mStatWeights[ItemModType.ITEM_MOD_HEALTH] = 0.45f;
        }

        #endregion

        #region Hunter Constants

        public static class Spells
        {
            public const uint ARCANE_SHOT_1 = 3044;
            public const uint ASPECT_OF_THE_BEAST_1 = 13161;
            public const uint ASPECT_OF_THE_CHEETAH_1 = 5118;
            public const uint ASPECT_OF_THE_HAWK_1 = 13165;
            public const uint ASPECT_OF_THE_MONKEY_1 = 13163;
            public const uint ASPECT_OF_THE_PACK_1 = 13159;
            public const uint ASPECT_OF_THE_WILD_1 = 20043;
            public const uint AUTO_SHOT_1 = 75;
            public const uint BEAST_LORE_1 = 1462;
            public const uint CALL_PET_1 = 883;
            public const uint CONCUSSIVE_SHOT_1 = 5116;
            public const uint DETERRENCE_1 = 19263;
            public const uint DISENGAGE_1 = 781;
            public const uint DISMISS_PET_1 = 2641;
            public const uint DISTRACTING_SHOT_1 = 20736;
            public const uint EAGLE_EYE_1 = 6197;
            public const uint EXPLOSIVE_TRAP_1 = 13813;
            public const uint EYES_OF_THE_BEAST_1 = 1002;
            public const uint FEED_PET_1 = 6991;
            public const uint FEIGN_DEATH_1 = 5384;
            public const uint FLARE_1 = 1543;
            public const uint FREEZING_TRAP_1 = 1499;
            public const uint FROST_TRAP_1 = 13809;
            public const uint HUNTERS_MARK_1 = 1130;
            public const uint IMMOLATION_TRAP_1 = 13795;
            public const uint MEND_PET_1 = 136;
            public const uint MONGOOSE_BITE_1 = 1495;
            public const uint MULTISHOT_1 = 2643;
            public const uint RAPID_FIRE_1 = 3045;
            public const uint RAPTOR_STRIKE_1 = 2973;
            public const uint REVIVE_PET_1 = 982;
            public const uint SCARE_BEAST_1 = 1513;
            public const uint SCORPID_STING_1 = 3043;
            public const uint SERPENT_STING_1 = 1978;
            public const uint TAME_BEAST_1 = 1515;
            public const uint TRACK_BEASTS_1 = 1494;
            public const uint TRACK_DEMONS_1 = 19878;
            public const uint TRACK_DRAGONKIN_1 = 19879;
            public const uint TRACK_ELEMENTALS_1 = 19880;
            public const uint TRACK_GIANTS_1 = 19882;
            public const uint TRACK_HIDDEN_1 = 19885;
            public const uint TRACK_HUMANOIDS_1 = 19883;
            public const uint TRACK_UNDEAD_1 = 19884;
            public const uint TRANQUILIZING_SHOT_1 = 19801;
            public const uint VIPER_STING_1 = 3034;
            public const uint VOLLEY_1 = 1510;
            public const uint WING_CLIP_1 = 2974;
            public const uint AIMED_SHOT_1 = 19434;
            public const uint BESTIAL_WRATH_1 = 19574;
            public const uint BLACK_ARROW_1 = 3674;
            public const uint COUNTERATTACK_1 = 19306;
            public const uint INTIMIDATION_1 = 19577;
            public const uint READINESS_1 = 23989;
            public const uint SCATTER_SHOT_1 = 19503;
            public const uint TRUESHOT_AURA_1 = 19506;
            public const uint WYVERN_STING_1 = 19386;
        }

        #endregion
    }
}
