using System;
using System.Collections.Generic;
using System.Text;

namespace Kata_Checkout
{
    public class SpecialNormal : Special
    {
        /// <summary>
        /// Creates a special with the given parameters.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="type">The type of the discount (flat, percentage, or dollar amount)</param>
        /// <param name="discount">the value of the special</param>
        /// <param name="applyCount">How many items are needed to fire this special</param>
        /// <param name="applyTo">How many items this special can be applied to</param>
        /// <param name="applyLimit">How many times this special can fire. Defaults to infinite.</param>
        public SpecialNormal(string name,int discount, DISCOUNT_TYPE type, int applyCount, int applyTo, int applyLimit = -1)
        {
            itemAffected = name;
            discount_type = type;
            itemCostChange = discount;
            itemsNeededToFire = applyCount;
            itemsApplied = applyTo;
            specialApplyLimit = applyLimit;

            if(discount_type == DISCOUNT_TYPE.SET_TO_AMOUNT)
            {
                CalculateFlatPrice();
            }
        }
    }
}
