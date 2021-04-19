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

        [Theory]
        [InlineData(120,"AAABBC")]
        [InlineData(180,"ABCD")]
        [InlineData(50,"AAAAA")]
        [InlineData(1800,"AAAAAAAAAABBBBBBBBBBCCCCCCCCCCDDDDDDDDDD")]
        public void PriceCalculationTheory(int expected, string cart)
        {
            AssertPrice(expected, cart);
        }

        [Fact]
        public void PriceCalculationWithoutPreRegisteredProducts()
        {
            sh.RegisterProduct('G', 30);
            sh.RegisterProduct('E', 60);
            AssertPrice(230, "BGGGEE");
        }

        [Fact]
        public void PriceByWeight()
        {
            sh.RegisterProduct('Q', 40); // Legyen 40/100g
            AssertPrice(580, "Q1200C"); // 1200/100=12. 12*40=480. 480+100=580.
        }
        [Theory]
        [InlineData(280, "AB16C")]
        [InlineData(1280, "A4B2C20DD")]
        [InlineData(460, "AABBC2DDD")]
        [InlineData(1080, "A100ABC")]
        public void MoreOfTheSameProductByNumber(int expected, string cart)
        {
            AssertPrice(expected, cart);
        }
    }
}
