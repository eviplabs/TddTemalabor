using System;
using Shopping;
using Xunit;

namespace ShoppingTests
{
    public class CalculatePriceTests
    {
        private readonly Shop Shop = new Shop();

        [Fact]
        public void PriceSum()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            var price = Shop.GetPrice("ABC");
            Assert.Equal(60, price);
        }

        [Fact]
        public void PriceSum2()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterProduct('D', 40);
            var price = Shop.GetPrice("ABCDA");
            Assert.Equal(110, price);
        }

        [Fact]
        public void PriceSum3()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterProduct('D', 40);
            Shop.RegisterProduct('E', 50);
            var price = Shop.GetPrice("ABCDE");
            Assert.Equal(150, price);
        }
    }
}
