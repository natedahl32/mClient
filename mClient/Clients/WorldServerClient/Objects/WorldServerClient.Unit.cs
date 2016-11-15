﻿using mClient.Constants;
using mClient.Shared;
using mClient.World;
using mClient.World.Creature;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mClient.Clients
{
    public class Unit : Object
    {
        public Unit(WoWGuid guid) : base(guid)
        {
        }

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
        public IEnumerable<UInt32> Auras
        {
            get
            {
                var auras = new List<UInt32>();
                for (int i = (int)UnitFields.UNIT_FIELD_AURA; i <= (int)UnitFields.UNIT_FIELD_AURA_LAST; i++)
                    if (GetFieldValue(i) > 0)
                        auras.Add(GetFieldValue(i));
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
                if (Auras.Any(a => a == SpellAuras.GHOST_1 || a == SpellAuras.GHOST_2 || a == SpellAuras.GHOST_WISP))
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

        #endregion

        #region Public Methods

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

        #endregion
    }
}