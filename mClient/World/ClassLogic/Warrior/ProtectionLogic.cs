
namespace mClient.World.ClassLogic.Warrior
{
    public class ProtectionLogic : WarriorLogic
    {
        #region Constructors

        public ProtectionLogic(Player player) : base(player)
        {

        }

        #endregion

        #region Private Methods

        protected override uint Stance()
        {
            return DEFENSIVE_STANCE;
        }

        #endregion
    }
}
