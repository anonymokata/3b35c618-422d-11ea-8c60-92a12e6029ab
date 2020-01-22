using System;
using System.Collections.Generic;
using System.Text;

namespace Kata_Checkout
{
    /// <summary>
    /// This class lets us interface with the 'database'.
    /// </summary>
    public class Database_API
    {

        /// <summary>
        /// Returns true if there is already an item in the database
        /// with the same name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool TryGetItem(string name)
        {
            bool result = false;

            int i = 0;
            while (!result && i < Database.GetItemCount())
            {
                result = Database.GetItemAt(i).Match(name);
                i++;
            }

            return result;
        }

        /// <summary>
        /// Adds an item to the list of items.
        /// </summary>
        /// <param name="name">Name of the item.</param>
        /// <param name="cost">Cost of the item.</param>
        /// <param name="sellByWeight">If true, the item price is dependent on its weight.</param>
        public static void AddItem(string name, float cost, bool sellByWeight=false)
        {
            Item newItem = new Item(name, cost, sellByWeight);
            Database.AddItem(newItem);
        }

        /// <summary>
        /// Removes all item entries of the same name from the list of items.
        /// </summary>
        /// <param name="name"></param>
        public static void Remove(string name)
        {
            Database.RemoveItem(name);
        }

        /// <summary>
        /// Gets the item that matches the given search parameter
        /// (in our case, the name of the item).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Item GetItem(string name)
        {
            return Database.GetItem(name);
        }

        /// <summary>
        /// Gets the item that exists at the given pointer location in the database.
        /// </summary>
        /// <param name="pointer"></param>
        /// <returns></returns>
        public static Item GetItem(int pointer)
        {
            return Database.GetItemAt(pointer);
        }

        /// <summary>
        /// Returns the price of an item given its name.
        /// Will return 0 if the name is not in the list.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static float GetCost(string name)
        {
            return Database.GetCost(name);
        }

        /// <summary>
        /// Gets the count of how many items are in the database.
        /// </summary>
        /// <returns></returns>
        public static int GetItemCount()
        {
            return Database.GetItemCount();
        }

        /// <summary>
        /// Gets the count of how many specials are in the database.
        /// </summary>
        /// <returns></returns>
        public static int GetSpecialsCount()
        {
            return Database.GetSpecialCount();
        }

        /// <summary>
        /// Purges the item list. Use for testing only.
        /// </summary>
        public static void Clean()
        {
            Database.PurgeItems();
        }

        /// <summary>
        /// Adds a special to the database.
        /// </summary>
        /// <param name="s"></param>
        public static void AddSpecial(Special s)
        {
            Database.AddSpecial(s);
        }

        /// <summary>
        /// Gets the special that affects the item with the given name.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Special GetSpecial(string v)
        {
            Special ret = new Special();
            bool matchFound = false;
            int i = 0;
            while(!matchFound && i < Database.GetSpecialCount())
            {
                matchFound = Database.GetSpecialAtPosition(i).Match(v);
                if (matchFound)
                {
                    ret = Database.GetSpecialAtPosition(i);
                }
                i++;
            }

            return ret;
        }

        /// <summary>
        /// Gets the special that has the given ID.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static Special GetSpecial(int i)
        {
            return Database.GetSpecialAt(i);
        }

        /// <summary>
        /// remove all specials that affect the given item from the database.
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveSpecial(string name)
        {
            Database.RemoveSpecial(name);
        }

        /// <summary>
        /// Returns true if there is a deal that can be applied with the given
        /// purchase and the amount of items within said purchase.
        /// </summary>
        /// <param name="talliedItems"></param>
        /// <returns></returns>
        internal static bool TryGetMatchingDeal(string name, int amount)
        {
            bool hasDeal = false;
            int i = Database.GetSpecialCount();

            for(int j = 0; j < i; j++)
            {
                Special item = Database.GetSpecialAtPosition(j);
                if(item.Match(name) && item.itemsNeededToFire <= amount || amount == -1)
                {
                    hasDeal = true;
                    break;
                }
            }

            return hasDeal;
        }
        
        /// <summary>
        /// Returns a deal that can be applied to the given purchase.
        /// Takes the name of the item for matching and the amount of purchases
        /// for checking activation.
        /// </summary>
        /// <param name="name">The name of the item that's looking for a special.</param>
        /// <param name="amount">The amount of the item that have been purchased so far.</param>
        /// <returns></returns>
        internal static Special GetMatchingDeal(string name, int amount)
        {
            Special special = new Special();
            int i = Database.GetSpecialCount();
            for(int j = 0; j < GetSpecialsCount(); j++)
            {
                Special item = Database.GetSpecialAtPosition(j);
                if(item.Match(name) && item.itemsNeededToFire <= amount)
                {
                    special = item;
                    break;
                }
            }

            return special;
        }

        
    }
}
