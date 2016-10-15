using System;
using System.Linq;

using mClient.Network;
using mClient.Shared;
using mClient.Constants;
using System.Collections;
using System.Collections.Generic;
using mClient.Clients.UpdateBlocks;

namespace mClient.Clients
{
    public partial class WorldServerClient
    {
        
		[PacketHandlerAtribute(WorldServerOpCode.SMSG_COMPRESSED_UPDATE_OBJECT)]
		public void HandleCompressedObjectUpdate(PacketIn packet)
		{
			try
			{
				Int32 size = packet.ReadInt32();
				byte[] decomped = mClient.Shared.Compression.Decompress(size, packet.ReadRemaining());
				packet = new PacketIn(decomped, 1);
				HandleObjectUpdate(packet);
			}
			catch(Exception ex)
			{
				Log.WriteLine(LogType.Error, "{1} \n {0}", ex.StackTrace, ex.Message);
			}
		}

		[PacketHandlerAtribute(WorldServerOpCode.SMSG_UPDATE_OBJECT)]
		public void HandleObjectUpdate(PacketIn packet)
		{
            UInt32 UpdateBlocks = packet.ReadUInt32();
            byte hasTransport = packet.ReadByte();

            for(int allBlocks = 0; allBlocks < UpdateBlocks; allBlocks++)
            {
                UpdateType type = (UpdateType)packet.ReadByte();
                    
                WoWGuid updateGuid;
                uint fCount;

                switch (type)
                {
                    case UpdateType.Values:
                        Object getObject;
                        updateGuid = packet.ReadPackedGuidToWoWGuid();
                        if (objectMgr.objectExists(updateGuid))
                        {
                            getObject = objectMgr.getObject(updateGuid);
                        }
                        else
                        {
                            getObject = Object.CreateObjectByType(updateGuid, ObjectType.Player);
                            objectMgr.addObject(getObject);
                        }
                        Log.WriteLine(LogType.Normal, "Handling Fields Update for object: {0}", getObject.Guid.ToString());
                        HandleUpdateObjectFieldBlock(packet, getObject);
                        objectMgr.updateObject(getObject);
                        break;

                    case UpdateType.Create:
                    case UpdateType.CreateSelf:
                        updateGuid = packet.ReadPackedGuidToWoWGuid();
                        var objectType = packet.ReadByte();
                        fCount = GeUpdateFieldsCount(objectType);

                        Object newObject;
                        if (objectMgr.objectExists(updateGuid))
                        {
                            newObject = objectMgr.getObject(updateGuid);
                            newObject.Type = (ObjectType)objectType;
                        }
                        else
                        {
                            newObject = Object.CreateObjectByType(updateGuid, (ObjectType)objectType);
                            objectMgr.addObject(newObject);
                        }
                        
                        HandleUpdateMovementBlock(packet, newObject);
                        HandleUpdateObjectFieldBlock(packet, newObject);
                        objectMgr.updateObject(newObject);
                        Log.WriteLine(LogType.Normal, "Handling Creation of object: {0}", newObject.Guid.ToString());
                        break;

                    case UpdateType.OutOfRange:
                        UInt32 outOfRangeGuids = packet.ReadUInt32();
                        WoWGuid guid;
                        for (int i = 0; i < outOfRangeGuids; i++)
                            guid = packet.ReadPackedGuidToWoWGuid();
                        
                        break;

                    case UpdateType.NearObjects:
                        UInt32 guidCount = packet.ReadUInt32();
                        WoWGuid nearGuid;
                        for (int i = 0; i < guidCount; i++)
                            nearGuid = packet.ReadPackedGuidToWoWGuid();

                        break;

                    case UpdateType.Movement:
                        Object moveObject;
                        var moveGuid = packet.ReadPackedGuidToWoWGuid();
                        if (objectMgr.objectExists(moveGuid))
                        {
                            moveObject = objectMgr.getObject(moveGuid);
                        }
                        else
                        {
                            // shouldn't ever happen
                            moveObject = Object.CreateObjectByType(moveGuid, ObjectType.Object);
                            objectMgr.addObject(moveObject);
                        }

                        HandleUpdateMovementBlock(packet, moveObject);
                        objectMgr.updateObject(moveObject);

                        break;
                    //case UpdateType.OutOfRange:
                    //    fCount = packet.ReadByte();
                    //    for (int j = 0; j < fCount; j++)
                    //    {
                    //        WoWGuid guid = new WoWGuid(packet.ReadUInt64());
                    //        Log.WriteLine(LogType.Normal, "Handling delete for object: {0}", guid.ToString());
                    //        if (objectMgr.objectExists(guid))
                    //            objectMgr.delObject(guid);
                    //    }
                    //    break;
                }
            }
              
        }
        

