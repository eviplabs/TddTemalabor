using System;
using System.Collections.Generic;
using System.Text;
using Shopping;
using Xunit;

namespace ShoppingTests
{
    public class GenericPriceCalculations
    {
        #region Variables
        private readonly Shop sh = new Shop();
        #endregion

        #region Init
        public GenericPriceCalculations()
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
        public void PriceCalculation()
        {
            AssertPrice(120, "AAABBC");
        }

        [Fact]
        public void PriceCalculationWithoutPreRegisteredProducts()
        {
            sh.RegisterProduct('G', 30);
            sh.RegisterProduct('E', 60);
            AssertPrice(230, "BGGGEE");
        }
        [Fact]
        public void RoundingGetPrice()
        {
            sh.RegisterProduct('Z', 5);
            sh.RegisterDiscount("Z", new AmountDiscount(sh.products['Z'], 5, 0.9));
            Assert.Equal(23, sh.GetPrice("ZZZZZ"));
            AssertPrice(23, "ZZZZZ");
        }
        [Fact]
        public void PriceByWeight()
        {
            sh.RegisterProduct('Q', 40); // Legyen 40/100g
            AssertPrice(580, "Q1200C"); // 1200/100=12. 12*40=480. 480+100=580.
        }
        [Fact]
        public void MoreOfTheSameProductByNumber()
        {
            AssertPrice(230, "A2B8C");
            //10*2+20*8+50=230
        }
    }
}
