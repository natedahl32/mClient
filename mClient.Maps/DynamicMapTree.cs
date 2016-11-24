using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Maps
{
    public class DynamicMapTree
    {
        #region Declarations

        #endregion

        #region Constructors

        #endregion

        #region Public Methods

        /// <summary>
        /// get the hit position and return true if we hit something (in this case the dest position will hold the hit-position)
        /// otherwise the result pos will be the dest pos
        /// </summary>
        /// <param name="srcX"></param>
        /// <param name="srcY"></param>
        /// <param name="srcZ"></param>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <param name="destZ"></param>
        /// <param name="modifyDist"></param>
        /// <returns></returns>
        public bool GetHitPosition(float srcX, float srcY, float srcZ, ref float destX, ref float destY, ref float destZ, float modifyDist)
        {
            // check all static objects first
            bool result0 = false;

            // TODO: Check dynamic objects

            return result0;
        }

        #endregion
    }
}
