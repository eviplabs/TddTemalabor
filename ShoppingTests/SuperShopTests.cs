using Xunit;
using Shopping;

namespace ShoppingTests
{
    public class SuperShopTests : TestBase
    {
        #region Init
        public SuperShopTests() 
        { 
            sh.RegisterSuperShopCard("1");
            sh.RegisterSuperShopCard("123");
        }
        #endregion

        #region Helper Methods
        private void AssertPriceSSBuffer(uint expected, string bufferCart, string cart)
        {
            sh.GetPrice(bufferCart);
            AssertPrice(expected, cart);
        }
        #endregion

        [Theory]
        [InlineData("Bv1")]
        [InlineData("Bv123")]
        public void SuperShopBaseDiscount(string cart)
        {
            AssertPrice(18, cart);
        }

        [Theory]
        [InlineData(8, "Dv1", "Av1p")]
        [InlineData(88, "ABCDv1", "Dv1p")]
        public void PayingWithSuperShopCard(uint expected, string bufferCart, string cart)
        {
            AssertPriceSSBuffer(expected, bufferCart, cart);
        }

        [Fact]
        public void PayingWithSuperShopCardWithoutPoints()
        {
            AssertPriceSSBuffer(162, "Av1", "ABCDv1p");
        }

        [Fact]
        public void PayingWithSuperShopCardRemainingPoints()
        {
            AssertPriceSSBuffer(0, "D12v1", "Av1p"); //1200*0.9 => 11 pont, 2 pontja marad, de az ár 0
            AssertPrice(7, "Av1p");
        }

        [Fact]
        public void PayingWithSuperShopCardNoRemainingPointsAndPrice()
        {
            AssertPriceSSBuffer(0, "D10v1", "Av1p");  //1100*0.9=990 => 10 point
            AssertPrice(9, "Av1p");
        }

        [Fact]
        public void PayingWithSuperShopCardMultiDigitID()
        {
            AssertPriceSSBuffer(160, "AABCDv123", "ABCDv123p");
        }

        [Fact]
        public void SuperShopDiscountWithIDFirst()
        {
            AssertPrice(18, "v1B");
        }

        [Fact]
        public void PayingWithSuperShopCardPaymentSignFirst()
        {
            AssertPriceSSBuffer(160, "AABCDv1", "pABCDv1");
        }

        [Fact]
        public void PayingWithSuperShopCartUserIdSignFirst()
        {
            AssertPriceSSBuffer(160, "AABCDv1", "v1ABCDp");
        }

        [Fact]
        public void PayingWithSuperShopCardRemainingPointsWithCounts()
        {
            AssertPriceSSBuffer(0, "D12v1", "Av1p");
            AssertPrice(9, "Av1");            
        }
    }
}