        public void HandleUpdateMovementBlock(PacketIn packet, Object newObject)
        {
            var movementBlock = MovementBlock.Read(packet);
            newObject.Position = new Coordinate(movementBlock.Movement.Position.X, movementBlock.Movement.Position.Y, movementBlock.Movement.Position.Z, movementBlock.Movement.Facing);

            //UInt16 flags = packet.ReadUInt16();


            //if((flags & (UInt16)ObjectUpdateFlags.UPDATEFLAG_LIVING) >= 1)
            //{
            //    UInt32 moveFlags = packet.ReadUInt32();
            //    UInt32 time = packet.ReadUInt32();
            //    newObject.Position = new Coordinate(packet.ReadFloat(), packet.ReadFloat(), packet.ReadFloat(), packet.ReadFloat());

            //    if ((moveFlags & (UInt32)MovementFlags.MOVEMENTFLAG_ONTRANSPORT) >= 1)
            //    {
            //        var transportGuid = packet.ReadUInt64();
            //        var transportX = packet.ReadFloat();
            //        var transportY = packet.ReadFloat();
            //        var transportZ = packet.ReadFloat();
            //        var transportO = packet.ReadFloat();
            //    }

            //    if (((moveFlags & (UInt32)MovementFlags.MOVEMENTFLAG_SWIMMING) >= 1))
            //    {
            //        var pitch = packet.ReadFloat();
            //    }

            //    var fallTime = packet.ReadUInt32();

            //    if ((moveFlags & (UInt32)MovementFlags.MOVEMENTFLAG_FALLING) >= 1)
            //    {
            //        var jumpVelocity = packet.ReadFloat();
            //        var jumpSinAngle = packet.ReadFloat();
            //        var jumpCosAngle = packet.ReadFloat();
            //        var jumpXYSpeed = packet.ReadFloat();
            //    }

            //    if ((moveFlags & (UInt32)MovementFlags.MOVEMENTFLAG_SPLINE_ELEVATION) >= 1)
            //    {
            //        var unk1 = packet.ReadFloat();
            //    }

            //    // Read all speeds
            //    var moveWalkSpeed = packet.ReadFloat();
            //    var moveRunSpeed = packet.ReadFloat();
            //    var moveRunBackSpeed = packet.ReadFloat();
            //    var moveSwimSpeed = packet.ReadFloat();
            //    var moveSwimBackSpeed = packet.ReadFloat();
            //    var moveTurnRateSpeed = packet.ReadFloat();

            //    if ((moveFlags & (UInt32)MovementFlags.MOVEMENTFLAG_SPLINE_ENABLED) >= 1) //spline ;/
            //    {
            //        UInt32 splineFlags = packet.ReadUInt32();

            //        // if (splineFlags.final_point) - looks to be always true
            //        packet.ReadFloat();
            //        packet.ReadFloat();
            //        packet.ReadFloat();
            //        // if (splineFlags.final_target) - looks to be always true
            //        packet.ReadUInt64();
            //        // if (splineFlags.final_angle) - looks to be always true
            //        packet.ReadFloat();

            //        // TimePassed, Duration and Id
            //        packet.ReadUInt32();
            //        packet.ReadUInt32();
            //        packet.ReadUInt32();

            //        // Nodes
            //        packet.ReadUInt32();

            //        // Spline Count
            //        UInt32 splineCount = packet.ReadUInt32();

            //        for (UInt32 j = 0; j < splineCount; j++)
            //        {
            //            packet.ReadBytes(12); // skip 3 float
            //        }

            //        // Final floats
            //        packet.ReadFloat();
            //        packet.ReadFloat();
            //        packet.ReadFloat();
            //    }
            //}
            //else if ((flags & (UInt32)ObjectUpdateFlags.UPDATEFLAG_HAS_POSITION) >= 1)
            //{
            //    newObject.Position = new Coordinate(packet.ReadFloat(), packet.ReadFloat(), packet.ReadFloat(), packet.ReadFloat());
            //}

            //if ((flags & (UInt32)ObjectUpdateFlags.UPDATEFLAG_HIGHGUID) >= 1)
            //{
            //    packet.ReadUInt32();
            //}

            //if ((flags & (UInt32)ObjectUpdateFlags.UPDATEFLAG_ALL) >= 1)
            //{
            //    packet.ReadUInt32();
            //}

            //if ((flags & (UInt32)ObjectUpdateFlags.UPDATEFLAG_FULLGUID) >= 1)
            //{
            //    byte mask = packet.ReadByte();
            //    WoWGuid guid = new WoWGuid(mask, packet.ReadBytes(WoWGuid.BitCount8(mask)));
            //}

            //if ((flags & (UInt32)ObjectUpdateFlags.UPDATEFLAG_TRANSPORT) >= 1)
            //{
            //    packet.ReadUInt32();
            //}
        }

