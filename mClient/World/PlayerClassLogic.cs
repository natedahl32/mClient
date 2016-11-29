using mClient.Clients;
using mClient.Constants;
using mClient.DBC;
using mClient.Terrain;
using mClient.World.ClassLogic;
using mClient.World.Items;
using System;
using System.Collections.Generic;

namespace mClient.World
{
    public abstract class PlayerClassLogic
    {
        #region Declarations

        public const uint RECENTLY_BANDAGED = 11196;

        private Player mPlayer;

        // racial
        protected uint STONEFORM,
            ESCAPE_ARTIST,
            PERCEPTION,
            SHADOWMELD,
            BLOOD_FURY,
            WAR_STOMP,
            BERSERKING,
            WILL_OF_THE_FORSAKEN;

        // stat weights, used for determining gear upgrades
        protected readonly Dictionary<ItemModType, float> mStatWeights = new Dictionary<ItemModType, float>();

        // player spec
        private MainSpec mAssignedSpec;

        #endregion

        #region Constructors

        protected PlayerClassLogic(Player player)
        {
            if (player == null) throw new ArgumentNullException("player");
            mPlayer = player;
            mAssignedSpec = MainSpec.NONE;

            // Add empty stat weights for each stat
            mStatWeights.Add(ItemModType.ITEM_MOD_AGILITY, 0f);
            mStatWeights.Add(ItemModType.ITEM_MOD_HEALTH, 0f);
            mStatWeights.Add(ItemModType.ITEM_MOD_INTELLECT, 0f);
            mStatWeights.Add(ItemModType.ITEM_MOD_MANA, 0f);
            mStatWeights.Add(ItemModType.ITEM_MOD_SPIRIT, 0f);
            mStatWeights.Add(ItemModType.ITEM_MOD_STAMINA, 0f);
            mStatWeights.Add(ItemModType.ITEM_MOD_STRENGTH, 0f);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the player this logic belongs to
        /// </summary>
        public Player Player { get { return mPlayer; } }

        /// <summary>
        /// Gets the name of this class
        /// </summary>
        public abstract string ClassName { get; }

        /// <summary>
        /// Gets whether or not the player has any out of combat buffs to give out (including self).
        /// </summary>
        public abstract bool HasOOCBuffs { get; }

        /// <summary>
        /// Gets all group members that need a buff
        /// </summary>
        public abstract Dictionary<SpellEntry, IList<Player>> GroupMembersNeedingOOCBuffs { get; }

        /// <summary>
        /// Gets whether or not this player is a melee combatant
        /// </summary>
        public abstract bool IsMelee { get; }

        /// <summary>
        /// Gets the next spell to cast in a DPS rotation for the class
        /// </summary>
        public abstract SpellEntry NextSpellInRotation { get; }

        /// <summary>
        /// Ignores spells that we think we should learn. These are generally broken spells in the DBC files that we can't weed out using normal methods
        /// </summary>
        public abstract IEnumerable<uint> IgnoreLearningSpells { get; }

        /// <summary>
        /// Gets the assigned spec of the player
        /// </summary>
        public MainSpec Spec
        {
            get { return mAssignedSpec; }
            protected set
            {
                var oldSpec = mAssignedSpec;
                mAssignedSpec = value;
                if (oldSpec != mAssignedSpec)
                    SetStatWeights();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes all spells the player currently has.
        /// </summary>
        public virtual void InitializeSpells()
        {
            // Racial abilities
            STONEFORM = InitSpell(RacialTraits.STONEFORM_ALL);
            ESCAPE_ARTIST = InitSpell(RacialTraits.ESCAPE_ARTIST_ALL);
            PERCEPTION = InitSpell(RacialTraits.PERCEPTION_ALL);
            SHADOWMELD = InitSpell(RacialTraits.SHADOWMELD_ALL);
            BLOOD_FURY = InitSpell(RacialTraits.BLOOD_FURY_ALL);
            WAR_STOMP = InitSpell(RacialTraits.WAR_STOMP_ALL);
            BERSERKING = InitSpell(RacialTraits.BERSERKING_ALL);
            WILL_OF_THE_FORSAKEN = InitSpell(RacialTraits.WILL_OF_THE_FORSAKEN_ALL);
        }

        /// <summary>
        /// Compares items to determine which is better. 
        /// If result > 0 than item1 is better than item2. 
        /// If result < 0 than item2 is better than item1.
        /// If result == 0 than the two items are equal.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public virtual float CompareItems(Item item1, Item item2)
        {
            // Get base item compare
            float baseCompare = CompareItems(item1.BaseInfo, item2.BaseInfo);

            // Get item enchantment score
            float item1Score = GetItemEnchantmentScore(item1);
            float item2Score = GetItemEnchantmentScore(item2);

            // Return the score difference
            return (item1Score - item2Score) + baseCompare;
        }

        /// <summary>
        /// Compares items to determine which is better. 
        /// If result > 0 than item1 is better than item2. 
        /// If result < 0 than item2 is better than item1.
        /// If result == 0 than the two items are equal.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public virtual float CompareItems(ItemInfo item1, ItemInfo item2)
        {
            // Make sure stat weights are set for our current spec
            SetStatWeights();

            // Get item score for stats
            float item1Score = GetItemScore(item1);
            float item2Score = GetItemScore(item2);

            // Return the score difference
            return item1Score - item2Score;
        }

        /// <summary>
        /// Compares items to determine which is better. 
        /// If result > 0 than item1 is better than item2. 
        /// If result < 0 than item2 is better than item1.
        /// If result == 0 than the two items are equal.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="randomPropertyIdForItem1"></param>
        /// <param name="item2"></param>
        /// <param name="randomPropertyIdForItem2"></param>
        /// <returns></returns>
        public virtual float CompareItems(ItemInfo item1, uint randomPropertyIdForItem1, ItemInfo item2, uint randomPropertyIdForItem2)
        {
            // Get base score
            float baseScore = CompareItems(item1, item2);

            float item1Score = 0f;
            float item2Score = 0f;

            // Add in random property enchantment if supplied
            if (randomPropertyIdForItem1 > 0)
            {
                var randomItemProperty = ItemRandomPropertiesTable.Instance.getById(randomPropertyIdForItem1);
                if (randomItemProperty != null)
                {
                    for (int i = 0; i < 3; i++)
                        if (randomItemProperty.EnchantId[i] > 0)
                            item1Score += GetEnchantmentScore(randomItemProperty.EnchantId[i]);
                }
            }
            if (randomPropertyIdForItem2 > 0)
            {
                var randomItemProperty = ItemRandomPropertiesTable.Instance.getById(randomPropertyIdForItem2);
                if (randomItemProperty != null)
                {
                    for (int i = 0; i < 3; i++)
                        if (randomItemProperty.EnchantId[i] > 0)
                            item2Score += GetEnchantmentScore(randomItemProperty.EnchantId[i]);
                }
            }

            // Return the score difference
            return (item1Score - item2Score) + baseScore;
        }

        /// <summary>
        /// Gets the item score for a spell (stat mod only)
        /// </summary>
        /// <param name="spell"></param>
        /// <returns></returns>
        public float GetSpellScore(SpellEntry spell)
        {
            float score = 0f;
            for (uint effIndex = 0; effIndex < SpellConstants.MAX_EFFECT_INDEX; effIndex++)
            {
                if (spell.EffectApplyAuraName[effIndex] == (int)AuraType.SPELL_AURA_MOD_STAT)
                {
                    var value = spell.CalculateSimpleValue((SpellEffectIndex)effIndex);

                    switch (spell.EffectMiscValue[effIndex])
                    {
                        case (int)Stats.STAT_STRENGTH:
                            score += (value * mStatWeights[ItemModType.ITEM_MOD_STRENGTH]);
                            break;
                        case (int)Stats.STAT_AGILITY:
                            score += (value * mStatWeights[ItemModType.ITEM_MOD_AGILITY]);
                            break;
                        case (int)Stats.STAT_STAMINA:
                            score += (value * mStatWeights[ItemModType.ITEM_MOD_STAMINA]);
                            break;
                        case (int)Stats.STAT_INTELLECT:
                            score += (value * mStatWeights[ItemModType.ITEM_MOD_INTELLECT]);
                            break;
                        case (int)Stats.STAT_SPIRIT:
                            score += (value * mStatWeights[ItemModType.ITEM_MOD_SPIRIT]);
                            break;
                        default:
                            break;
                    }
                }
            }
            return score;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets stat weights based on spec
        /// </summary>
        protected virtual void SetStatWeights()
        {
            if (Spec == MainSpec.NONE)
            {
                mStatWeights[ItemModType.ITEM_MOD_STAMINA] = 0.75f;
                mStatWeights[ItemModType.ITEM_MOD_HEALTH] = 0.8f;

                if (IsMelee)
                {
                    mStatWeights[ItemModType.ITEM_MOD_AGILITY] = 0.5f;
                    mStatWeights[ItemModType.ITEM_MOD_STRENGTH] = 0.5f;
                    mStatWeights[ItemModType.ITEM_MOD_INTELLECT] = 0.1f;
                    mStatWeights[ItemModType.ITEM_MOD_MANA] = 0.1f;
                    mStatWeights[ItemModType.ITEM_MOD_SPIRIT] = 0.1f;
                }
                else
                {
                    mStatWeights[ItemModType.ITEM_MOD_AGILITY] = 0.1f;
                    mStatWeights[ItemModType.ITEM_MOD_STRENGTH] = 0.1f;
                    mStatWeights[ItemModType.ITEM_MOD_INTELLECT] = 0.5f;
                    mStatWeights[ItemModType.ITEM_MOD_MANA] = 0.55f;
                    mStatWeights[ItemModType.ITEM_MOD_SPIRIT] = 0.4f;
                }
            }
        }

        /// <summary>
        /// Initializes a spell by getting the current rank of the spell the player currently has
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        protected uint InitSpell(uint spellId)
        {
            // If the player does not have the spell
            if (!Player.HasSpell((ushort)spellId))
                return 0;

            var spell = spellId;
            uint nextSpell = 0;
            do
            {
                nextSpell = SkillLineAbilityTable.Instance.getParentForSpell(spell);
                if (nextSpell > 0)
                {
                    if (Player.HasSpell((ushort)nextSpell))
                        spell = nextSpell;
                    else
                        return spell;
                }
            } while (nextSpell != 0);
            return spell;
        }

        /// <summary>
        /// Checks if the player owns the spell and can cast it
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        protected bool HasSpellAndCanCast(uint spellId)
        {
            // No spell? Can't cast it
            if (spellId == 0) return false;

            var spell = Spell(spellId);
            if (spell == null)
                return false;

            // On the GCD? Can't cast it then
            // TODO: Check if spell is affected by GCD
            if (Player.GCD.HasGCD) return false;

            // Is it on cooldown?
            if (Player.SpellCooldowns.HasCooldown(spellId)) return false;

            // Have enough power to cast it?
            if (!Player.PlayerObject.CanCastSpell(spell)) return false;

            // Are we in range to cast it?
            if (spell.RangeIndex > 0 && Player.PlayerAI.TargetSelection != null)
            {
                var rangeEntry = SpellRangeTable.Instance.getByID(spell.RangeIndex);
                if (rangeEntry != null)
                {
                    var distance = TerrainMgr.CalculateDistance(Player.Position, Player.PlayerAI.TargetSelection.Position);
                    if (distance <= rangeEntry.MinimumRange || distance >= rangeEntry.MaximumRange)
                        return false;
                }
            }

            // TODO: More checks needed. Have reagents? for example

            return true;
        }

        /// <summary>
        /// Convenience method to get spell from DBC
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        protected SpellEntry Spell(uint spellId)
        {
            return SpellTable.Instance.getSpell(spellId);
        }

        /// <summary>
        /// Gets an items score based on state weights defined
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected float GetItemScore(ItemInfo item)
        {
            float score = 0f;

            for (int i = 0; i < ItemConstants.MAX_ITEM_MOD; i++)
            {
                var statValue = item.GetStatValue((ItemModType)i);
                if (statValue == 0)
                    continue;

                // Health needs to be divided by the units of health per stamian otherwise health will be overvalued due to the amount of it on items. Same for mana.
                if (i == (int)ItemModType.ITEM_MOD_HEALTH)
                    statValue = statValue / 10;
                if (i == (int)ItemModType.ITEM_MOD_MANA)
                    statValue = statValue / 15;

                // Calculate the score
                score += (statValue * mStatWeights[(ItemModType)i]);
            }

            return score;
        }

        /// <summary>
        /// Gets the score for enchantments on items
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected float GetItemEnchantmentScore(Item item)
        {
            float score = 0f;

            for (int i = (int)EnchantmentSlot.PERM_ENCHANTMENT_SLOT; i < (int)EnchantmentSlot.MAX_ENCHANTMENT_SLOT; i++)
            {
                uint enchantmentId = item.GetEnchantmentIdForSlot((EnchantmentSlot)i);
                if (enchantmentId == 0)
                    continue;

                score += GetEnchantmentScore(enchantmentId);
            }

            return score;
        }

        /// <summary>
        /// Gets the score for an enchantment
        /// </summary>
        /// <param name="enchantmentId">Id of the enchantment to calculate score for</param>
        /// <returns></returns>
        protected float GetEnchantmentScore(uint enchantmentId)
        {
            float score = 0f;

            if (enchantmentId == 0)
                return score;

            var enchant = SpellItemEnchantmentTable.Instance.getById(enchantmentId);
            if (enchant == null)
                return score;

            for (int s = 0; s < 3; s++)
            {
                uint displayType = enchant.EnchantmentType[s];
                uint amount = enchant.EnchantmentAmount[s];
                uint spellId = enchant.SpellId[s];

                // check the spell
                if (spellId > 0)
                {
                    var spell = SpellTable.Instance.getSpell(spellId);
                    if (spell != null)
                    {
                        score += GetSpellScore(spell);
                    }
                }

                // checks enchant types
                switch ((ItemEnchantmentType)displayType)
                {
                    case ItemEnchantmentType.ITEM_ENCHANTMENT_TYPE_NONE:
                        break;
                    case ItemEnchantmentType.ITEM_ENCHANTMENT_TYPE_COMBAT_SPELL: // TODO: Add calculation for combat spells
                        break;
                    case ItemEnchantmentType.ITEM_ENCHANTMENT_TYPE_DAMAGE: // TODO: Add calculation for damage type enchantments
                        break;
                    case ItemEnchantmentType.ITEM_ENCHANTMENT_TYPE_EQUIP_SPELL: // TODO: Add calculations
                        break;
                    case ItemEnchantmentType.ITEM_ENCHANTMENT_TYPE_RESISTANCE: // TODO: Add calculations
                        break;
                    case ItemEnchantmentType.ITEM_ENCHANTMENT_TYPE_STAT:
                        uint value = amount;

                        // Need to modify the health value so it isn't overvalued. Same with mana
                        if (spellId == (int)ItemModType.ITEM_MOD_HEALTH)
                            value = value / 10;
                        if (spellId == (int)ItemModType.ITEM_MOD_MANA)
                            value = value / 15;

                        score += (value * mStatWeights[(ItemModType)spellId]);

                        break;
                    case ItemEnchantmentType.ITEM_ENCHANTMENT_TYPE_TOTEM: // TODO: Add calculations
                        break;
                    default:
                        break;
                }
            }

            return score;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creats a class logic instance based on class
        /// </summary>
        /// <param name="class"></param>
        /// <returns></returns>
        public static PlayerClassLogic CreateClassLogic(Classname @class, Player player)
        {
            switch (@class)
            {
                case Classname.Druid:
                    return new DruidLogic(player);
                case Classname.Hunter:
                    return new HunterLogic(player);
                case Classname.Mage:
                    return new MageLogic(player);
                case Classname.Paladin:
                    return new PaladinLogic(player);
                case Classname.Priest:
                    return new PriestLogic(player);
                case Classname.Rogue:
                    return new RogueLogic(player);
                case Classname.Shaman:
                    return new ShamanLogic(player);
                case Classname.Warlock:
                    return new WarlockLogic(player);
                case Classname.Warrior:
                    return new WarriorLogic(player);
                default:
                    break;
            }
            return null;
        }

        #endregion
    }
}
