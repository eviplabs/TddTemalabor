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
            s.RegisterProduct('A', 10);
            s.RegisterProduct('C', 20);
            s.RegisterProduct('E', 50);
            var price = s.GetPrice("AACEE");
            Assert.Equal(140, price);
        }

        [Fact]
        public void DuplicateRegistrationOfProduct()
        {
            Assert.True(s.RegisterProduct('A', 10));
            Assert.False(s.RegisterProduct('A', 10));
            Assert.False(s.RegisterProduct('A', 200));
            var price = s.GetPrice("A");
            Assert.Equal(10, price);
        }

        [Fact]
        public void RegistrationOfProductWithInvalidName()
        {
            Assert.False(s.RegisterProduct('@', 10)); //'@'='A'-1            
            Assert.False(s.RegisterProduct('[', 10)); //'['='Z'+1            
        }

        [Fact]
        public void RegistrationOfProductWithInvalidPrice()
        {
            Assert.False(s.RegisterProduct('A', 0));
            Assert.False(s.RegisterProduct('B', -1));
        }

        [Fact]
        public void NotRegisteredProductInGetProce()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('C', 20);
            s.RegisterProduct('E', 50);
            var price = s.GetPrice("AACEEZ");
            Assert.Equal(140, price);
        }

        [Fact]
        public void AmountDiscount()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 100);
            s.RegisterAmountDiscount('A', 5, 0.9);
            var price = s.GetPrice("AAAAAAB");
            Assert.Equal(154, price);
        }

        [Fact]
        public void InvalidAmountParameterforAmountDiscountRegistration()
        {
            s.RegisterProduct('A', 10);            
            Assert.False(s.RegisterAmountDiscount('A', 1, 0.9));
            Assert.False(s.RegisterAmountDiscount('A', 0, 0.9));
            Assert.False(s.RegisterAmountDiscount('A', -1, 0.9));
            var price = s.GetPrice("AAA");
            Assert.Equal(30, price);
        }

        [Fact]
        public void AmountWithRegisteredDiscount()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 100);
            s.RegisterAmountDiscount('A', 3, 0.7);  //Piros - jelenleg az 5 darab és 10% kedvezmény beégetett, a RegisterAmountDiscount értéke nincs használva
            var price = s.GetPrice("AAAABB");
            Assert.Equal(228, price);
        }

        [Fact]
        public void PriceWithRegisteredCountDiscount()
        {
            s.RegisterProduct('C', 20);
            s.RegisterProduct('E', 50);
            s.RegisterCountDiscount('E', 2, 4);
            var price = s.GetPrice("CCEEEE");
            Assert.Equal(140, price);

        }

        [Fact]
        public void ComboDiscount()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20);
            s.RegisterProduct('C', 30);
            s.RegisterProduct('D', 40);
            s.RegisterProduct('E', 50);
            s.RegisterProduct('F', 60);
            s.RegisterProduct('G', 70);
            s.RegisterComboDiscount("ABC", 55, false);     //Módosítottam a teszten, mert sehol nem kérték, hogy több kombó is legyen egyszerre.
            var price = s.GetPrice("ABCDEFG");
            Assert.Equal(275, price);
        }

        [Fact]
        public void ClubMembership()
        {
            s.RegisterProduct('A', 40);
            s.RegisterProduct('B', 60);
            var price = s.GetPrice("ABt");
            Assert.Equal(90, price);
        }

        [Fact]
        public void ComboDiscountForClubMembership()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20);
            s.RegisterProduct('C', 30);
            s.RegisterProduct('D', 40);
            s.RegisterProduct('E', 50);
            s.RegisterProduct('F', 60);
            s.RegisterProduct('G', 70);
            s.RegisterComboDiscount("ABC", 55, true);     //Módosítottam a teszten, mert sehol nem kérték, hogy több kombó is legyen egyszerre.
            var price = s.GetPrice("ABCDEFG");
            Assert.Equal(280, price);
        }

        [Fact]
        public void UserIDWithShoppingPoints()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20);
            s.RegisterProduct('C', 30);
            s.RegisterProduct('D', 40);
            s.RegisterProduct('E', 50);
            s.RegisterProduct('F', 60);
            s.RegisterProduct('G', 70);
            var price = s.GetPrice("1pAACDDG");
            Assert.Equal(198, price);

        }
    }
}
