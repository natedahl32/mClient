﻿using FluentBehaviourTree;
using mClient.World.AI.Activity.Death;

namespace mClient.World.AI
{
    public partial class PlayerAI
    {
        /// <summary>
        /// Gets or sets whether or not we are teleporting to our corpse
        /// </summary>
        public bool IsTeleportingToCorpse { get; set; }

        protected IBehaviourTreeNode CreateDeathAITree()
        {
            // TODO: Need to code in a wait for rez if we are in combat
            var builder = new BehaviourTreeBuilder();
            return builder
                .Sequence("death-sequence")
                    .Do("Is Dead?", t =>IsDead())
                    .Do("Find Corpse", t => FindCorpse())
                    .Do("Release spirit!!", t => ReleaseSpirit())
                 .End()
                 .Build();
        }

        /// <summary>
        /// Determines whether or not we are dead
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus IsDead()
        {
            if (Player.PlayerObject.IsDead)
                return BehaviourTreeStatus.Success;
            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Finds the corpse 
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus FindCorpse()
        {
            if (Player.PlayerCorpse != null)
                return BehaviourTreeStatus.Success;

            // Start the find corpse activity to the queue
            Player.PlayerAI.StartActivity(new FindCorpse(this));
            return BehaviourTreeStatus.Running;
        }

        /// <summary>
        /// Releases spirit for the player
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus ReleaseSpirit()
        {
            // Start the release spirit activity
            Player.PlayerAI.StartActivity(new ReleaseSpirit(this));
            return BehaviourTreeStatus.Success;
        }
    }
}