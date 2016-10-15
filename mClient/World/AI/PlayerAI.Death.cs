using FluentBehaviourTree;

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
                    .Do("Is Dead?", t =>
                    {
                        if (Player.PlayerObject.IsDead)
                            return BehaviourTreeStatus.Success;
                        return BehaviourTreeStatus.Failure;
                    })
                    .Do("Do I have a corpse?", t =>
                    {
                        if (Player.PlayerCorpse == null)
                        {
                            if (!IsTeleportingToCorpse)
                            {
                                Client.SendCorpseQuery();
                                IsTeleportingToCorpse = true;
                                return BehaviourTreeStatus.Running;
                            }
                            else
                                return BehaviourTreeStatus.Running;
                        }

                        // We have our corpse now
                        IsTeleportingToCorpse = false;
                        return BehaviourTreeStatus.Success;
                    })
                    .Do("Release spirit!!", t =>
                    {
                        Client.ReclaimCorpse();
                        Player.ResurrectFromDeath();
                        return BehaviourTreeStatus.Success;
                    })
                 .End()
                 .Build();
        }
    }
}
