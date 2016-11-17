using mClient.Constants;
using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class WarriorLogic : PlayerClassLogic
    {
        #region Declarations

        // ARMS
        protected uint BATTLE_STANCE,
            CHARGE,
            HEROIC_STRIKE,
            REND,
            THUNDER_CLAP,
            HAMSTRING,
            MOCKING_BLOW,
            RETALIATION,
            SWEEPING_STRIKES,
            MORTAL_STRIKE,
            TASTE_FOR_BLOOD,
            SUDDEN_DEATH;

        // PROTECTION
        protected uint DEFENSIVE_STANCE,
            BLOODRAGE,
            SUNDER_ARMOR,
            TAUNT,
            SHIELD_BASH,
            REVENGE,
            SHIELD_BLOCK,
            DISARM,
            SHIELD_WALL,
            SHIELD_SLAM,
            CONCUSSION_BLOW,
            LAST_STAND;

        // FURY
        protected uint BERSERKER_STANCE,
            BATTLE_SHOUT,
            DEMORALIZING_SHOUT,
            OVERPOWER,
            CLEAVE,
            INTIMIDATING_SHOUT,
            EXECUTE,
            CHALLENGING_SHOUT,
            SLAM,
            INTERCEPT,
            DEATH_WISH,
            BERSERKER_RAGE,
            WHIRLWIND,
            PUMMEL,
            BLOODTHIRST,
            RECKLESSNESS,
            PIERCING_HOWL,
            SLAM_PROC,
            BLOODSURGE;

        // general
        protected uint SHOOT,
            SHOOT_BOW,
            SHOOT_GUN,
            SHOOT_XBOW;

        #endregion

        #region Constructors

        public WarriorLogic(Player player) : base(player)
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
                // Stances
                if (BATTLE_STANCE > 0 || DEFENSIVE_STANCE > 0 || BERSERKER_STANCE > 0)
                    return true;
                // Battle Shout
                if (BATTLE_SHOUT > 0)
                    return true;

                // No buffs yet
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
                // TODO: Need to check for other stances in certain cases
                if (BATTLE_STANCE > 0 && !Player.HasAura(BATTLE_STANCE))
                    needBuffs.Add(Spell(BATTLE_STANCE), new List<Player>() { Player });

                // Check each player in the group for buffs
                foreach (var groupMember in Player.CurrentGroup.PlayersInGroup)
                {
                    // Battle Shout buff
                    if (BATTLE_SHOUT > 0)
                    {
                        var battleShoutSpell = Spell(BATTLE_SHOUT);
                        // TODO: Make sure group member is in range of spell as well
                        // TODO: Make sure group member does not have BETTER buff than the one we are casting
                        if (!groupMember.HasAura(BATTLE_SHOUT))
                            if (needBuffs.ContainsKey(battleShoutSpell))
                                needBuffs[battleShoutSpell].Add(groupMember);
                            else
                                needBuffs.Add(battleShoutSpell, new List<Player>() { groupMember });
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
                // Buffs we should have before going into combat but maybe we don't because we didn't have enough rage
                if (HasSpellAndCanCast(BATTLE_SHOUT) && !Player.HasAura(BATTLE_SHOUT)) return Spell(BATTLE_SHOUT);

                // DPS abilities
                if (HasSpellAndCanCast(HEROIC_STRIKE)) return Spell(HEROIC_STRIKE);
                    
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
                return new List<uint>() { 20647 };
            }
        }

        #endregion

        #region Public Methods

        public override void InitializeSpells()
        {
            base.InitializeSpells();

            // Abilities
            SHOOT_BOW = InitSpell(Spells.SHOOT_BOW_1);
            SHOOT_GUN = InitSpell(Spells.SHOOT_GUN_1);
            SHOOT_XBOW = InitSpell(Spells.SHOOT_XBOW_1);

            // Spells
            BATTLE_STANCE = InitSpell(Spells.BATTLE_STANCE_1);
            CHARGE = InitSpell(Spells.CHARGE_1);
            OVERPOWER = InitSpell(Spells.OVERPOWER_1);
            HEROIC_STRIKE = InitSpell(Spells.HEROIC_STRIKE_1);
            REND = InitSpell(Spells.REND_1);
            THUNDER_CLAP = InitSpell(Spells.THUNDER_CLAP_1);
            HAMSTRING = InitSpell(Spells.HAMSTRING_1);
            MOCKING_BLOW = InitSpell(Spells.MOCKING_BLOW_1);
            RETALIATION = InitSpell(Spells.RETALIATION_1);
            SWEEPING_STRIKES = InitSpell(Spells.SWEEPING_STRIKES_1);
            MORTAL_STRIKE = InitSpell(Spells.MORTAL_STRIKE_1);
            BLOODRAGE = InitSpell(Spells.BLOODRAGE_1);
            DEFENSIVE_STANCE = InitSpell(Spells.DEFENSIVE_STANCE_1);
            SUNDER_ARMOR = InitSpell(Spells.SUNDER_ARMOR_1);
            TAUNT = InitSpell(Spells.TAUNT_1);
            SHIELD_BASH = InitSpell(Spells.SHIELD_BASH_1);
            REVENGE = InitSpell(Spells.REVENGE_1);
            SHIELD_BLOCK = InitSpell(Spells.SHIELD_BLOCK_1);
            DISARM = InitSpell(Spells.DISARM_1);
            SHIELD_WALL = InitSpell(Spells.SHIELD_WALL_1);
            SHIELD_SLAM = InitSpell(Spells.SHIELD_SLAM_1);
            CONCUSSION_BLOW = InitSpell(Spells.CONCUSSION_BLOW_1);
            LAST_STAND = InitSpell(Spells.LAST_STAND_1);
            BATTLE_SHOUT = InitSpell(Spells.BATTLE_SHOUT_1);
            DEMORALIZING_SHOUT = InitSpell(Spells.DEMORALIZING_SHOUT_1);
            CLEAVE = InitSpell(Spells.CLEAVE_1);
            INTIMIDATING_SHOUT = InitSpell(Spells.INTIMIDATING_SHOUT_1);
            EXECUTE = InitSpell(Spells.EXECUTE_1);
            CHALLENGING_SHOUT = InitSpell(Spells.CHALLENGING_SHOUT_1);
            SLAM = InitSpell(Spells.SLAM_1);
            BERSERKER_STANCE = InitSpell(Spells.BERSERKER_STANCE_1);
            INTERCEPT = InitSpell(Spells.INTERCEPT_1);
            DEATH_WISH = InitSpell(Spells.DEATH_WISH_1);
            BERSERKER_RAGE = InitSpell(Spells.BERSERKER_RAGE_1);
            WHIRLWIND = InitSpell(Spells.WHIRLWIND_1);
            PUMMEL = InitSpell(Spells.PUMMEL_1);
            BLOODTHIRST = InitSpell(Spells.BLOODTHIRST_1);
            RECKLESSNESS = InitSpell(Spells.RECKLESSNESS_1);
            PIERCING_HOWL = InitSpell(Spells.PIERCING_HOWL_1);

            // Procs
            SLAM_PROC = InitSpell(Procs.SLAM_PROC_1);
            BLOODSURGE = InitSpell(Procs.BLOODSURGE_1);
            TASTE_FOR_BLOOD = InitSpell(Procs.TASTE_FOR_BLOOD_1);
            SUDDEN_DEATH = InitSpell(Procs.SUDDEN_DEATH_1);
        }

        #endregion

        #region Warrior Constants

        public static class Procs
        {
            public const uint SLAM_PROC_1 = 46916;
            public const uint BLOODSURGE_1 = 46915;
            public const uint TASTE_FOR_BLOOD_1 = 56638;
            public const uint SUDDEN_DEATH_1 = 52437;
        }

        public static class Spells
        {
            public const uint BATTLE_SHOUT_1 = 6673;
            public const uint BATTLE_STANCE_1 = 2457;
            public const uint BERSERKER_RAGE_1 = 18499;
            public const uint BERSERKER_STANCE_1 = 2458;
            public const uint BLOODRAGE_1 = 2687;
            public const uint BLOODTHIRST_1 = 23881;
            public const uint CHALLENGING_SHOUT_1 = 1161;
            public const uint CHARGE_1 = 100;
            public const uint CLEAVE_1 = 845;
            public const uint CONCUSSION_BLOW_1 = 12809;
            public const uint DEATH_WISH_1 = 12292;
            public const uint DEFENSIVE_STANCE_1 = 71;
            public const uint DEMORALIZING_SHOUT_1 = 1160;
            public const uint DISARM_1 = 676;
            public const uint EXECUTE_1 = 5308;
            public const uint HAMSTRING_1 = 1715;
            public const uint HEROIC_STRIKE_1 = 78;
            public const uint INTERCEPT_1 = 20252;
            public const uint INTERVENE_1 = 3411;
            public const uint INTIMIDATING_SHOUT_1 = 5246;
            public const uint LAST_STAND_1 = 12975;
            public const uint MOCKING_BLOW_1 = 694;
            public const uint MORTAL_STRIKE_1 = 12294;
            public const uint OVERPOWER_1 = 7384;
            public const uint PIERCING_HOWL_1 = 12323;
            public const uint PUMMEL_1 = 6552;
            public const uint RECKLESSNESS_1 = 1719;
            public const uint REND_1 = 772;
            public const uint RETALIATION_1 = 20230;
            public const uint REVENGE_1 = 6572;
            public const uint SHIELD_BASH_1 = 72;
            public const uint SHIELD_BLOCK_1 = 2565;
            public const uint SHIELD_SLAM_1 = 23922;
            public const uint SHIELD_WALL_1 = 871;
            public const uint SHOOT_BOW_1 = 2480;
            public const uint SHOOT_GUN_1 = 7918;
            public const uint SHOOT_XBOW_1 = 7919;
            public const uint SLAM_1 = 1464;
            public const uint SUNDER_ARMOR_1 = 7386;
            public const uint SWEEPING_STRIKES_1 = 12328;
            public const uint TAUNT_1 = 355;
            public const uint THUNDER_CLAP_1 = 6343;
            public const uint WHIRLWIND_1 = 1680;
        }

        #endregion
    }
}
