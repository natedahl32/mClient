﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Constants
{
    public struct WoWVersion
    {
        public WoWVersion(byte a, byte b, byte c, ushort d)
        {
            major = a; minor = b; update = c; build = d;
        }

        public WoWVersion(String versionString)
        {
            String[] versionParts = versionString.Split(new char[] { '.' });
            Byte.TryParse(versionParts[0], out major);
            Byte.TryParse(versionParts[1], out minor);
            Byte.TryParse(versionParts[2], out update);
            UInt16.TryParse(versionParts[3], out build);
        }

        public byte major;
        public byte minor;
        public byte update;
        public UInt16 build;
    }

    public enum ServiceType
    {
        None = 0,
        Logon = 1,
        World = 2,
        Count
    }

    public enum QueryQueueType
    {
        Creature,
        Object,
        Name
    }

    public enum MoneyConstants
    {
        COPPER = 1,
        SILVER = COPPER * 100,
        GOLD = SILVER * 100
    }

    public static class ConstantValues
    {
        public const int MAXIMUM_PLAYERS_IN_GROUP = 5;
    }
}
