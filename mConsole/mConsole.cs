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
using mClient.World.Guild;
using mClient.World.Talents;

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
            GuildManager.Instance.Load();
            SpecManager.Instance.Load();

            // For testing purposes we can create a spec programmatically
            //CreateSpec();

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
                GuildManager.Instance.Serialize();
                SpecManager.Instance.Serialize();
            }
            return false;
        }

        static void CreateSpec()
        {
            var spec = new Spec("Protection Warrior Off-Tank & Group Spec");
            spec.Description = "Talent spec for end game group and off-tank raid content";
            spec.ForClass = Classname.Warrior;
            spec.Talents[0] = 12320;
            spec.Talents[1] = 12852;
            spec.Talents[2] = 12853;
            spec.Talents[3] = 12855;
            spec.Talents[4] = 12856;
            spec.Talents[5] = 16462;
            spec.Talents[6] = 16463;
            spec.Talents[7] = 16464;
            spec.Talents[8] = 16465;
            spec.Talents[9] = 16466;
            spec.Talents[10] = 12298;
            spec.Talents[11] = 12724;
            spec.Talents[12] = 12725;
            spec.Talents[13] = 12726;
            spec.Talents[14] = 12727;
            spec.Talents[15] = 12299;
            spec.Talents[16] = 12761;
            spec.Talents[17] = 12762;
            spec.Talents[18] = 12763;
            spec.Talents[19] = 12764;
            spec.Talents[20] = 12303;
            spec.Talents[21] = 12788;
            spec.Talents[22] = 12789;
            spec.Talents[23] = 12791;
            spec.Talents[24] = 12792;
            spec.Talents[25] = 12945;
            spec.Talents[26] = 12301;
            spec.Talents[27] = 12818;
            spec.Talents[29] = 12975;
            spec.Talents[30] = 12297;
            spec.Talents[31] = 12809;
            spec.Talents[32] = 12312;
            spec.Talents[33] = 12803;
            spec.Talents[34] = 12750;
            spec.Talents[35] = 12751;
            spec.Talents[36] = 12752;
            spec.Talents[37] = 12753;
            spec.Talents[38] = 16538;
            spec.Talents[39] = 16539;
            spec.Talents[40] = 16540;
            spec.Talents[41] = 16541;
            spec.Talents[42] = 16542;
            spec.Talents[43] = 23922;
            spec.Talents[44] = 12324;
            spec.Talents[45] = 12876;
            spec.Talents[46] = 12877;
            spec.Talents[47] = 12878;
            spec.Talents[48] = 12879;
            spec.Talents[49] = 12323;
            spec.Talents[50] = 12295;
            spec.Talents[51] = 12676;
            SpecManager.Instance.Add(spec);
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
