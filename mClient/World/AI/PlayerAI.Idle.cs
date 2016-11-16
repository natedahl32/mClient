using FluentBehaviourTree;

namespace mClient.World.AI
{
    public partial class PlayerAI
    {
        public IBehaviourTreeNode CreateIdleAITree()
        {
            // TODO: Need to code in a wait for rez if we are in combat
            var builder = new BehaviourTreeBuilder();
            return builder
                .Selector("idle-selector")
                    .Do("Has Current Activity?", t =>            // idel logic doesn't run if we have an activity we are currently doing
                    {
                        if (HasCurrentActivity)
                            return BehaviourTreeStatus.Success;
                        return BehaviourTreeStatus.Failure;
                    })
                    .Do("Buff Group", t => BuffUp())
                    .Do("Should I stay?", t => Stay())
                    .Do("Should I follow?", t => Follow())
                    .Do("Do I have a move command?", t => NoMoveCommand())
                 .End()
                 .Build();
        }

        /// <summary>
        /// Buffs either self or group
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus BuffUp()
        {
            // If we don't even have any buffs than nothing to worry about!
            if (!Player.ClassLogic.HasOOCBuffs)
                return BehaviourTreeStatus.Failure;

            // Get any members of the party that need buffs
            var playersNeedingBuffs = Player.ClassLogic.GroupMembersNeedingOOCBuffs;
            if (playersNeedingBuffs.Count == 0)
                return BehaviourTreeStatus.Failure;

            // TODO: Push the first buff in the list to an activity that will perform the buff

            // We have some buffs to give out
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus Stay()
        {
            if (Player.MoveCommand == Constants.MoveCommands.Stay)
            {
                Player.PlayerAI.ClearFollowTarget();
                return BehaviourTreeStatus.Success;
            }

            // Not staying
            return BehaviourTreeStatus.Failure;
        }

        private BehaviourTreeStatus Follow()
        {
            if (Player.MoveCommand == Constants.MoveCommands.Follow)
            {
                // If we don't have a player that issued the command we can't follow them
                if (Player.IssuedMoveCommand == null)
                    return BehaviourTreeStatus.Failure;

                // Follow the target that issued the move command
                SetFollowTarget(Player.IssuedMoveCommand);
                return BehaviourTreeStatus.Success;
            }

            return BehaviourTreeStatus.Failure;
        }

        private BehaviourTreeStatus NoMoveCommand()
        {
            // We just stay if we don't have a move command
            if (Player.MoveCommand == Constants.MoveCommands.None)
            {
                Player.PlayerAI.ClearFollowTarget();
                return BehaviourTreeStatus.Success;
            }

            // Not staying
            return BehaviourTreeStatus.Failure;
        }
    }
}
