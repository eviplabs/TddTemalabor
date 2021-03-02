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
        private readonly Shop s = new Shop();

        [Fact]
        public void Instantiation()
        {
            Assert.NotNull(s);
        }

        [Fact]
        public void ProductRegistration()
        {
            s.RegisterProduct("A", 10); //szerintem ez véletlen volt char
            s.RegisterProduct("C", 20);
            s.RegisterProduct("E", 50);
            var price = s.GetPrice("AACEE");
            Assert.Equal(140, price);
        }

        [Fact]
        public void NotRegisteredProductInGetProce()
        {
            s.RegisterProduct("A", 10);
            s.RegisterProduct("C", 20);
            s.RegisterProduct("E", 50);
            var price = s.GetPrice("AACEEZ");
            Assert.Equal(140, price);
        }

        [Fact]
        public void AmountDiscount()
        {
            s.RegisterProduct("A", 10);
            s.RegisterProduct("B", 100);
            s.RegisterAmountDiscount("A", 5, 0.9); 
            var price = s.GetPrice("AAAAAAB");
            Assert.Equal(154, price);
        }

        [Fact]
        public void AmountWithRegisteredDiscount()
        {
            s.RegisterProduct("A", 10);
            s.RegisterProduct("B", 100);
            s.RegisterAmountDiscount("A", 3, 0.7);  //Piros - jelenleg az 5 darab és 10% kedvezmény beégetett, a RegisterAmountDiscount értéke nincs használva
            var price = s.GetPrice("AAAABB");
            Assert.Equal(228, price);
        }

        [Fact]
        public void PriceWithRegisteredCountDiscount()
        {
            s.RegisterProduct("C", 20);
            s.RegisterProduct("E", 50);
            s.RegisterCountDiscount("E", 2, 3);
            var price = s.GetPrice("CCEEEE");
            Assert.Equal(190, price);

        }
    }
}
