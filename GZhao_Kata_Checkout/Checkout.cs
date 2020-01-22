using System;
using System.Collections.Generic;
using System.Text;

namespace Gzhao_checkout_total
{
    public class Checkout
    {
        private enum CheckOutOrders : int {ADD = 0, REMOVE = 1, ADD_ITEM = 2, ADD_SPECIAL = 3, TOTAL = 4, RECEIPT = 5, HELP = 6};

        private static PurchaseItemManager pim;

        private const int SPACING_NAME = 30;
        private const int SPACING_VALUE = 10;

        private static bool done = false;

        public static void Main(String[] args)
        {
            Database_Builder.BuildData();

            pim = new PurchaseItemManager();
            
            WriteShopHeader();
            WriteProducts();
            WritePromptHelp();
            WritePrompt();

            while (!done)
            {
                WritePrompt();
            }
        }

        private static void WriteShopHeader()
        {
            Console.WriteLine("Hello and welcome to the ShopEasy Online Order Service!");
        }

        private static void WriteProducts()
        {
            Console.WriteLine("We have the following selection:");
            Console.WriteLine(WriteItems());
            Console.WriteLine("And the following specials:");
            Console.WriteLine(WriteSpecials());
        }

        private static void WritePromptHelp()
        {
            Console.WriteLine("We have the following selection:");
            Console.WriteLine(WriteItems());
            Console.WriteLine("And the following specials:");
            Console.WriteLine(WriteSpecials());

            Console.WriteLine("Command List (ignore the brackets: they're there to make things look neat.");
            Console.WriteLine("Note that all operation commands (not the print ones) have their own prompts.");
            Console.WriteLine("[a] add a purchase.");
            Console.WriteLine("[r] remove a purchase.");
            Console.WriteLine("[i] add an item (to the database).");
            Console.WriteLine("[s] add a special to the database.");
            Console.WriteLine("[h] print the store's goods/specials and this command list again.");
            Console.WriteLine("[t] print the total of our purchase.");
            Console.WriteLine("[x] print the receipt of our purchases.");
            Console.WriteLine("[q] quit this program.");
        }

        private static void WritePrompt()
        {
            Console.WriteLine("What is your command? (one char answers please)");
            string input = Console.ReadLine();

            char result = CleanInputForChar(input);

            int action = -1;

            switch (result)
            {
                case 'a': action = (int)CheckOutOrders.ADD; break;
                case 'r': action = (int)CheckOutOrders.REMOVE; break;
                case 'i': action = (int)CheckOutOrders.ADD_ITEM; break;
                case 's': action = (int)CheckOutOrders.ADD_SPECIAL; break;
                case 't': action = (int)CheckOutOrders.TOTAL; break;
                case 'x': action = (int)CheckOutOrders.RECEIPT; break;
                case 'h': action = (int)CheckOutOrders.HELP; break;
                case 'q': action = 9; break;
            }

            TakeAction(action);
        }

        private static void TakeAction(int action)
        {
            switch (action)
            {
                case (int)CheckOutOrders.ADD:
                    WriteAction_PlaceOrder();
                    break;
                case (int)CheckOutOrders.REMOVE:
                    WriteAction_RemoveOrder();
                    break;
                case (int)CheckOutOrders.ADD_ITEM:
                    WriteAction_Add_To_DB_Item();
                    break;
                case (int)CheckOutOrders.ADD_SPECIAL:
                    WriteAction_Add_To_DB_Special();
                    break;
                case (int)CheckOutOrders.TOTAL:
                    WriteAction_Total();
                    break;
                case (int)CheckOutOrders.RECEIPT:
                    WriteAction_Receipt();
                    break;
                case (int)CheckOutOrders.HELP:
                    WritePromptHelp();
                    break;
                case 9:
                    WriteExit();
                    break;
                default:
                    WriteConfused();
                    break;
            }
        }

