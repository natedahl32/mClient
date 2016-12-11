using mClient.Constants;

namespace mClient.DBC
{
    public class MapEntry
    {
        #region Properties

        public uint Id { get; set; }

        public MapTypes MapType { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Ref to AreaTable.dbc
        /// </summary>
        public uint LinkedZone { get; set; }

        public bool IsDungeon
        {
            get { return MapType == MapTypes.MAP_INSTANCE || MapType == MapTypes.MAP_RAID; }
        }

        public bool IsNonRaidDungeon
        {
            get { return MapType == MapTypes.MAP_INSTANCE; }
        }

        public bool IsRaid
        {
            get { return MapType == MapTypes.MAP_RAID; }
        }

        public bool IsBattleground
        {
            get { return MapType == MapTypes.MAP_BATTLEGROUND; }
        }

        public bool IsMountAllowed
        {
            get { return !IsDungeon || Id == 309 || Id == 209 || Id == 509 || Id == 269; }
        }

        public bool IsContinent
        {
            get { return Id == 0 || Id == 1; }
        }

        #endregion
    }
}
