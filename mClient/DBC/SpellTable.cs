using mClient.Constants;
using mClient.Shared;
using System.Collections.Generic;

namespace mClient.DBC
{
    public class SpellTable : DBCFile
    {
        private Dictionary<uint, SpellEntry> mSpellEntries = new Dictionary<uint, SpellEntry>();

        #region Singleton

        static readonly SpellTable instance = new SpellTable();

        static SpellTable() { }

        SpellTable() : base(@"Spell.dbc")
        { }

        public static SpellTable Instance { get { return instance; } }

        #endregion

        protected override void dataLoaded()
        {
            for (uint i = 0; i < Records; i++)
            {
                var entry = new SpellEntry();
                entry.SpellId = getFieldAsUint32(i, 0);
                entry.School = getFieldAsUint32(i, 1);
                entry.Category = getFieldAsUint32(i, 2);
                entry.Dispel = getFieldAsUint32(i, 4);
                entry.Mechanic = getFieldAsUint32(i, 5);
                entry.Attributes = (SpellAttributes)getFieldAsUint32(i, 6);
                entry.AttributesEx = (SpellAttributesEx)getFieldAsUint32(i, 7);
                entry.AttributesEx2 = (SpellAttributesEx2)getFieldAsUint32(i, 8);
                entry.AttributesEx3 = (SpellAttributesEx3)getFieldAsUint32(i, 9);
                entry.AttributesEx4 = (SpellAttributesEx4)getFieldAsUint32(i, 10);
                entry.Stances = getFieldAsUint32(i, 11);
                entry.StancesNot = getFieldAsUint32(i, 12);
                entry.Targets = getFieldAsUint32(i, 13);
                entry.TargetCreatureType = getFieldAsUint32(i, 14);
                entry.RequiresSpellFocus = getFieldAsUint32(i, 15);
                entry.CasterAuraState = getFieldAsUint32(i, 16);
                entry.TargetAuraState = getFieldAsUint32(i, 17);
                entry.CastingTimeIndex = getFieldAsUint32(i, 18);
                entry.RecoveryTime = getFieldAsUint32(i, 19);
                entry.CategoryRecoveryTime = getFieldAsUint32(i, 20);
                entry.InterruptFlags = getFieldAsUint32(i, 21);
                entry.AuraInterruptFlags = getFieldAsUint32(i, 22);
                entry.ChannelInterruptFlags = getFieldAsUint32(i, 23);
                entry.ProcFlags = getFieldAsUint32(i, 24);
                entry.ProcChance = getFieldAsUint32(i, 25);
                entry.ProcCharges = getFieldAsUint32(i, 26);
                entry.MaxLevel = getFieldAsUint32(i, 27);
                entry.BaseLevel = getFieldAsUint32(i, 28);
                entry.SpellLevel = getFieldAsUint32(i, 29);
                entry.DurationIndex = getFieldAsUint32(i, 30);
                entry.PowerType = (Powers)getFieldAsUint32(i, 31);
                entry.ManaCost = getFieldAsUint32(i, 32);
                entry.ManaCostPerLevel = getFieldAsUint32(i, 33);
                entry.ManaPerSecond = getFieldAsUint32(i, 34);
                entry.ManaPerSecondPerLevel = getFieldAsUint32(i, 35);
                entry.RangeIndex = getFieldAsUint32(i, 36);
                entry.Speed = getFieldAsFloat(i, 37);
                entry.ModalSpellNext = getFieldAsUint32(i, 38);
                entry.StackAmount = getFieldAsUint32(i, 39);
                entry.Totem = new uint[2];
                entry.Totem[0] = getFieldAsUint32(i, 40);
                entry.Totem[1] = getFieldAsUint32(i, 41);
                entry.Reagents = new int[8];
                entry.Reagents[0] = getFieldAsInt32(i, 42);
                entry.Reagents[1] = getFieldAsInt32(i, 43);
                entry.Reagents[2] = getFieldAsInt32(i, 44);
                entry.Reagents[3] = getFieldAsInt32(i, 45);
                entry.Reagents[4] = getFieldAsInt32(i, 46);
                entry.Reagents[5] = getFieldAsInt32(i, 47);
                entry.Reagents[6] = getFieldAsInt32(i, 48);
                entry.Reagents[7] = getFieldAsInt32(i, 49);
                entry.ReagentsCount = new uint[8];
                entry.ReagentsCount[0] = getFieldAsUint32(i, 50);
                entry.ReagentsCount[1] = getFieldAsUint32(i, 51);
                entry.ReagentsCount[2] = getFieldAsUint32(i, 52);
                entry.ReagentsCount[3] = getFieldAsUint32(i, 53);
                entry.ReagentsCount[4] = getFieldAsUint32(i, 54);
                entry.ReagentsCount[5] = getFieldAsUint32(i, 55);
                entry.ReagentsCount[6] = getFieldAsUint32(i, 56);
                entry.ReagentsCount[7] = getFieldAsUint32(i, 57);
                entry.EquippedItemClass = getFieldAsInt32(i, 58);
                entry.EquippedItemSubClassMask = getFieldAsInt32(i, 59);
                entry.EquippedItemInventoryTypeMask = getFieldAsInt32(i, 60);
                entry.Effect = new uint[3];
                entry.Effect[0] = getFieldAsUint32(i, 61);
                entry.Effect[1] = getFieldAsUint32(i, 62);
                entry.Effect[2] = getFieldAsUint32(i, 63);
                entry.EffectDieSides = new int[3];
                entry.EffectDieSides[0] = getFieldAsInt32(i, 64);
                entry.EffectDieSides[1] = getFieldAsInt32(i, 65);
                entry.EffectDieSides[2] = getFieldAsInt32(i, 66);
                entry.EffectBaseDice = new uint[3];
                entry.EffectBaseDice[0] = getFieldAsUint32(i, 67);
                entry.EffectBaseDice[1] = getFieldAsUint32(i, 68);
                entry.EffectBaseDice[2] = getFieldAsUint32(i, 69);
                entry.EffectDicePerLevel = new float[3];
                entry.EffectDicePerLevel[0] = getFieldAsFloat(i, 70);
                entry.EffectDicePerLevel[1] = getFieldAsFloat(i, 71);
                entry.EffectDicePerLevel[2] = getFieldAsFloat(i, 72);
                entry.EffectRealPointsPerLevel = new float[3];
                entry.EffectRealPointsPerLevel[0] = getFieldAsFloat(i, 73);
                entry.EffectRealPointsPerLevel[1] = getFieldAsFloat(i, 74);
                entry.EffectRealPointsPerLevel[2] = getFieldAsFloat(i, 75);
                entry.EffectBasePoints = new int[3];
                entry.EffectBasePoints[0] = getFieldAsInt32(i, 76);
                entry.EffectBasePoints[1] = getFieldAsInt32(i, 77);
                entry.EffectBasePoints[2] = getFieldAsInt32(i, 78);
                entry.EffectMechanic = new uint[3];
                entry.EffectMechanic[0] = getFieldAsUint32(i, 79);
                entry.EffectMechanic[1] = getFieldAsUint32(i, 80);
                entry.EffectMechanic[2] = getFieldAsUint32(i, 81);
                entry.EffectImplicitTargetA = new uint[3];
                entry.EffectImplicitTargetA[0] = getFieldAsUint32(i, 82);
                entry.EffectImplicitTargetA[1] = getFieldAsUint32(i, 83);
                entry.EffectImplicitTargetA[2] = getFieldAsUint32(i, 84);
                entry.EffectImplicitTargetB = new uint[3];
                entry.EffectImplicitTargetB[0] = getFieldAsUint32(i, 85);
                entry.EffectImplicitTargetB[1] = getFieldAsUint32(i, 86);
                entry.EffectImplicitTargetB[2] = getFieldAsUint32(i, 87);
                entry.EffectRadiusIndex = new uint[3];
                entry.EffectRadiusIndex[0] = getFieldAsUint32(i, 88);
                entry.EffectRadiusIndex[1] = getFieldAsUint32(i, 89);
                entry.EffectRadiusIndex[2] = getFieldAsUint32(i, 90);
                entry.EffectApplyAuraName = new uint[3];
                entry.EffectApplyAuraName[0] = getFieldAsUint32(i, 91);
                entry.EffectApplyAuraName[0] = getFieldAsUint32(i, 92);
                entry.EffectApplyAuraName[0] = getFieldAsUint32(i, 93);
                entry.EffectAmplitude = new uint[3];
                entry.EffectAmplitude[0] = getFieldAsUint32(i, 94);
                entry.EffectAmplitude[1] = getFieldAsUint32(i, 95);
                entry.EffectAmplitude[2] = getFieldAsUint32(i, 96);
                entry.EffectMultipleValue = new float[3];
                entry.EffectMultipleValue[0] = getFieldAsFloat(i, 97);
                entry.EffectMultipleValue[1] = getFieldAsFloat(i, 98);
                entry.EffectMultipleValue[2] = getFieldAsFloat(i, 99);
                entry.EffectChainTarget = new uint[3];
                entry.EffectChainTarget[0] = getFieldAsUint32(i, 100);
                entry.EffectChainTarget[1] = getFieldAsUint32(i, 101);
                entry.EffectChainTarget[2] = getFieldAsUint32(i, 102);
                entry.EffectItemType = new uint[3];
                entry.EffectItemType[0] = getFieldAsUint32(i, 103);
                entry.EffectItemType[1] = getFieldAsUint32(i, 104);
                entry.EffectItemType[2] = getFieldAsUint32(i, 105);
                entry.EffectMiscValue = new int[3];
                entry.EffectMiscValue[0] = getFieldAsInt32(i, 106);
                entry.EffectMiscValue[1] = getFieldAsInt32(i, 107);
                entry.EffectMiscValue[2] = getFieldAsInt32(i, 108);
                entry.EffectTriggerSpell = new uint[3];
                entry.EffectTriggerSpell[0] = getFieldAsUint32(i, 109);
                entry.EffectTriggerSpell[1] = getFieldAsUint32(i, 110);
                entry.EffectTriggerSpell[2] = getFieldAsUint32(i, 111);
                entry.EffectPointsPerComboPoint = new float[3];
                entry.EffectPointsPerComboPoint[0] = getFieldAsFloat(i, 112);
                entry.EffectPointsPerComboPoint[0] = getFieldAsFloat(i, 113);
                entry.EffectPointsPerComboPoint[0] = getFieldAsFloat(i, 114);
                entry.SpellVisual = getFieldAsUint32(i, 115);
                entry.SpellIconID = getFieldAsUint32(i, 117);
                entry.ActiveIconID = getFieldAsUint32(i, 118);
                entry.SpellName = getStringForField(i, 120);
                entry.Rank = getStringForField(i, 129);
                entry.ManaCostPercentage = getFieldAsUint32(i, 156);
                entry.StartRecoveryCategory = getFieldAsUint32(i, 157);
                entry.StartRecoveryTime = getFieldAsUint32(i, 158);
                entry.MaxTargetLevel = getFieldAsUint32(i, 159);
                entry.SpellFamilyName = getFieldAsUint32(i, 160);
                entry.SpellFamilyFlags1 = getFieldAsUint32(i, 161);
                entry.SpellFamilyFlags2 = getFieldAsUint32(i, 162);
                entry.MaxAffectedTargets = getFieldAsUint32(i, 163);
                entry.DamageClass = getFieldAsUint32(i, 164);
                entry.PreventionType = getFieldAsUint32(i, 165);
                entry.DamageMultiplier = new float[3];
                entry.DamageMultiplier[0] = getFieldAsFloat(i, 167);
                entry.DamageMultiplier[1] = getFieldAsFloat(i, 168);
                entry.DamageMultiplier[2] = getFieldAsFloat(i, 169);

                mSpellEntries.Add(entry.SpellId, entry);
            }
        }

        public SpellEntry getSpell(uint spellId)
        {
            return mSpellEntries[spellId];
        }

        public string getSpellName(uint spellId)
        {
            return mSpellEntries[spellId].SpellName;
        }
    }
}
