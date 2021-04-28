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
            Assert.Equal(90, price);
        }

        [Fact]
        public void When_MoretInGetPrice_Expect_IgnoreThemExceptOne()
        {
            var price = Shop.GetPrice("ABCDtttttt");
            Assert.Equal(90, price);
        }

        [Fact]
        public void Register_Club_Membership_With_Different_Prices()
        {
            var price = Shop.GetPrice("ABCDt");
            Assert.Equal(90, price);
        }

        [Fact]
        public void Register_Club_Membership_Exclusive_Combo_Discount()
        {
            //A RegisterComboDiscount 3. paramétere lenne, hogy csak tagok használhatják-e
            Shop.RegisterComboDiscount("ABC", 50, true);
            var price = Shop.GetPrice("ABCt");
            Assert.Equal(45, price);

            price = Shop.GetPrice("ABC");
            Assert.Equal(60, price);
        }
    }
}
