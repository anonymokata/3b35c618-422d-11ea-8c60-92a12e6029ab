using Microsoft.VisualStudio.TestTools.UnitTesting;
using GZhao_Kata_Checkout;

namespace GZhao_Kata_Checkout_Test
{
    [TestClass]
    public class Database_API_Test
    {
        [TestMethod]
        public void Test_Add_One_To_Roster()
        {
            Database_API.AddItem("Beef", 10);
            
            Assert.AreEqual(1,Database_API.GetItemCount());

            Database_API.Clean();
        }

        [TestMethod]
        public void Test_Roster_Add_Remove()
        {
            Database_API.AddItem("Beef", 10);

            Database_API.Remove("Beef");

            Assert.AreEqual(0,Database_API.GetItemCount());

            Database_API.Clean();
        }

        [TestMethod]
        public void Test_Roster_Add_Not_Duplicate()
        {
            Database_API.AddItem("Beef", 10);
            Database_API.AddItem("Beef", 10);

            Assert.AreEqual(1, Database_API.GetItemCount());

            Database_API.Clean();
        }

        [TestMethod]
        public void Test_Roster_Remove_One()
        {
            Database_API.AddItem("Beef", 10);
            Database_API.AddItem("Chicken", 10);
            Database_API.AddItem("Peas", 10);

            Database_API.Remove("Beef");

            Assert.AreEqual(2, Database_API.GetItemCount());

            Database_API.Clean();
        }

        [TestMethod]
        public void Test_Add_Special_To_Roster()
        {
            Database_Builder.BuildData();

            Database_API.AddItem("Fish", 10);
            Database_API.AddSpecial(new SpecialNormal("fish", 14, Special.DISCOUNT_TYPE.REDUCE_BY_PERCENTAGE, 2,2));

            Assert.AreEqual(7, Database_API.GetSpecialsCount());

            Database_API.Clean();
        }

        [TestMethod]
        public void Test_Add_Special_To_Roster_Replace()
        {
            Database_Builder.BuildData();

            Database_API.AddSpecial(new SpecialNormal("candy", 14, Special.DISCOUNT_TYPE.REDUCE_BY_PERCENTAGE, 2, 2));

            Assert.AreEqual(6, Database_API.GetSpecialsCount());

            Database_API.Clean();
        }
    }
}
