using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mClient.Shared;
using mClient.Constants;

namespace mClient.Clients
{
    public class Object
    {
        #region Declarations

        private Coordinate mPosition = null;
        private UInt32[] mFields;

        #endregion

        #region Properties

        public IEnumerable<UInt32> Fields { get { return mFields.AsEnumerable(); } }

        public Coordinate Position
        {
            get { return mPosition; }
            set
            {
                if (value == null)
                    return;

                mPosition = value;
            }
        }

        public UInt32 MaxHealth
        {
            get { return GetFieldValue((int)UpdateFields.UNIT_FIELD_MAXHEALTH); }
            private set { SetField((int)UpdateFields.UNIT_FIELD_MAXHEALTH, value); }
        }

        public UInt32 CurrentHealth
        {
            get { return GetFieldValue((int)UpdateFields.UNIT_FIELD_HEALTH); }
            private set { SetField((int)UpdateFields.UNIT_FIELD_HEALTH, value); }
        }

        public UInt32 Level
        {
            get { return GetFieldValue((int)UpdateFields.UNIT_FIELD_LEVEL); }
            private set { SetField((int)UpdateFields.UNIT_FIELD_LEVEL, value); }
        }

        public UInt32 ObjectFieldEntry
        {
            get { return GetFieldValue((int)UpdateFields.OBJECT_FIELD_ENTRY); }
            private set { SetField((int)UpdateFields.OBJECT_FIELD_ENTRY, value); }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a field value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private UInt32 GetFieldValue(int field)
        {
            if (mFields == null)
                return 0;
            return mFields[field];
        }

        #endregion

        public string Name = null;

        public WoWGuid Guid;
        public ObjectType Type;
        
        //public MovementInfo Movement;

        

        public Object(WoWGuid guid)
        {
            this.Guid = guid;
            mFields = new UInt32[(int)UpdateFields.UNIT_END];
        }

        public void SetPlayer(Character character)
        {
            Name = character.Name;
            Guid = new WoWGuid(character.GUID);
            Position = new Coordinate(character.X, character.Y, character.Z);
        }

        public void UpdatePlayer(Object obj)
        {
        }

        public void Update(uint currentHealth, uint maxHealth, uint level, uint currentPower, uint maxPower)
        {
            this.CurrentHealth = currentHealth;
            this.MaxHealth = maxHealth;
            this.Level = level;
            // TODO: Do we even need power?
        }

        public void SetField(int x, UInt32 value)
        {
            mFields[x] = value;
        }

        

    }
}
