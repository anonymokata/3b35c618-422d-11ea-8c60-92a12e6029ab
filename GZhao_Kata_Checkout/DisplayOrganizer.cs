using System;
using System.Collections.Generic;
using System.Text;

namespace GZhao_Kata_Checkout
{
    class DisplayOrganizer
    {
        /// <summary>
        /// Prepare a string for display given the information.
        /// </summary>
        /// <param name="entry">The string we're preparing.</param>
        /// <param name="padding">The total 'box' size of the display.</param>
        /// <param name="isMoney">If true, display as money.</param>
        /// <returns></returns>
        public static string AddEntry(string entry, int padding)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(entry);
            int i = padding - entry.Length;
            while (i > 0)
            {
                builder.Append(" ");
                i--;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Add some brackets.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string AddBrackets(int number)
        {
            return "[" + number + "]. ";
        }

        /// <summary>
        /// Add a discount to the display.
        /// </summary>
        /// <param name="special"></param>
        /// <returns></returns>
        public static string AddSpecial(Special special, int padding)
        {
            StringBuilder builder = new StringBuilder();
            int i = padding;
            while (i > 0)
            {
                builder.Append(" ");
                i--;
            }
            switch (special.discount_type)
            {
                case Special.DISCOUNT_TYPE.REDUCE_BY_DOLLAR:
                    builder.Append("$"+special.itemCostChange+" off.");
                    break;
                case Special.DISCOUNT_TYPE.REDUCE_BY_PERCENTAGE:
                    builder.Append(special.itemCostChange + "% off.");
                    break;
                case Special.DISCOUNT_TYPE.SET_TO_AMOUNT:
                    builder.Append(special.itemsApplied + " for " + "$" + special.itemCostChange);
                    break;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Returns a marker if the given boolean happens to be true.
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string AddMark(bool flag)
        {
            string marker = " ";

            if (flag)
            {
                marker = "*";
            }

            return marker;
        }
    }
}
