using System;
using System.Collections.Generic;
using System.Text;
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
            int result = sh.GetPrice(cart);
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
            sh.GetPrice("DDDDDDDDDDDDv1"); //1200
            AssertPrice(0, "Av1p"); //1 pontja marad, de az ár 0
            AssertPrice(8, "Av1p");
        }
        [Fact]
        public void PayingWithSuperShopCardMultiDigitID()
        {
            sh.RegisterSuperShopCard("123");
            sh.GetPrice("AABCDv123"); //180
            AssertPrice(160, "ABCDv123p");
        }
    }
}
