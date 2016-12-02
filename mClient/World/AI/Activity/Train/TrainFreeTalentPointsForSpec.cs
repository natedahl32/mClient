using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Train
{
    public class TrainFreeTalentPointsForSpec : BaseActivity
    {
        #region Declarations

        private uint mSpentFreePoint = 0;

        #endregion

        #region Constructors

        public TrainFreeTalentPointsForSpec(PlayerAI ai) : base(ai)
        {
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Training Free Talent Points"; }
        }

        #endregion

        #region Public Methods

        public override void Process()
        {
            // If we don't have anymore free talent points we can exit out
            if (PlayerAI.Player.PlayerObject.FreeTalentPoints == 0)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // If we just spent the free talent point we are on just wait until we update our free talent points
            if (mSpentFreePoint == PlayerAI.Player.PlayerObject.FreeTalentPoints) return;

            // Get the next talent to purchase
            var nextTalent = PlayerAI.Player.NextTalentToPurchase;
            // If we didn't get one we can get out
            if (nextTalent == 0)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // We currently have the spell id, we need to get the talent id and rank to send to the server
            var talentSpellPos = TalentTable.Instance.getTalentSpellPosBySpell(nextTalent);
            if (talentSpellPos == null)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // Learn the talent now
            mSpentFreePoint = PlayerAI.Player.PlayerObject.FreeTalentPoints;
            PlayerAI.Client.LearnTalent(talentSpellPos.TalentId, (uint)(talentSpellPos.Rank - 1));
        }

        #endregion
    }
}
