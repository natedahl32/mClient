using mClient.Constants;
using mClient.Shared;
using PObject = mClient.Clients.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mClient.Clients;
using mClient.World.AI;
using mClient.World.Quest;

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

        // Player AI
        private PlayerAI mPlayerAI = null;
        private PlayerChatHandler mPlayerChatHandler = null;

        // Enemy
        private List<WoWGuid> mEnemyList = new List<WoWGuid>();

        // Spells
        private List<UInt16> mSpellList = new List<UInt16>();

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
        /// Gets the players corpse
        /// </summary>
        public Corpse PlayerCorpse { get { return mPlayerCorpse; } }

        /// <summary>
        /// Gets whether or not this is a melee character
        /// </summary>
        public bool IsMelee
        {
            get
            {
                // Pure melee
                if (Class == (byte)Classname.Warrior ||
                    Class == (byte)Classname.Rogue)
                    return true;

                // TODO: Depends on spec, will report them as melee for now
                if (Class == (byte)Classname.Paladin ||
                    Class == (byte)Classname.Druid ||
                    Class == (byte)Classname.Shaman)
                    return true;

                // Anything else is never melee
                return false;

            }
        }

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
                mSpellList.Add(spellId);
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
            List<QuestGiver> toRemove;
            lock (mQuestGiversLock)
                toRemove = mQuestGivers.Where(qg => !questGivers.Any(qg2 => qg.Guid == qg2.Guid)).ToList();

            // Remove any that don't exist anymore
            foreach (var q in toRemove)
                lock (mQuestGiversLock)
                    mQuestGivers.Remove(q);

            // Add any that don't exist already
            foreach (var q in questGivers)
                if (!mQuestGivers.Any(qg => qg.Guid == q.Guid))
                    lock (mQuestGiversLock)
                        mQuestGivers.Add(q);
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

        #endregion
    }
}
