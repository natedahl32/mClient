﻿using System;
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
        private UInt32[] mFields = new UInt32[2000];

        #endregion

        #region Properties

        public virtual Coordinate Position
        {
            get { return mPosition; }
            set
            {
                if (value == null)
                    return;

                mPosition = value;
            }
        }

        public UInt32 ObjectFieldEntry
        {
            get { return GetFieldValue((int)ObjectFields.OBJECT_FIELD_ENTRY); }
            private set { SetField((int)ObjectFields.OBJECT_FIELD_ENTRY, value); }
        }

        public UInt64 ObjectFieldGuid
        {
            get { return GetGuid(GetFieldValue((int)ObjectFields.OBJECT_FIELD_GUID), GetFieldValue(((int)ObjectFields.OBJECT_FIELD_GUID) + 1)); }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a field value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected UInt32 GetFieldValue(int field)
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

        

        protected Object(WoWGuid guid)
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

        #region Converstion Methods for Fields

        protected static UInt64 GetGuid(UInt32 val, UInt32 val2)
        {
            var bytes1 = BitConverter.GetBytes(val);
            var bytes2 = BitConverter.GetBytes(val2);

            return BitConverter.ToUInt64(bytes1.Concat(bytes2).ToArray(), 0);
        }

        protected static WoWGuid GetWoWGuid(UInt32 val, UInt32 val2)
        {
            return new WoWGuid(GetGuid(val, val2));
        }

        #endregion

        #region Static Builder Methods

        public static Object CreateObjectByType(WoWGuid guid, ObjectType type)
        {
            switch(type)
            {
                case ObjectType.Corpse:
                    return new Corpse(guid) { Type = ObjectType.Corpse };
                case ObjectType.Container:
                    return new Container(guid) { Type = ObjectType.Container };
                case ObjectType.DynamicObject:
                    return new DynamicObject(guid) { Type = ObjectType.DynamicObject };
                case ObjectType.GameObject:
                    return new GameObject(guid) { Type = ObjectType.GameObject };
                case ObjectType.Item:
                    return new Item(guid) { Type = ObjectType.Item };
                case ObjectType.Object:
                    return new Object(guid) { Type = ObjectType.Object };
                case ObjectType.Player:
                    return new PlayerObj(guid) { Type = ObjectType.Player };
                case ObjectType.Unit:
                    return new Unit(guid) { Type = ObjectType.Unit };
                default:
                    return null;
            }
        }

        #endregion
    }
}