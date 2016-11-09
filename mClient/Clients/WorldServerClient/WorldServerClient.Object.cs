﻿using System;
using System.Linq;

using mClient.Network;
using mClient.Shared;
using mClient.Constants;
using System.Collections;
using System.Collections.Generic;
using mClient.Clients.UpdateBlocks;
using mClient.World.Creature;
using mClient.World.GameObject;

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
                        //Log.WriteLine(LogType.Normal, "Handling Fields Update for object: {0}", getObject.Guid.ToString());
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
                        player.ObjectAdded(newObject);
                        //Log.WriteLine(LogType.Normal, "Handling Creation of object: {0}", newObject.Guid.ToString());
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
                newObject.SetField(this, kvp.Key, kvp.Value);
                //Log.WriteLine(LogType.Normal, "Update Field: {0} = {1}", kvp.Key, kvp.Value);
            }
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
                GameObjectQuery(fguid, query.Entry);
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

        public void CreatureQuery(UInt32 entry)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_CREATURE_QUERY);
            packet.Write(entry);
            packet.Write((UInt64)0);
            Send(packet);
        }

        public void GameObjectQuery(WoWGuid guid, UInt32 entry)
		{
			PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_GAMEOBJECT_QUERY);
			packet.Write(entry);
            packet.Write(guid.GetNewGuid());
			Send(packet);
		}

        public void GameObjectQuery(UInt32 entry)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_GAMEOBJECT_QUERY);
            packet.Write(entry);
            packet.Write((UInt64)0);
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

        [PacketHandlerAtribute(WorldServerOpCode.SMSG_GAMEOBJECT_QUERY_RESPONSE)]
        public void Handle_GameObjectQuery(PacketIn packet)
        {
            var go = new GameObjectInfo();
            go.GameObjectId = packet.ReadUInt32();

            try
            {
                go.GameObjectType = packet.ReadUInt32();
                packet.ReadUInt32();
                go.Name = packet.ReadString();
                packet.ReadBytes(3);
                var data = new List<UInt32>();
                for (int i = 0; i < GameObjectInfo.MAX_GAMEOBJECT_DATA_COUNT; i++)
                    data.Add(packet.ReadUInt32());
                go.Data = data;

                GameObjectManager.Instance.Add(go);
            }
            catch(Exception ex)
            { }
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

            // Add them to the creature manager
            var creatureInfo = new CreatureInfo()
            {
                CreatureId = entry.entry,
                Name = entry.name,
                SubName = entry.subname,
                CreatureFlags = entry.flags,
                CreatureType = entry.subtype,
                CreatureFamily = entry.family,
                CreatureRank = entry.rank
            };
            CreatureManager.Instance.Add(creatureInfo);
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

