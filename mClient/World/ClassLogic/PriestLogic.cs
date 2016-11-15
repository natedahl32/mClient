using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.ClassLogic
{
    public class PriestLogic : PlayerClassLogic
    {
        #region Constructors

        public PriestLogic(Player player) : base(player)
        {
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
