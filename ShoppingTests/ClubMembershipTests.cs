using System;
using Xunit;
using Shopping;

namespace ShoppingTests
{
    public class ClubMembershipTests
    {
        private readonly Shop Shop = new Shop();

        [Fact]
        public void RegisterClubMembership()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 50);
            Shop.RegisterProduct('D', 100);
            var price = Shop.GetPrice("ABCDt");
            //(10+20+50+100)*0.9 = 162
            Assert.Equal(162, price);
        }

        [Fact]
        public void When_MoretInGetPrice_Expect_IgnoreThemExceptOne()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 50);
            Shop.RegisterProduct('D', 100);
            var price = Shop.GetPrice("ABCDtttttt");
            //(10+20+50+100)*0.9 = 162
            Assert.Equal(162, price);
        }

        [Fact]
        public void Register_Club_Membership_With_Different_Prices()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterProduct('D', 150);
            var price = Shop.GetPrice("ABCDt");
            //(10+20+30+150)*0.9 = 189
            Assert.Equal(189, price);
        }

        [Fact]
        public void Register_Club_Membership_Exclusive_Combo_Discount()
        {
            Shop.RegisterProduct('A', 20);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterProduct('D', 150);
            //A RegisterComboDiscount 3. paramétere lenne, hogy csak tagok használhatják-e
            Shop.RegisterComboDiscount("ABC", 60, true);
            //(60)*0.9 = 54
            var price = Shop.GetPrice("ABCt");
            Assert.Equal(54, price);

            //20+20+30 = 70
            price = Shop.GetPrice("ABC");
            Assert.Equal(70, price);
        }
    }
}