        /// <summary>
        /// Buy an item.
        /// </summary>
        private static void WriteAction_PlaceOrder()
        {
            Console.Write("Enter the name of the item being purchased: ");
            string itemName = Console.ReadLine().Trim();
            

            bool hasItem = Database_API.TryGetItem(itemName);
            if (!hasItem)
            {
                Console.WriteLine("No such item exists in the database.");
            }
            else
            {
                if (Database_API.GetItem(itemName).priceByWeight)
                {
                    Console.Write("enter the weight of the purchase: ");
                    float quantity = float.Parse(Console.ReadLine());
                    pim.Add(itemName, quantity);
                }
                else
                {
                    Console.Write("enter how many items are being purchased. ");
                    int quantity = int.Parse(Console.ReadLine());

                    while(quantity > 0)
                    {
                        pim.Add(itemName);
                        quantity--;
                    }
                }
            }

            Console.WriteLine("Purchases added. Please review the receipt for specifics.");
            WriteAction_Total();
        }

        /// <summary>
        /// Remove an order from the receipt.
        /// </summary>
        private static void WriteAction_RemoveOrder()
        {
            int removalType = 0;
            Console.WriteLine("Select the removal mode: 1) By item, 2) By position. (1/2).");
            removalType = int.Parse(Console.ReadLine());
            
            if(removalType == 1)
            {
                Console.WriteLine("Note that this mode removes the most recent purchase of a given name.");
                Console.Write("Enter the name of the item you want removed: ");
                string removal = Console.ReadLine().Trim();

                pim.RemoveLast(removal);
            }
            else if (removalType == 2)
            {
                Console.WriteLine("The current list of purchases is as thus: ");
                Console.WriteLine(pim.GetReceipt(true, false));
                Console.WriteLine("Enter the number of the item you wish to delete: ");
                int remove = int.Parse(Console.ReadLine());

                pim.RemoveSpecific(remove+1);
            }

            if (removalType == 1 || removalType == 2)
            {
                Console.WriteLine("Removal complete.");
            }
            else
            {
                Console.WriteLine("Removal order failed.");
            }
        }

        /// <summary>
        /// Leave.
        /// </summary>
        private static void WriteExit()
        {
            done = true;
            Console.WriteLine("Done. Press any key to exit.");
            Console.Read();
        }

        private static void WriteConfused()
        {
            Console.WriteLine("Input unrecognized.");
        } 

        /// <summary>
        /// prints the receipt of the current purchase.
        /// </summary>
        private static void WriteAction_Receipt()
        {
            Console.WriteLine(pim.GetReceipt(false, true));
        }

        /// <summary>
        /// Write the total value of the purchase.
        /// </summary>
        private static void WriteAction_Total()
        {
            Console.Write("Subtotal (Without discounts): ");
            Console.WriteLine(pim.TotalNoSpecialPurchase());
            Console.Write("Subtotal (With discounts): ");
            Console.WriteLine(pim.TotalPurchase());
        }

        /// <summary>
        /// Write the prompt needed to add an item into the database.
        /// </summary>
        private static void WriteAction_Add_To_DB_Item()
        {
            Console.WriteLine("Note that if there is duplicates within the database, then then new entry" +
                "will replace the old one.");
            Console.Write("Enter the name of your item: ");
            string name = Console.ReadLine();
            Console.Write("Is the item priced by weight? (y/n, n default): ");
            string byWeight = Console.ReadLine();
            Console.Write("Enter the price of the item: ");
            string price = Console.ReadLine();

            bool weighted = false;
            if (byWeight.TrimStart().ToLower()[0] == 'y')
            {
                weighted = true;
            }
            float value = float.Parse(price);

            Database_API.AddItem(name, value, weighted);

            Console.WriteLine("Adding complete.");
        }

