﻿using System;
using Shopping;
using Xunit;

namespace ShoppingTests
{
    public class DiscountTests
    {
        private readonly Shop Shop = new Shop();

        public DiscountTests()
        {
            SetupTestEnvironment.SetupEnvironment(Shop);
        }

        [Fact]
        public void RegisterAmountDiscount()
        {           
            Shop.RegisterAmountDiscount('A', 5, 0.9);
            var price = Shop.GetPrice("AAAAAAB");
            Assert.Equal(74, price);
        }

        [Fact]
        public void RegisterTwoAmountDiscount()
        {           
            Shop.RegisterAmountDiscount('A', 5, 0.9);
            Shop.RegisterAmountDiscount('B', 3, 0.8);
            var price = Shop.GetPrice("AAAABBBB");
            Assert.Equal(104, price);
        }

        [Fact]
        public void RegisterCountDiscount()
        {            
            Shop.RegisterCountDiscount('A', 3, 4);
            var price = Shop.GetPrice("AAAAAEEE");
            Assert.Equal(190, price);
        }

        [Fact]
        public void RegisterTwoCountDiscount()
        {          
            Shop.RegisterCountDiscount('A', 3, 4);
            Shop.RegisterCountDiscount('B', 2, 3);
            var price = Shop.GetPrice("AAAABBBC");
            Assert.Equal(100, price);
        }

        [Fact]
        public void RegisterTwoCountDiscountDifferentNumbers()
        {          
            Shop.RegisterCountDiscount('A', 3, 5);
            Shop.RegisterCountDiscount('B', 2, 4);
            var price = Shop.GetPrice("AAAAABBBBCC");
            Assert.Equal(130, price);
        }

        [Fact]
        public void RegisterComboDiscount()
        {          
            Shop.RegisterComboDiscount("ABC", 50);
            var price = Shop.GetPrice("CAAAABB");
            Assert.Equal(100, price);
        }

        [Fact]
        public void RegisterTwoComboDiscount()
        {           
            Shop.RegisterComboDiscount("ABC", 50);
            Shop.RegisterComboDiscount("BBB", 40);
            var price = Shop.GetPrice("CAAAABBBB");
            Assert.Equal(120, price);
        }

        [Fact]
        public void RegisterComboDiscountRepeatProductNames()
        {            
            Shop.RegisterComboDiscount("ABC", 50);
            var price = Shop.GetPrice("ABCABC");
            Assert.Equal(100, price);
        }

        [Fact]
        public void When_MoreDiscountOnSameItem_Expect_AmountDiscountIsMoreImportant()
        {           
            Shop.RegisterAmountDiscount('A', 5, 0.5);
            Shop.RegisterCountDiscount('A', 3, 5);
            var price = Shop.GetPrice("AAAAA");
            Assert.Equal(25, price);
        }

        [Fact]
        public void MoreThanOneDiscountInPlaceButOnlyCountDiscountHasPriority()
        {           
            Shop.RegisterCountDiscount('A', 3, 5);
            Shop.RegisterComboDiscount("ABC", 50, false);
            var price = Shop.GetPrice("AAAAABC");
            Assert.Equal(80, price);
        }

        [Fact]
        public void MoreThanOneDiscountInPlaceAndBothShouldBeApplied()
        {           
            Shop.RegisterCountDiscount('A', 3, 5);
            Shop.RegisterComboDiscount("BBB", 50, false);
            var price = Shop.GetPrice("AAAAABBBC");
            Assert.Equal(110, price);
        }

        [Fact]
        public void AllDiscountsCanBeClubMemberSpecific() 
        {   
            Shop.RegisterCountDiscount('A', 3, 5, true);
            Shop.RegisterComboDiscount("BBB", 50, false);
            var price = Shop.GetPrice("AAAAABBBC");
            Assert.Equal(130, price);
        }

        [Fact]
        public void AllDiscountsCanBeClubMemberSpecificIncludingAmountDiscount()
        {           
            Shop.RegisterCountDiscount('A', 3, 5, true);
            Shop.RegisterComboDiscount("BBB", 50, false);
            Shop.RegisterAmountDiscount('A', 3, 0.5, false);
            var price = Shop.GetPrice("AAAAABBBC");
            Assert.Equal(105, price);
        }
    }
}
