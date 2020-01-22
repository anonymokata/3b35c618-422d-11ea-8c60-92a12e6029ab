using System.Collections.Generic;
using System.Text;

namespace Gzhao_checkout_total
{
    public class PurchaseItemManager
    {
        /// <summary>
        /// The receipt. Each item on the receipt represents one purchase of some kind.
        /// </summary>
        private List<ItemInCart> listOfItems;

        /// <summary>
        /// The specials that this purchase applies for.
        /// </summary>
        SpecialTokenManager stm;
        
        /// <summary>
        /// A tally of all the items we've purchased (by entry)
        /// </summary>
        private Dictionary<string, int> itemRosterTally;
        /// <summary>
        /// A tally of all the items we've purchased (by entry),
        /// but the tally reflects how many items are left that are not used
        /// to DETERMINE if a special is affecting or not.
        /// </summary>
        private Dictionary<string, int> itemRosterTallyUsed;

        /// <summary>
        /// Tally state trigger.
        /// Add = We're incrementing a special.
        /// Remove = We're decrementing a special.
        /// </summary>
        private enum TALLY_STATE: int {ADD = 0, REMOVE = 1}
        
        public PurchaseItemManager()
        {
            listOfItems = new List<ItemInCart>();
            stm = new SpecialTokenManager();

            itemRosterTally = new Dictionary<string, int>();
            itemRosterTallyUsed = new Dictionary<string, int>();
        }
        
        /// <summary>
        /// Adds an item into the cart of the given number (of items).
        /// </summary>
        /// <param name="itemName">The item being purchased.</param>
        /// <param name="itemNumber">How many is being purchased.</param>
        public void Add(string itemName, float itemNumber=1)
        {
            ItemInCart newItem = new ItemInCart(Database_API.GetItem(itemName), itemNumber);
            string itemNameClean = newItem.GetName();
            listOfItems.Add(newItem);

            if (itemRosterTally.ContainsKey(itemNameClean))
            {
                itemRosterTally[itemNameClean]++;
                itemRosterTallyUsed[itemNameClean]++;
            }
            else
            {
                itemRosterTally.Add(itemNameClean, 1);
                itemRosterTallyUsed.Add(itemNameClean, 1);
            }

            Consolidate(itemNameClean, TALLY_STATE.ADD);
        }

        /// <summary>
        /// Remove the last item in the item Roster.
        /// </summary>
        public void RemoveLast()
        {
            string name = listOfItems[listOfItems.Count - 1].GetName();
            listOfItems.RemoveAt(listOfItems.Count - 1);

            itemRosterTally[name]--;
            itemRosterTallyUsed[name]--;

            Consolidate(name, TALLY_STATE.REMOVE);
        }

        /// <summary>
        /// Removes the item at the given position from the item roster.
        /// </summary>
        /// <param name="position"></param>
        public void RemoveSpecific(int position)
        {
            ItemInCart iic = listOfItems[position];
            listOfItems.RemoveAt(position);

            itemRosterTally[iic.GetName()]--;
            itemRosterTallyUsed[iic.GetName()]--;

            Consolidate(iic.GetName(), TALLY_STATE.REMOVE);
        }

        /// <summary>
        /// Removes the most recent entry with the matching name from the item roster.
        /// </summary>
        /// <param name="itemName"></param>
        public void RemoveLast(string itemName)
        {
            int i = listOfItems.Count;
            string name = "";

            while(i > 0)
            {
                i--;

                bool match = listOfItems[i].Match(itemName);

                if(match)
                {
                    name = listOfItems[i].GetName();

                    listOfItems.RemoveAt(i);
                    
                    break;
                }
            }

            itemRosterTally[name]--;
            itemRosterTallyUsed[name]--;

            Consolidate(Database_API.GetItem(itemName).name, TALLY_STATE.REMOVE);
        }

        /// <summary>
        /// The amount of items (in entries) that have been purchased.
        /// </summary>
        /// <returns></returns>
        public int TotalPurchasedEntries()
        {
            return listOfItems.Count;
        }

