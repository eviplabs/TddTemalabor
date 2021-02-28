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
        [Fact]
        public void Instantiation()
        {
            var s = new Shop();
            Assert.NotNull(s);
        }

        [Fact]
        public void ProductRegistration()
        {
            var s = new Shop();
            s.RegisterProduct("A", 10);
            var price = s.GetPrice("A");
            Assert.Equal(10, price);
        }

        [Fact]
        public void ProductRegistration2()
        {
            var s = new Shop();
            s.RegisterProduct("B", 20);
            var price = s.GetPrice("B");
            Assert.Equal(20, price);
        }

        [Fact]
        public void ProductRegistration3()
        {
            var s = new Shop();
            s.RegisterProduct("C", 40);
            var price = s.GetPrice("C");
            Assert.Equal(40, price);
        }

        [Fact]
        public void PriceSum()
        {
            var s = new Shop();
            s.RegisterProduct("A", 10);
            s.RegisterProduct("B", 20);
            s.RegisterProduct("C", 30);
            var price = s.GetPrice("ABC");
            Assert.Equal(60, price);
        }

        [Fact]
        public void PriceSum2()
        {
            var s = new Shop();
            s.RegisterProduct("A", 10);
            s.RegisterProduct("B", 20);
            s.RegisterProduct("C", 30);
            s.RegisterProduct("D", 40);
            var price = s.GetPrice("ABCDA");
            Assert.Equal(130, price);
        }
    }
}
