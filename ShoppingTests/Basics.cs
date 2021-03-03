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
        [Fact]
        public void ComboDiscount()
        {
            var s = new Shop();
            s.RegisterProduct("A", 10);
            s.RegisterProduct("B", 20);
            s.RegisterProduct("C", 30);
            s.RegisterProduct("D", 40);
            s.RegisterProduct("E", 50);
            s.RegisterProduct("F", 60);
            s.RegisterProduct("G", 70);
            s.RegisterComboDiscount("ABC", 55);     //Módosítottam a teszten, mert sehol nem kérték, hogy több kombó is legyen egyszerre.
            var price =  s.GetPrice("ABCDEFG");
            Assert.Equal(275, price);
        }

        [Fact]
        public void ClubMembership()
        {
            s.RegisterProduct("A", 40);
            s.RegisterProduct("B", 60);
            s.RegisterClubMembership("t");
            var price = s.GetPrice("ABt");
            Assert.Equal(90, price);
        }
    }
}
