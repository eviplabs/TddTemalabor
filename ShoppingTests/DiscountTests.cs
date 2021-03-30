using System;
using Shopping;
using Xunit;

namespace ShoppingTests
{
    public class DiscountTests
    {
        private readonly Shop Shop = new Shop();

        [Fact]
        public void RegisterAmountDiscount()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 100);
            Shop.RegisterAmountDiscount('A', 5, 0.9);
            var price = Shop.GetPrice("AAAAAAB");
            Assert.Equal(154, price);
        }

        [Fact]
        public void RegisterTwoAmountDiscount()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 100);
            Shop.RegisterAmountDiscount('A', 5, 0.9);
            Shop.RegisterAmountDiscount('B', 3, 0.8);
            var price = Shop.GetPrice("AAAABBBB");
            Assert.Equal(360, price);
        }

        [Fact]
        public void RegisterCountDiscount()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('E', 50);
            Shop.RegisterCountDiscount('A', 3, 4);
            // 3 áráért 4-et vihet​
            var price = Shop.GetPrice("AAAAAEEE");
            // 5*10+3*50 helyett 4*10+3*50​
            Assert.Equal(190, price);
        }

        [Fact]
        public void RegisterTwoCountDiscount()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterCountDiscount('A', 3, 4);
            Shop.RegisterCountDiscount('B', 2, 3);
            var price = Shop.GetPrice("AAAABBBC");
            // 4*10 + 3​*20 + 30 --> 3*10 + 2*20 + 30
            Assert.Equal(100, price);
        }

        [Fact]
        public void RegisterTwoCountDiscountDifferentNumbers()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterCountDiscount('A', 3, 5);
            Shop.RegisterCountDiscount('B', 2, 4);
            var price = Shop.GetPrice("AAAAABBBBCC");
            Assert.Equal(130, price);
        }

        [Fact]
        public void RegisterComboDiscount()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 50);
            Shop.RegisterProduct('D', 100);
            Shop.RegisterComboDiscount("ABC", 60);
            var price = Shop.GetPrice("CAAAABB");
            // ABC+AAAB -> 60+3*10+20​
            Assert.Equal(110, price);
        }

        [Fact]
        public void RegisterTwoComboDiscount()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 50);
            Shop.RegisterProduct('D', 100);
            Shop.RegisterComboDiscount("ABC", 60);
            Shop.RegisterComboDiscount("BBB", 40);
            var price = Shop.GetPrice("CAAAABBBB");
            // ABC+AAAB -> 60+3*10+20​
            Assert.Equal(130, price);
        }

        [Fact]
        public void RegisterComboDiscountRepeatProductNames()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 50);
            Shop.RegisterProduct('D', 100);
            Shop.RegisterComboDiscount("ABC", 60);
            var price = Shop.GetPrice("ABCABC");
            // ABC+AAAB -> 60+3*10+20​
            Assert.Equal(120, price);
        }

        [Fact]
        public void When_MoreDiscountOnSameItem_Expect_AmountDiscountIsMoreImportant()
        {
            Shop.RegisterProduct('A', 20);
            Shop.RegisterAmountDiscount('A', 5, 0.5);
            Shop.RegisterCountDiscount('A', 3, 5);
            var price = Shop.GetPrice("AAAAA");
            Assert.Equal(50, price);
        }

        [Fact]
        public void MoreThanOneDiscountInPlaceButOnlyCountDiscountHasPriority()
        {
            Shop.RegisterProduct('A', 20);
            Shop.RegisterProduct('B', 30);
            Shop.RegisterProduct('C', 50);

            Shop.RegisterCountDiscount('A', 3, 5);
            Shop.RegisterComboDiscount("ABC", 60, false);
            var price = Shop.GetPrice("AAAAABC");
            //20*3+30+50 = 140
            Assert.Equal(140, price);
        }

        [Fact]
        public void MoreThanOneDiscountInPlaceAndBothShouldBeApplied()
        {
            Shop.RegisterProduct('A', 20);
            Shop.RegisterProduct('B', 30);
            Shop.RegisterProduct('C', 50);

            Shop.RegisterCountDiscount('A', 3, 5);
            Shop.RegisterComboDiscount("BBB", 60, false);
            var price = Shop.GetPrice("AAAAABBBC");
            //20*3+60+50 = 170
            Assert.Equal(170, price);
        }

        [Fact]
        public void AllDiscountsCanBeClubMemberSpecific() 
        {
            Shop.RegisterProduct('A', 20);
            Shop.RegisterProduct('B', 30);
            Shop.RegisterProduct('C', 50);

            Shop.RegisterCountDiscount('A', 3, 5, true);
            Shop.RegisterComboDiscount("BBB", 60, false);
            var price = Shop.GetPrice("AAAAABBBC");
            //(20*5+60+50) = 210
            Assert.Equal(210, price);
        }
    }
}
