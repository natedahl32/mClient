using System;
using mClient.Constants;

namespace mClient
{
    public static class Config
    {
        public static string Login;
        public static string Password;
        public static string Host;
        public static WoWVersion Version;
        public static long LogFilter;
        public static bool Retail;
        public static bool LogToFile;

        static Config()
        {
            Login = "bot1";
            Password = "bot1";
            Host = "127.0.0.1";

            // Classic client build
            Version.major = 1;
            Version.minor = 12;
            Version.update = 1;
            Version.build = 5875;


            Retail = false;

            LogFilter = 0x0000000000000000;
            LogToFile = true;
        }
    }
}
