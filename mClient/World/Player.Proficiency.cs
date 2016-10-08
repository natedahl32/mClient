using mClient.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.World
{
    public class Proficiency
    {

        #region Constructors

        public Proficiency(ItemClass iClass, UInt32 subClassMask)
        {
            this.ItemClass = iClass;
            this.ItemSubClassMask = subClassMask;
            this.ProficiencyLevel = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Item Class of this proficiency
        /// </summary>
        public ItemClass ItemClass { get; private set; }

        /// <summary>
        /// Gets the Item sub class mask
        /// </summary>
        public UInt32 ItemSubClassMask { get; private set; }

        /// <summary>
        /// Gets the level of proficiency
        /// </summary>
        public UInt32 ProficiencyLevel { get; private set; }

        #endregion
    }
}
