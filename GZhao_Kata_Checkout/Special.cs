using System;
using System.Collections.Generic;
using System.Text;

namespace GZhao_Kata_Checkout
{
    /// <summary>
    /// A special is a deal that exists for a certain amount of items.
    /// Note: Do note try to create this class.
    /// </summary>
    public class Special
    {
        /// <summary>
        /// The name of the item affected.
        /// </summary>
        public string itemAffected { get; protected set; }
        
        /// <summary>
        /// How does this special work with the item's cost.
        /// SET_TO_AMOUNT = Sets the affected items to have the given cost in total.
        /// REDUCE_BY_PERCENTAGE = Reduces the affected items' cost by the given percentage.
        /// REDUCE_BY_DOLLAR = Reduces the affected items' costs by the given flat dollar value.
        /// </summary>
        public enum DISCOUNT_TYPE : int {SET_TO_AMOUNT = 0, REDUCE_BY_PERCENTAGE = 1, REDUCE_BY_DOLLAR = 2}
        
        /// <summary>
        /// The category of this special.
        /// NORMAL = affects all items.
        /// DEFERRED = does not affect the triggering item.
        /// </summary>
        public enum SPECIAL_TT : int { NORMAL = 0, DEFERRED = 1}

        /// <summary>
        /// The amount that this item is either:
        /// SET_TO_AMOUNT = sets the value of all items affected by the special so it equals this.
        /// REDUCE_BY_PERCENTAGE = reduce each item in the special by this percentage (20 = 20% off).
        /// REDUCE_BY_DOLLAR = reduce each item in the special by this flat value (20 = 20 dollars from its current cost).
        /// </summary>
        public float itemCostChange { get; protected set; }

        /// <summary>
        /// Used only for set_to_amount deals. to prevent
        /// weird calculation shenanigans from being revealed.
        /// </summary>
        public float preSetAmount { get; private set; }
        
        /// <summary>
        /// The amount of items that need to be purchased before this special fires.
        /// </summary>
        public int itemsNeededToFire { get; protected set; }

        /// <summary>
        /// The amount of items that this special applies to.
        /// </summary>
        public int itemsApplied { get; protected set; }

        /// <summary>
        /// How many items this special can be applied per purchase.
        /// Defaults to infinite.
        /// </summary>
        public int specialApplyLimit { get; protected set; }

        /// <summary>
        /// The type of this special (normal or deferred).
        /// </summary>
        public SPECIAL_TT special_type { get; protected set; }

        /// <summary>
        /// How does this special calculate changes to cost?
        /// flat or percentage or bundled.
        /// </summary>
        public DISCOUNT_TYPE discount_type { get; protected set; }

        /// <summary>
        /// Do not do this.
        /// </summary>
        public Special()
        {
        }
        
        /// <summary>
        /// Returns true if this Special's affected item is the same as the string being given.
        /// Case insensitive and culled for leading/trailing spaces.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Match(string name)
        {
            string temp = name.ToLower().Trim();
            return temp.Equals(itemAffected.ToLower());
        }

        /// <summary>
        /// Fires when the special is of type FLAT.
        /// Changes the total so that the total of all values added together becomes equal
        /// to the given total.
        /// </summary>
        protected void CalculateFlatPrice()
        {
            preSetAmount = itemCostChange;
            itemCostChange /= itemsApplied;
        }
    }
}