        /// <summary>
        /// The total cost of this purchase.
        /// </summary>
        /// <returns></returns>
        public float TotalPurchase()
        {
            float total = 0;
            foreach(ItemInCart item in listOfItems)
            {
                total += item.GetPrice();
            }
            return total;
        }

        /// <summary>
        /// The total cost of this purchase without any specials attached.
        /// </summary>
        /// <returns></returns>
        public float TotalNoSpecialPurchase()
        {
            float t_total = 0;
            foreach(ItemInCart item in listOfItems)
            {
                t_total += item.GetOriginalPrice();
            }

            return t_total;
        }

        /// <summary>
        /// Take an item and check to see if the amount of purchases of its type
        /// allows for a special.
        /// </summary>
        private void Consolidate(string key, TALLY_STATE state)
        {
            //TRUE when a deal is either possible or is about to be impossible.
            bool hasSpecial = Database_API.TryGetMatchingDeal(key, itemRosterTallyUsed[key])
                || itemRosterTallyUsed[key] < 0;
            bool deferred = false;
            
            bool specialStateReset = false;
            if (hasSpecial)
            {
                Special special = Database_API.GetSpecial(key);
                if (itemRosterTallyUsed[key] >= special.itemsNeededToFire && state == TALLY_STATE.ADD)
                {
                    itemRosterTallyUsed[key] -= special.itemsNeededToFire;
                    stm.AddToken(special);
                    specialStateReset = true;
                }
                else if (itemRosterTallyUsed[key] < 0 && state == TALLY_STATE.REMOVE)
                {
                    itemRosterTallyUsed[key] += special.itemsNeededToFire;
                    stm.RemoveToken(special);
                    specialStateReset = true;
                }

                deferred = special.special_type == Special.SPECIAL_TT.DEFERRED;
            }

            //If we're removing, do a purge just in case.
            specialStateReset = state == TALLY_STATE.REMOVE;

            if (specialStateReset || deferred)
            {
                //Run if we're either removing, or else have a deferred special.
                PurgeSpecials(key);
            }
            ApplySpecials();
        }

        /// <summary>
        /// Get the name of a product at the current pointer.
        /// </summary>
        /// <param name="pointer"></param>
        /// <returns></returns>
        public ItemInCart GetAtPosition(int pointer)
        {
            return listOfItems[pointer];
        }

        /// <summary>
        /// Applies the specials that this purchase qualifies for to the items
        /// that this purchase has purchased.
        /// </summary>
        private void ApplySpecials()
        {
            SpecialManager.ApplySpecials(listOfItems, stm);
        }

        /// <summary>
        /// Clean the special tag from all currently purchased items.
        /// </summary>
        private void PurgeSpecials()
        {
            foreach(ItemInCart item in listOfItems)
            {
                item.ClearSpecial();
            }
        }

        /// <summary>
        /// Clean the special tags from the items of the given category.
        /// </summary>
        /// <param name="key"></param>
        private void PurgeSpecials(string key)
        {
            foreach(ItemInCart item in listOfItems)
            {
                if (item.Match(key))
                {
                    item.ClearSpecial();
                }
            }
        }

        /// <summary>
        /// Displays the receipt of this purchase.
        /// </summary>
        /// <param name=""></param>
        public string GetReceipt(bool showOrdering, bool showSpecials)
        {
            StringBuilder builder = new StringBuilder();
            int counter = 0;

            foreach(ItemInCart item in listOfItems)
            {
                if (showOrdering)
                {
                    counter++;
                    builder.Append(DisplayOrganizer.AddBrackets(counter));
                }
                //Displays the item and its price.
                builder.Append(DisplayOrganizer.AddEntry(item.GetName(), 30));
                builder.AppendLine(DisplayOrganizer.AddEntry(item.GetOriginalPrice().ToString(), 0));
                //Displays the special and its affects.
                if (showSpecials && item.isDiscounted)
                {
                    builder.AppendLine(DisplayOrganizer.AddSpecial(item.special_ID, 30));
                }
            }
            
            return builder.ToString();
        }
    }
}
