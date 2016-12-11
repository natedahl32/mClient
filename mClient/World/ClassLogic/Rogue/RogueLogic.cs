using mClient.Clients;
using mClient.Constants;
using mClient.DBC;
using mClient.World.Items;
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

        // Poisons
        protected static IList<uint> INSTANT_POISONS = new List<uint> { Poisons.INSTANT_POISON_6,
                                                                 Poisons.INSTANT_POISON_5,
                                                                 Poisons.INSTANT_POISON_4,
                                                                 Poisons.INSTANT_POISON_3,
                                                                 Poisons.INSTANT_POISON_2,
                                                                 Poisons.INSTANT_POISON_1};
        protected static IList<uint> DEADLY_POISONS = new List<uint> { Poisons.DEADLY_POISON_5,
                                                                 Poisons.DEADLY_POISON_4,
                                                                 Poisons.DEADLY_POISON_3,
                                                                 Poisons.DEADLY_POISON_2,
                                                                 Poisons.DEADLY_POISON_1};
        protected static IList<uint> CRIPPLING_POISONS = new List<uint> { Poisons.CRIPPLING_POISON_2,
                                                                 Poisons.CRIPPLING_POISON_1};
        protected static IList<uint> MIND_NUMBING_POISONS = new List<uint> { Poisons.MIND_NUMBING_POISON_3,
                                                                 Poisons.MIND_NUMBING_POISON_2,
                                                                 Poisons.MIND_NUMBING_POISON_1};
        protected static IList<uint> WOUND_POISONS = new List<uint> { Poisons.WOUND_POISON_4,
                                                                 Poisons.WOUND_POISON_3,
                                                                 Poisons.WOUND_POISON_2,
                                                                 Poisons.WOUND_POISON_1};

        #endregion

        #region Constructors

        public RogueLogic(Player player) : base(player)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of this class
        /// </summary>
        public override string ClassName
        {
            get { return "Rogue"; }
        }

        /// <summary>
        /// Gets whether or not the player has any buffs to give out (including self buffs)
        /// </summary>
        public override bool HasOOCBuffs
        {
            get
            {
                // Check for poisons
                if (GetPoisonForMH() > 0 || GetPoisonForOH() > 0)
                    return true;

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
                var needBuffs = new Dictionary<SpellEntry, IList<Player>>();


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
                var currentTarget = Player.PlayerAI.TargetSelection;
                if (currentTarget == null)
                    return null;

                var comboPoints = Player.PlayerObject.ComboPoints;
                var currentEnergy = Player.PlayerObject.CurrentEnergy;

                // If we are maxed on combo points but we don't have enough energy for a finisher, we need to wait for energy
                if (comboPoints == 5 && currentEnergy < 25)
                {
                    // TODO: Can we do anything to increase our energy? ability? item?
                    return null;
                }

                // Slice and Dice is our top priority, use it with any number of combo points if it is not up
                if (comboPoints > 0 && HasSpellAndCanCast(SLICE_DICE) && !Player.HasAura(SLICE_DICE)) return Spell(SLICE_DICE);

                // Get the slice and dice aura if it is up and evaluate time left on it
                var sliceAndDiceAura = Player.PlayerObject.GetAuraForSpell(SLICE_DICE);
                if (sliceAndDiceAura != null)
                {
                    // If we have a second or less left on slice and dice and we have combo points, refresh it
                    if (sliceAndDiceAura.Duration <= 1.0f && comboPoints > 0)
                        if (HasSpellAndCanCast(SLICE_DICE))
                            return Spell(SLICE_DICE);
                    
                    if (sliceAndDiceAura.Duration <= 3.0f && currentEnergy < 25)
                    {
                        // If we don't have the energy to recast and we have 3 seconds or less left on it, just wait to regenerate some energy
                        if (currentEnergy < 25) return null;
                        // If we have 4 or more combo points, go ahead and refresh it early
                        if (comboPoints >= 4)
                            if (HasSpellAndCanCast(SLICE_DICE))
                                return Spell(SLICE_DICE);
                    }
                }

                // If we have 5 combo points, use a finisher
                if (comboPoints == 5)
                {
                    // Eviscerate
                    if (HasSpellAndCanCast(EVISCERATE)) return Spell(EVISCERATE);
                    // TODO: Rupture? Maybe based on spec?
                }

                // Sinister Strike
                if (HasSpellAndCanCast(SINISTER_STRIKE)) return Spell(SINISTER_STRIKE);
                // TODO: Backstab? Maybye based on spec?

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

        public override float CompareItems(ItemInfo item1, ItemInfo item2)
        {
            // Get the base value of the compare
            var baseCompare = base.CompareItems(item1, item2);

            float item1Score = 0f;
            float item2Score = 0f;

            if (item1.ItemClass == ItemClass.ITEM_CLASS_WEAPON && item2.ItemClass == ItemClass.ITEM_CLASS_WEAPON)
            {
                // TODO: Calculate slow vs fast on mainhand/offhand weapons
                item1Score += (item1.DPS * 0.9f);
                item2Score += (item2.DPS * 0.9f);
            }

            // Reduce armor score so it isn't overvalued. There can be a lot on items
            float item1Armor = item1.Resistances[SpellSchools.SPELL_SCHOOL_NORMAL] / 20f;
            float item2Armor = item2.Resistances[SpellSchools.SPELL_SCHOOL_NORMAL] / 20f;

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

            mStatWeights[ItemModType.ITEM_MOD_STAMINA] = 0.5f;
            mStatWeights[ItemModType.ITEM_MOD_SPIRIT] = 0.01f;
            mStatWeights[ItemModType.ITEM_MOD_INTELLECT] = 0.01f;
            mStatWeights[ItemModType.ITEM_MOD_STRENGTH] = 0.8f;
            mStatWeights[ItemModType.ITEM_MOD_AGILITY] = 0.9f;
            mStatWeights[ItemModType.ITEM_MOD_MANA] = 0.01f;
            mStatWeights[ItemModType.ITEM_MOD_HEALTH] = 0.5f;
        }

        /// <summary>
        /// Gets the poison to use for main hand
        /// </summary>
        /// <returns></returns>
        protected virtual uint GetPoisonForMH()
        {
            uint poison = 0;
            poison = GetPoison(INSTANT_POISONS);
            // Don't use other poisons unless we are solo so we don't take up a debuff slot
            if (!Player.IsInGroup)
            {
                if (poison == 0) poison = GetPoison(CRIPPLING_POISONS);
                if (poison == 0) poison = GetPoison(DEADLY_POISONS);
                if (poison == 0) poison = GetPoison(MIND_NUMBING_POISONS);
                if (poison == 0) poison = GetPoison(WOUND_POISONS);
            }
            
            return poison;
        }

        /// <summary>
        /// Gets the poison to use for offhand
        /// </summary>
        /// <returns></returns>
        protected virtual uint GetPoisonForOH()
        {
            uint poison = 0;
            // If we are in group play, use instant poison in offhand as well. Otherwise don't use a poison in
            // group play so we don't take up an unneeded debuff slot.
            if (Player.IsInGroup)
                poison = GetPoison(INSTANT_POISONS);
            else
            {
                if (poison == 0) poison = GetPoison(CRIPPLING_POISONS);
                if (poison == 0) poison = GetPoison(INSTANT_POISONS);
                if (poison == 0) poison = GetPoison(DEADLY_POISONS);
                if (poison == 0) poison = GetPoison(MIND_NUMBING_POISONS);
                if (poison == 0) poison = GetPoison(WOUND_POISONS);
            }

            return poison;
        }

        /// <summary>
        /// Gets the best poison type that we have and can cast
        /// </summary>
        /// <returns></returns>
        protected uint GetPoison(IList<uint> poisons)
        {
            foreach (var p in poisons)
            {
                // TODO: Check if we can cast it? Maybe we have item in inventory but can't use it due to level?
                if (Player.PlayerObject.HasItemInInventory(p))
                    return p;
            }

            return 0;
        }

        #endregion

        #region Rogue Constants

        public static class Poisons
        {
            public const uint INSTANT_POISON_1 = 6947;
            public const uint INSTANT_POISON_2 = 6949;
            public const uint INSTANT_POISON_3 = 6950;
            public const uint INSTANT_POISON_4 = 8926;
            public const uint INSTANT_POISON_5 = 8927;
            public const uint INSTANT_POISON_6 = 8928;

            public const uint CRIPPLING_POISON_1 = 3775;
            public const uint CRIPPLING_POISON_2 = 3776;

            public const uint DEADLY_POISON_1 = 2892;
            public const uint DEADLY_POISON_2 = 2893;
            public const uint DEADLY_POISON_3 = 8984;
            public const uint DEADLY_POISON_4 = 8985;
            public const uint DEADLY_POISON_5 = 20844;

            public const uint MIND_NUMBING_POISON_1 = 5237;
            public const uint MIND_NUMBING_POISON_2 = 6951;
            public const uint MIND_NUMBING_POISON_3 = 9186;

            public const uint WOUND_POISON_1 = 10918;
            public const uint WOUND_POISON_2 = 10920;
            public const uint WOUND_POISON_3 = 10921;
            public const uint WOUND_POISON_4 = 10922;
        }

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
