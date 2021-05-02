using Xunit;
using Shopping;

namespace ShoppingTests
{
    public class CountDcTests
    {
        #region Variables
        private readonly Shop sh = new Shop();
        #endregion

        #region Init
        public CountDcTests()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 20);
            sh.RegisterProduct('C', 50);
            sh.RegisterProduct('D', 100);
        }
        #endregion

        #region Helper Methods
        private void AssertPrice(double expected, string cart)
        {
            int result = sh.GetPrice(cart);
            Assert.Equal(expected, result);
        }
        #endregion

        [Fact]
        public void RegisterCountDiscount()
        {
            sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], 2, 3));
            AssertPrice(120, "AAAD");
        }

        [Fact]
        public void RegisterCountDiscountWithoutClaimingFreeProducts()
        {
            sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], 2, 3));
            AssertPrice(120, "AAD");
        }
        [Fact]
        public void ToggleDiscountOnlyForClubMembersCount()
        {
            sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], 1, 2, true));
            sh.RegisterSuperShopCard("1");
            AssertPrice(9, "AAv1");
            AssertPrice(20, "AA");
        }
        [Fact]
        public void RegisterCountDiscountWithoutReleventProductsInCart()
        {
            sh.RegisterDiscount("C", new CountDiscount(sh.products['C'], 2, 3));
            AssertPrice(440, "AAAADDDD");
        }
        [Fact]
        public void RegisterCountDiscountMultipleCountDiscounts()
        {
            sh.RegisterDiscount("C", new CountDiscount(sh.products['C'], 2, 4));
            sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], 3, 4));
            AssertPrice(130, "A4C4");
        }
        [Fact]
        public void RegisterCountDiscountWithOnlyOneFreeItem()
        {
            sh.RegisterDiscount("C", new CountDiscount(sh.products['C'], 2, 4));
            AssertPrice(100, "C3");
        }
        [Fact]
        public void RegisterCountDiscountAppliedTwice()
        {
            sh.RegisterDiscount("C", new CountDiscount(sh.products['C'], 2, 3));
            AssertPrice(200, "C6");
        }
        [Fact]
        public void RegisterCountDiscountAppliedOnceAndAHalf()
        {
            sh.RegisterDiscount("C", new CountDiscount(sh.products['C'], 2, 4));
            AssertPrice(100, "C4");
            AssertPrice(150, "C5");
            AssertPrice(200, "C6");
            AssertPrice(200, "C7");
            AssertPrice(200, "C8");
            AssertPrice(250, "C9");
            AssertPrice(300, "C10");
            AssertPrice(300, "C11");
            AssertPrice(300, "C12");
            AssertPrice(350, "C13");
        }
    }
}
