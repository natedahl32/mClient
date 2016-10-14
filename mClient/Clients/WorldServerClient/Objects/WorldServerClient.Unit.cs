using mClient.Constants;
using mClient.Shared;
using System;

namespace mClient.Clients
{
    public class Unit : Object
    {
        public Unit(WoWGuid guid) : base(guid)
        {
        }

        #region Properties

        public UInt32 MaxHealth
        {
            get { return GetFieldValue((int)UnitFields.UNIT_FIELD_MAXHEALTH); }
            private set { SetField((int)UnitFields.UNIT_FIELD_MAXHEALTH, value); }
        }

        public UInt32 CurrentHealth
        {
            get { return GetFieldValue((int)UnitFields.UNIT_FIELD_HEALTH); }
            private set { SetField((int)UnitFields.UNIT_FIELD_HEALTH, value); }
        }

        public UInt32 Level
        {
            get { return GetFieldValue((int)UnitFields.UNIT_FIELD_LEVEL); }
            private set { SetField((int)UnitFields.UNIT_FIELD_LEVEL, value); }
        }

        #endregion

        #region Public Methods

        public void Update(uint currentHealth, uint maxHealth, uint level, uint currentPower, uint maxPower)
        {
            this.CurrentHealth = currentHealth;
            this.MaxHealth = maxHealth;
            this.Level = level;
            // TODO: Do we even need power?
        }

        #endregion
    }
}