        /// <summary>
        /// Write the prompt needed to add an item into the database.
        /// </summary>
        private static void WriteAction_Add_To_DB_Special()
        {
            bool hasItem = false;
            Console.WriteLine("If a special is added to the database that already affects an item, then the new" +
                "special will replace the old one.");
            Console.Write("Enter the name of the item being affected: ");
            string name = Console.ReadLine().Trim();
            hasItem = Database_API.TryGetItem(name);

            string isDeferred;
            string disType;
            string disValue;
            int fireCount = 0;
            int affectCount = 0;
            int fireLimit = 0;
            bool defer = false;

            if (hasItem)
            {
                Console.WriteLine("Is the special deferred? (As in, 'buy x get y FREE') (y/n): ");
                isDeferred = Console.ReadLine();
                defer = isDeferred.TrimStart().ToLower()[0] == 'y';

                Console.WriteLine("How does the special calculate its discount? " +
                    "(1: flat (2 dollars off per item), 2: by percentage, 3: set price (as in, 3 for 5) (1/2/3): ");
                disType = Console.ReadLine();
                int type = int.Parse(disType);
                Special.DISCOUNT_TYPE typeAdd = Special.DISCOUNT_TYPE.SET_TO_AMOUNT;
                switch (type)
                {
                    case 1: typeAdd = Special.DISCOUNT_TYPE.SET_TO_AMOUNT; break;
                    case 2: typeAdd = Special.DISCOUNT_TYPE.REDUCE_BY_PERCENTAGE; break;
                    case 3: typeAdd = Special.DISCOUNT_TYPE.REDUCE_BY_DOLLAR; break;
                }
                
                Console.Write("What is the value of the discount? (If it's a percentage," +
                    "enter the desired value without percentage sign: ");
                disValue = Console.ReadLine();
                int value = int.Parse(disValue);

                if (!defer)
                {
                    Console.Write("How many items does this special need to fire? ");
                    fireCount = int.Parse(Console.ReadLine());

                    Console.Write("How many items does this special affect when fired? ");
                    affectCount = int.Parse(Console.ReadLine());
                }

                Console.Write("How many times can a customer use this special? ");
                fireLimit = int.Parse(Console.ReadLine());

                if (defer)
                {
                    Database_API.AddSpecial(new SpecialDeferred(name, value, typeAdd, fireLimit));
                }
                else
                {
                    Database_API.AddSpecial(new SpecialNormal(name, value, 
                        typeAdd, fireCount, affectCount, fireLimit));
                }

                Console.WriteLine("Special added successfully.");
            }
            else
            {
                Console.WriteLine("No such item exists in the database. Make sure it's spelled correctly.");
            }
        }

        /// <summary>
        /// Writes a list of all items available in the database.
        /// </summary>
        /// <returns>A string of all items in the database.</returns>
        private static string WriteItems()
        {
            StringBuilder builder = new StringBuilder();

            int h = 0;
            int i = Database_API.GetItemCount();

            while (h < i)
            {
                Item item = Database_API.GetItem(h);
                builder.Append(DisplayOrganizer.AddEntry(item.name, SPACING_NAME));
                builder.Append(DisplayOrganizer.AddEntry(item.price.ToString(), SPACING_VALUE));
                builder.AppendLine(DisplayOrganizer.AddMark(item.priceByWeight));
                h++;
            }
            return builder.ToString();
        }

        /// <summary>
        /// Writes a list of all specials available in the database.
        /// </summary>
        /// <returns></returns>
        private static string WriteSpecials()
        {
            StringBuilder builder = new StringBuilder();

            int h = 0;
            int i = Database_API.GetSpecialsCount();

            while (h < i)
            {
                Special item = Database_API.GetSpecial(h);
                builder.Append(DisplayOrganizer.AddEntry(item.itemAffected, SPACING_NAME));
                builder.AppendLine(FormatSP(item, SPACING_NAME));
                h++;
            }
            return builder.ToString();
        }

        /// <summary>
        /// Takes a Special as an input and returns its attribute
        /// in a readable format.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="blanks"></param>
        /// <returns></returns>
        private static string FormatSP(Special input, int blanks)
        {
            StringBuilder formatter = new StringBuilder();

            formatter.Append("Buy ");
            formatter.Append(input.itemsApplied.ToString());
            switch (input.discount_type)
            {
                case Special.DISCOUNT_TYPE.REDUCE_BY_DOLLAR:
                    formatter.Append(" and get a discount of $");
                    formatter.Append(input.itemCostChange.ToString());
                    formatter.Append(" per item.");
                    break;
                case Special.DISCOUNT_TYPE.REDUCE_BY_PERCENTAGE:
                    formatter.Append(" and get ");
                    formatter.Append(input.itemCostChange.ToString());
                    formatter.Append("% off per item.");
                    break;
                case Special.DISCOUNT_TYPE.SET_TO_AMOUNT:
                    formatter.Append(" for $");
                    formatter.Append(input.preSetAmount.ToString());
                    formatter.Append(".");
                    break;
            }
            
            return formatter.ToString();
        }

        

        /// <summary>
        /// Returns a clean (trimmed, lowercase) char for a given input string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static char CleanInputForChar(string input)
        {
            return input.TrimStart().ToLower()[0];
        }
    }
}
