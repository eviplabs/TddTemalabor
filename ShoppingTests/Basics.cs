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
    }
}
