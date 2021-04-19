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
    }
}