        public void HandleUpdateObjectFieldBlock(PacketIn packet, Object newObject)
        {
            byte blocksCount = packet.ReadByte();
            var updateMask = new int[blocksCount];

            for (int i = 0; i < updateMask.Length; ++i)
                updateMask[i] = packet.ReadInt32();

            var mask = new BitArray(updateMask);

            var values = new Dictionary<int, uint>();

            for (int i = 0; i < mask.Count; ++i)
                if (mask[i])
                    values[i] = packet.ReadUInt32();

            foreach (var kvp in values)
            {
                newObject.SetField(kvp.Key, kvp.Value);
                Log.WriteLine(LogType.Normal, "Update Field: {0} = {1}", kvp.Key, kvp.Value);
            }
            //uint length = packet.ReadByte();
            
            //UpdateMask UpdateMask = new UpdateMask();
            //UpdateMask.SetCount((ushort)(length));
            //UpdateMask.SetMask(packet.ReadBytes((int)length * 4), (ushort)length);
            //UInt32[] Fields = new UInt32[UpdateMask.GetCount()];

            //for (int i = 0; i < UpdateMask.GetCount(); i++)
            //{
            //    if (UpdateMask.GetBit((ushort)i))
            //    {
            //        UInt32 val = packet.ReadUInt32();
            //        newObject.SetField(i, val);
            //        Log.WriteLine(LogType.Normal, "Update Field: {0} = {1}", i, val);
            //    }
            //}
        }

        
        [PacketHandlerAtribute(WorldServerOpCode.SMSG_DESTROY_OBJECT)]
        public void DestroyObject(PacketIn packet)
        {
            WoWGuid guid = new WoWGuid(packet.ReadUInt64());
            objectMgr.delObject(guid);

        }

        public uint GeUpdateFieldsCount(uint updateId)
        {
            switch ((ObjectType)updateId)
            {
                case ObjectType.Object:
                    return (uint)GameObjectFields.GAMEOBJECT_END;

                case ObjectType.Unit:
                    return (uint)UnitFields.UNIT_END;

                case ObjectType.Player:
                    return (uint)PlayerFields.PLAYER_END;

                case ObjectType.Item:
                    return (uint)ItemFields.ITEM_END;

                case ObjectType.Container:
                    return (uint)ContainerFields.CONTAINER_END;

                case ObjectType.DynamicObject:
                    return (uint)DynamicObjectFields.DYNAMICOBJECT_END;

                case ObjectType.Corpse:
                    return (uint)CorpseFields.CORPSE_END;

                case ObjectType.GameObject:
                    return (uint)GameObjectFields.GAMEOBJECT_END;

                default:
                    return 0;
            }
        }

        public Object GetOrQueueObject(QueryQueue query)
        {
            Object obj = null;
            WoWGuid fguid = new WoWGuid(query.Guid);
            if (objectMgr.objectExists(fguid))
            {
                obj = objectMgr.getObject(fguid);
                return obj;
            }

            lock(mQueryQueueLock)
            {
                // Object does not exist, queue the query
                mQueryQueue.Add(query);
            }
            
            if (query.QueryType == QueryQueueType.Creature)
                CreatureQuery(fguid, query.Entry);
            else if (query.QueryType == QueryQueueType.Object)
                ObjectQuery(fguid, query.Entry);
            else if (query.QueryType == QueryQueueType.Name)
                QueryName(fguid);

            // No object was found but it was queued up. Return nothing.
            return null;
        }

