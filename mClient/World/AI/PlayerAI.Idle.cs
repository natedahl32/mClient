using FluentBehaviourTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    .Do("Should I stay?", t => Stay())
                    .Do("Should I follow?", t => Follow())
                    .Do("Do I have a move command?", t => NoMoveCommand())
                 .End()
                 .Build();
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
