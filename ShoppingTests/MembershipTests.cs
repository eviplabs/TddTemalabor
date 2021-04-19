using Shopping;
using System;
using Xunit;

namespace ShoppingTests
{
    // Stryker.NET mutation testing tool:
    //  https://github.com/stryker-mutator/stryker-net
    // Moq package:
    //  https://github.com/moq/moq4
    public class MembershipTests : ShoppingTestBase
    {

        [Fact]
        public void Instantiation()
        {
            Assert.NotNull(s);
        }

        [Fact]
        public void ClubMembership()
        {
            RegisterProducts(2);

            var price = s.GetPrice("ABv112");
            Assert.Equal(27, price);
        }

        [Fact]
        public void ComboDiscountForClubMembership()
        {
            RegisterProducts(7);

            s.RegisterComboDiscount("ABC", 55, true);     //Módosítottam a teszten, mert sehol nem kérték, hogy több kombó is legyen egyszerre.
            var price = s.GetPrice("ABCDEFG");
            Assert.Equal(280, price);
        }

        [Fact]
        public void UserIDWithShoppingPoints()
        {
            RegisterProducts(7);

            var price = s.GetPrice("1pAACDDG");
            Assert.Equal(179, price); //10%-os tagsagi kedvezmeny is ervenyesul.
        }

        [Fact]
        public void LongUserIDWithShoppingPoints()
        {
            RegisterProducts(7);


            // 123 -as user vásárol, de nem használja fel, 1 pontot kap
            var price = s.GetPrice("AACDDGv123");
            Assert.Equal(180, price); //A helyes osszeg a funkcio valtozasa miatt megvaltozott.

            // 1-es user vásárol, fel is használja az 1 pontot.
            var price2 = s.GetPrice("AACDDGv1p");
            Assert.Equal(179, price2);
            // 123-as user vásárol és fel is használja a pontokat, 1 + 1 pontot (előző vásárlásból)
            var price3 = s.GetPrice("AACDDGv123p");
            Assert.Equal(178, price3);//200 helyett 180 az ar a 10% alapkedvezmeny miatt, es arra jon ra 1-1 pont, ami levonodik.

            // 123-as user 185 pontot gyűjt
            for (int i = 1; i <= 185; i++)
            {
                s.GetPrice("AACDDGv123");
            }
            // 123-as user vásárol és fel is használja a pontokat, 185 + 1 pontot (előző vásárlásból)
            var price4 = s.GetPrice("AACDDGv123p");
            Assert.Equal(0, price4);
            Assert.Equal(6, s.superShop.CustomerPoints[123]);
        }

        [Fact]
        public void ComboDiscountForClubMembershipWithSuperShopCard()
        {
            RegisterProducts(7);

            s.RegisterComboDiscount("ABC", 55, true);
            var price = s.GetPrice("ABCDEFGv123");
            Assert.Equal(247, price);
        }

        [Fact]
        public void ClubMembershipWithSuperShopCard()
        {
            RegisterProducts(2);

            var price = s.GetPrice("ABv234");
            Assert.Equal(27, price);
        }

        [Fact]
        public void MembershipBasedDiscounts()
        {
            RegisterProducts(5);

            s.RegisterAmountDiscount('B', 3, 0.8, true); //Membershiptol fuggo
            s.RegisterCountDiscount('C', 2, 3, true); //Membershiptol fuggo
            var price = s.GetPrice("BBBE");
            var price2 = s.GetPrice("ACCC");

            Assert.Equal(110, price);
            Assert.Equal(100, price2);
        }
        
    }
}
