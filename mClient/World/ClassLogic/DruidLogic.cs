using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class DruidLogic : PlayerClassLogic
    {
        #region Declarations

        // druid cat/bear/dire bear/moonkin/tree of life forms
        protected uint CAT_FORM,
               BEAR_FORM,
               DIRE_BEAR_FORM,
               MOONKIN_FORM,
               TRAVEL_FORM,
               AQUATIC_FORM,
               TREE_FORM;

        // druid cat attacks
        protected uint CLAW,
               COWER,
               TIGERS_FURY,
               RAKE,
               RIP,
               SHRED,
               FEROCIOUS_BITE;

        // druid bear/dire bear attacks & buffs
        protected uint BASH,
               MAUL,
               SWIPE,
               DEMORALIZING_ROAR,
               CHALLENGING_ROAR,
               GROWL,
               ENRAGE,
               FAERIE_FIRE_FERAL,
               FERAL_CHARGE_BEAR,
               POUNCE,
               PROWL,
               RAVAGE;

        // druid caster DPS attacks & debuffs
        protected uint MOONFIRE,
               ROOTS,
               WRATH,
               OMEN_OF_CLARITY,
               STARFIRE,
               INSECT_SWARM,
               FAERIE_FIRE,
               HIBERNATE,
               ENTANGLING_ROOTS,
               HURRICANE,
               NATURES_GRASP,
               SOOTHE_ANIMAL;

        // druid buffs
        protected uint MARK_OF_THE_WILD,
               GIFT_OF_THE_WILD,
               THORNS,
               INNERVATE,
               NATURES_SWIFTNESS,
               BARKSKIN,
               DASH,
               FRENZIED_GENERATION;

        // druid heals
        protected uint REJUVENATION,
               REGROWTH,
               HEALING_TOUCH,
               SWIFTMEND,
               TRANQUILITY,
               REBIRTH,
               REMOVE_CURSE,
               ABOLISH_POISON,
               CURE_POISON;

        // procs
        protected uint ECLIPSE,
            ECLIPSE_SOLAR,
            ECLIPSE_LUNAR;

        #endregion

        #region Constructors

        public DruidLogic(Player player) : base(player)
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
                // TODO: Dependant upon spec
                return true;
            }
        }

        #endregion

        #region Public Methods

        public override void InitializeSpells()
        {
            base.InitializeSpells();

            // Spells
            ABOLISH_POISON = InitSpell(Spells.ABOLISH_POISON_1);
            AQUATIC_FORM = InitSpell(Spells.AQUATIC_FORM_1);
            BARKSKIN = InitSpell(Spells.BARKSKIN_1);
            BASH = InitSpell(Spells.BASH_1);
            BEAR_FORM = InitSpell(Spells.BEAR_FORM_1);
            CAT_FORM = InitSpell(Spells.CAT_FORM_1);
            CHALLENGING_ROAR = InitSpell(Spells.CHALLENGING_ROAR_1);
            CLAW = InitSpell(Spells.CLAW_1);
            COWER = InitSpell(Spells.COWER_1);
            CURE_POISON = InitSpell(Spells.CURE_POISON_1);
            DASH = InitSpell(Spells.DASH_1);
            DEMORALIZING_ROAR = InitSpell(Spells.DEMORALIZING_ROAR_1);
            DIRE_BEAR_FORM = InitSpell(Spells.DIRE_BEAR_FORM_1);
            ENRAGE = InitSpell(Spells.ENRAGE_1);
            ENTANGLING_ROOTS = InitSpell(Spells.ENTANGLING_ROOTS_1);
            FAERIE_FIRE = InitSpell(Spells.FAERIE_FIRE_1);
            FAERIE_FIRE_FERAL = InitSpell(Spells.FAERIE_FIRE_FERAL_1);
            FERAL_CHARGE_BEAR = InitSpell(Spells.FERAL_CHARGE_BEAR_1);
            FEROCIOUS_BITE = InitSpell(Spells.FEROCIOUS_BITE_1);
            FRENZIED_GENERATION = InitSpell(Spells.FRENZIED_REGENERATION_1);
            GIFT_OF_THE_WILD = InitSpell(Spells.GIFT_OF_THE_WILD_1);
            GROWL = InitSpell(Spells.GROWL_1);
            HEALING_TOUCH = InitSpell(Spells.HEALING_TOUCH_1);
            HIBERNATE = InitSpell(Spells.HIBERNATE_1);
            HURRICANE = InitSpell(Spells.HURRICANE_1);
            INNERVATE = InitSpell(Spells.INNERVATE_1);
            INSECT_SWARM = InitSpell(Spells.INSECT_SWARM_1);
            MARK_OF_THE_WILD = InitSpell(Spells.MARK_OF_THE_WILD_1);
            MAUL = InitSpell(Spells.MAUL_1);
            MOONFIRE = InitSpell(Spells.MOONFIRE_1);
            MOONKIN_FORM = InitSpell(Spells.MOONKIN_FORM_1);
            NATURES_GRASP = InitSpell(Spells.NATURES_GRASP_1);
            NATURES_SWIFTNESS = InitSpell(Spells.NATURES_SWIFTNESS_DRUID_1);
            OMEN_OF_CLARITY = InitSpell(Spells.OMEN_OF_CLARITY_1);
            POUNCE = InitSpell(Spells.POUNCE_1);
            PROWL = InitSpell(Spells.PROWL_1);
            RAKE = InitSpell(Spells.RAKE_1);
            RAVAGE = InitSpell(Spells.RAVAGE_1);
            REBIRTH = InitSpell(Spells.REBIRTH_1);
            REGROWTH = InitSpell(Spells.REGROWTH_1);
            REJUVENATION = InitSpell(Spells.REJUVENATION_1);
            REMOVE_CURSE = InitSpell(Spells.REMOVE_CURSE_DRUID_1);
            RIP = InitSpell(Spells.RIP_1);
            SHRED = InitSpell(Spells.SHRED_1);
            SOOTHE_ANIMAL = InitSpell(Spells.SOOTHE_ANIMAL_1);
            STARFIRE = InitSpell(Spells.STARFIRE_1);
            SWIFTMEND = InitSpell(Spells.SWIFTMEND_1);
            SWIPE = InitSpell(Spells.SWIPE_BEAR_1);
            THORNS = InitSpell(Spells.THORNS_1);
            TIGERS_FURY = InitSpell(Spells.TIGERS_FURY_1);
            TRANQUILITY = InitSpell(Spells.TRANQUILITY_1);
            TRAVEL_FORM = InitSpell(Spells.TRAVEL_FORM_1);
            WRATH = InitSpell(Spells.WRATH_1);
            ECLIPSE = InitSpell(Spells.ECLIPSE_1);

            // Procs
            ECLIPSE_LUNAR = InitSpell(Procs.ECLIPSE_LUNAR_1);
            ECLIPSE_SOLAR = InitSpell(Procs.ECLIPSE_SOLAR_1);

        }

        #endregion

        #region Druid Constants

        public static class Reagents
        {
            public const uint WILD_THORNROOT = 17026;
        }

        public static class Procs
        {
            public const uint ECLIPSE_SOLAR_1 = 48517;
            public const uint ECLIPSE_LUNAR_1 = 48518;
        }

        public static class Spells
        {
            public const uint ABOLISH_POISON_1 = 2893;
            public const uint AQUATIC_FORM_1 = 1066;
            public const uint BARKSKIN_1 = 22812;
            public const uint BASH_1 = 5211;
            public const uint BEAR_FORM_1 = 5487;
            public const uint CAT_FORM_1 = 768;
            public const uint CHALLENGING_ROAR_1 = 5209;
            public const uint CLAW_1 = 1082;
            public const uint COWER_1 = 8998;
            public const uint CURE_POISON_1 = 8946;
            public const uint DASH_1 = 1850;
            public const uint DEMORALIZING_ROAR_1 = 99;
            public const uint DIRE_BEAR_FORM_1 = 9634;
            public const uint ENRAGE_1 = 5229;
            public const uint ENTANGLING_ROOTS_1 = 339;
            public const uint FAERIE_FIRE_1 = 770;
            public const uint FAERIE_FIRE_FERAL_1 = 16857;
            public const uint FERAL_CHARGE_BEAR_1 = 16979;
            public const uint FEROCIOUS_BITE_1 = 22568;
            public const uint FRENZIED_REGENERATION_1 = 22842;
            public const uint GIFT_OF_THE_WILD_1 = 21849;
            public const uint GROWL_1 = 6795;
            public const uint HEALING_TOUCH_1 = 5185;
            public const uint HIBERNATE_1 = 2637;
            public const uint HURRICANE_1 = 16914;
            public const uint INNERVATE_1 = 29166;
            public const uint INSECT_SWARM_1 = 5570;
            public const uint MARK_OF_THE_WILD_1 = 1126;
            public const uint MAUL_1 = 6807;
            public const uint MOONFIRE_1 = 8921;
            public const uint MOONKIN_FORM_1 = 24858;
            public const uint NATURES_GRASP_1 = 16689;
            public const uint NATURES_SWIFTNESS_DRUID_1 = 17116;
            public const uint OMEN_OF_CLARITY_1 = 16864;
            public const uint POUNCE_1 = 9005;
            public const uint PROWL_1 = 5215;
            public const uint RAKE_1 = 1822;
            public const uint RAVAGE_1 = 6785;
            public const uint REBIRTH_1 = 20484;
            public const uint REGROWTH_1 = 8936;
            public const uint REJUVENATION_1 = 774;
            public const uint REMOVE_CURSE_DRUID_1 = 2782;
            public const uint RIP_1 = 1079;
            public const uint SHRED_1 = 5221;
            public const uint SOOTHE_ANIMAL_1 = 2908;
            public const uint STARFIRE_1 = 2912;
            public const uint SWIFTMEND_1 = 18562;
            public const uint SWIPE_BEAR_1 = 779;
            public const uint THORNS_1 = 467;
            public const uint TIGERS_FURY_1 = 5217;
            public const uint TRANQUILITY_1 = 740;
            public const uint TRAVEL_FORM_1 = 783;
            public const uint WRATH_1 = 5176;

            public const uint ECLIPSE_1 = 48525;
        }

        #endregion
    }
}
