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
            //Ellen�rzi, hogy a price objektum integer-e
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
            sh.RegisterAmountDiscount("A", 5, 0.9);
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
            sh.RegisterAmountDiscount("B", 5, 0.9);
            var price = sh.GetPrice("AAAAAAD");
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
            sh.RegisterCountDiscount("A",2,3);
            Assert.Equal(120, sh.GetPrice("AAAD"));
        }

        [Fact]
        public void RegisterCountDiscountWithoutClaimingFreeProducts()
        {
            sh.RegisterCountDiscount("A", 2, 3);
            Assert.Equal(120, sh.GetPrice("AAD"));
        }

        [Fact]
        public void RegisterComboDiscount()
        {
            sh.RegisterComboDiscount("ABC", 60);
            Assert.Equal(110, sh.GetPrice("CAAAABB"));
        }

        [Fact]
        public void RegisterComboDiscountWithFalseDiscount()
        {
            sh.RegisterComboDiscount("ABCD", 60);
            Assert.Equal(130, sh.GetPrice("CAAAABB"));
        }

        [Fact]
        public void RegisterComboDiscountWithMultipleDiscounts()
        {
            sh.RegisterComboDiscount("ABC", 60);
            Assert.Equal(130, sh.GetPrice("AABBCCA"));
        }

        [Fact]
        public void RoundingGetPrice()
        {
            sh.RegisterProduct('Z', 5);
            sh.RegisterAmountDiscount("Z", 5, 0.9);
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
            sh.RegisterComboDiscount("ABC", 60 , true);     // 3. taggal (bool) megadhat� hogy a kedvezm�ny csak klubtagoknak j�r-e
            //20+40+100=160 (comboDiscount csak tagoknak)
            Assert.Equal(160, sh.GetPrice("AABBCC"));
            //(60+60)*0,9  comboDiscount �s MemberShipDiscount is
            Assert.Equal(108, sh.GetPrice("AABBCCt"));
        }

        [Fact]
        public void PayingWithSuperShopCard()
        {
            sh.GetPrice("ABCD1"); //180
            Assert.Equal(178, sh.GetPrice("ABCD1p"));
        }

        [Fact]
        public void PayingWithSuperShopCardWithoutPoints()
        {
            sh.GetPrice("A1"); //ez�rt 0 pont j�r
            Assert.Equal(180, sh.GetPrice("ABCD1p"));
        }

        [Fact]
        public void PayingWithSuperShopCardRemainingPoints()
        {
            sh.GetPrice("DDDDDDDDDDD1"); //1100
            Assert.Equal(0, sh.GetPrice("A1p")); //1 pontja marad, de az �r 0
            Assert.Equal(9, sh.GetPrice("A1p"));
        }
        [Fact]
        public void MultipleTypeDiscounts()
        {
            sh.RegisterAmountDiscount("A",4,0.9); //amount discount helyes m�l�d�s�b�l ad�d�an ez 260 - 5 - ...
            sh.RegisterComboDiscount("ABC",50);
            sh.RegisterCountDiscount("C",1,2);
            Assert.Equal(166,sh.GetPrice("AAAAABBBCCC")); //260 - 90 - 4 - 0
        }
        [Fact]
        public void MultipleComboDiscounts()
        {
            sh.RegisterComboDiscount("AB",20);
            sh.RegisterComboDiscount("AC",20);
            sh.RegisterComboDiscount("ABC",20);
            Assert.Equal(120,sh.GetPrice("ABCD"));
        }
        #endregion
    }
}
