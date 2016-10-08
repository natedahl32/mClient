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

        #endregion

        #region Properties

        public Coordinate Position
        {
            get { return mPosition; }
            set
            {
                if (value == null)
                    return;

                if (mPosition == null || mPosition.X != value.Y || mPosition.Y != value.Y || mPosition.Z != value.Z || mPosition.O != value.O)
                {
                    var newPosition = value;
                }
                mPosition = value;
            }
        }

        #endregion

        public string Name = null;

        public WoWGuid Guid;
        public ObjectType Type;
        public UInt32[] Fields;
        //public MovementInfo Movement;

        public UInt32 Health
        {
            get
            {
                return Fields[(int)UpdateFields.UNIT_FIELD_HEALTH];
            }
        }

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
            Fields[x] = value;
        }

        

    }
}
