namespace mClient.World.Guild
{
    public class GuildInfo
    {
        #region Constructors

        public GuildInfo(uint id, string name)
        {
            GuildId = id;
            GuildName = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ID of the guild
        /// </summary>
        public uint GuildId { get; private set; }

        /// <summary>
        /// Gets the name of the guild
        /// </summary>
        public string GuildName { get; private set; }

        #endregion
    }
}
