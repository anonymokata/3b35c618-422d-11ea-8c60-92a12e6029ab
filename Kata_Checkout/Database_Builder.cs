using System;
using System.Collections.Generic;
using System.Text;

namespace Kata_Checkout
{
    public class Database_Builder
    {
        private static bool builtItems = false;
        private static bool builtSpecials = false;

        public static void BuildData()
        {
            if (!builtItems)
            {
                //Items that sell by weight
                Database_API.AddItem("Beef", 10, true);
                Database_API.AddItem("Chicken", 10, true);
                Database_API.AddItem("Peas", 10, true);

                //Items that sell by unit
                Database_API.AddItem("Soup", 5);
                Database_API.AddItem("Pencils", 6);
                Database_API.AddItem("Carpet", 100);
                Database_API.AddItem("Candy", 4);

                //Items that sell by weight but we actually care about it.
                Database_API.AddItem("Flour", 6, true);

                builtItems = true;
            }

            if (!builtSpecials)
            {
                Database_API.AddSpecial(new SpecialNormal("Soup", 3, Special.DISCOUNT_TYPE.REDUCE_BY_DOLLAR, 3, 3));
                Database_API.AddSpecial(new SpecialNormal("Candy", 2, Special.DISCOUNT_TYPE.REDUCE_BY_DOLLAR, 2, 2));
                Database_API.AddSpecial(new SpecialNormal("Chicken", 50, Special.DISCOUNT_TYPE.REDUCE_BY_PERCENTAGE, 2, 1));

                //Bundling Deals
                Database_API.AddSpecial(new SpecialNormal("Flour", 6, Special.DISCOUNT_TYPE.SET_TO_AMOUNT, 3, 3));

                //Deals with a limit
                Database_API.AddSpecial(new SpecialNormal("Carpet", 60, Special.DISCOUNT_TYPE.REDUCE_BY_DOLLAR, 1, 1, 1));

                //Deals that affect the next item rather than the first one.
                Database_API.AddSpecial(new SpecialDeferred("Beef", 50, Special.DISCOUNT_TYPE.REDUCE_BY_PERCENTAGE, 1));

                builtSpecials = true;
            }
        }

        public static void Unbuilt()
        {
            builtSpecials = false;
            builtItems = false;
        }
    }
}
