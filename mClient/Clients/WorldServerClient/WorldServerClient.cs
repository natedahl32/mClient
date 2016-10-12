﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Resources;

using mClient.Shared;
using mClient.Network;
using mClient.Crypt;
using mClient.Constants;
using mClient.Terrain;
using mClient.World;

namespace mClient.Clients
{
    public partial class WorldServerClient
    {

        private UInt32 ServerSeed;
        private UInt32 ClientSeed;
        private Random random = new Random();

        public Socket mSocket = null;

        [DllImport("winmm.dll", EntryPoint = "timeGetTime")]
        public static extern uint MM_GetTime();

        
        private System.Timers.Timer aTimer = new System.Timers.Timer();
        private System.Timers.Timer uTimer = new System.Timers.Timer();
        private UInt32 Ping_Seq;
        private UInt32 Ping_Req_Time;
        private UInt32 Ping_Res_Time;
        public UInt32 Latency;

        // Connection Info
        readonly string mUsername;
        private byte[] mKey;
        public bool Connected;

        //Packet Handling
        private PacketHandler pHandler;
        private PacketLoop pLoop = null;
        public PacketCrypt mCrypt;
        
        //Managers
        public ObjectMgr objectMgr = null;
        public MovementMgr movementMgr = null;
        public CombatMgr combatMgr = null;
        public TerrainMgr terrainMgr = null;

        // Player
        public Player player = null;

        // Queues
        protected System.Object mQueryQueueLock = new System.Object();
        public List<QueryQueue> mQueryQueue = new List<QueryQueue>();

        //
        public Realm realm;
        public Character[] Charlist = new Character[0];
        
        public WorldServerClient(string user, Realm rl, byte[] key)
        {
            mUsername = user.ToUpper();
            objectMgr = new ObjectMgr();
            terrainMgr = new TerrainMgr();
            movementMgr = new MovementMgr(this);
            combatMgr = new CombatMgr(this);
            realm = rl;
            mKey = key;
        }

        public WorldServerClient(Realm rl, byte[] key)
        {
            mUsername = Config.Login.ToUpper();
            objectMgr = new ObjectMgr();
            movementMgr = new MovementMgr(this);
            combatMgr = new CombatMgr(this);
            terrainMgr = new TerrainMgr();
            realm = rl;
            mKey = key;
        }


        public void Connect()
        {
            string[] address = realm.Address.Split(':');
            byte[] test = new byte[1];
            test[0] = 10;
            mCrypt = new PacketCrypt(test);
            IPAddress WSAddr = Dns.GetHostAddresses(address[0])[0];
            int WSPort = Int32.Parse(address[1]);
            IPEndPoint ep = new IPEndPoint(WSAddr, WSPort);
            
            try
            {
                mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                mSocket.Connect(ep);
                Log.WriteLine(LogType.Success, "Successfully connected to WorldServer at: {0}!", realm.Address);

            }
            catch (SocketException ex)
            {
                Log.WriteLine(LogType.Error, "Failed to connect to realm: {0}", ex.Message);
                Disconnect();
                return;
            }

            byte[] nullA = new byte[24];
            mCrypt = new PacketCrypt(nullA);
            Connected = true;
            pHandler = new PacketHandler(this);
            pLoop = new PacketLoop(this, mSocket);
            pLoop.Start();
            pHandler.Initialize();
        }

        void CreatePlayer(Object playerObject, Character c)
        {
            // Make sure we don't already have a player here
            if (player != null) throw new ApplicationException("Player already exists. Cannot create another player!");

            player = new Player(playerObject, c.Race, c.Class, c.Level, c.MapID, c.Gender, c.GuildId, c.CharacterFlags, this);
            player.PlayerAI.StartAI();
        }

        void PingLoop()
        {
            uTimer.Elapsed += new ElapsedEventHandler(Ping);
            uTimer.Interval = 1000000;
            uTimer.Enabled = true;

            Ping_Seq = 1;
            Latency = 1;
        }

        void Ping(object source, ElapsedEventArgs e)
        {
            while(!mSocket.Connected)
            {
                uTimer.Enabled = false;
                uTimer.Stop();
                return;
            }

            Ping_Req_Time = MM_GetTime();

            PacketOut ping = new PacketOut(WorldServerOpCode.CMSG_PING);
            ping.Write(Ping_Seq);
            ping.Write(Latency);
            Send(ping);
        }

        public void Send(PacketOut packet)
        {
            try
            {
                if (!Connected)
                    return;
                Log.WriteLine(LogType.Network, "Sending packet: {0}", packet.packetId);
                if (!Connected)
                    return;
                Byte[] Data = packet.ToArray();

                int Length = Data.Length;
                byte[] Packet = new byte[2 + Length];
                Packet[0] = (byte)(Length >> 8);
                Packet[1] = (byte)(Length & 0xff);
                Data.CopyTo(Packet, 2);
                mCrypt.Encrypt(Packet, 0, 6);
                //While writing this part of code I had a strange feeling of Deja-Vu or whatever it's called :>

                Log.WriteLine(LogType.Packet,"{0}", packet.ToHex());
                mSocket.Send(Packet);
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogType.Error, "Exception Occured");
                Log.WriteLine(LogType.Error, "Message: {0}", ex.Message);
                Log.WriteLine(LogType.Error, "Stacktrace: {0}", ex.StackTrace);
            }
            
        }

        public void StartHeartbeat()
        {
            aTimer.Elapsed += new ElapsedEventHandler(Heartbeat);
            aTimer.Interval = 500;
            aTimer.Enabled = true;
        }

        public void HandlePacket(PacketIn packet)
        {
            //Log.WriteLine(LogType.Packet, "{0}", packet.ToHex());
            pHandler.HandlePacket(packet);
           
        }

        public void Disconnect()
        {
            Event e = new Event(EventType.EVENT_DISCONNECT, "", null);
            mCore.Event(e);
        }

        public void HardDisconnect()
        {
            if (mSocket != null && mSocket.Connected)
                mSocket.Close();
            
            if (movementMgr != null)
                movementMgr.Stop();
            if (combatMgr != null)
                combatMgr.Stop();
            if (pLoop != null)
                pLoop.Stop();
            Connected = false;
        }

        ~WorldServerClient()
        {
            HardDisconnect();
        }
    }
}
