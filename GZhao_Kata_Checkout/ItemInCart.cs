using System;
using System.Collections.Generic;
using System.Text;

namespace GZhao_Kata_Checkout
{
    public class ItemInCart
    {
        /// <summary>
        /// The item that's being purchased.
        /// </summary>
        private string itemRef;

        /// <summary>
        /// the amount of this item that is being purchased.
        /// Really only useful for item by weight.
        /// </summary>
        public float quantity { get; private set; }

        /// <summary>
        /// If true, this item's price has been discounted.
        /// </summary>
        public bool isDiscounted { get; private set; }

        /// <summary>
        /// If true, this item is considered to be the header item
        /// of a deferred special. It gets no discounts, but causes other
        /// items in its class to get discounts.
        /// </summary>
        public bool isDeferHeader { get; private set; }

        /// <summary>
        /// The special that this item is affected by.
        /// </summary>
        public Special special_ID { get; private set; }

        public ItemInCart(Item item, float amt = 1)
        {
            itemRef = item.name;
            quantity = amt;
        }
        
        /// <summary>
        /// Changes this item's price based on the special as given.
        /// </summary>
        /// <param name="special"></param>
        public bool SetSpecialValue(Special special)
        {
            bool flagSet = false;
            if (!isDiscounted)
            {
                isDiscounted = true;
                special_ID = special;
                flagSet = true;
            }

            return flagSet;
        }

        /// <summary>
        /// Sets this item as a deferred special header.
        /// </summary>
        public void SetDeferHeader()
        {
            isDeferHeader = true;
        }

        /// <summary>
        /// Removes this item's special marker.
        /// </summary>
        public void ClearSpecial()
        {
            isDeferHeader = false;
            isDiscounted = false;
            special_ID = null;
        }

        /// <summary>
        /// Returns the name of the item being purchased.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return itemRef;
        }

        /// <summary>
        /// Returns true if the target string matches the name of the item referenced.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool Match(string target)
        {
            string nn = GetName().ToLower().Trim();

            return nn.Equals(target.ToLower().Trim());
        }

        /// <summary>
        /// Gets the price of the purchase.
        /// </summary>
        /// <returns></returns>
        public float GetPrice()
        {
            Item itemdb = Database_API.GetItem(itemRef);
            float total = itemdb.price;
            if (itemdb.priceByWeight)
            {
                total *= quantity;
            }

            if (isDiscounted && !isDeferHeader)
            {
                switch (special_ID.discount_type)
                {
                    case Special.DISCOUNT_TYPE.REDUCE_BY_DOLLAR:
                        total -= special_ID.itemCostChange;
                        break;
                    case Special.DISCOUNT_TYPE.REDUCE_BY_PERCENTAGE:
                        total *= (100 - special_ID.itemCostChange) * 0.01f;
                        break;
                    case Special.DISCOUNT_TYPE.SET_TO_AMOUNT:
                        total = special_ID.itemCostChange;
                        break;
                }
            }
            return total;
        }

        public float GetOriginalPrice()
        {
            Item itemdb = Database_API.GetItem(itemRef);
            float total = itemdb.price;
            if (itemdb.priceByWeight)
            {
                total *= quantity;
            }

            return total;
        }
    }
}
