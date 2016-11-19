using mClient.Clients;
using mClient.DBC;
using mClient.Terrain;
using mClient.World.AI.Activity.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.AI.Activity.Quest
{
    public class UseInventoryItemOnGO : BaseActivity
    {
        #region Declarations

        private uint mUseItemId;
        private Clients.Object mUseOnGameObject;
        private bool mIsDone = false;

        // Inventory slot containing the item to use
        private InventoryItemSlot mInvSlot;
        // Spell effect on the item, used to check range
        private SpellEntry mItemSpell;

        #endregion

        #region Constructors

        public UseInventoryItemOnGO(uint itemId, Clients.Object obj, PlayerAI ai) : base(ai)
        {
            if (itemId == 0) throw new ArgumentNullException("itemId");
            if (obj == null) throw new ArgumentNullException("obj");

            mUseItemId = itemId;
            mUseOnGameObject = obj;
        }

        #endregion

        #region Properties

        public override string ActivityName
        {
            get { return "Use Inventory Item on GO"; }
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            base.Start();

            // Get the item we want to use and make sure we have it in inventory
            mInvSlot = PlayerAI.Player.PlayerObject.GetInventoryItem(mUseItemId);
            if (mInvSlot == null)
            {
                mIsDone = true;
                return;
            }

            // Get the first spell found on the item
            var itemSpellEffect = mInvSlot.Item.BaseInfo.SpellEffects.Where(se => se.SpellId > 0).FirstOrDefault();
            if (itemSpellEffect != null)
                mItemSpell = SpellTable.Instance.getSpell(itemSpellEffect.SpellId);
        }

        public override void Process()
        {
            // Check if we are done
            if (mIsDone)
            {
                PlayerAI.CompleteActivity();
                return;
            }

            // If we have an item spell, check the range on it to make sure we are in range to use
            if (mItemSpell != null)
            {
                var range = SpellRangeTable.Instance.getByID(mItemSpell.RangeIndex);
                if (range != null && (range.MaximumRange > 0 || range.MinimumRange > 0))
                {
                    // If we are not in range, move towards our target
                    var distance = TerrainMgr.CalculateDistance(PlayerAI.Player.Position, mUseOnGameObject.Position);
                    if (range.MinimumRange > 0 && distance < range.MinimumRange)
                    {
                        // TODO: Move away from the object
                    }
                    if (range.MaximumRange > 0 && distance > range.MaximumRange)
                    {
                        PlayerAI.StartActivity(new MoveTowardsObject(mUseOnGameObject, range.MaximumRange, PlayerAI));
                        return;
                    }
                }
            }

            // Use the item on the target now and complete the activity
            PlayerAI.Client.UseItemInInventoryOnTarget((byte)mInvSlot.Bag, (byte)mInvSlot.Slot, mUseOnGameObject.Guid);
            PlayerAI.CompleteActivity();
        }

        #endregion
    }
}
