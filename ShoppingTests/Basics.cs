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
        public Basics()
        {
            SetupTestEnvironment.SetupEnvironment(Shop);
        }
        [Fact]
        public void Instantiation()
        {
            Assert.NotNull(Shop);
        }

        [Fact]
        public void ProductRegistration()
        {
            var price = Shop.GetPrice("A");
            Assert.Equal(10, price);
        }

        [Fact]
        public void ProductRegistration2()
        {
            var price = Shop.GetPrice("B");
            Assert.Equal(20, price);
        }

        [Fact]
        public void ProductRegistration3()
        {
            var price = Shop.GetPrice("C");
            Assert.Equal(30, price);
        }

    }
}