        public void CreatureQuery(WoWGuid guid, UInt32 entry)
		{
			PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_CREATURE_QUERY);
            packet.Write(entry);
			packet.Write(guid.GetNewGuid());
			Send(packet);
		}

		public void ObjectQuery(WoWGuid guid, UInt32 entry)
		{
			PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_Object_QUERY);
			packet.Write(entry);
            packet.Write(guid.GetNewGuid());
			Send(packet);
		}

		public void QueryName(WoWGuid guid)
		{
			PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_NAME_QUERY);
            packet.Write(guid.GetNewGuid());
			Send(packet);
		}

		public void QueryName(UInt64 guid)
		{
			PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_NAME_QUERY);
			packet.Write(guid);
			Send(packet);
		}

        [PacketHandlerAtribute(WorldServerOpCode.SMSG_CREATURE_QUERY_RESPONSE)]
        public void Handle_CreatureQuery(PacketIn packet)
        {
            Entry entry = new Entry();
            entry.entry = packet.ReadUInt32();
            entry.name = packet.ReadString();
            entry.blarg = packet.ReadBytes(3);
            entry.subname = packet.ReadString();
            entry.flags = packet.ReadUInt32();
            entry.subtype = packet.ReadUInt32();
            entry.family = packet.ReadUInt32();
            entry.rank = packet.ReadUInt32();

            foreach (Object obj in objectMgr.getObjectArray())
            {
                if (obj.ObjectFieldEntry == entry.entry)
                {
                    obj.Name = entry.name;
                    objectMgr.updateObject(obj);

                    // Get any query associated with this object and invoke the callbacks
                    var query = mQueryQueue.Where(q => q.Guid == obj.Guid.GetOldGuid() && q.QueryType == QueryQueueType.Creature).SingleOrDefault();
                    if (query != null)
                    {
                        lock(mQueryQueueLock)
                        {
                            foreach (var callback in query.Callbacks)
                                callback.Invoke(obj);
                            mQueryQueue.Remove(query);
                        }
                    }
                }
            }

                


        }


		[PacketHandlerAtribute(WorldServerOpCode.SMSG_NAME_QUERY_RESPONSE)]
		public  void Handle_NameQuery(PacketIn packet)
		{

            WoWGuid guid = new WoWGuid(packet.ReadUInt64());
            string name = packet.ReadString();
            packet.ReadByte();
            Race Race = (Race)packet.ReadUInt32();
            Gender Gender = (Gender)packet.ReadUInt32();
            Classname Class = (Classname)packet.ReadUInt32();

            Object obj = null;
            if (objectMgr.objectExists(guid))    // Update existing Object
            {
                obj = objectMgr.getObject(guid);
                obj.Name = name;
                objectMgr.updateObject(obj);
            }
            else                // Create new Object        -- FIXME: Add to new 'names only' list?
            {
                obj = Object.CreateObjectByType(guid, ObjectType.Player);
                obj.Name = name;
                objectMgr.addObject(obj);

                // Now using the callbacks handler below
                /* Process chat message if we looked them up now */
                //for (int i = 0; i < ChatQueued.Count; i++)
                //{
                //    ChatQueue message = (ChatQueue)ChatQueued[i];
                //    if (message.GUID.GetOldGuid() == guid.GetOldGuid())
                //    {
                //        // Process the chat event
                //        object[] param = new object[] { (ChatMsg)message.Type, message.Channel, name, message.Message };
                //        mCore.Event(new Event(EventType.EVENT_CHAT_MSG, "0", param));

                //        // The event takes care of this now
                //        //Log.WriteLine(LogType.Chat, "[{1}] {0}", message.Message, name);
                //        ChatQueued.Remove(message);
                //    }
                //}

            }

            // Get any query associated with this object and invoke the callbacks
            var query = mQueryQueue.Where(q => q.Guid == obj.Guid.GetOldGuid() && q.QueryType == QueryQueueType.Name).SingleOrDefault();
            if (query != null)
            {
                lock(mQueryQueueLock)
                {
                    foreach (var callback in query.Callbacks)
                        callback.Invoke(obj);
                    mQueryQueue.Remove(query);
                }
            }
        }
        
    }

}

