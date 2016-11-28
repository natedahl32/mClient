using mClient.Shared;
using System;
using System.Linq;

namespace mClient.World.Guild
{
    public class GuildManager : AbstractObjectManager<GuildInfo>
    {
        #region Singleton

        static readonly GuildManager instance = new GuildManager();

        static GuildManager() { }

        GuildManager()
        { }

        public static GuildManager Instance { get { return instance; } }

        #endregion

        #region Properties

        protected override string SerializeToFile
        {
            get
            {
                return "guilds.json";
            }
        }

        #endregion

        #region Public Methods

        public override bool Exists(GuildInfo obj)
        {
            if (obj == null) return false;
            lock (mLock)
                return mObjects.Any(i => i.GuildId == obj.GuildId);
        }

        public bool Exists(UInt32 guildId)
        {
            lock (mLock)
                return mObjects.Any(i => i.GuildId == guildId);
        }

        public override GuildInfo Get(uint id)
        {
            lock (mLock)
                return mObjects.Where(i => i != null && i.GuildId == id).SingleOrDefault();
        }

        #endregion
    }
}
