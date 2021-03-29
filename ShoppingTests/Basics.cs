using Shopping;
using System;
using System.Collections.Generic;
using Xunit;

namespace ShoppingTests
{
    #region Documentation
    // Stryker.NET mutation testing tool: 
    //  https://github.com/stryker-mutator/stryker-net
    // Moq package:
    //  https://github.com/moq/moq4
    #endregion

    public class Basics
    {
        #region Variables
        private readonly Shop sh = new Shop();
        #endregion

        #region Init
        public Basics()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 20);
            sh.RegisterProduct('C', 50);
            sh.RegisterProduct('D', 100);
        }
        #endregion

        #region Tests
        [Fact]
        public void Instantiation()
        {
            Assert.NotNull(sh);
        }

        [Fact]
        public void IsPriceInt()
        {
            var price = sh.GetPrice("AAA");
            //Ellenörzi, hogy a price objektum integer-e
            Assert.IsType<int>(price);
        }

        [Fact]
        public void AccuratePriceCalculation()
        {
            Assert.Equal(120 ,sh.GetPrice("AAABBC"));
        }

        [Fact]
        public void AccuratePriceCalculationWithoutPreRegisteredProducts()
        {
            sh.RegisterProduct('G', 30);
            sh.RegisterProduct('E', 60);
            Assert.Equal(230, sh.GetPrice("BGGGEE"));
        }

        [Fact]
        public void IsProductNameCaseSensitive()
        {
            sh.RegisterProduct('z', 10);
            Assert.Equal(10, sh.GetPrice("Z"));
        }

        [Fact]
        public void RegisterAmountDiscount()
        {
            sh.RegisterDiscount("A", new AmountDiscount(5, 0.9));
            var price = sh.GetPrice("AAAAAAD");
            Assert.Equal(154, price);
        }

        [Fact]
        public void RegisterAmountDiscountWithoutDiscount()
        {
            var price = sh.GetPrice("AAAAAAD");
            Assert.Equal(160, price);
        }

        [Fact]
        public void RegisterAmountDiscountWithFalseDiscount()
        {
            sh.RegisterDiscount("B", new AmountDiscount(5, 0.9));
            var price = sh.GetPrice("AAAAAAD");
            Assert.Equal(160, price);
        }

        [Fact]
        public void RegisterAmountDiscountWithDifferentCharacters()
        {
            sh.RegisterProduct('E', 10);
            sh.RegisterProduct('G', 100);
            sh.RegisterDiscount("E", new AmountDiscount(5, 0.9));
            var price = sh.GetPrice("EEEEEEG");
            Assert.Equal(154, price);
        }

        [Fact]
        public void RegisterCountDiscount()
        {
            sh.RegisterDiscount("A", new CountDiscount(2, 3));
            Assert.Equal(120, sh.GetPrice("AAAD"));
        }

        [Fact]
        public void RegisterCountDiscountWithoutClaimingFreeProducts()
        {
            sh.RegisterDiscount("A", new CountDiscount(2, 3));
            Assert.Equal(120, sh.GetPrice("AAD"));
        }

        [Fact]
        public void RegisterComboDiscount()
        {
            sh.RegisterDiscount("ABC", new ComboDiscount(60));
            Assert.Equal(110, sh.GetPrice("CAAAABB"));
        }

        [Fact]
        public void RegisterComboDiscountWithFalseDiscount()
        {
            sh.RegisterDiscount("ABCD", new ComboDiscount(60));
            Assert.Equal(130, sh.GetPrice("CAAAABB"));
        }

        [Fact]
        public void RegisterComboDiscountWithMultipleAppliableDiscounts()
        {
            sh.RegisterDiscount("ABC", new ComboDiscount(60));
            Assert.Equal(130, sh.GetPrice("AABBCCA"));
        }

        [Fact]
        public void RoundingGetPrice()
        {
            sh.RegisterProduct('Z', 5);
            sh.RegisterDiscount("Z", new AmountDiscount(5, 0.9));
            Assert.Equal(23, sh.GetPrice("ZZZZZ"));
        }
        [Fact]
        public void ClubMemberShipExists()
        {
            Assert.Equal(9, sh.GetPrice("At"));
        }

        [Fact]
        public void ClubMemberShipDiscount()
        {
            Assert.Equal(18, sh.GetPrice("Bt"));
        }

        [Fact]
        public void ClubMemberShipDiscountWithMultipleIndicators()
        {
            Assert.Equal(18, sh.GetPrice("Bttt"));
        }
        
        [Fact]
        public void ComboDiscountWithMemberShip()
        {
            sh.RegisterDiscount("ABC", new ComboDiscount(60, true)); // 3. taggal (bool) megadható hogy a kedvezmény csak klubtagoknak jár-e
            //20+40+100=160 (comboDiscount csak tagoknak)
            Assert.Equal(160, sh.GetPrice("AABBCC"));
            //(60+60)*0,9  comboDiscount és MemberShipDiscount is
            Assert.Equal(108, sh.GetPrice("AABBCCt"));
        }

        [Fact]
        public void PayingWithSuperShopCard()
        {
            sh.RegisterSuperShopCard(1);
            sh.GetPrice("ABCD1"); //180
            Assert.Equal(178, sh.GetPrice("ABCD1p"));
        }

        [Fact]
        public void PayingWithSuperShopCardWithoutPoints()
        {
            sh.RegisterSuperShopCard(1);
            sh.GetPrice("A1"); //ezért 0 pont jár
            Assert.Equal(180, sh.GetPrice("ABCD1p"));
        }

        [Fact]
        public void PayingWithSuperShopCardRemainingPoints()
        {
            sh.RegisterSuperShopCard(1);
            sh.GetPrice("DDDDDDDDDDD1"); //1100
            Assert.Equal(0, sh.GetPrice("A1p")); //1 pontja marad, de az ár 0
            Assert.Equal(9, sh.GetPrice("A1p"));
        }
        [Fact]
        public void MultipleTypeDiscounts()
        {
            sh.RegisterDiscount("A", new AmountDiscount(4, 0.9));
            sh.RegisterDiscount("ABC", new ComboDiscount(50));
            sh.RegisterDiscount("C", new CountDiscount(1, 2));
            Assert.Equal(186,sh.GetPrice("AAAAAAABBBCCC")); //280 - 90 - 4 - 0
        }
        [Fact]
        public void MultipleComboDiscounts()
        {
            sh.RegisterDiscount("AB", new ComboDiscount(20));
            sh.RegisterDiscount("AC", new ComboDiscount(20));
            sh.RegisterDiscount("ABC", new ComboDiscount(20));
            Assert.Equal(120,sh.GetPrice("ABCD"));
        }
        [Fact]
        public void MultipleAppliableComboDiscounts()
        {
            sh.RegisterDiscount("AB", new ComboDiscount(20));
            sh.RegisterDiscount("AC", new ComboDiscount(20));
            sh.RegisterDiscount("ABC", new ComboDiscount(20));
            Assert.Equal(140,sh.GetPrice("AABBCD"));
        }
        #endregion
    }
}
