using Shopping;
using System;
using Xunit;

namespace ShoppingTests
{
    // Stryker.NET mutation testing tool:
    //  https://github.com/stryker-mutator/stryker-net
    // Moq package:
    //  https://github.com/moq/moq4
    public class DiscountTests : ShoppingTestBase
    {

        [Fact]
        public void Instantiation()
        {
            Assert.NotNull(s);
        }

        [Fact]
        public void AmountDiscount()
        {
            RegisterProducts(2);
            s.RegisterAmountDiscount('A', 5, 0.9);
            var price = s.GetPrice("AAAAAAB");
            Assert.Equal(74, price);
        }

        [Fact]
        public void AmountDiscountDuplicateRegistration()
        {
            RegisterProducts(1);
            Assert.True(s.RegisterAmountDiscount('A', 2, 0.5));
            Assert.False(s.RegisterAmountDiscount('A', 2, 0.5));
            Assert.False(s.RegisterAmountDiscount('A', 3, 0.5));
            Assert.False(s.RegisterAmountDiscount('A', 2, 0.8));
            Assert.False(s.RegisterAmountDiscount('A', 3, 0.8));
            var price = s.GetPrice("AA");
            Assert.Equal(10, price);
        }

        [Fact]
        public void AmountDiscountRegistrationForNotExistingProduct()
        {
            Assert.False(s.RegisterAmountDiscount('A', 2, 0.9));
        }

        [Fact]
        public void AmountDiscountRegistrationWithInvalidAmountParameter()
        {
            RegisterProducts(1);
            Assert.False(s.RegisterAmountDiscount('A', 1, 0.9));
            Assert.False(s.RegisterAmountDiscount('A', 0, 0.9));
            Assert.False(s.RegisterAmountDiscount('A', -1, 0.9));
            var price = s.GetPrice("AAA");
            Assert.Equal(30, price);
        }

        [Fact]
        public void AmountDiscountRegistrationWithInvalidFactor()
        {
            RegisterProducts(1);
            // 0 < szorzo < 1
            Assert.False(s.RegisterAmountDiscount('A', 2, 1.5));
            Assert.False(s.RegisterAmountDiscount('A', 2, 1.0));
            Assert.False(s.RegisterAmountDiscount('A', 2, 0));
            Assert.False(s.RegisterAmountDiscount('A', 2, -0.8));
            var price = s.GetPrice("AAA");
            Assert.Equal(30, price);
        }

        [Fact]
        public void AmountWithRegisteredDiscount()
        {
            RegisterProducts(2);
            s.RegisterAmountDiscount('A', 3, 0.7);
            var price = s.GetPrice("AAAABB");
            Assert.Equal(68, price);
        }

        [Fact]
        public void PriceWithRegisteredCountDiscount()
        {
            RegisterProducts(2);
            s.RegisterCountDiscount('B', 2, 4);
            var price = s.GetPrice("AABBBB");
            Assert.Equal(60, price);

        }

        [Fact]
        public void ComboDiscount()
        {
            RegisterProducts(7);
            s.RegisterComboDiscount("ABC", 55, false);     //Módosítottam a teszten, mert sehol nem kérték, hogy több kombó is legyen egyszerre.
            var price = s.GetPrice("ABCDEFG");
            Assert.Equal(275, price);

            price = s.GetPrice("ABD");
            Assert.Equal(70, price);
        }


        [Fact]
        public void AmountAndCountDiscountAtTheSameTime()
        {
            RegisterProducts(2);
            s.RegisterAmountDiscount('A', 2, 0.9);
            s.RegisterCountDiscount('A', 2, 3);
            var price = s.GetPrice("AAAB");   //Eredeti ár 3*10+20=50,  Amounttal 2*10*0.9+10+20=48,  Counttal 2*10+20=40
            Assert.Equal(40, price);
        }

        [Fact]
        public void ComboAndCountDiscountAtTheSameTime()
        {
            RegisterProducts(3);
            s.RegisterCountDiscount('A', 2, 3);
            s.RegisterComboDiscount("ABC", 40, false);
            var price = s.GetPrice("AAABBC"); //Eredeti ár 3*10+2*20+30=100, Counttal 2*10+2*20+30=90, Comboval 2*10+20+40=80
            Assert.Equal(80, price);
        }

        [Fact]
        public void ComboAndAmountDiscountAtTheSameTime()
        {
            RegisterProducts(3);
            s.RegisterAmountDiscount('C', 2, 0.8);
            s.RegisterComboDiscount("ABC", 40, false);
            var price = s.GetPrice("ABCCC");  //Eredeti ár 10+20+3*30=120, Amounttal 10 + 20 + 2*30*0.8 + 30 = 108, Comboval 40+2*30=100
            Assert.Equal(100, price);
        }


        [Fact]
        public void CouponDiscount()
        {
            RegisterProducts(4);
            s.RegisterCouponDiscount("69420", 0.5);
            s.RegisterCouponDiscount("69420", 0.5);// Két kupont regisztrálunk két sorban
            var price = s.GetPrice("ABCDk69420");
            var p = s.GetPrice("ABCD") * 0.5;
            Assert.Equal(price, s.GetPrice("ABCD") * 0.5);
            price = s.GetPrice("ABCDk69420");
            Assert.Equal(price, s.GetPrice("ABCD") * 0.5);
            price = s.GetPrice("ABCDk69420");
            Assert.Equal(price, s.GetPrice("ABCD")); // Elfogyott a kupon
        }

    }
}
