using System;
using System.Collections.Generic;
using System.Text;

namespace Kata_Checkout
{
    public class Item
    {
        /// <summary>
        /// Name of the item.
        /// </summary>
        public string name { get; private set; }

        /// <summary>
        /// Full price of the item.
        /// </summary>
        public float price { get; private set; }
        
        /// <summary>
        /// When true, the item's price is determined by it's weight.
        /// The price of the item is referenced to one pound.
        /// </summary>
        public bool priceByWeight { get; private set; }
        
        /// <summary>
        /// Create a new item.
        /// </summary>
        /// <param name="newName">Name of the item.</param>
        /// <param name="newPrice">Price of the item.</param>
        /// <param name="notSingleType">If true, the item is sold as single items. when false, item price is per pound. Defaults to false.</param>
        public Item(string newName, float newPrice, bool sellByWeight=false)
        {
            name = newName.Trim();
            price = newPrice;
            priceByWeight = sellByWeight;
        }

        /// <summary>
        /// Returns true if this item has the same name as the given string.
        /// Strings are sanitized for leading/trailing spaces and are case insensitive.
        /// </summary>
        /// <param name="matchTarget"></param>
        /// <returns></returns>
        public bool Match(string matchTarget)
        {
            string mt = matchTarget.ToLower().Trim();
            return mt.Equals(name.ToLower());
        }
    }
}
