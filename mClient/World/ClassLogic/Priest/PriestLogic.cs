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
    public class PriestLogic : PlayerClassLogic
    {
        #region Declarations

        // holy
        protected uint CLEARCASTING,
               DESPERATE_PRAYER,
               FLASH_HEAL,
               GREATER_HEAL,
               HEAL,
               HOLY_FIRE,
               HOLY_NOVA,
               LESSER_HEAL,
               MANA_BURN,
               PRAYER_OF_HEALING,
               RENEW,
               RESURRECTION,
               SHACKLE_UNDEAD,
               SMITE,
               CURE_DISEASE,
               ABOLISH_DISEASE,
               PRIEST_DISPEL_MAGIC;

        // ranged
        protected uint SHOOT;

        // shadowmagic
        protected uint FADE,
               SHADOW_WORD_PAIN,
               MIND_BLAST,
               SCREAM,
               MIND_FLAY,
               DEVOURING_PLAGUE,
               SHADOW_PROTECTION,
               PRAYER_OF_SHADOW_PROTECTION,
               SHADOWFORM,
               VAMPIRIC_EMBRACE;

        // discipline
        protected uint POWER_WORD_SHIELD,
               INNER_FIRE,
               POWER_WORD_FORTITUDE,
               PRAYER_OF_FORTITUDE,
               FEAR_WARD,
               POWER_INFUSION,
               MASS_DISPEL,
               DIVINE_SPIRIT,
               PRAYER_OF_SPIRIT,
               INNER_FOCUS,
               ELUNES_GRACE,
               LEVITATE,
               LIGHTWELL,
               MIND_CONTROL,
               MIND_SOOTHE,
               MIND_VISION,
               PSYCHIC_SCREAM,
               SILENCE;

        #endregion

        #region Constructors

        public PriestLogic(Player player) : base(player)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of this class
        /// </summary>
        public override string ClassName
        {
            get { return "Priest"; }
        }

        /// <summary>
        /// Gets whether or not the player has any buffs to give out (including self buffs)
        /// </summary>
        public override bool HasOOCBuffs
        {
            get
            {
                // Inner fire
                if (INNER_FIRE > 0) return true;
                // PW:F
                if (POWER_WORD_FORTITUDE > 0) return true;
                // Prayer of Fortitude
                if (PRAYER_OF_FORTITUDE > 0) return true;
                // Shadow protection
                if (SHADOW_PROTECTION > 0) return true;
                // Prayer of Shadow Protection
                if (PRAYER_OF_SHADOW_PROTECTION > 0) return true;

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

                // Inner fire on self
                if (HasSpellAndCanCast(INNER_FIRE) && !Player.HasAura(INNER_FIRE))
                    needBuffs.Add(Spell(INNER_FIRE), new List<Player>() { Player });

                // Check 
                if (Player.CurrentGroup != null && Player.CurrentGroup.PlayersInGroup.Count() > 0)
                    foreach (var groupMember in Player.CurrentGroup.PlayersInGroup)
                    {
                        // Prayer of Fortitude
                        if (HasSpellAndCanCast(PRAYER_OF_FORTITUDE))
                        {
                            var pf = Spell(PRAYER_OF_FORTITUDE);
                            // TODO: Make sure group member is in range of spell as well
                            // TODO: Make sure group member does not have BETTER buff than the one we are casting
                            if (!groupMember.HasAura(POWER_WORD_FORTITUDE) && !groupMember.HasAura(PRAYER_OF_FORTITUDE))
                                needBuffs.AddOrUpdateDictionaryList(pf, groupMember);
                        }
                        // Power Word: Fortitude
                        else if (HasSpellAndCanCast(POWER_WORD_FORTITUDE))
                        {
                            var pwf = Spell(POWER_WORD_FORTITUDE);
                            // TODO: Make sure group member is in range of spell as well
                            // TODO: Make sure group member does not have BETTER buff than the one we are casting
                            if (!groupMember.HasAura(POWER_WORD_FORTITUDE) && !groupMember.HasAura(PRAYER_OF_FORTITUDE))
                                needBuffs.AddOrUpdateDictionaryList(pwf, groupMember);
                        }

                        // Prayer of Shadow Protection
                        if (HasSpellAndCanCast(PRAYER_OF_SHADOW_PROTECTION))
                        {
                            var psp = Spell(PRAYER_OF_SHADOW_PROTECTION);
                            // TODO: Make sure group member is in range of spell as well
                            // TODO: Make sure group member does not have BETTER buff than the one we are casting
                            if (!groupMember.HasAura(SHADOW_PROTECTION) && !groupMember.HasAura(PRAYER_OF_SHADOW_PROTECTION))
                                needBuffs.AddOrUpdateDictionaryList(psp, groupMember);
                        }
                        // Shadow Protection
                        else if (HasSpellAndCanCast(SHADOW_PROTECTION))
                        {
                            var sp = Spell(SHADOW_PROTECTION);
                            // TODO: Make sure group member is in range of spell as well
                            // TODO: Make sure group member does not have BETTER buff than the one we are casting
                            if (!groupMember.HasAura(SHADOW_PROTECTION) && !groupMember.HasAura(PRAYER_OF_SHADOW_PROTECTION))
                                needBuffs.AddOrUpdateDictionaryList(sp, groupMember);
                        }
                    }

                return needBuffs;
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
                var currentTarget = Player.PlayerAI.TargetSelection;
                if (currentTarget == null)
                    return null;

                // Shadow Word Pain
                if (HasSpellAndCanCast(SHADOW_WORD_PAIN) && !currentTarget.HasAura(SHADOW_WORD_PAIN)) return Spell(SHADOW_WORD_PAIN);
                // Smite
                if (HasSpellAndCanCast(SMITE) && !currentTarget.HasAura(SMITE)) return Spell(SMITE);

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
            ABOLISH_DISEASE = InitSpell(Spells.ABOLISH_DISEASE_1);
            CURE_DISEASE = InitSpell(Spells.CURE_DISEASE_1);
            DESPERATE_PRAYER = InitSpell(Spells.DESPERATE_PRAYER_1);
            DEVOURING_PLAGUE = InitSpell(Spells.DEVOURING_PLAGUE_1);
            PRIEST_DISPEL_MAGIC = InitSpell(Spells.DISPEL_MAGIC_1);
            DIVINE_SPIRIT = InitSpell(Spells.DIVINE_SPIRIT_1);
            ELUNES_GRACE = InitSpell(Spells.ELUNES_GRACE_1);
            FADE = InitSpell(Spells.FADE_1);
            FEAR_WARD = InitSpell(Spells.FEAR_WARD_1);
            FLASH_HEAL = InitSpell(Spells.FLASH_HEAL_1);
            GREATER_HEAL = InitSpell(Spells.GREATER_HEAL_1);
            HEAL = InitSpell(Spells.HEAL_1);
            HOLY_FIRE = InitSpell(Spells.HOLY_FIRE_1);
            HOLY_NOVA = InitSpell(Spells.HOLY_NOVA_1);
            INNER_FIRE = InitSpell(Spells.INNER_FIRE_1);
            INNER_FOCUS = InitSpell(Spells.INNER_FOCUS_1);
            LESSER_HEAL = InitSpell(Spells.LESSER_HEAL_1);
            LEVITATE = InitSpell(Spells.LEVITATE_1);
            LIGHTWELL = InitSpell(Spells.LIGHTWELL_1);
            MANA_BURN = InitSpell(Spells.MANA_BURN_1);
            MIND_BLAST = InitSpell(Spells.MIND_BLAST_1);
            MIND_CONTROL = InitSpell(Spells.MIND_CONTROL_1);
            MIND_FLAY = InitSpell(Spells.MIND_FLAY_1);
            MIND_SOOTHE = InitSpell(Spells.MIND_SOOTHE_1);
            MIND_VISION = InitSpell(Spells.MIND_VISION_1);
            POWER_INFUSION = InitSpell(Spells.POWER_INFUSION_1);
            POWER_WORD_FORTITUDE = InitSpell(Spells.POWER_WORD_FORTITUDE_1);
            POWER_WORD_SHIELD = InitSpell(Spells.POWER_WORD_SHIELD_1);
            PRAYER_OF_FORTITUDE = InitSpell(Spells.PRAYER_OF_FORTITUDE_1);
            PRAYER_OF_HEALING = InitSpell(Spells.PRAYER_OF_HEALING_1);
            PRAYER_OF_SHADOW_PROTECTION = InitSpell(Spells.PRAYER_OF_SHADOW_PROTECTION_1);
            PRAYER_OF_SPIRIT = InitSpell(Spells.PRAYER_OF_SPIRIT_1);
            PSYCHIC_SCREAM = InitSpell(Spells.PSYCHIC_SCREAM_1);
            RENEW = InitSpell(Spells.RENEW_1);
            RESURRECTION = InitSpell(Spells.RESURRECTION_1);
            SHACKLE_UNDEAD = InitSpell(Spells.SHACKLE_UNDEAD_1);
            SHADOW_PROTECTION = InitSpell(Spells.SHADOW_PROTECTION_1);
            SHADOW_WORD_PAIN = InitSpell(Spells.SHADOW_WORD_PAIN_1);
            SHADOWFORM = InitSpell(Spells.SHADOWFORM_1);
            SHOOT = InitSpell(Spells.SHOOT_1);
            SMITE = InitSpell(Spells.SMITE_1);
            SILENCE = InitSpell(Spells.SILENCE_1);
            VAMPIRIC_EMBRACE = InitSpell(Spells.VAMPIRIC_EMBRACE_1);
        }

        public override float CompareItems(ItemInfo item1, ItemInfo item2)
        {
            // Get the base value of the compare
            var baseCompare = base.CompareItems(item1, item2);

            float item1Score = 0f;
            float item2Score = 0f;

            if (item1.ItemClass == ItemClass.ITEM_CLASS_WEAPON && item2.ItemClass == ItemClass.ITEM_CLASS_WEAPON)
            {
                if (item1.SubClass == (int)ItemSubclassWeapon.ITEM_SUBCLASS_WEAPON_WAND)
                    item1Score += (item1.DPS * 0.9f);
                if (item2.SubClass == (int)ItemSubclassWeapon.ITEM_SUBCLASS_WEAPON_WAND)
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

            if (Player.TalentSpec == MainSpec.PRIEST_SPEC_DISCIPLINE || Player.TalentSpec == MainSpec.PRIEST_SPEC_HOLY)
            {
                mStatWeights[ItemModType.ITEM_MOD_STAMINA] = 0.45f;
                mStatWeights[ItemModType.ITEM_MOD_SPIRIT] = 0.6f;
                mStatWeights[ItemModType.ITEM_MOD_INTELLECT] = 0.9f;
                mStatWeights[ItemModType.ITEM_MOD_STRENGTH] = 0.01f;
                mStatWeights[ItemModType.ITEM_MOD_AGILITY] = 0.01f;
                mStatWeights[ItemModType.ITEM_MOD_MANA] = 0.85f;
                mStatWeights[ItemModType.ITEM_MOD_HEALTH] = 0.5f;
            }
            else if (Player.TalentSpec == MainSpec.PRIEST_SPEC_SHADOW)
            {
                mStatWeights[ItemModType.ITEM_MOD_STAMINA] = 0.45f;
                mStatWeights[ItemModType.ITEM_MOD_SPIRIT] = 0.35f;
                mStatWeights[ItemModType.ITEM_MOD_INTELLECT] = 0.9f;
                mStatWeights[ItemModType.ITEM_MOD_STRENGTH] = 0.01f;
                mStatWeights[ItemModType.ITEM_MOD_AGILITY] = 0.01f;
                mStatWeights[ItemModType.ITEM_MOD_MANA] = 0.55f;
                mStatWeights[ItemModType.ITEM_MOD_HEALTH] = 0.5f;
            }
        }

        #endregion

        #region Priest Constants

        public static class Reagents
        {
            public const uint SACRED_CANDLE = 17029;
        }

        public static class Spells
        {
            public const uint ABOLISH_DISEASE_1 = 552;
            public const uint CURE_DISEASE_1 = 528;
            public const uint DESPERATE_PRAYER_1 = 19236;
            public const uint DEVOURING_PLAGUE_1 = 2944;
            public const uint DISPEL_MAGIC_1 = 527;
            public const uint DIVINE_SPIRIT_1 = 14752;
            public const uint ELUNES_GRACE_1 = 2651;
            public const uint FADE_1 = 586;
            public const uint FEAR_WARD_1 = 6346;
            public const uint FLASH_HEAL_1 = 2061;
            public const uint GREATER_HEAL_1 = 2060;
            public const uint HEAL_1 = 2054;
            public const uint HOLY_FIRE_1 = 14914;
            public const uint HOLY_NOVA_1 = 15237;
            public const uint INNER_FIRE_1 = 588;
            public const uint INNER_FOCUS_1 = 14751;
            public const uint LESSER_HEAL_1 = 2050;
            public const uint LEVITATE_1 = 1706;
            public const uint LIGHTWELL_1 = 724;
            public const uint MANA_BURN_1 = 8129;
            public const uint MIND_BLAST_1 = 8092;
            public const uint MIND_CONTROL_1 = 605;
            public const uint MIND_FLAY_1 = 15407;
            public const uint MIND_SOOTHE_1 = 453;
            public const uint MIND_VISION_1 = 2096;
            public const uint POWER_INFUSION_1 = 10060;
            public const uint POWER_WORD_FORTITUDE_1 = 1243;
            public const uint POWER_WORD_SHIELD_1 = 17;
            public const uint PRAYER_OF_FORTITUDE_1 = 21562;
            public const uint PRAYER_OF_HEALING_1 = 596;
            public const uint PRAYER_OF_SHADOW_PROTECTION_1 = 27683;
            public const uint PRAYER_OF_SPIRIT_1 = 27681;
            public const uint PSYCHIC_SCREAM_1 = 8122;
            public const uint RENEW_1 = 139;
            public const uint RESURRECTION_1 = 2006;
            public const uint SHACKLE_UNDEAD_1 = 9484;
            public const uint SHADOW_PROTECTION_1 = 976;
            public const uint SHADOW_WORD_PAIN_1 = 589;
            public const uint SHADOWFORM_1 = 15473;
            public const uint SHOOT_1 = 5019;
            public const uint SILENCE_1 = 15487;
            public const uint SMITE_1 = 585;
            public const uint VAMPIRIC_EMBRACE_1 = 15286;
            public const uint WEAKNED_SOUL = 6788;
        }

        #endregion
    }
}
