using Xunit;
using Shopping;

namespace ShoppingTests
{
    public class SuperShopTests
    {
        #region Variables
        private readonly Shop sh = new Shop();
        #endregion

        #region Init
        public SuperShopTests()
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
            uint result = sh.GetPrice(cart);
            Assert.Equal(expected, result);
        }
        #endregion

        [Fact]
        public void SuperShopBaseDiscount()
        {
            sh.RegisterSuperShopCard("1");
            AssertPrice(18, "Bv1");
        }

        [Fact]
        public void PayingWithSuperShopCard()
        {
            sh.RegisterSuperShopCard("1");
            sh.GetPrice("AABCDv1"); //180
            AssertPrice(160, "ABCDv1p");
        }
        [Fact]
        public void PayingWithSuperShopCardWithoutPoints()
        {
            sh.RegisterSuperShopCard("1");
            sh.GetPrice("Av1"); // ezért 0 pont jár
            AssertPrice(162, "ABCDv1p");
        }
        [Fact]
        public void PayingWithSuperShopCardRemainingPoints()
        {
            sh.RegisterSuperShopCard("1");
            sh.GetPrice("D12v1"); //1200*0.9 => 11 pont
            AssertPrice(0, "Av1p"); //2 pontja marad, de az ár 0
            AssertPrice(7, "Av1p");
        }
        [Fact]
        public void PayingWithSuperShopCardNoRemainingPointsAndPrice()
        {
            sh.RegisterSuperShopCard("1");
            sh.GetPrice("D10v1"); //1100*0.9=990 => 10 point
            AssertPrice(0, "Av1p");
            AssertPrice(9, "Av1p");
        }
        [Fact]
        public void PayingWithSuperShopCardMultiDigitID()
        {
            sh.RegisterSuperShopCard("123");
            sh.GetPrice("AABCDv123"); //180
            AssertPrice(160, "ABCDv123p");
        }
        [Fact]
        public void SuperShopDiscountWithDiscountFirst()
        {
            sh.RegisterSuperShopCard("1");
            AssertPrice(18, "v1B");
        }
        [Fact]
        public void PayingWithSuperShopCardPaymentSignFirst()
        {
            sh.RegisterSuperShopCard("1");
            sh.GetPrice("AABCDv1"); //180
            AssertPrice(160, "pABCDv1");
        }
        [Fact]
        public void PayingWithSuperShopCartUserIdSignFirst()
        {
            sh.RegisterSuperShopCard("1");
            sh.GetPrice("AABCDv1"); //180
            AssertPrice(160, "v1ABCDp");
        }
        [Fact]
        public void PayingWithSuperShopCardRemainingPointsWithCounts()
        {
            sh.RegisterSuperShopCard("1");
            sh.GetPrice("D12v1"); //1200
            AssertPrice(0, "Av1p"); //2 pontja marad, de nem fizet vele
            AssertPrice(9, "Av1");
        }
    }
}
