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
        }

        public UInt32 CurrentHealth
        {
            get { return GetFieldValue((int)UpdateFields.UNIT_FIELD_HEALTH); }
        }

        public UInt32 ObjectFieldEntry
        {
            get { return GetFieldValue((int)UpdateFields.OBJECT_FIELD_ENTRY); }
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

        public void SetField(int x, UInt32 value)
        {
            mFields[x] = value;
        }

        

    }
}
