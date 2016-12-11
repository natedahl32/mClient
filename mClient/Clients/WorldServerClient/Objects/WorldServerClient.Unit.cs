using mClient.Constants;
using mClient.DBC;
using mClient.Shared;
using mClient.World;
using mClient.World.Creature;
using mClient.World.Items;
using mClient.World.Spells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mClient.Clients
{
    public class Unit : Object
    {
        #region Declarations

        // Default movement speed modifier
        private const float DEFAULT_MOVEMENT_SPEED_MODIFIER = 1.0f;

        private float mMovementSpeedModifier;
        private List<uint> mTrainerSpellsAvailable;
        private List<VendorItem> mVendorItemsAvailable;
        private System.Object mVendorItemsLock = new System.Object();

        // Holds aura durations
        protected uint[] mAuraDurations = new uint[SpellConstants.MAX_AURAS];

        #endregion

        #region Constructors

        public Unit(WoWGuid guid) : base(guid)
        {
            mMovementSpeedModifier = DEFAULT_MOVEMENT_SPEED_MODIFIER;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the monster movement manager
        /// </summary>
        public NpcMoveMgr MonsterMovement { get; set; }

        /// <summary>
        /// Gets or sets whether or not this object is an npc
        /// </summary>
        public bool IsNPC { get; set; }

        /// <summary>
        /// Gets whether or not the unit is a vendor
        /// </summary>
        public bool IsVendor
        {
            get { return ((NPCFlags)GetFieldValue((int)UnitFields.UNIT_NPC_FLAGS)).HasFlag(NPCFlags.UNIT_NPC_FLAG_VENDOR); }
        }

        /// <summary>
        /// Gets whether or not the unit has gossip
        /// </summary>
        public bool IsGossip
        {
            get { return ((NPCFlags)GetFieldValue((int)UnitFields.UNIT_NPC_FLAGS)).HasFlag(NPCFlags.UNIT_NPC_FLAG_GOSSIP); }
        }

        /// <summary>
        /// Gets whether or not the unit is a repair npc
        /// </summary>
        public bool IsRepair
        {
            get { return ((NPCFlags)GetFieldValue((int)UnitFields.UNIT_NPC_FLAGS)).HasFlag(NPCFlags.UNIT_NPC_FLAG_REPAIR); }
        }

        /// <summary>
        /// Gets whether or not this unit is a quest giver
        /// </summary>
        public bool IsQuestGiver
        {
            get { return ((NPCFlags)GetFieldValue((int)UnitFields.UNIT_NPC_FLAGS)).HasFlag(NPCFlags.UNIT_NPC_FLAG_QUESTGIVER); }
        }

        /// <summary>
        /// Gets whether or not this unit is a flight master
        /// </summary>
        public bool IsFlightMaster
        {
            get { return ((NPCFlags)GetFieldValue((int)UnitFields.UNIT_NPC_FLAGS)).HasFlag(NPCFlags.UNIT_NPC_FLAG_FLIGHTMASTER); }
        }

        /// <summary>
        /// Gets whether or not this unit is a trainer
        /// </summary>
        public bool IsTrainer
        {
            get { return ((NPCFlags)GetFieldValue((int)UnitFields.UNIT_NPC_FLAGS)).HasFlag(NPCFlags.UNIT_NPC_FLAG_TRAINER); }
        }

        /// <summary>
        /// Gets whether or not this unit is an innkeeper
        /// </summary>
        public bool IsInnkeeper
        {
            get { return ((NPCFlags)GetFieldValue((int)UnitFields.UNIT_NPC_FLAGS)).HasFlag(NPCFlags.UNIT_NPC_FLAG_INNKEEPER); }
        }

        /// <summary>
        /// Gets whether or not this unit is a banker
        /// </summary>
        public bool IsBanker
        {
            get { return ((NPCFlags)GetFieldValue((int)UnitFields.UNIT_NPC_FLAGS)).HasFlag(NPCFlags.UNIT_NPC_FLAG_BANKER); }
        }

        /// <summary>
        /// Gets whether or not this unit is an auctioneer
        /// </summary>
        public bool IsAuctioneer
        {
            get { return ((NPCFlags)GetFieldValue((int)UnitFields.UNIT_NPC_FLAGS)).HasFlag(NPCFlags.UNIT_NPC_FLAG_AUCTIONEER); }
        }

        /// <summary>
        /// Gets whether or not this unit is a stable master
        /// </summary>
        public bool IsStableMaster
        {
            get { return ((NPCFlags)GetFieldValue((int)UnitFields.UNIT_NPC_FLAGS)).HasFlag(NPCFlags.UNIT_NPC_FLAG_STABLEMASTER); }
        }

        /// <summary>
        /// Gets the base CreatureInfo for this unit if it is a creature
        /// </summary>
        public CreatureInfo BaseCreatureInfo { get { return CreatureManager.Instance.Get(this.ObjectFieldEntry); } }

        /// <summary>
        /// Gets the max health for this unit
        /// </summary>
        public UInt32 MaxHealth
        {
            get { return GetFieldValue((int)UnitFields.UNIT_FIELD_MAXHEALTH); }
            private set { SetField((int)UnitFields.UNIT_FIELD_MAXHEALTH, value); }
        }

        /// <summary>
        /// Gets the current health for this unit
        /// </summary>
        public UInt32 CurrentHealth
        {
            get { return GetFieldValue((int)UnitFields.UNIT_FIELD_HEALTH); }
            private set { SetField((int)UnitFields.UNIT_FIELD_HEALTH, value); }
        }

        /// <summary>
        /// Gets the current health percentage for this unit
        /// </summary>
        public float HealthPercentage
        {
            get { return (CurrentHealth * 100.0f) / MaxHealth; }
        }

        /// <summary>
        /// Gets the current mana value for this unit
        /// </summary>
        public uint CurrentMana
        {
            get { return GetCurrentPower(Powers.POWER_MANA); }
        }

        /// <summary>
        /// Gets the maximum mana value for this unit
        /// </summary>
        public uint MaximumMana
        {
            get { return GetMaximumPower(Powers.POWER_MANA); }
        }

        /// <summary>
        /// Gets the current mana percentage for this unit
        /// </summary>
        public float ManaPercentage
        {
            get { return (CurrentMana * 100.0f) / MaximumMana; }
        }

        /// <summary>
        /// Gets the current rage value for this unit
        /// </summary>
        public uint CurrentRage
        {
            get { return GetCurrentPower(Powers.POWER_RAGE); }
        }

        /// <summary>
        /// Gets the maximum rage value for this unit
        /// </summary>
        public uint MaximumRage
        {
            get { return GetMaximumPower(Powers.POWER_RAGE); }
        }

        /// <summary>
        /// Gets the current focus value for this unit
        /// </summary>
        public uint CurrentFocus
        {
            get { return GetCurrentPower(Powers.POWER_FOCUS); }
        }

        /// <summary>
        /// Gets the maximum focus value for this unit
        /// </summary>
        public uint MaximumFocus
        {
            get { return GetMaximumPower(Powers.POWER_FOCUS); }
        }

        /// <summary>
        /// Gets the current energy value for this unit
        /// </summary>
        public uint CurrentEnergy
        {
            get { return GetCurrentPower(Powers.POWER_ENERGY); }
        }

        /// <summary>
        /// Gets the maximum energy value for this unit
        /// </summary>
        public uint MaximumEnergy
        {
            get { return GetMaximumPower(Powers.POWER_ENERGY); }
        }

        /// <summary>
        /// Gets the current happiness value for this unit
        /// </summary>
        public uint CurrentHappiness
        {
            get { return GetCurrentPower(Powers.POWER_HAPPINESS); }
        }

        /// <summary>
        /// Gets the maximum happiness value for this unit
        /// </summary>
        public uint MaximumHappiness
        {
            get { return GetMaximumPower(Powers.POWER_HAPPINESS); }
        }

        /// <summary>
        /// Gets the current level of this unit
        /// </summary>
        public UInt32 Level
        {
            get { return GetFieldValue((int)UnitFields.UNIT_FIELD_LEVEL); }
            private set { SetField((int)UnitFields.UNIT_FIELD_LEVEL, value); }
        }

        /// <summary>
        /// Returns spell ids of the auras currently on a player
        /// </summary>
        public IEnumerable<AuraHolder> Auras
        {
            get
            {
                var auras = new List<AuraHolder>();
                for (int i = (int)UnitFields.UNIT_FIELD_AURA; i <= (int)UnitFields.UNIT_FIELD_AURA_LAST; i++)
                {
                    if (GetFieldValue(i) > 0)
                    {
                        var holder = new AuraHolder();
                        holder.Spell = SpellTable.Instance.getSpell(GetFieldValue(i));
                        if (holder.Spell != null)
                        {
                            var auraSlot = i - (int)UnitFields.UNIT_FIELD_AURA;
                            holder.Duration = mAuraDurations[auraSlot] / 1000.0f;
                            holder.Stacks = GetAuraStacks((byte)auraSlot);
                            auras.Add(holder);
                        }
                    }
                }
                return auras;
            }
        }

        /// <summary>
        /// Gets whether or not the unit is currently dead. Can be if the unit has no current health left
        /// or if they have the ghost buff (in case of player)
        /// </summary>
        public bool IsDead
        {
            get
            {
                // If we have no health left but we do have some max health (avoids death triggered when we login right away and haven't gotten data yet)
                if (CurrentHealth <= 0 && MaxHealth > 0)
                    return true;
                if (Auras.Any(a => a.Spell.SpellId == SpellAuras.GHOST_1 || a.Spell.SpellId == SpellAuras.GHOST_2 || a.Spell.SpellId == SpellAuras.GHOST_WISP))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Gets whether or not the unit is moving
        /// </summary>
        public virtual bool IsMoving
        {
            get
            {
                if (IsNPC && MonsterMovement != null)
                    return MonsterMovement.IsMoving;
                return false;
            }
        }

        /// <summary>
        /// Gets or sets whether or not this unit has been looted (if it is lootable)
        /// </summary>
        public bool HasBeenLooted { get; set; }

        /// <summary>
        /// Gets this units target guid
        /// </summary>
        public WoWGuid TargetGuid
        {
            get
            {
                var value1 = GetFieldValue((int)UnitFields.UNIT_FIELD_TARGET);
                var value2 = GetFieldValue(((int)UnitFields.UNIT_FIELD_TARGET) + 1);
                var guid = GetGuid(value1, value2);
                return new WoWGuid(guid);
            }
        }

        /// <summary>
        /// Gets the cast speed mod for this unit
        /// </summary>
        public float CastSpeedMod
        {
            get { return GetFieldValueAsFloat((int)UnitFields.UNIT_MOD_CAST_SPEED); }
        }

        /// <summary>
        /// Gets the percent increase in speed the unit should move
        /// </summary>
        public float MovementSpeedModifier
        {
            get { return mMovementSpeedModifier; }
        }

        /// <summary>
        /// Gets the stand state of the unit
        /// </summary>
        public UnitStandStateType StandState
        {
            get
            {
                var val = GetFieldValue((int)UnitFields.UNIT_FIELD_BYTES_1);
                // Stand state is the first byte of this field
                var bytes = BitConverter.GetBytes(val);
                return (UnitStandStateType)bytes[0];
            }
        }

        /// <summary>
        /// Gets all spells that are available from this trainer. If null is returned we have not yet received spells that are available from this trainer
        /// </summary>
        public IEnumerable<uint> TrainerSpellsAvailable
        {
            get
            {
                return mTrainerSpellsAvailable;
            }
        }

        /// <summary>
        /// Gets all vendor items available from this vendor. If null, that means we have not received a list from this vendor yet for their items OR this unit is not a vendor.
        /// </summary>
        public IEnumerable<VendorItem> VendorItemsAvailable
        {
            get { return mVendorItemsAvailable; }
        }

        /// <summary>
        /// Gets the id of the mount displayed for this unit
        /// </summary>
        public uint MoutDisplayId
        {
            get { return GetFieldValue((int)UnitFields.UNIT_FIELD_MOUNTDISPLAYID); }
        }

        /// <summary>
        /// Gets whether or not this unit is mounted
        /// </summary>
        public bool IsMounted
        {
            get { return MoutDisplayId > 0; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if the unit has an aura defined by spell id
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        public bool HasAura(uint spellId)
        {
            return Auras.Any(a => a.Spell.SpellId == spellId);
        }

        /// <summary>
        /// Sets the mouted flag to off for the unit
        /// </summary>
        public void Unmounted()
        {
            SetField((int)UnitFields.UNIT_FIELD_MOUNTDISPLAYID, 0);
        }

        /// <summary>
        /// Adds a percentage to the movement speed modifier. 
        /// </summary>
        /// <param name="percentage">Percentage to add</param>
        /// <remarks>100% is the default modifier so to make a 70% increase in movement speed the percentage needs to be 170. Or to make a 30% decrease the percentage
        /// needs to be 70.</remarks>
        public void AddMovementSpeedModifierPercentage(int percentage)
        {
            float modValue = percentage / 100;
            mMovementSpeedModifier *= modValue;
        }

        /// <summary>
        /// Removes a percentage from the movement speed modifier
        /// </summary>
        /// <param name="percentage">Percentage to remove</param>
        public void RemoveMovementSpeedModifierPercentage(int percentage)
        {
            float modValue = percentage / 100;
            mMovementSpeedModifier /= modValue;
        }

        /// <summary>
        /// Removes any auras
        /// </summary>
        /// <param name="spellId">Id of spell to remove</param>
        public void RemoveAura(int spellId)
        {
            for (int i = (int)UnitFields.UNIT_FIELD_AURA; i <= (int)UnitFields.UNIT_FIELD_AURA_LAST; i++)
                if (GetFieldValue(i) > 0 && GetFieldValue(i) == spellId)
                    SetField(i, 0);
        }

        public void Update(uint currentHealth, uint maxHealth, uint level, uint currentPower, uint maxPower)
        {
            this.CurrentHealth = currentHealth;
            this.MaxHealth = maxHealth;
            this.Level = level;
            // TODO: Do we even need power?
        }

        /// <summary>
        /// Gets the current power type value
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public uint GetCurrentPower(Powers power)
        {
            return GetFieldValue((int)UnitFields.UNIT_FIELD_POWER1 + (int)power);
        }

        /// <summary>
        /// Gets the maximum power type value
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public uint GetMaximumPower(Powers power)
        {
            return GetFieldValue((int)UnitFields.UNIT_FIELD_MAXPOWER1 + (int)power);
        }

        /// <summary>
        /// Gets whether or not this unit can cast the given spell based on their power amount
        /// </summary>
        /// <param name="spell"></param>
        /// <returns></returns>
        public bool CanCastSpell(SpellEntry spell)
        {
            var cost = CalculateSpellPowerCost(spell);
            return GetCurrentPower(spell.PowerType) >= cost;
        }

        /// <summary>
        /// Called when a spell is cast by a unit. Updates the power of the unit depending on the spell
        /// </summary>
        /// <param name="spell"></param>
        public void SpellCast(SpellEntry spell)
        {
            // decrease power used for spell
            DecreasePower(spell.PowerType, CalculateSpellPowerCost(spell));
        }

        /// <summary>
        /// Gets the attack time for a weapon
        /// </summary>
        /// <param name="att"></param>
        /// <returns></returns>
        public uint GetAttackTime(WeaponAttackType att)
        {
            return (uint)(GetFieldValueAsFloat((int)UnitFields.UNIT_FIELD_BASEATTACKTIME + (int)att) / 1.0f); // modAttackSpeedPct is same for all weapon attack types
        }

        /// <summary>
        /// Adds spells that are available from a trainer to this trainer. This unit must be a trainer in order to use this method
        /// </summary>
        /// <param name="spellsAvailable">List of spell ids that are available</param>
        public void AddTrainerSpellsAvailable(IList<uint> spellsAvailable)
        {
            if (!IsTrainer) throw new ApplicationException($"Unable to add spells available to unit with entry {ObjectFieldEntry} because the unit is not a trainer.");
            mTrainerSpellsAvailable = spellsAvailable.ToList();
        }

        /// <summary>
        /// Updates the vendor items available from this vendor (unit must be a vendor or an error will be thrown)
        /// </summary>
        /// <param name="items"></param>
        public void UpdateVendorItems(IList<VendorItem> items)
        {
            if (!IsVendor) throw new ApplicationException("Trying to update vendor items for non-vendor Unit!");
            lock (mVendorItemsLock)
                mVendorItemsAvailable = items.ToList();
        }

        /// <summary>
        /// Updates aura durations for the player
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="duration"></param>
        /// <remarks>In Vanilla, we only know durations for our own auras</remarks>
        public void UpdateAuraDuration(byte slot, uint duration)
        {
            mAuraDurations[slot] = duration;
        }

        /// <summary>
        /// Updates our aura durations
        /// </summary>
        /// <param name="elapsed"></param>
        /// <remarks>In Vanilla, we only know durations for our own auras</remarks>
        public void UpdateAuraDurations(uint elapsed)
        {
            for (int i = 0; i < SpellConstants.MAX_AURAS; i++)
                if (mAuraDurations[i] > 0)
                    mAuraDurations[i] = Math.Max(0, mAuraDurations[i] - elapsed);
        }

        /// <summary>
        /// Updates the aura
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="aura"></param>
        public void UpdateAura(byte slot, uint aura)
        {
            SetField((int)UnitFields.UNIT_FIELD_AURA + slot, aura);
        }

        /// <summary>
        /// Gets the aura holder for a spell
        /// </summary>
        /// <param name="spell"></param>
        /// <returns></returns>
        public AuraHolder GetAuraForSpell(uint spell)
        {
            return Auras.Where(a => a.Spell.SpellId == spell).SingleOrDefault();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Decreases the power type by an amount
        /// </summary>
        /// <param name="power"></param>
        /// <param name="value"></param>
        private void DecreasePower(Powers power, uint value)
        {
            var current = GetCurrentPower(power);
            var updated = current - value;
            if (updated < 0)
                updated = 0;
            SetField((int)UnitFields.UNIT_FIELD_POWER1 + (int)power, updated);
        }

        /// <summary>
        /// Calculates the cost of a spell in power for this unit
        /// </summary>
        /// <param name="spell">Spell entry that we are calculating for</param>
        /// <returns></returns>
        /// <remarks>We use this to determine whether or not we can cast a spell</remarks>
        private uint CalculateSpellPowerCost(SpellEntry spell)
        {
            // Spell drain all existing power on cast (Paladin LoH for example)
            if (spell.HasAttribute(SpellAttributesEx.SPELL_ATTR_EX_DRAIN_ALL_POWER))
            {
                if (spell.PowerType == Powers.POWER_HEALTH)
                    return CurrentHealth;
                if ((int)spell.PowerType < PlayerConstants.MAX_POWERS)
                    return GetCurrentPower(spell.PowerType);
                return 0;
            }

            // Get base power cost
            int powerCost = (int)spell.ManaCost;
            if (spell.ManaCostPercentage > 0)
            {
                switch (spell.PowerType)
                {
                    case Powers.POWER_HEALTH:
                        powerCost += (int)(spell.ManaCostPercentage * GetFieldValue((int)UnitFields.UNIT_FIELD_BASE_HEALTH) / 100);
                        break;
                    case Powers.POWER_MANA:
                        powerCost += (int)(spell.ManaCostPercentage * GetFieldValue((int)UnitFields.UNIT_FIELD_BASE_MANA) / 100);
                        break;
                    case Powers.POWER_RAGE:
                    case Powers.POWER_FOCUS:
                    case Powers.POWER_ENERGY:
                    case Powers.POWER_HAPPINESS:
                        powerCost += (int)(spell.ManaCostPercentage * GetMaximumPower(spell.PowerType) / 100);
                        break;
                    default:
                        return 0;
                }
            }

            SpellSchoolMask schoolMask = (SpellSchoolMask)(1 << (int)spell.School);
            SpellSchools school = SpellEntry.GetFirstSchoolInMask(schoolMask);
            // Flat mod from caster auras by spell school
            powerCost += (int)(GetFieldValue((int)UnitFields.UNIT_FIELD_POWER_COST_MODIFIER + (int)school));
            // TODO: Apply cost mod by spell (Spell.cpp - Line #5367)

            if (spell.HasAttribute(SpellAttributes.SPELL_ATTR_LEVEL_DAMAGE_CALCULATION))
                powerCost = Convert.ToInt32((powerCost / (1.117f * spell.SpellLevel / Level - 0.1327f)));

            // PCT mod from user auras by school
            powerCost = (int)(powerCost * (1.0f + GetFieldValueAsFloat((int)UnitFields.UNIT_FIELD_POWER_COST_MULTIPLIER + (int)school)));
            if (powerCost < 0)
                powerCost = 0;
            return (uint)powerCost;
        }

        /// <summary>
        /// Get the number of stacks for an aura in a given slot
        /// </summary>
        /// <param name="auraSlot"></param>
        /// <returns></returns>
        protected byte GetAuraStacks(byte auraSlot)
        {
            // Aura stacks/applications are packed in bits of data in the AURAAPPLICATIONS field
            int index = auraSlot / 4;
            uint value = GetFieldValue((int)UnitFields.UNIT_FIELD_AURAAPPLICATIONS + index);
            // Get the byte number to retrieve the data for
            int byteNumber = (auraSlot % 4);
            byte[] bytes = BitConverter.GetBytes(value);
            return (byte)(bytes[byteNumber] + 1);
        }

        #endregion
    }
}
