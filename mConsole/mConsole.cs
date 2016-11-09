using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using mClient;
using mClient.Clients;
using mClient.Constants;
using mClient.Shared;
using mConsole.Commands;
using mClient.World.Quest;
using mClient.World.Items;
using mClient.World.Creature;
using mClient.World.GameObject;

namespace mConsole
{
    partial class mConsole
    {
        static byte[] k;
        static LogonServerClient lclient;
        static WorldServerClient wclient;

        static ConsoleEventDelegate handler;
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        public static void Main(string[] args)
        {
            // event delegate setup to catch when the console app is closed
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);

            Console.WindowWidth = 100;
            mCore.Init(EventHandler);
            CommandHandler.Initialize();

            // Load our caches
            CreatureManager.Instance.Load();
            GameObjectManager.Instance.Load();
            QuestManager.Instance.Load();
            ItemManager.Instance.Load();

            while (true)
            {
                Console.Write(">");
                string command = Console.ReadLine();
                CommandHandler.HandleCommand(command);
            }
        
            
        }

        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                // Serialize our managers
                CreatureManager.Instance.Serialize();
                GameObjectManager.Instance.Serialize();
                QuestManager.Instance.Serialize();
                ItemManager.Instance.Serialize();
            }
            return false;
        }

        public static void HandleRealmlist(Realm[] rlist)
        {
            int x = 0;
            foreach (Realm rl in rlist)
            {
                Log.WriteLine(LogType.Normal, "[{0}] {1} - {2}", x + 1, rl.Name, rl.Address); x++;
            }
      
        }

        static void HandleCharlist(Character[] rlist)
        {
            int x = 0;
            foreach (Character rl in rlist)
            {
                Log.WriteLine(LogType.Normal, "[{0}] {1} - {2}", x + 1, rl.Name, rl.Level); x++;
            }
        }

        static void HandleChatMsg(ChatMsg msg, string v1, string v2, string v3)
        {
            Log.WriteLine(LogType.Chat, "[{0}][{1}][{2}]{3}", msg, v1, v2, v3);
        }

        delegate void EventInvoke(Event e);

        // Event Handler
        public static void EventHandler(Event e)
        {
                EventHandle(e);
        }

        public static void EventHandle(Event e)
        {
            switch (e.eventType)
            {
                case EventType.EVENT_REALMLIST:
                    HandleRealmlist((Realm[])e.eventArgs[0]);
                    break;
                case EventType.EVENT_CHARLIST:
                    HandleCharlist((Character[])e.eventArgs[0]);
                    break;
                case EventType.EVENT_LOG:
                    Console.WriteLine((String)e.eventArgs[0] + "\n\r");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case EventType.EVENT_CHAT_MSG:
                    HandleChatMsg((ChatMsg)e.eventArgs[0], (string)e.eventArgs[1], (string)e.eventArgs[2], (string)e.eventArgs[3]);
                    break;
                case EventType.EVENT_ERROR:
                    //MessageBox.Show((string)e.eventArgs[0], "Error!");
                    break;
                case EventType.EVENT_DISCONNECT_LS:
                case EventType.EVENT_DISCONNECT_WS:
                    //HandleDisconnect();
                    break;
            }
        }
    }
}
