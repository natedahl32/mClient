using FluentBehaviourTree;
using mClient.Clients;
using mClient.World.AI.Activity.Combat;
using mClient.World.AI.Activity.Item;
using mClient.World.AI.Activity.Movement;
using mClient.World.AI.Activity.Train;
using System.Linq;

namespace mClient.World.AI
{
    public partial class PlayerAI
    {
        private const float MAX_TRAINER_DISTANCE = 30.0f;

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
                    .Do("Check Group Status", t => CheckGroupStatus())
                    .Sequence("Train New Skills")
                        .Do("Has Skills to Train", t => HasSkillsToTrain())
                        .Do("Find Class Trainer", t => FindClassTrainer())
                    .End()
                    .Sequence("Repair Items")
                        .Do("Need to Repair?", t => NeedToRepair())
                        .Do("Find Repair Man", t => FindRepairMan())
                    .End()
                    .Do("Apply Talent Points", t => ApplyFreeTalentPoints())
                    .Do("Buff Group", t => BuffUp())
                    .Do("Should I stay?", t => Stay())
                    .Do("Should I follow?", t => Follow())
                    .Do("Do I have a move command?", t => NoMoveCommand())
                 .End()
                 .Build();
        }

        /// <summary>
        /// Checks status of the group
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus CheckGroupStatus()
        {
            // If we are not in a group failure
            if (Player.CurrentGroup == null)
                return BehaviourTreeStatus.Failure;

            // We are in a group, if everyone in our group is offline than leave the group
            foreach (var groupMember in Player.CurrentGroup.PlayersInGroup)
                if (groupMember.Guid.GetOldGuid() != Player.Guid.GetOldGuid() && groupMember.GroupData.OnlineState == 1)
                    return BehaviourTreeStatus.Failure;

            // All members in our group are offline, so leave the group
            Client.DisbandFromGroup();
            Player.RemoveFromGroup();
            return BehaviourTreeStatus.Success;
        }

        /// <summary>
        /// Applies free talent points using our current spec
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus ApplyFreeTalentPoints()
        {
            // Check if we have free talent points available
            if (Player.PlayerObject.FreeTalentPoints == 0)
                return BehaviourTreeStatus.Failure;

            // Check if we have a spec applied to us
            if (Player.TalentSpec == Constants.MainSpec.NONE)
                return BehaviourTreeStatus.Failure;

            // Apply talent points for our current spec
            StartActivity(new TrainFreeTalentPointsForSpec(this));
            return BehaviourTreeStatus.Success;
        }

        /// <summary>
        /// Determines if we need to repair or not
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus NeedToRepair()
        {
            // Need to repair if total item durability is less than 75%
            if (Player.PlayerObject.EquipmentDurabilityPercentage < 0.75f)
                return BehaviourTreeStatus.Success;

            // Need to repair if any one piece is below 50% durability
            foreach (var item in Player.PlayerObject.EquippedItems)
                if (item.MaxDurability > 0 && (item.Durability / item.MaxDurability) < 0.5f)
                    return BehaviourTreeStatus.Success;

            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Finds a repair man to repair our gear
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus FindRepairMan()
        {
            // Find a repair man that is in range of us
            var closestDistance = 1000000.0f;
            mClient.Clients.Unit chosenUnit = null;
            foreach (var u in Client.objectMgr.GetAllUnits())
            {
                // If the unit is not a trainer
                if (!u.IsRepair) continue;

                // Check the distance on them
                var distance = Client.movementMgr.CalculateDistance(u.Position);
                if (distance < closestDistance && distance < MAX_TRAINER_DISTANCE)
                {
                    closestDistance = distance;
                    chosenUnit = u;
                }
            }

            // If we found one return success
            if (chosenUnit != null)
            {
                StartActivity(new RepairAllItems(chosenUnit, this));
                return BehaviourTreeStatus.Success;
            }

            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Determines if we have have skills we need to train
        /// </summary>
        /// <returns></returns>
        private BehaviourTreeStatus HasSkillsToTrain()
        {
            // Check for spells we need to train and also that we can afford
            if (Player.AvailableSpellsToLearn.Count() > 0 && Player.AvailableSpellsToLearn.Any(s => s.MoneyCost == 0 || s.MoneyCost < Player.PlayerObject.Money))
                return BehaviourTreeStatus.Success;

            return BehaviourTreeStatus.Failure;
        }

        private BehaviourTreeStatus FindClassTrainer()
        {
            // Find a class trainer that is in range of us
            var closestDistance = 1000000.0f;
            mClient.Clients.Unit chosenUnit = null;
            foreach (var u in Client.objectMgr.GetAllUnits())
            {
                // If the unit is not a trainer
                if (!u.IsTrainer) continue;

                // If the trainer is not a class trainer and for our class
                var myTrainerSubName = Player.ClassLogic.ClassName + " Trainer";
                if (u.BaseCreatureInfo != null && u.BaseCreatureInfo.SubName != myTrainerSubName) continue;

                // Does the trainer have any spells that we need?
                if (u.TrainerSpellsAvailable != null && !Player.AvailableSpellsToLearn.Any(s => u.TrainerSpellsAvailable.Contains(s.SpellId)))
                    continue;

                // Right kind of class trainer, check the distance on them
                var distance = Client.movementMgr.CalculateDistance(u.Position);
                if (distance < closestDistance && distance < MAX_TRAINER_DISTANCE)
                {
                    closestDistance = distance;
                    chosenUnit = u;
                }
            }

            // If we found one return success
            if (chosenUnit != null)
            {
                StartActivity(new TrainAvailableSpells(chosenUnit, this));
                return BehaviourTreeStatus.Success;
            }
                
            return BehaviourTreeStatus.Failure;
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

            // Push the first buff in the list to an activity that will perform the buff
            var entry = playersNeedingBuffs.First();
            StartActivity(new OutOfCombatBuff(entry.Key, entry.Value, this));

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

                // We can't do anything without the position of the command issuer.
                if (Player.IssuedMoveCommand.Position == null)
                    return BehaviourTreeStatus.Failure;

                // If the player issuing the command is mounted we do NOT follow them. This is IMPORTANT as it is the only we can ascertain whether or not the person who issued the 
                // follow command is on a mount path. Until we find a different way this MUST be here or the bot will try to follow to their death!
                if (Player.IssuedMoveCommand.IsMounted)
                {
                    // Remove the follow target just to make sure
                    ClearFollowTarget();
                    return BehaviourTreeStatus.Failure;
                }

                // Must be in party (TODO: Or Raid)
                if (Player.CurrentGroup == null)
                    return BehaviourTreeStatus.Failure;
                var partyMember = Player.CurrentGroup.GetGroupMember(Player.IssuedMoveCommand.Guid.GetOldGuid());
                if (partyMember == null)
                    return BehaviourTreeStatus.Failure;

                // If we are not close to them, teleport to them. Null position from the issuer probably means we are too far away.
                if (Client.movementMgr.CalculateDistance(Player.IssuedMoveCommand.Position) >= MovementMgr.MAXIMUM_FOLLOW_DISTANCE || partyMember.MapID != Player.MapID)
                {
                    StartActivity(new TeleportToCoordinate(partyMember.MapID, Player.IssuedMoveCommand.Position, this));
                    return BehaviourTreeStatus.Success;
                }

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
