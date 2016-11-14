using mClient.Constants;
using System;
using System.Collections.Generic;

namespace mClient.World.GameObject
{
    public class GameObjectInfo
    {
        #region Declarations

        public const int MAX_GAMEOBJECT_DATA_COUNT = 24;

        #endregion

        #region Constructors

        // Required for serialization
        public GameObjectInfo() { }

        protected GameObjectInfo(uint id, GameObjectType type, string name, int[] data)
        {
            GameObjectId = id;
            GameObjectType = type;
            Name = name;
            Data = data;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the id of the game object
        /// </summary>
        public UInt32 GameObjectId { get; set; }


        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public GameObjectType GameObjectType { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the game object data
        /// </summary>
        public int[] Data { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a game object of a specific type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static GameObjectInfo Create(uint id, GameObjectType type, string name, int[] data)
        {
            switch (type)
            {
                case GameObjectType.Door:
                    return new Door(id, type, name, data);
                case GameObjectType.Button:
                    return new Button(id, type, name, data);
                case GameObjectType.QuestGiver:
                    return new Questgiver(id, type, name, data);
                case GameObjectType.Chest:
                    return new Chest(id, type, name, data);
                case GameObjectType.Binder:
                    return new Binder(id, type, name, data);
                case GameObjectType.Generic:
                    return new Generic(id, type, name, data);
                case GameObjectType.Trap:
                    return new Trap(id, type, name, data);
                case GameObjectType.Chair:
                    return new Chair(id, type, name, data);
                case GameObjectType.SpellFocus:
                    return new SpellFocus(id, type, name, data);
                case GameObjectType.Text:
                    return new Text(id, type, name, data);
                case GameObjectType.Goober:
                    return new Goober(id, type, name, data);
                case GameObjectType.Transport:
                    return new Transport(id, type, name, data);
                case GameObjectType.AreaDamage:
                    return new AreaDamage(id, type, name, data);
                case GameObjectType.Camera:
                    return new Camera(id, type, name, data);
                case GameObjectType.MapObject:
                    return new MapObject(id, type, name, data);
                case GameObjectType.MOTransport:
                    return new MOTransport(id, type, name, data);
                case GameObjectType.DuelFlag:
                    return new DuelFlag(id, type, name, data);
                case GameObjectType.FishingNode:
                    return new FishingNode(id, type, name, data);
                case GameObjectType.SummoningRitual:
                    return new SummoningRitual(id, type, name, data);
                case GameObjectType.Mailbox:
                    return new Mailbox(id, type, name, data);
                case GameObjectType.AuctionHouse:
                    return new AuctionHouse(id, type, name, data);
                case GameObjectType.GuardPost:
                    return new GuardPost(id, type, name, data);
                case GameObjectType.SpellCaster:
                    return new SpellCaster(id, type, name, data);
                case GameObjectType.MeetingStone:
                    return new MeetingStone(id, type, name, data);
                case GameObjectType.FlagStand:
                    return new FlagStand(id, type, name, data);
                case GameObjectType.FishingHole:
                    return new FishingHole(id, type, name, data);
                case GameObjectType.FlagDrop:
                    return new FlagDrop(id, type, name, data);
                case GameObjectType.MiniGame:
                    return new MiniGame(id, type, name, data);
                case GameObjectType.CapturePoint:
                    return new CapturePoint(id, type, name, data);
                case GameObjectType.AuraGenerator:
                    return new AuraGenerator(id, type, name, data);
                default:
                    break;
            }

            return null;
        }

        #endregion
    }
}
