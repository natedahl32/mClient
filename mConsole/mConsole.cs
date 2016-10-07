using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mClient;
using mClient.Clients;
using mClient.Constants;
using mClient.Shared;
using mConsole.Commands;

namespace mConsole
{
    partial class mConsole
    {
        static byte[] k;
        static LogonServerClient lclient;
        static WorldServerClient wclient;
        
        public static void Main(string[] args)
        {
            Console.WindowWidth = 100;
            mCore.Init(EventHandler);
            CommandHandler.Initialize();

            while (true)
            {
                Console.Write(">");
                string command = Console.ReadLine();
                CommandHandler.HandleCommand(command);
            }
        
            
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
                case EventType.EVENT_DISCONNECT:
                    //HandleDisconnect();
                    break;
            }
        }
    }
}
