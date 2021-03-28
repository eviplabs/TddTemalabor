using Shopping;
using System;
using Xunit;

namespace ShoppingTests
{
    // Stryker.NET mutation testing tool:
    //  https://github.com/stryker-mutator/stryker-net
    // Moq package:
    //  https://github.com/moq/moq4
    public class Basics
    {
        private readonly Shop Shop = new Shop();
        [Fact]
        public void Instantiation()
        {
            Assert.NotNull(Shop);
        }

        [Fact]
        public void ProductRegistration()
        {
            Shop.RegisterProduct('A', 10);
            var price = Shop.GetPrice("A");
            Assert.Equal(10, price);
        }

        [Fact]
        public void ProductRegistration2()
        {
            Shop.RegisterProduct('B', 20);
            var price = Shop.GetPrice("B");
            Assert.Equal(20, price);
        }

        [Fact]
        public void ProductRegistration3()
        {
            Shop.RegisterProduct('C', 40);
            var price = Shop.GetPrice("C");
            Assert.Equal(40, price);
        }
    }
}
