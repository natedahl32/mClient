﻿using mClient.Constants;
using mClient.Shared;
using PObject = mClient.Clients.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mClient.Clients;
using mClient.World.AI;
using mClient.World.Quest;
using mClient.DBC;
using mClient.World.AI.Activity.Messages;
using mClient.World.AI.Activity.Movement;
using mClient.World.Spells;

namespace mClient.World
{
    /// <summary>
    /// Player class represents a PC in the game.
    /// </summary>
    public class Player
    {
        #region Declarations

        private IList<Proficiency> mProficiencies = new List<Proficiency>();
        private PlayerObj mPlayerObject = null;
        private Corpse mPlayerCorpse = null;

        // Group the player is in and group member data for this player
        private Group mGroup = null;
        private GroupMemberData mGroupData = null;

        // Player AI and Logic
        private PlayerAI mPlayerAI = null;
        private PlayerChatHandler mPlayerChatHandler = null;
        private PlayerClassLogic mClassLogic = null;

        // Enemy
        private List<WoWGuid> mEnemyList = new List<WoWGuid>();

        // Spells
        private List<UInt16> mSpellList = new List<UInt16>();
        private List<SpellEntry> mAvailableSpells = new List<SpellEntry>(); // spells that are available, but we do not have yet
        private GlobalCooldown mGCD;
        private SpellCooldownManager mSpellCooldownManager;

        // Movement commands
        private MoveCommands mMoveCommand = MoveCommands.None;
        private PlayerObj mIssuedMoveCommand = null;

        // Quest givers
        private List<QuestGiver> mQuestGivers = new List<QuestGiver>();
        private System.Object mQuestGiversLock = new System.Object();

        // Loot
        private List<WoWGuid> mLootable = new List<WoWGuid>();

        #endregion

        #region Constructors

        public Player(PlayerObj playerObject)
        {
            if (playerObject == null) throw new ArgumentNullException("playerObject");
            mPlayerObject = playerObject;
            mPlayerChatHandler = new PlayerChatHandler(this);
        }

        public Player(WoWGuid guid, string name)
        {
            if (guid == null) throw new ArgumentNullException("guid");
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            mPlayerObject = (PlayerObj)PObject.CreateObjectByType(guid, ObjectType.Player);
            mPlayerObject.Name = name;
        }

        public Player(WoWGuid guid, string name, byte race, byte pClass, byte level, UInt32 mapId, byte gender, UInt32 guildId, UInt32 cFlags) : 
            this(guid, name)
        {
            this.Race = race;
            this.Class = pClass;
            this.Level = level;
            this.MapID = mapId;
            this.Gender = gender;
            this.GuildId = guildId;
            this.CharacterFlags = cFlags;
        }

