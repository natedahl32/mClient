using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Death
{
    public class ReleaseSpirit : BaseActivity
    {
        #region Constructors

        public ReleaseSpirit(PlayerAI ai) : base(ai)
        {
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Release Spirit"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();
            PlayerAI.Client.ReclaimCorpse();
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
