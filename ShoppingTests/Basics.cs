using Shopping;
using System;
using System.Collections.Generic;
using Xunit;

namespace ShoppingTests
{
    // Stryker.NET mutation testing tool: 
    //  https://github.com/stryker-mutator/stryker-net
    // Moq package:
    //  https://github.com/moq/moq4
    public class Basics
    {
        private readonly Shop sh = new Shop();

        [Fact]
        public void Instantiation()
        {
            Assert.NotNull(sh);
        }

        [Fact]
        public void ProductRegistration()
        {
            sh.RegisterProduct('A', 10);
            var price = sh.GetPrice("AAA");
            //Ellenörzi, hogy a price objektum integer-e
            Assert.IsType<int>(price);
        }

        [Fact]
        public void AccuratePriceCalculation()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 20);
            sh.RegisterProduct('C', 50);
            Assert.Equal(120 ,sh.GetPrice("AAABBC"));
        }

        [Fact]
        public void AccuratePriceCalculation2()
        {
            sh.RegisterProduct('A', 20);
            sh.RegisterProduct('G', 30);
            sh.RegisterProduct('E', 60);
            Assert.Equal(230, sh.GetPrice("AGGGEE"));
        }

        [Fact]
        public void isProductNameCaseSensitive()
        {
            sh.RegisterProduct('a', 10);
            Assert.Equal(10, sh.GetPrice("A"));
        }

        [Fact]
        public void RegisterAmountDiscount()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 100);
            sh.RegisterAmountDiscount("A", 5, 0.9);
            var price = sh.GetPrice("AAAAAAB");
            Assert.Equal(154, price);
        }

        [Fact]
        public void RegisterAmountDiscountWithoutDiscount()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 100);
            var price = sh.GetPrice("AAAAAAB");
            Assert.Equal(160, price);
        }

        [Fact]
        public void RegisterAmountDiscountWithFalseDiscount()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 100);
            sh.RegisterAmountDiscount("B", 5, 0.9);
            var price = sh.GetPrice("AAAAAAB");
            Assert.Equal(160, price);
        }

        [Fact]
        public void RegisterAmountDiscountWithDifferentCharacters()
        {
            sh.RegisterProduct('E', 10);
            sh.RegisterProduct('G', 100);
            sh.RegisterAmountDiscount("E", 5, 0.9);
            var price = sh.GetPrice("EEEEEEG");
            Assert.Equal(154, price);
        }
        [Fact]
        public void RegisterCountDiscount()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 100);
            sh.RegisterCountDiscount("A",2,3);
            Assert.Equal(120, sh.GetPrice("AAAB"));
        }
        [Fact]
        public void RegisterCountDiscountWithoutClaimingFreeProducts1()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 100);
            sh.RegisterCountDiscount("A", 2, 3);
            Assert.Equal(120, sh.GetPrice("AAB"));
        }
        [Fact]
        public void RegisterComboDiscount()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 20);
            sh.RegisterProduct('C', 50);
            sh.RegisterProduct('D', 100);
            sh.RegisterComboDiscount("ABC", 60);
            Assert.Equal(110, sh.GetPrice("CAAAABB"));
        }
        [Fact]
        public void RegisterComboDiscountWithFalseDiscount()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 20);
            sh.RegisterProduct('C', 50);
            sh.RegisterProduct('D', 100);
            sh.RegisterComboDiscount("ABCD", 60);
            Assert.Equal(130, sh.GetPrice("CAAAABB"));
        }
        [Fact]
        public void RegisterComboDiscountWitDiscountX2()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 20);
            sh.RegisterProduct('C', 50);
            sh.RegisterProduct('D', 100);
            sh.RegisterComboDiscount("ABC", 60);
            Assert.Equal(130, sh.GetPrice("AABBCCA"));
        }
        [Fact]
        public void RoundingGetPrice()
        {
            sh.RegisterProduct('A', 5);
            sh.RegisterAmountDiscount("A", 5, 0.9);
            Assert.Equal(23, sh.GetPrice("AAAAA"));
        }
        [Fact]
        public void ClubMemberShipExists()
        {
            sh.RegisterProduct('A', 10);
            Assert.Equal(9, sh.GetPrice("At"));
        }
    }
}
