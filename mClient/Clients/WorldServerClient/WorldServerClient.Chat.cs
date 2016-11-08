using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

using mClient.Shared;
using mClient.Network;
using mClient.Constants;

namespace mClient.Clients
{
    partial class WorldServerClient
    {
        private ArrayList ChatQueued = new ArrayList();

        [PacketHandlerAtribute(WorldServerOpCode.SMSG_NOTIFICATION)]
        public void HandleNotification(PacketIn packet)
        {
            var message = packet.ReadString();
            Log.WriteLine(LogType.Debug, "Server Notification: {0}", message);
        }

        [PacketHandlerAtribute(WorldServerOpCode.SMSG_CHANNEL_NOTIFY)]
        public void HandleChannelNotify(PacketIn packet)
        {
            // ????
            //Log.WriteLine(LogType.Success, "Dostalem takie gowno: {0}", packet.ReadByte());
        }

        [PacketHandlerAtribute(WorldServerOpCode.SMSG_MESSAGECHAT)]
        public void HandleChat(PacketIn packet)
        {
            try
            {
                
                string channel = null;
                UInt64 senderGuid = 0;
                WoWGuid fguid = null, fguid2 = null;
                string username = null;

                byte Type = packet.ReadByte();
                UInt32 Language = packet.ReadUInt32();

                UInt32 SenderNameLength = 0;
                string senderName = string.Empty;

                // based on message type, read the packets in
                switch ((ChatMsg)Type)
                {
                    case ChatMsg.MonsterWhisper:
                    case ChatMsg.RaidBossWhisper:
                    case ChatMsg.RaidBossEmote:
                    case ChatMsg.MonsterEmote:
                        SenderNameLength = packet.ReadUInt32();
                        senderName = Encoding.Default.GetString(packet.ReadBytes((int)SenderNameLength));
                        senderGuid = packet.ReadUInt64();
                        break;
                    case ChatMsg.Say:
                    case ChatMsg.Party:
                    case ChatMsg.Yell:
                        packet.ReadUInt64();
                        senderGuid = packet.ReadUInt64();
                        break;
                    case ChatMsg.MonsterSay:
                    case ChatMsg.MonsterYell:
                        senderGuid = packet.ReadUInt64();
                        SenderNameLength = packet.ReadUInt32();
                        senderName = Encoding.Default.GetString(packet.ReadBytes((int)SenderNameLength));
                        packet.ReadUInt64();
                        break;
                    case ChatMsg.Channel:
                        channel = packet.ReadString();
                        packet.ReadUInt32();
                        senderGuid = packet.ReadUInt64();
                        break;
                    default:
                        senderGuid = packet.ReadUInt64();
                        break;
                }

                //guid = packet.ReadUInt64();
                fguid = new WoWGuid(senderGuid);

                UInt32 Length = packet.ReadUInt32();
                string Message = Encoding.Default.GetString(packet.ReadBytes((int)Length));
                // chat tag
                packet.ReadByte();

                //Message = Regex.Replace(Message, @"\|H[a-zA-z0-9:].|h", ""); // Why do i should need spells and quest linked? ;>
                //Message = Regex.Replace(Message, @"\|[rc]{1}[a-zA-z0-9]{0,8}", ""); // Colorfull chat message also isn't the most important thing.

                byte afk = 0;
           
                if (fguid.GetOldGuid() == 0)
                {
                    username = "System";
                }
                else
                {
                    if (objectMgr.objectExists(fguid))
                        username = objectMgr.getObject(fguid).Name;
                }

                if (username == null)
                {
                    
                    ChatQueue que = new ChatQueue();
                    que.GUID = fguid;
                    que.Type = Type;
                    que.Language = Language;
                    if ((ChatMsg)Type == ChatMsg.Channel)
                        que.Channel = channel;
                    que.Length = Length;
                    que.Message = Message;
                    que.AFK = afk;

                    // Create a new query for the player
                    var query = new QueryQueue(QueryQueueType.Name, senderGuid) { ExtraData = que };
                    query.AddCallback((o) => HandleChatQuery(o));
                    var obj = GetOrQueueObject(query);
                    if (obj == null)
                        return; // handled by callback
                }

                // Have the player handle the chat message
                player.HandleChatMessage(this, (ChatMsg)Type, fguid, username, Message, channel);

                object[] param = new object[] { (ChatMsg)Type, channel, username, Message };
                mCore.Event(new Event(EventType.EVENT_CHAT_MSG, "0", param)); 
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogType.Error, "Exception Occured");
                Log.WriteLine(LogType.Error, "Message: {0}", ex.Message);
                Log.WriteLine(LogType.Error, "Stacktrace: {0}", ex.StackTrace);
            }
        }

        private void HandleChatQuery(Object obj)
        {
            // Get the query for this object
            var query = mQueryQueue.Where(q => q.Guid == obj.Guid.GetOldGuid() && q.QueryType == QueryQueueType.Name).SingleOrDefault();

            // Get the group member data stored in the query
            var data = query.ExtraData as ChatQueue;

            // Have the player handle the chat message
            player.HandleChatMessage(this, (ChatMsg)data.Type, obj.Guid, obj.Name, data.Message, data.Channel);

            object[] param = new object[] { (ChatMsg)data.Type, data.Channel, obj.Name, data.Message };
            mCore.Event(new Event(EventType.EVENT_CHAT_MSG, "0", param));
        }

        public void SendChatMsg(ChatMsg Type, Languages Language, string Message)
        {
            if (Type != ChatMsg.Whisper || Type != ChatMsg.Channel)
                SendChatMsg(Type, Language, Message, "");
        }

        public void SendChatMsg(ChatMsg Type, Languages Language, string Message, string To)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_MESSAGECHAT);
            packet.Write((UInt32)Type);
            packet.Write((UInt32)Language);
            if ((Type == ChatMsg.Whisper || Type == ChatMsg.Channel) && To != "")
                packet.Write(To);
            packet.Write(Message);
            Send(packet);
        }

        public void SendEmoteMsg(ChatMsg Type, Languages Language, string Message, string To)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_TEXT_EMOTE);
            packet.Write((UInt32)Type);
            packet.Write((UInt32)Language);
            packet.Write(Message);
            Send(packet);
        }

        public void JoinChannel(string channel, string password)
        {
            PacketOut packet = new PacketOut(WorldServerOpCode.CMSG_JOIN_CHANNEL);
            packet.Write((UInt32)0);
            packet.Write((UInt16)0);
            packet.Write(channel);
            packet.Write("");
            Send(packet);
        }
    }

    
   

    public class ChatQueue
    {
        public WoWGuid GUID;
        public byte Type;
        public UInt32 Language;
        public string Channel;
        public UInt32 Length;
        public string Message;
        public byte AFK;

    };
}
