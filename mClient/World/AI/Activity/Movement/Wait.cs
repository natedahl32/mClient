using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Movement
{
    public class Wait : BaseActivity
    {
        [DllImport("winmm.dll", EntryPoint = "timeGetTime")]
        public static extern uint MM_GetTime();

        #region Declarations

        private uint mWaitFor;
        private uint mStartTime;

        #endregion

        #region Constructors

        public Wait(uint waitForMilliseconds, PlayerAI ai) : base(ai)
        {
            mWaitFor = waitForMilliseconds;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Waiting..."; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();
            mStartTime = MM_GetTime();
        }

        public override void Process()
        {
            // Just wait for time to elapse
            if ((MM_GetTime() - mStartTime) > mWaitFor)
            {
                PlayerAI.CompleteActivity();
                return;
            }
        }

        #endregion
    }
}
