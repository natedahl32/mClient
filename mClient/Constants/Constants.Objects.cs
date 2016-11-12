using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient.Constants
{
    public enum Classname : uint
    {
        Warrior = 1,
        Paladin = 2,
        Hunter = 3,
        Rogue = 4,
        Priest = 5,
        //DeathKnight = 6,
        Shaman = 7,
        Mage = 8,
        Warlock = 9,
        //??	= 10
        Druid = 11
    }

    public enum Race
    {
        Human = 1,
        Orc = 2,
        Dwarf = 3,
        NightElf = 4,
        Undead = 5,
        Tauren = 6,
        Gnome = 7,
        Troll = 8,
        Goblin = 9,
        BloodElf = 10,
        Draenei = 11,
        FelOrc = 12,
        Naga = 13,
        Broken = 14,
        Skeleton = 15
    }

    public enum Gender : int
    {
        Male = 0,
        Female = 1,
        Neutral = 2
    }

    public enum UpdateType
    {
        /// <summary>
        /// Update type that update only object field values.
        /// </summary>
        Values = 0,
        /// <summary>
        /// Update type that update only object movement.
        /// </summary>
        Movement = 1,
        /// <summary>
        /// Update type that create an object (full update).
        /// </summary>
        Create = 2,
        /// <summary>
        /// Update type that create an object (gull update, self use).
        /// </summary>
        CreateSelf = 3,
        /// <summary>
        /// Update type that update only objects out of range.
        /// </summary>
        OutOfRange = 4,
        /// <summary>
        /// Update type that update only near objects.
        /// </summary>
        NearObjects = 5,
    }

    [Flags]
    public enum ObjectUpdateFlags
    {
        UPDATEFLAG_NONE = 0x0000,
        UPDATEFLAG_SELF = 0x0001,
        UPDATEFLAG_TRANSPORT = 0x0002,
        UPDATEFLAG_FULLGUID = 0x0004,
        UPDATEFLAG_HIGHGUID = 0x0008,
        UPDATEFLAG_ALL = 0x0010,
        UPDATEFLAG_LIVING = 0x0020,
        UPDATEFLAG_HAS_POSITION = 0x0040
    }

    public struct Entry
    {
        public UInt32 Type;
        public UInt32 DisplayID;
        public UInt32 entry;
        public string name;
        public byte[] blarg;
        public string subname;
        public UInt32 flags;
        public UInt32 subtype;
        public UInt32 family;
        public UInt32 rank;
    }

    public enum ObjectType
    {
        Object = 0,
        Item = 1,
        Container = 2,
        Unit = 3,
        Player = 4,
        GameObject = 5,
        DynamicObject = 6,
        Corpse = 7,
        AIGroup = 8,
        AreaTrigger = 9,
        Count,
        None = 0xFF
    }

    public enum GameObjectType
    {
        Door,
        Button,
        QuestGiver,
        Chest,
        Binder,
        Generic,
        Trap,
        Chair,
        SpellFocus,
        Text,
        Goober,
        Transport,
        AreaDamage,
        Camera,
        MapObject,
        MOTransport,
        DuelFlag,
        FishingNode,
        SummoningRitual,
        Mailbox,
        AuctionHouse,
        GuardPost,
        SpellCaster,
        MeetingStone,
        FlagStand,
        FishingHole,
        FlagDrop,
        MiniGame,
        CapturePoint = 29,
        AuraGenerator = 30
    }

    public enum GameObjectFlags
    {
        GO_FLAG_IN_USE = 0x00000001,                   // disables interaction while animated
        GO_FLAG_LOCKED = 0x00000002,                   // require key, spell, event, etc to be opened. Makes "Locked" appear in tooltip
        GO_FLAG_INTERACT_COND = 0x00000004,                   // cannot interact (condition to interact)
        GO_FLAG_TRANSPORT = 0x00000008,                   // any kind of transport? Object can transport (elevator, boat, car)
        GO_FLAG_NO_INTERACT = 0x00000010,                   // players cannot interact with this go (often need to remove flag in event)
        GO_FLAG_NODESPAWN = 0x00000020,                   // never despawn, typically for doors, they just change state
        GO_FLAG_TRIGGERED = 0x00000040                    // typically, summoned objects. Triggered by spell or other events
    }

    public enum GameObjectDynamicLowFlags
    {
        GO_DYNFLAG_LO_ACTIVATE = 0x01,                 // enables interaction with GO
        GO_DYNFLAG_LO_ANIMATE = 0x02,                 // possibly more distinct animation of GO
        GO_DYNFLAG_LO_NO_INTERACT = 0x04,                 // appears to disable interaction (not fully verified)
        GO_DYNFLAG_LO_SPARKLE = 0x08,                 // makes GO sparkle TODO is it valid??
    }
}
