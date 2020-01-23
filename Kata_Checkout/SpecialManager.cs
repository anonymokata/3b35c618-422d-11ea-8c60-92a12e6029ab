using System;
using System.Collections.Generic;
using System.Text;

namespace Kata_Checkout
{
    public class SpecialManager
    {
        /// <summary>
        /// Applies the specials in the list of tokens into the list of items.
        /// Specials are applied top-down the list.
        /// </summary>
        /// <param name="itemRosterAsPurchased">The list of purchased items (in entries).</param>
        /// <param name="listOfApplyingSpecials">The list of applying specials.</param>
        internal static void ApplySpecials(List<ItemInCart> itemRosterAsPurchased, SpecialTokenManager stm)
        {
            int i = 0;

            while (i < stm.GetTokenListSize())
            {
                SpecialToken token = stm.GetToken(i);
                //with this token, we will: apply bonuses to items top-down.

                //The items that this special affects. ALL OF IT.
                Queue<ItemInCart> appliedRoster = new Queue<ItemInCart>();
                
                foreach(ItemInCart item in itemRosterAsPurchased)
                {
                    if (token.CanBeAppliedTo(item))
                    {
                        appliedRoster.Enqueue(item);
                    }
                }
                //The list now only has items that qualify for the special.
                
                //Sorted by highest price to the lowest.
                Queue<ItemInCart> sortedRoster = SortByPrice(appliedRoster);

                bool header = true;
                int fireCount = token.GetAffectedCount();
                while (sortedRoster.Count > 0 && fireCount > 0)
                {
                    ItemInCart sri = sortedRoster.Dequeue();
                    if (token.special.special_type == Special.SPECIAL_TT.DEFERRED && header)
                    {
                        //if it's deferred, the first item that qualifies for the special would be
                        //the most expensive, and thus the qualifing item that doesn't get
                        //the deal. Ergo, it's the header.
                        sri.SetDeferHeader();
                        header = false;

                        //boo boo bandaid.
                        //to stop the header from eating a fire count.
                        fireCount++;
                    }
                    else if (!sri.isDiscounted)
                    {
                        sri.SetSpecialValue(token.special);
                    }
                    fireCount--;
                }
                i++;
            }
        }

        /// <summary>
        /// Sorts the incoming queue and returns it with the most expensive item on top.
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        private static Queue<ItemInCart> SortByPrice(Queue<ItemInCart> queue)
        {
            Queue<ItemInCart> result = new Queue<ItemInCart>();
            List<ItemInCart> list = new List<ItemInCart>();

            ItemInCart hold = null;
            while(queue.Count > 0)
            {
                hold = queue.Dequeue();

                int i = 0;
                
                while(i < list.Count)
                {
                    ItemInCart item = list[i];
                    if(hold.GetOriginalPrice() > item.GetOriginalPrice())
                    {
                        list[i] = hold;
                        hold = item;
                    }

                    i++;
                }

                //At the end, hold has to be added to the list, so...
                list.Add(hold);
            }

            int j = 0;
            while(j < list.Count)
            {
                result.Enqueue(list[j]);
                j++;
            }

            return result;
        }
    }
}
