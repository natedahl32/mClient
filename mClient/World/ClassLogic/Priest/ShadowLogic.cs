using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic.Priest
{
    public class ShadowLogic : PriestLogic
    {
        #region Constructors

        public ShadowLogic(Player player) : base(player)
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
                // Shadowform
                if (SHADOWFORM > 0) return true;
                
                return base.HasOOCBuffs;
            }
        }

        /// <summary>
        /// Gets all group members that need a buff
        /// </summary>
        public override Dictionary<SpellEntry, IList<Player>> GroupMembersNeedingOOCBuffs
        {
            get
            {
                var needBuffs = base.GroupMembersNeedingOOCBuffs;

                // Shadowform 
                if (HasSpellAndCanCast(SHADOWFORM) && !Player.HasAura(SHADOWFORM))
                    needBuffs.Add(Spell(SHADOWFORM), new List<Player>() { Player });

                return needBuffs;
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

                // Devouring Plague
                if (HasSpellAndCanCast(DEVOURING_PLAGUE) && !currentTarget.HasAura(DEVOURING_PLAGUE)) return Spell(DEVOURING_PLAGUE);
                // Shadow Word Pain
                if (HasSpellAndCanCast(SHADOW_WORD_PAIN) && !currentTarget.HasAura(SHADOW_WORD_PAIN)) return Spell(SHADOW_WORD_PAIN);
                // Mind Blast
                if (HasSpellAndCanCast(MIND_BLAST)) return Spell(MIND_BLAST);
                // Mind Flay
                if (HasSpellAndCanCast(MIND_FLAY)) return Spell(MIND_FLAY);

                return base.NextSpellInRotation;
            }
        }

        #endregion
    }
}
