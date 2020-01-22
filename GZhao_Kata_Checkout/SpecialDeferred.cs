using System;
using System.Collections.Generic;
using System.Text;

namespace Gzhao_checkout_total
{
    public class SpecialDeferred : Special
    {
        
        /// <summary>
        /// Creates a new special of the deferred type.
        /// note that they will only ever be toggled on a 1:1 basis.
        /// That is, one trigger item = one discounted item.
        /// </summary>
        /// <param name="name">The name of the affected item.</param>
        /// <param name="dt">The type of the discount (flat, by percentage, or by dollar amount)</param>
        /// <param name="discount">The value of the discount.</param>
        /// <param name="limit">How many times this discount can fire per person (default to infinite).</param>
        public SpecialDeferred(string name, int discount, DISCOUNT_TYPE dt, int limit = -1)
        {
            itemAffected = name;
            itemCostChange = discount;
            itemsNeededToFire = 1;
            itemsApplied = 1;
            specialApplyLimit = limit;

            special_type = SPECIAL_TT.DEFERRED;
            discount_type = dt;

            if (discount_type == DISCOUNT_TYPE.SET_TO_AMOUNT)
            {
                CalculateFlatPrice();
            }
        }

    }
}
