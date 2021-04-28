using System;
using Xunit;
using Shopping;

namespace ShoppingTests
{
    public class ClubMembershipTests
    {
        private readonly Shop Shop = new Shop();
        public ClubMembershipTests()
        {
            SetupTestEnvironment.SetupEnvironment(Shop);
        }
        [Fact]
        public void RegisterClubMembership()
        {
            var price = Shop.GetPrice("ABCDt");
            //(10+20+30+40)*0.9 = 162
            Assert.Equal(90, price);
        }

        [Fact]
        public void When_MoretInGetPrice_Expect_IgnoreThemExceptOne()
        {
            var price = Shop.GetPrice("ABCDtttttt");
            //(10+20+50+100)*0.9 = 162
            Assert.Equal(90, price);
        }

        [Fact]
        public void Register_Club_Membership_With_Different_Prices()
        {
            var price = Shop.GetPrice("ABCDt");
            //(10+20+30+150)*0.9 = 189
            Assert.Equal(90, price);
        }

        [Fact]
        public void Register_Club_Membership_Exclusive_Combo_Discount()
        {
            //A RegisterComboDiscount 3. paramétere lenne, hogy csak tagok használhatják-e
            Shop.RegisterComboDiscount("ABC", 50, true);
            //(60)*0.9 = 54
            var price = Shop.GetPrice("ABCt");
            Assert.Equal(45, price);

            //20+20+30 = 70
            price = Shop.GetPrice("ABC");
            Assert.Equal(60, price);
        }
    }
}
