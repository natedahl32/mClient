using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Combat
{
    public class CastSingleTargetSpell : BaseActivity
    {
        #region Declarations

        private SpellEntry mCastingSpell;

        #endregion

        #region Constructors

        public CastSingleTargetSpell(SpellEntry spell, PlayerAI ai) : base(ai)
        {
            if (spell == null) throw new ArgumentNullException("spell");
            mCastingSpell = spell;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Casting Spell"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            PlayerAI.Client.CastSpell(PlayerAI.TargetSelection, mCastingSpell.SpellId);
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
