using System;
using System.Collections.Generic;
using System.Text;

namespace GZhao_Kata_Checkout
{
    class DisplayObjectReceipt
    {

        private List<string> listForDisplay;

        public DisplayObjectReceipt()
        {
            listForDisplay = new List<string>();
        }
        
        /// <summary>
        /// Add an object into the display.
        /// </summary>
        /// <param name="item">The name of the item being added.</param>
        /// <param name="isSpecial">Set to true for Specials.</param>
        public void Add(string item, bool isSpecial = false)
        {
            string marker = "i";
            if (isSpecial)
            {
                marker = "s";
            }

            listForDisplay.Add(marker + item);
        }

        /// <summary>
        /// Remove an object from the given position.
        /// </summary>
        /// <param name="position">the position of the item being removed.</param>
        public void Remove(int position)
        {
            listForDisplay.RemoveAt(position);
        }
    }
}
