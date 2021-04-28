using System;
using Shopping;
using Xunit;

namespace ShoppingTests
{
    public class CalculatePriceTests
    {
        private readonly Shop Shop = new Shop();
        public CalculatePriceTests()
        {
            SetupTestEnvironment.SetupEnvironment(Shop);
        }
        [Fact]
        public void PriceSum()
        {
            var price = Shop.GetPrice("ABC");
            Assert.Equal(60, price);
        }

        [Fact]
        public void PriceSum2()
        {
            var price = Shop.GetPrice("ABCDA");
            Assert.Equal(110, price);
        }

        [Fact]
        public void PriceSum3()
        {
            var price = Shop.GetPrice("ABCDE");
            Assert.Equal(150, price);
        }
    }
}
