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
        private readonly IInventory inventory = new InMemoryInventory();

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
        public void AmountDiscountDuplicateRegistration()
        {
            s.RegisterProduct('A', 10);
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
            s.RegisterProduct('A', 10);
            Assert.False(s.RegisterAmountDiscount('A', 1, 0.9));
            Assert.False(s.RegisterAmountDiscount('A', 0, 0.9));
            Assert.False(s.RegisterAmountDiscount('A', -1, 0.9));
            var price = s.GetPrice("AAA");
            Assert.Equal(30, price);
        }

        [Fact]
        public void AmountDiscountRegistrationWithInvalidFactor()
        {
            s.RegisterProduct('A', 10);
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

            var price = s.GetPrice("ABv112");
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
            Assert.Equal(179, price); //10%-os tagsagi kedvezmeny is ervenyesul.
        }

        [Fact]
        public void AmountAndCountDiscountAtTheSameTime()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20);
            s.RegisterAmountDiscount('A', 2, 0.9);
            s.RegisterCountDiscount('A', 2, 3);
            var price = s.GetPrice("AAAB");   //Eredeti ár 3*10+20=50,  Amounttal 2*10*0.9+10+20=48,  Counttal 2*10+20=40
            Assert.Equal(40, price);
        }

        [Fact]
        public void ComboAndCountDiscountAtTheSameTime()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20);
            s.RegisterProduct('C', 30);
            s.RegisterCountDiscount('A', 2, 3);
            s.RegisterComboDiscount("ABC", 40, false);
            var price = s.GetPrice("AAABBC"); //Eredeti ár 3*10+2*20+30=100, Counttal 2*10+2*20+30=90, Comboval 2*10+20+40=80
            Assert.Equal(80, price);
        }

        [Fact]
        public void ComboAndAmountDiscountAtTheSameTime()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20);
            s.RegisterProduct('C', 30);
            s.RegisterAmountDiscount('C', 2, 0.8);
            s.RegisterComboDiscount("ABC", 40, false);
            var price = s.GetPrice("ABCCC");  //Eredeti ár 10+20+3*30=120, Amounttal 10 + 20 + 2*30*0.8 + 30 = 108, Comboval 40+2*30=100
            Assert.Equal(100, price);
        }

        [Fact]
        public void LongUserIDWithShoppingPoints()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20);
            s.RegisterProduct('C', 30);
            s.RegisterProduct('D', 40);
            s.RegisterProduct('E', 50);
            s.RegisterProduct('F', 60);
            s.RegisterProduct('G', 70);


            // 123 -as user vásárol, de nem használja fel, 1 pontot kap
            var price = s.GetPrice("AACDDGv123");
            Assert.Equal(180, price); //A helyes osszeg a funkcio valtozasa miatt megvaltozott.

            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20);
            s.RegisterProduct('C', 30);
            s.RegisterProduct('D', 40);
            s.RegisterProduct('E', 50);
            s.RegisterProduct('F', 60);
            s.RegisterProduct('G', 70);
            // 1-es user vásárol, fel is használja az 1 pontot.
            var price2 = s.GetPrice("AACDDGv1p");
            Assert.Equal(179, price2);

            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20);
            s.RegisterProduct('C', 30);
            s.RegisterProduct('D', 40);
            s.RegisterProduct('E', 50);
            s.RegisterProduct('F', 60);
            s.RegisterProduct('G', 70);
            // 123-as user vásárol és fel is használja a pontokat, 1 + 1 pontot (előző vásárlásból)
            var price3 = s.GetPrice("AACDDGv123p");
            Assert.Equal(178, price3);//200 helyett 180 az ar a 10% alapkedvezmeny miatt, es arra jon ra 1-1 pont, ami levonodik.
        }

        [Fact]
        public void ComboDiscountForClubMembershipWithSuperShopCard()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20);
            s.RegisterProduct('C', 30);
            s.RegisterProduct('D', 40);
            s.RegisterProduct('E', 50);
            s.RegisterProduct('F', 60);
            s.RegisterProduct('G', 70);

            s.RegisterComboDiscount("ABC", 55, true);
            var price = s.GetPrice("ABCDEFGv123");
            Assert.Equal(247, price);
        }

        [Fact]
        public void ClubMembershipWithSuperShopCard()
        {
            s.RegisterProduct('A', 40);
            s.RegisterProduct('B', 60);

            var price = s.GetPrice("ABv234");
            Assert.Equal(90, price);
        }

        [Fact]
        public void MembershipBasedDiscounts()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20);
            s.RegisterProduct('C', 30);
            s.RegisterProduct('D', 40);
            s.RegisterProduct('E', 50);

            s.RegisterAmountDiscount('B', 3, 0.8, true); //Membershiptol fuggo
            s.RegisterCountDiscount('C', 2, 3, true); //Membershiptol fuggo
            var price = s.GetPrice("BBBE");
            var price2 = s.GetPrice("ACCC");

            Assert.Equal(110, price);
            Assert.Equal(100, price2);
        }
        [Fact]
        public void CouponDiscount()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20);
            s.RegisterProduct('C', 30);
            s.RegisterProduct('D', 40);
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

        [Fact]
        public void MoreProductWithOneCode()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20);
            s.RegisterProduct('C', 30);
            var price = s.GetPrice("A2B3C");
            Assert.Equal(110, price);
        }

        [Fact]
        public void WeighBasedPricing()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20, true);
            var price = s.GetPrice("A2B1200");
            Assert.Equal(44, price); //2*10+1.2*20
        }

        [Fact]
        public void RefreshInventory()
        {
            s.RegisterProduct('A', 10, false, 5);  // A regisztráció során mostmár megadható a kezdeti szám (ennyi darab van raktáron kezdetben)
            s.RegisterProduct('B', 20, false, 5);
            var price = s.GetPrice("AB");
            var quantity = s.inventory.GetProductQuantity('A'); // Vásárlás után a darabszámnak eggyel csökkenni kell
            Assert.Equal(4, quantity);
        }

        [Fact]
        public void ProductStorno()
        {
            s.RegisterProduct('B', 20, false, 5);
            s.RegisterProduct('C', 30, false, 5);
            var price = s.GetPrice("BCv111");
            //var stornoValue = ... ("C" termekre);
            Assert.Equal(27, stornoValue);
            
        }
    }
}
