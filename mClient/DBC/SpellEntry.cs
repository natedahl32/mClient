using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.DBC
{
    public class SpellEntry
    {
        #region Properties

        public uint SpellId { get; set; }
        public uint School { get; set; }
        public uint Category { get; set; }
        public uint Dispel { get; set; }
        public uint Mechanic { get; set; }
        public SpellAttributes Attributes { get; set; }
        public SpellAttributesEx AttributesEx { get; set; }
        public SpellAttributesEx2 AttributesEx2 { get; set; }
        public SpellAttributesEx3 AttributesEx3 { get; set; }
        public SpellAttributesEx4 AttributesEx4 { get; set; }
        public uint Stances { get; set; }
        public uint StancesNot { get; set; }
        public uint Targets { get; set; }
        public uint TargetCreatureType { get; set; }
        public uint RequiresSpellFocus { get; set; }
        public uint CasterAuraState { get; set; }
        public uint TargetAuraState { get; set; }
        public uint CastingTimeIndex { get; set; }
        public uint RecoveryTime { get; set; }
        public uint CategoryRecoveryTime { get; set; }
        public uint InterruptFlags { get; set; }
        public uint AuraInterruptFlags { get; set; }
        public uint ChannelInterruptFlags { get; set; }
        public uint ProcFlags { get; set; }
        public uint ProcChance { get; set; }
        public uint ProcCharges { get; set; }
        public uint MaxLevel { get; set; }
        public uint BaseLevel { get; set; }
        public uint SpellLevel { get; set; }
        public uint DurationIndex { get; set; }
        public Powers PowerType { get; set; }
        public uint ManaCost { get; set; }
        public uint ManaCostPerLevel { get; set; }
        public uint ManaPerSecond { get; set; }
        public uint ManaPerSecondPerLevel { get; set; }
        public uint RangeIndex { get; set; }
        public float Speed { get; set; }
        public uint ModalSpellNext { get; set; }
        public uint StackAmount { get; set; }
        public uint[] Totem { get; set; }
        public int[] Reagents { get; set; }
        public uint[] ReagentsCount { get; set; }
        public int EquippedItemClass { get; set; }
        public int EquippedItemSubClassMask { get; set; }
        public int EquippedItemInventoryTypeMask { get; set; }
        public SpellEffects[] Effect { get; set; }        
        public int[] EffectDieSides { get; set; }
        public uint[] EffectBaseDice { get; set; }
        public float[] EffectDicePerLevel { get; set; }
        public float[] EffectRealPointsPerLevel { get; set; }
        public int[] EffectBasePoints { get; set; }
        public uint[] EffectMechanic { get; set; }
        public uint[] EffectImplicitTargetA { get; set; }
        public uint[] EffectImplicitTargetB { get; set; }
        public uint[] EffectRadiusIndex { get; set; }
        public uint[] EffectApplyAuraName { get; set; }
        public uint[] EffectAmplitude { get; set; }
        public float[] EffectMultipleValue { get; set; }
        public uint[] EffectChainTarget { get; set; }
        public uint[] EffectItemType { get; set; }
        public int[] EffectMiscValue { get; set; }
        public uint[] EffectTriggerSpell { get; set; }
        public float[] EffectPointsPerComboPoint { get; set; }
        public uint SpellVisual { get; set; }
        public uint SpellIconID { get; set; }
        public uint ActiveIconID { get; set; }
        public string SpellName { get; set; }
        public string Rank { get; set; }
        public uint ManaCostPercentage { get; set; }
        public uint StartRecoveryCategory { get; set; }
        public uint StartRecoveryTime { get; set; }
        public uint MaxTargetLevel { get; set; }
        public uint SpellFamilyName { get; set; }
        public uint SpellFamilyFlags1 { get; set; }
        public uint SpellFamilyFlags2 { get; set; }
        public uint MaxAffectedTargets { get; set; }
        public uint DamageClass { get; set; }
        public uint PreventionType { get; set; }
        public float[] DamageMultiplier { get; set; }

        /// <summary>
        /// Gets whether or not the spell affects all members of a group or not
        /// </summary>
        public bool IsGroupSpell
        {
            get
            {
                for (int i = 0; i < SpellConstants.MAX_EFFECT_INDEX; i++)
                {
                    // Check implicit target A
                    if (EffectImplicitTargetA[i] == (uint)Constants.Targets.TARGET_ALL_PARTY ||
                        EffectImplicitTargetA[i] == (uint)Constants.Targets.TARGET_ALL_PARTY_AROUND_CASTER ||
                        EffectImplicitTargetA[i] == (uint)Constants.Targets.TARGET_ALL_PARTY_AROUND_CASTER_2 ||
                        EffectImplicitTargetA[i] == (uint)Constants.Targets.TARGET_AREAEFFECT_PARTY)
                        return true;

                    // Not sure if we need to check implicit target B also, but doesn't hurt to try right?
                    if (EffectImplicitTargetB[i] == (uint)Constants.Targets.TARGET_ALL_PARTY ||
                        EffectImplicitTargetB[i] == (uint)Constants.Targets.TARGET_ALL_PARTY_AROUND_CASTER ||
                        EffectImplicitTargetB[i] == (uint)Constants.Targets.TARGET_ALL_PARTY_AROUND_CASTER_2 ||
                        EffectImplicitTargetB[i] == (uint)Constants.Targets.TARGET_AREAEFFECT_PARTY)
                        return true;
                }

                return false;
            }
        }

        #endregion

        #region Public Methods

        public bool HasAttribute(SpellAttributes attribute)
        {
            return Attributes.HasFlag(attribute);
        }

        public bool HasAttribute(SpellAttributesEx attribute)
        {
            return AttributesEx.HasFlag(attribute);
        }

        public bool HasAttribute(SpellAttributesEx2 attribute)
        {
            return AttributesEx2.HasFlag(attribute);
        }

        public bool HasAttribute(SpellAttributesEx3 attribute)
        {
            return AttributesEx3.HasFlag(attribute);
        }

        public bool HasAttribute(SpellAttributesEx4 attribute)
        {
            return AttributesEx4.HasFlag(attribute);
        }

        #endregion

        #region Static Methods

        public static SpellSchools GetFirstSchoolInMask(SpellSchoolMask mask)
        {
            for (int i = 0; i < SpellConstants.MAX_SPELL_SCHOOL; i++)
                if (mask.HasFlag((SpellSchoolMask)(1 << i)))
                    return (SpellSchools)i;
            return SpellSchools.SPELL_SCHOOL_NORMAL;
        }

        #endregion
    }
}
