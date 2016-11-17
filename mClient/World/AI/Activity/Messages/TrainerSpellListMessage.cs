using mClient.Constants;
using mClient.Shared;
using mClient.World.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Messages
{
    public class TrainerSpellListMessage : ActivityMessage
    {
        #region Constructors

        protected TrainerSpellListMessage(WorldServerOpCode messageType) : base(messageType)
        {
            CanLearnSpells = new List<TrainerSpellData>();
            CanNotLearnSpells = new List<TrainerSpellData>();
        }

        public TrainerSpellListMessage() : this(WorldServerOpCode.SMSG_TRAINER_LIST)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the guid of the trainer that sent the list
        /// </summary>
        public WoWGuid TrainerGuid { get; set; }

        /// <summary>
        /// Gets a list of spells that are in a state that we can learn them
        /// </summary>
        public List<TrainerSpellData> CanLearnSpells { get; set; }

        /// <summary>
        /// Gets a list of spells that are in a state that we can NOT learn them
        /// </summary>
        public List<TrainerSpellData> CanNotLearnSpells { get; set; }

        #endregion
    }
}
