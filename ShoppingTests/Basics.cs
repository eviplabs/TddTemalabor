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
            s.RegisterProduct("A", 10); //szerintem ez v�letlen volt char
            s.RegisterProduct("C", 20);
            s.RegisterProduct("E", 50);
            var price = s.GetPrice("AACEE");
            Assert.Equal(140, price);
        }

        [Fact]
        public void NotRegisteredProductInGetProce()
        {
            var s = new Shop();
            s.RegisterProduct("A", 10);
            s.RegisterProduct("C", 20);
            s.RegisterProduct("E", 50);
            var price = s.GetPrice("AACEEZ");
            Assert.Equal(140, price);
        }

        [Fact]
        public void AmountDiscount()
        {
            var s = new Shop();
            s.RegisterProduct("A", 10);
            s.RegisterProduct("B", 100);
            s.RegisterAmountDiscount("A", 5, 0.9);  //Piros
            var price = s.GetPrice("AAAAAAB");
            Assert.Equal(154, price);
        }
    }
}
