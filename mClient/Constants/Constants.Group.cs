namespace mClient.Constants
{
    public enum LootMethod : byte
    {
        FREE_FOR_ALL = 0,
        ROUND_ROBIN = 1,
        MASTER_LOOT = 2,
        GROUP_LOOT = 3,
        NEED_BEFORE_GREED = 4,

        NOT_GROUP_TYPE_LOOT = 5                                 // internal use only
    }
}
