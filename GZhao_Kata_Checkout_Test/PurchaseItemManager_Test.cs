using Microsoft.VisualStudio.TestTools.UnitTesting;
using GZhao_Kata_Checkout;
using System.Collections.Generic;

namespace GZhao_Kata_Checkout_Test
{
    [TestClass]
    public class PurchaseItemManager_Test
    {
        private void BuildDataTest()
        {
            Database_Builder.BuildData();
        }


        [TestMethod]
        public void Test_Buy_Item()
        {
            BuildDataTest();
            
            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("Beef", 1);
            pim.Add("Chicken", 1);

            Assert.AreEqual(2, pim.TotalPurchasedEntries());
            Assert.AreEqual(20, pim.TotalNoSpecialPurchase());
        }

        [TestMethod]
        public void Test_Buy_Item_Multiple()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("Beef", 1);
            pim.Add("beef", 1);
            pim.Add("beeF", 1);
            pim.Add("chicken", 1);
            pim.Add("Chicken", 1);
            pim.Add("soup", 1);
            pim.Add("Soup", 1);
            pim.Add("soUp", 1);

            Assert.AreEqual(8, pim.TotalPurchasedEntries());
            Assert.AreEqual(65, pim.TotalNoSpecialPurchase());
        }
        
        [TestMethod]
        public void Test_Special_Deal_Fixed_Not_Active()
        {
            BuildDataTest();
            

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("soup", 1);
            pim.Add("soup", 1);

            Assert.AreEqual(10, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Special_Deal_Fixed_Active()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("soup", 1);
            pim.Add("soup", 1);
            pim.Add("soup", 1);
            
            Assert.AreEqual(6, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Item_Special_Active()
        {
            BuildDataTest();

            ItemInCart item = new ItemInCart(Database_API.GetItem("Soup"), 1);
            item.SetSpecialValue(Database_API.GetSpecial("Soup"));

            Assert.AreEqual(2, item.GetPrice());
        }

        [TestMethod]
        public void Test_CartItem_Price_After_Special()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("soup");
            pim.Add("soup");
            pim.Add("soup");

            Assert.AreEqual(2, pim.GetAtPosition(0).GetPrice());
            Assert.AreEqual(2, pim.GetAtPosition(1).GetPrice());
            Assert.AreEqual(2, pim.GetAtPosition(2).GetPrice());
        }

        [TestMethod]
        public void Test_Multiple_Applied_Specials()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("soup");
            pim.Add("soup");
            pim.Add("soup");
            pim.Add("soup");
            pim.Add("soup");
            pim.Add("soup");

            Assert.AreEqual(12, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Multiple_Applied_Specials_Remove()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("soup");
            pim.Add("soup");
            pim.Add("soup");
            pim.Add("soup");
            pim.Add("soup");
            pim.Add("soup");

            pim.RemoveLast("Soup");

            //Three at discount (5 - 3)
            //Two at normal (5)
            //For a total of 16.
            Assert.AreEqual(16, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Special_Interwoven()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("soup");
            pim.Add("chicken");
            pim.Add("soup");
            pim.Add("beef");
            pim.Add("soup");
            pim.Add("peas");

            Assert.AreEqual(36, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Special_Multiple()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("soup");
            pim.Add("soup");
            pim.Add("soup");
            pim.Add("candy");
            pim.Add("Candy");

            //soup for 5-3, 2 x 3
            //candy for 4-2, 2 x 2
            Assert.AreEqual(10, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Specials_Remove()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("soup");
            pim.Add("soup");
            pim.Add("soup");

            pim.RemoveLast("soup");

            Assert.AreEqual(10, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Buy_By_Weight()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("beef", 2);

            Assert.AreEqual(20, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Buy_By_Weight_Multiple()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("beef", 2);
            pim.Add("beef", 3);
            pim.Add("chicken", 7);
            pim.Add("ChIcKeN", 3);

            Assert.AreEqual(150, pim.TotalNoSpecialPurchase());
        }

        [TestMethod]
        public void Test_Buy_By_Weight_Special()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();
            
            pim.Add("flour", 10);

            Assert.AreEqual(60, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Buy_By_Weight_Special_More_Activates_Than_Receives()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("chicken", 5);
            pim.Add("chicken", 10);

            Assert.AreEqual(100, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Buy_By_Weight_Special_More_Activates_Than_Receives_Flip()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("chicken", 10);
            pim.Add("chicken", 5);

            Assert.AreEqual(100, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Buy_By_Weight_Special_Multiple()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("chicken", 10);
            pim.Add("chiCken", 5);
            pim.Add("cHicken", 10);
            pim.Add("chickEn", 5);

            Assert.AreEqual(200, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Buy_By_Weight_Special_Multiple_Remove()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("chicken", 10);
            pim.Add("chiCken", 5);
            pim.Add("cHicken", 10);
            pim.Add("chickEn", 5);

            pim.RemoveLast();

            Assert.AreEqual(200, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Buy_By_Weight_Special_Interwoven()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("chicken", 10);
            pim.Add("soup");
            pim.Add("chiCken", 5);
            pim.Add("soup");
            pim.Add("cHicken", 5);
            pim.Add("soup");

            //3 for 6 on soup and 50% off on expensive-est chicken.
            
            Assert.AreEqual(156, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Buy_By_Weight_Special_Interwoven_Remove()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("chicken", 10);
            pim.Add("soup");
            pim.Add("chiCken", 5);
            pim.Add("soup");
            pim.Add("cHicken", 5);
            pim.Add("soup");

            pim.RemoveLast("soup");

            //3 for 6 on soup and 50% off on expensive-est chicken.
            //Except no soup for you.

            Assert.AreEqual(160, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Bundle()
        {
            BuildDataTest();

            PurchaseItemManager pim = new PurchaseItemManager();
            
            pim.Add("flour");
            pim.Add("flour");
            pim.Add("flour");

            //Three bags of flour for just $6 as opposed to 18.

            Assert.AreEqual(6, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Solo_Case()
        {
            BuildDataTest();
            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("Carpet");

            Assert.AreEqual(40, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Limited_Specials()
        {
            BuildDataTest();
            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("Carpet");
            pim.Add("carpet");

            Assert.AreEqual(140, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Limited_Removed()
        {
            BuildDataTest();
            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("Carpet");
            pim.Add("carpet");

            pim.RemoveSpecific(0);

            
            Assert.AreEqual(40, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Deferred()
        {
            BuildDataTest();
            PurchaseItemManager pim = new PurchaseItemManager();
            
            pim.Add("Beef", 10);
            pim.Add("Beef", 8);

            //10lb beef triggers the special
            //8lb beef gets the special (half off).
            Assert.AreEqual(140, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Deferred_Scale_Up()
        {
            BuildDataTest();
            PurchaseItemManager pim = new PurchaseItemManager();

            pim.Add("Beef", 10);
            pim.Add("Beef", 8);
            pim.Add("Beef", 20);

            //10lb beef triggers the special
            //8lb beef gets the special (half off).
            //20lb beef then causes itself to be the header, moving the special
            //from the 8 pounder to the 10 pounder.
            Assert.AreEqual(330, pim.TotalPurchase());
        }

        [TestMethod]
        public void Test_Deferred_Scale_Down()
        {
            BuildDataTest();
            PurchaseItemManager pim = new PurchaseItemManager();
            
            pim.Add("Beef", 20);
            pim.Add("Beef", 10);
            pim.Add("Beef", 8);

            pim.RemoveSpecific(1);
            
            //20 pounder starts. 10 gets the special.
            //10 then gets removed, so 8 gets the special.
            Assert.AreEqual(240, pim.TotalPurchase());
        }
    }
}