        public Player(PlayerObj playerObject, byte race, byte pClass, byte level, UInt32 mapId, byte gender, UInt32 guildId, UInt32 cFlags, WorldServerClient client) :
            this(playerObject)
        {
            this.mPlayerAI = new PlayerAI(this, client);
            this.mClassLogic = PlayerClassLogic.CreateClassLogic((Classname)pClass, this);
            this.mGCD = new GlobalCooldown(this);
            this.mSpellCooldownManager = new SpellCooldownManager(this);

            this.Race = race;
            this.Class = pClass;
            this.Level = level;
            this.MapID = mapId;
            this.Gender = gender;
            this.GuildId = guildId;
            this.CharacterFlags = cFlags;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the player object for this player
        /// </summary>
        public PlayerObj PlayerObject { get { return mPlayerObject; } }

        /// <summary>
        /// Gets the guid for the player
        /// </summary>
        public WoWGuid Guid { get { return PlayerObject.Guid; } }

        /// <summary>
        /// Gets the position of the player
        /// </summary>
        public Coordinate Position { get { return PlayerObject.Position; } }

        /// <summary>
        /// Gets the name of the player
        /// </summary>
        public string Name { get { return PlayerObject.Name; } }

        /// <summary>
        /// Gets the race of the player
        /// </summary>
        public byte Race { get; set; }

        /// <summary>
        /// Gets the class of the player
        /// </summary>
        public byte Class { get; set; }

        /// <summary>
        /// Gets the Level of the player
        /// </summary>
        public byte Level { get; set; }

        /// <summary>
        /// Gets the ID of the map of the player
        /// </summary>
        public UInt32 MapID { get; set; }

        /// <summary>
        /// Gets the Gender of the player
        /// </summary>
        public byte Gender { get; set; }

        /// <summary>
        /// Gets the current guild id of the player
        /// </summary>
        public UInt32 GuildId { get; set; }

        /// <summary>
        /// Gets the character flags of the player
        /// </summary>
        public UInt32 CharacterFlags { get; set; }

        /// <summary>
        /// Gets the players current group
        /// </summary>
        public Group CurrentGroup { get { return mGroup; } }

        /// <summary>
        /// Gets the group member data
        /// </summary>
        public GroupMemberData GroupData { get { return mGroupData; } set { mGroupData = value; } }

        /// <summary>
        /// Gets item proficiencies for this player
        /// </summary>
        public IEnumerable<Proficiency> Proficiencies
        {
            get { return mProficiencies; }
        }

        /// <summary>
        /// Gets whether or not the player is in combat
        /// </summary>
        public bool IsInCombat { get { return mEnemyList.Count > 0; } }

        /// <summary>
        /// Gets all enemies currently in combat with the player
        /// </summary>
        public IEnumerable<WoWGuid> EnemyList { get { return mEnemyList; } }

        /// <summary>
        /// Gets the player AI for this player
        /// </summary>
        public PlayerAI PlayerAI { get { return mPlayerAI; } }

        /// <summary>
        /// Gets the class logic for this player
        /// </summary>
        public PlayerClassLogic ClassLogic { get { return mClassLogic; } }

        /// <summary>
        /// Gets the players corpse
        /// </summary>
        public Corpse PlayerCorpse { get { return mPlayerCorpse; } }

        /// <summary>
        /// Gets or sets the current move command for the player
        /// </summary>
        public MoveCommands MoveCommand
        {
            get { return mMoveCommand; }
            set { mMoveCommand = value; }
        }

        /// <summary>
        /// Gets the player object that issued the last move command given to us
        /// </summary>
        public PlayerObj IssuedMoveCommand
        {
            get { return mIssuedMoveCommand; }
        }

        /// <summary>
        /// Gets all quest givers that are in LOS of this player
        /// </summary>
        public IEnumerable<QuestGiver> QuestGivers
        {
            get
            {
                lock (mQuestGiversLock)
                    return mQuestGivers.ToList();
            }
        }

        /// <summary>
        /// Guids of objects that we can loot
        /// </summary>
        public IEnumerable<WoWGuid> Lootable
        {
            get { return mLootable; }
        }

        /// <summary>
        /// Gets all spells that are available to learn for this player
        /// </summary>
        public IEnumerable<SpellEntry> AvailableSpellsToLearn
        {
            get { return mAvailableSpells; }
        }

        /// <summary>
        /// Gets the spell cooldown manager for this player
        /// </summary>
        public SpellCooldownManager SpellCooldowns
        {
            get { return mSpellCooldownManager; }
        }

        /// <summary>
        /// Gets the global cooldown for the player
        /// </summary>
        public GlobalCooldown GCD
        {
            get { return mGCD; }
        }

        /// <summary>
        /// Gets a list of all spell ids the player has
        /// </summary>
        public IEnumerable<ushort> Spells
        {
            get { return mSpellList; }
        }

        #endregion

        #region Public Methods 

        /// <summary>
        /// Handles chat messages for this player
        /// </summary>
        public void HandleChatMessage(WorldServerClient client, ChatMsg type, WoWGuid senderGuid, string senderName, string message, string channel)
        {
            mPlayerChatHandler.HandleChat(client, type, senderGuid, senderName, message, channel);
        }

        /// <summary>
        /// Adds an item proficiency for the player
        /// </summary>
        /// <param name="itemClass"></param>
        /// <param name="subClass"></param>
        public void AddProficiency(ItemClass itemClass, UInt32 subClass)
        {
            if (!mProficiencies.Any(p => p.ItemClass == itemClass && p.ItemSubClassMask == subClass))
                mProficiencies.Add(new Proficiency(itemClass, subClass));
        }

        /// <summary>
        /// Sets a players group
        /// </summary>
        /// <param name="group"></param>
        public void SetGroup(Group group)
        {
            if (group == null) throw new ArgumentNullException("group");
            mGroup = group;
        }

        /// <summary>
        /// Adds the player to a new group
        /// </summary>
        public void AddToGroup()
        {
            // Add myself to my group
            mGroup = new Group();
            mGroup.AddPlayerToGroup(this);

            // Create my group data
            mGroupData = new GroupMemberData() { OnlineState = 1 };
        }

        /// <summary>
        /// Removes the player from the current group
        /// </summary>
        public void RemoveFromGroup()
        {
            mGroup = null;
            mGroupData = null;
            mPlayerAI.ClearFollowTarget();
        }

        /// <summary>
        /// Adds an enemy guid to the list
        /// </summary>
        /// <param name="guid"></param>
        public void AddEnemy(WoWGuid guid)
        {
            if (!mEnemyList.Any(e => e.GetOldGuid() == guid.GetOldGuid()))
                mEnemyList.Add(guid);
        }

        /// <summary>
        /// Removes an enemy guid from the list
        /// </summary>
        /// <param name="guid"></param>
        public void RemoveEnemy(UInt64 guid)
        {
            var enemy = mEnemyList.Where(e => e.GetOldGuid() == guid).SingleOrDefault();
            if (enemy != null)
                mEnemyList.Remove(enemy);
            if (mEnemyList.Count == 0)
                PlayerAI.NotInMeleeRange = false;
        }

        /// <summary>
        /// Adds a spell to the player
        /// </summary>
        /// <param name="spellId"></param>
        public void AddSpell(UInt16 spellId)
        {
            if (!mSpellList.Contains(spellId))
            {
                mSpellList.Add(spellId);
            }

            // Remove the spell from available spells
            RemoveAvailableSpellToLearn(spellId);
        }

        /// <summary>
        /// Updates all available spells that we can get but do not have yet. Called when logging in and when
        /// the player levels up.
        /// </summary>
        public void UpdateAvailableSpells()
        {
            mAvailableSpells = SkillLineAbilityTable.Instance.getAvailableSpellsForPlayer(this).ToList();
            // Remove all spells that we have
            mAvailableSpells.RemoveAll(s => mSpellList.Contains((ushort)s.SpellId));
            // Remove all spells that class logic says we should ignore for learning
            if (ClassLogic != null)
                mAvailableSpells.RemoveAll(s => ClassLogic.IgnoreLearningSpells.Contains(s.SpellId));
        }

        /// <summary>
        /// Called when an object is added
        /// </summary>
        /// <param name="obj"></param>
        public void ObjectAdded(PObject obj)
        {
            if (obj.Type == ObjectType.Corpse)
            {
                // If this is our corpse hold on to it so we can teleport back to it
                var corpse = obj as Corpse;
                if (corpse == null)
                    return;

                if (corpse.OwnerGuid.GetOldGuid() == this.Guid.GetOldGuid())
                    mPlayerCorpse = corpse;
            }
        }

        /// <summary>
        /// Adjusts fields for the player so they are no longer seen as dead
        /// </summary>
        public void ResurrectFromDeath()
        {
            if (PlayerObject.CurrentHealth <= 0)
                PlayerObject.SetField((int)UnitFields.UNIT_FIELD_HEALTH, 1);
            // Remove any ghost auras
            PlayerObject.RemoveAura(SpellAuras.GHOST_1);
            PlayerObject.RemoveAura(SpellAuras.GHOST_2);
            PlayerObject.RemoveAura(SpellAuras.GHOST_WISP);
        }

        /// <summary>
        /// Issue a move command to this player
        /// </summary>
        /// <param name="issuedFrom"></param>
        /// <param name="command"></param>
        public void IssueMoveCommand(PlayerObj issuedFrom, MoveCommands command)
        {
            this.mMoveCommand = command;
            this.mIssuedMoveCommand = issuedFrom;
        }

        /// <summary>
        /// Drops a quest based on the quest title
        /// </summary>
        /// <param name="questTitle"></param>
        public void DropQuest(string questTitle)
        {
            var quest = QuestManager.Instance.GetQuest(questTitle);
            if (quest != null)
                DropQuest(quest.QuestId);
        }

        /// <summary>
        /// Drops a quest based on the id
        /// </summary>
        /// <param name="questId"></param>
        public void DropQuest(UInt32 questId)
        {
            PlayerAI.Client.RemoveQuest(questId);
            PlayerObject.DropQuest(questId);
        }

        /// <summary>
        /// Updates our list of quest givers that are in our LOS
        /// </summary>
        /// <param name="questGivers"></param>
        public void UpdateQuestGivers(IList<QuestGiver> questGivers)
        {
            lock (mQuestGiversLock)
            {
                mQuestGivers.Clear();
                mQuestGivers = questGivers.ToList();
            }
        }

        /// <summary>
        /// Removes a quest giver from the list. It will get updated again if it still has a quest to get or complete for us.
        /// </summary>
        /// <param name="questGiverGuid"></param>
        public void RemoveQuestGiver(UInt64 questGiverGuid)
        {
            var existing = mQuestGivers.Where(q => q.Guid == questGiverGuid).SingleOrDefault();
            if (existing != null)
                lock (mQuestGiversLock)
                    mQuestGivers.Remove(existing);
        }

        /// <summary>
        /// Adds a lootable object
        /// </summary>
        /// <param name="guid"></param>
        public void AddLootable(WoWGuid guid)
        {
            if (!mLootable.Any(l => l.GetOldGuid() == guid.GetOldGuid()))
                mLootable.Add(guid);
        }

        /// <summary>
        /// Removes a lootable from the list
        /// </summary>
        /// <param name="guid"></param>
        public void RemoveLootable(WoWGuid guid)
        {
            mLootable.RemoveAll(l => l.GetOldGuid() == guid.GetOldGuid());
        }

        /// <summary>
        /// Gets whether or not the player has the skill for the item/subclass combination
        /// </summary>
        /// <param name="itemClass"></param>
        /// <param name="subClass"></param>
        /// <returns></returns>
        public bool HasProficiency(ItemClass itemClass, uint subClass)
        {
            return mProficiencies.Any(p => p.ItemClass == itemClass && p.ItemSubClassMask == subClass);
        }

        /// <summary>
        /// Gets whether or not the player has a spell
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        public bool HasSpell(ushort spellId)
        {
            return mSpellList.Contains(spellId);
        }

        /// <summary>
        /// Gets whether or not the player has all spells
        /// </summary>
        /// <param name="spellIds"></param>
        /// <returns></returns>
        public bool HasAllSpells(IEnumerable<ushort> spellIds)
        {
            foreach (var spellId in spellIds)
                if (!HasSpell(spellId))
                    return false;
            return true;
        }

        /// <summary>
        /// NOT WORKING - Gets the skill value of this player for the item/subclass combination
        /// </summary>
        /// <param name="itemClass"></param>
        /// <param name="subClass"></param>
        /// <returns></returns>
        public uint GetProficiencyValue(ItemClass itemClass, uint subClass)
        {
            var proficiency = mProficiencies.Where(p => p.ItemClass == itemClass && p.ItemSubClassMask == subClass).SingleOrDefault();
            if (proficiency != null)
                return proficiency.ProficiencyLevel;
            return 0;
        }

        /// <summary>
        /// Determines whether or not the passed item is useful
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsItemUseful(Clients.Item item)
        {
            // If not base item info, keep it becasue we can't tell for sure
            if (item.BaseInfo == null) return true;

            // If the item is grey quality it is not useful. Grey equipment can be useful at lower levels
            // but it is assumed those items have already been equipped.
            if (item.BaseInfo.Quality == ItemQualities.ITEM_QUALITY_POOR)
                return false;

            // Quest related items are useful
            if (item.BaseInfo.StartsQuestId > 0)
                return true;

            switch (item.BaseInfo.ItemClass)
            {
                case ItemClass.ITEM_CLASS_QUEST:
                    // TODO: Check if item is requirement for a quest we have. Or if it's used in a quest we are on.
                    return true;

                case ItemClass.ITEM_CLASS_WEAPON:
                    if (item.BaseInfo.SubClass >= Constants.ItemConstants.MAX_ITEM_SUBCLASS_WEAPON)
                        return false;
                    if (!HasProficiency(ItemClass.ITEM_CLASS_WEAPON, item.BaseInfo.SubClass))
                        return false;
                    // TODO: Check if it's an upgrade for us or not, for both primary and off spec. If it is for either
                    // it is useful.
                    return true;

                case ItemClass.ITEM_CLASS_ARMOR:
                    if (item.BaseInfo.SubClass >= Constants.ItemConstants.MAX_ITEM_SUBCLASS_ARMOR)
                        return false;
                    if (!HasProficiency(ItemClass.ITEM_CLASS_ARMOR, item.BaseInfo.SubClass))
                        return false;
                    // TODO: Check if it's an upgrade for us or not, for both primary and off spec. If it is for either
                    // it is useful.
                    return true;

                case ItemClass.ITEM_CLASS_KEY:
                    return true;

                case ItemClass.ITEM_CLASS_CONSUMABLE:
                    // TODO: Only useful if we don't have better already.
                    return true;

                case ItemClass.ITEM_CLASS_CONTAINER:
                    // TODO: Only useful if we don't have better already.
                    return true;

                case ItemClass.ITEM_CLASS_REAGENT:
                    // TODO: Most likely useful depending on class
                    return true;

                case ItemClass.ITEM_CLASS_RECIPE:
                    // If we have the spells for this item already than it is not useful
                    if (HasAllSpells(item.BaseInfo.SpellEffects.Select(s => (ushort)s.SpellId)))
                        return false;

                    switch ((ItemSubclassRecipe)item.BaseInfo.SubClass)
                    {
                        case ItemSubclassRecipe.ITEM_SUBCLASS_LEATHERWORKING_PATTERN:
                            if (PlayerObject.HasSkill(SkillType.SKILL_LEATHERWORKING))
                                return true;
                            break;
                        case ItemSubclassRecipe.ITEM_SUBCLASS_TAILORING_PATTERN:
                            if (PlayerObject.HasSkill(SkillType.SKILL_TAILORING))
                                return true;
                            break;
                        case ItemSubclassRecipe.ITEM_SUBCLASS_ENGINEERING_SCHEMATIC:
                            if (PlayerObject.HasSkill(SkillType.SKILL_ENGINEERING))
                                return true;
                            break;
                        case ItemSubclassRecipe.ITEM_SUBCLASS_BLACKSMITHING:
                            if (PlayerObject.HasSkill(SkillType.SKILL_BLACKSMITHING))
                                return true;
                            break;
                        case ItemSubclassRecipe.ITEM_SUBCLASS_COOKING_RECIPE:
                            if (PlayerObject.HasSkill(SkillType.SKILL_COOKING))
                                return true;
                            break;
                        case ItemSubclassRecipe.ITEM_SUBCLASS_ALCHEMY_RECIPE:
                            if (PlayerObject.HasSkill(SkillType.SKILL_ALCHEMY))
                                return true;
                            break;
                        case ItemSubclassRecipe.ITEM_SUBCLASS_FIRST_AID_MANUAL:
                            if (PlayerObject.HasSkill(SkillType.SKILL_FIRST_AID))
                                return true;
                            break;
                        case ItemSubclassRecipe.ITEM_SUBCLASS_ENCHANTING_FORMULA:
                            if (PlayerObject.HasSkill(SkillType.SKILL_ENCHANTING))
                                return true;
                            break;
                        case ItemSubclassRecipe.ITEM_SUBCLASS_FISHING_MANUAL:
                            if (PlayerObject.HasSkill(SkillType.SKILL_FISHING))
                                return true;
                            break;
                        default:
                            break;
                    }
                    break;

                case ItemClass.ITEM_CLASS_TRADE_GOODS:
                    switch ((ItemSubclassTradeGoods)item.BaseInfo.SubClass)
                    {
                        case ItemSubclassTradeGoods.ITEM_SUBCLASS_PARTS:
                        case ItemSubclassTradeGoods.ITEM_SUBCLASS_EXPLOSIVES:
                        case ItemSubclassTradeGoods.ITEM_SUBCLASS_DEVICES:
                            if (PlayerObject.HasSkill(SkillType.SKILL_ENGINEERING))
                                return true;
                            break;
                        case ItemSubclassTradeGoods.ITEM_SUBCLASS_CLOTH:
                            if (PlayerObject.HasSkill(SkillType.SKILL_TAILORING))
                                return true;
                            if (PlayerObject.HasSkill(SkillType.SKILL_FIRST_AID))
                                return true;
                            break;
                        case ItemSubclassTradeGoods.ITEM_SUBCLASS_LEATHER:
                            if (PlayerObject.HasSkill(SkillType.SKILL_LEATHERWORKING))
                                return true;
                            break;
                        case ItemSubclassTradeGoods.ITEM_SUBCLASS_METAL_STONE:
                            if (PlayerObject.HasSkill(SkillType.SKILL_BLACKSMITHING) ||
                                PlayerObject.HasSkill(SkillType.SKILL_ENGINEERING) ||
                                PlayerObject.HasSkill(SkillType.SKILL_MINING))
                                return true;
                            break;
                        case ItemSubclassTradeGoods.ITEM_SUBCLASS_MEAT:
                            if (PlayerObject.HasSkill(SkillType.SKILL_COOKING))
                                return true;
                            break;
                        case ItemSubclassTradeGoods.ITEM_SUBCLASS_HERB:
                            if (PlayerObject.HasSkill(SkillType.SKILL_HERBALISM) ||
                                PlayerObject.HasSkill(SkillType.SKILL_ALCHEMY))
                                return true;
                            break;
                        case ItemSubclassTradeGoods.ITEM_SUBCLASS_ELEMENTAL:
                            return true;
                        case ItemSubclassTradeGoods.ITEM_SUBCLASS_ENCHANTING:
                            if (PlayerObject.HasSkill(SkillType.SKILL_ENCHANTING))
                                return true;
                            break;
                        default:
                            break;
                    }
                    return true;

                case ItemClass.ITEM_CLASS_QUIVER:
                    // TODO: Most likely useful depending on our class
                    return true;

                case ItemClass.ITEM_CLASS_PROJECTILE:
                    // TODO: Most likely useful depending on our class
                    return true;

                case ItemClass.ITEM_CLASS_MONEY:
                    return true; // Money is always useful, not sure how these items work though
                default:
                    break;
            }

            // By default return false.
            return false;
        }

        /// <summary>
        /// Checks if the player has an aura defined by spell id
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        public bool HasAura(uint spellId)
        {
            return PlayerObject.Auras.Any(a => a.SpellId == spellId);
        }

        /// <summary>
        /// Called when the player levels up
        /// </summary>
        public void LevelUp(uint level)
        {
            // Need to find whether or not we have spells/skills that need to be updated
            // via a trainer. If there are we need to talk to a trainer to learn them.
            UpdateAvailableSpells();

            // TODO: Need to handle unspent talent points. They should be spent based on the spec assigned to the bot
        }

        /// <summary>
        /// Check spell availability for training base at SkillLineAbility/SkillRaceClassInfo data.
        /// Checked allowed race/class and dependent from race/class allowed min level
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        public bool IsSpellFitByClassAndRace(uint spellId)
        {
            uint racemask = (uint)(1 << (Race - 1));
            uint classmask = (uint)(1 << (Class - 1));

            // Get skill line abilities for this spell
            var skillLineAbilities = SkillLineAbilityTable.Instance.getForSpell(spellId);
            if (skillLineAbilities.Count() == 0)
                return false;
            if (skillLineAbilities.Count() == 1)
                return true;

            foreach (var sla in skillLineAbilities)
            {
                // Skip wrong race skills
                if (sla.CharacterRacesFlag > 0 && (sla.CharacterRacesFlag & racemask) == 0)
                    continue;

                // Skip wrong class skills
                if (sla.CharacterClassesFlag > 0 && (sla.CharacterClassesFlag & classmask) == 0)
                    continue;

                var skillRaceClassInfo = SkillRaceClassInfoTable.Instance.getBySkillID(sla.SkillLineId);
                foreach (var srci in skillRaceClassInfo)
                {
                    if ((srci.RaceMask & racemask) > 0 && (srci.ClassMask & classmask) > 0)
                    {
                        if ((srci.Flags & (uint)AbilitySkillFlags.ABILITY_SKILL_NONTRAINABLE) > 0)
                            return false;

                        if (srci.RequiredLevel > 0 && Level < srci.RequiredLevel)
                            return false;
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Handles a SMSG_SPELL_GO message. We need to handle these because they can affect player movement and possibly
        /// other things. 
        /// </summary>
        /// <param name="message"></param>
        public void HandleSpellGo(SpellCastGoMessage message)
        {
            // Get the spell and look for effects we need to handle. Most of these will deal with movement.
            var spell = SpellTable.Instance.getSpell(message.SpellId);
            if (spell == null)
                return;

            // Start a cooldown for spells that have a cast time, the cooldown has not been started yet because they can be canceled
            if (spell.CastTime > 0)
                mSpellCooldownManager.StartCooldown(spell);

            // Handle each effect of the spell
            for (int i = 0; i < SpellConstants.MAX_EFFECT_INDEX; i++)
            {
                switch (spell.Effect[i])
                {
                    case SpellEffects.SPELL_EFFECT_CHARGE:
                        HandleChargeEffect(spell, message);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Casts a spell at a target
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="target"></param>
        public void CastSpell(SpellEntry spell, Clients.Object target)
        {
            // Cast the spell
            PlayerAI.Client.CastSpell(target, spell.SpellId);
            // Trigger the GCD for the spell
            mGCD.TriggerGCD(spell);
            // If the spell has no cast time, trigger the cooldown for the spell
            if (spell.CastTime <= 0)
                mSpellCooldownManager.StartCooldown(spell);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Removes a spell that is available to learn from the list.
        /// </summary>
        /// <param name="spellId"></param>
        private void RemoveAvailableSpellToLearn(uint spellId)
        {
            mAvailableSpells.RemoveAll(s => s.SpellId == spellId);
        }

        #endregion

        #region Spell Effect Handlers

        /// <summary>
        /// Handles the charge spell effect for our player
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="message"></param>
        private void HandleChargeEffect(SpellEntry spell, SpellCastGoMessage message)
        {
            // TODO: Need to fix the charge effect on bots. When on a client it looks like it starts briefly and then stops right away and the bot never charges all the way to the target. I believe
            // this has something to do with our movement code where it is overwriting the charge effect. However, I'm not sure if we need to do anything here in the client for the charge effect or
            // if the server handles that. Judging by how the bot seems to start with the charge I assume the server handles it.


        }

        #endregion
    }
}
