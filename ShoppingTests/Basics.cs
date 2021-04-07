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
            sh.RegisterDiscount("A", new AmountDiscount(sh.products['A'], 5, 0.9));
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
            sh.RegisterDiscount("B", new AmountDiscount(sh.products['B'], 5, 0.9));
            var price = sh.GetPrice("AAAAAAD");
            Assert.Equal(160, price);
        }

        [Fact]
        public void RegisterAmountDiscountWithDifferentCharacters()
        {
            sh.RegisterProduct('E', 10);
            sh.RegisterProduct('G', 100);
            sh.RegisterDiscount("E", new AmountDiscount(sh.products['E'], 5, 0.9));
            var price = sh.GetPrice("EEEEEEG");
            Assert.Equal(154, price);
        }

        [Fact]
        public void RegisterCountDiscount()
        {
            sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], 2, 3));
            Assert.Equal(120, sh.GetPrice("AAAD"));
        }

        [Fact]
        public void RegisterCountDiscountWithoutClaimingFreeProducts()
        {
            sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], 2, 3));
            Assert.Equal(120, sh.GetPrice("AAD"));
        }

        [Fact]
        public void RegisterComboDiscount()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            productList.Add(sh.products['C']);
            sh.RegisterDiscount("ABC", new ComboDiscount(productList, 60));
            Assert.Equal(110, sh.GetPrice("CAAAABB"));
        }

        [Fact]
        public void RegisterComboDiscountWithFalseDiscount()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            productList.Add(sh.products['C']);
            productList.Add(sh.products['D']);
            sh.RegisterDiscount("ABCD", new ComboDiscount(productList, 60));
            Assert.Equal(130, sh.GetPrice("CAAAABB"));
        }

        [Fact]
        public void RegisterComboDiscountWithMultipleAppliableDiscounts()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            productList.Add(sh.products['C']);
            sh.RegisterDiscount("ABC", new ComboDiscount(productList, 60));
            Assert.Equal(130, sh.GetPrice("AABBCCA"));
        }

        [Fact]
        public void RoundingGetPrice()
        {
            sh.RegisterProduct('Z', 5);
            sh.RegisterDiscount("Z", new AmountDiscount(sh.products['Z'], 5, 0.9));
            Assert.Equal(23, sh.GetPrice("ZZZZZ"));
        }

        [Fact]
        public void ClubMemberShipDiscount()
        {
            sh.RegisterSuperShopCard("1");
            Assert.Equal(18, sh.GetPrice("B1"));
        }
        
        [Fact]
        public void ComboDiscountWithMemberShip()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            productList.Add(sh.products['C']);
            sh.RegisterSuperShopCard("1");
            sh.RegisterDiscount("ABC", new ComboDiscount(productList, 60, true)); // 3. taggal (bool) megadható hogy a kedvezmény csak klubtagoknak jár-e
            //20+40+100=160 (comboDiscount csak tagoknak)
            Assert.Equal(160, sh.GetPrice("AABBCC"));
            //(60+60)*0,9  comboDiscount és MemberShipDiscount is
            Assert.Equal(108, sh.GetPrice("AABBCC1"));
        }

        [Fact]
        public void PayingWithSuperShopCard()
        {
            sh.RegisterSuperShopCard("1");
            sh.GetPrice("ABCD1"); //180
            Assert.Equal(160, sh.GetPrice("ABCD1p"));
        }

        [Fact]
        public void PayingWithSuperShopCardWithoutPoints()
        {
            sh.RegisterSuperShopCard("1");
            sh.GetPrice("A1"); //ezért 0 pont jár
            Assert.Equal(162, sh.GetPrice("ABCD1p"));
        }

        [Fact]
        public void PayingWithSuperShopCardRemainingPoints()
        {
            sh.RegisterSuperShopCard("1");
            sh.GetPrice("DDDDDDDDDDD1"); //1100
            Assert.Equal(0, sh.GetPrice("A1p")); //1 pontja marad, de az ár 0
            Assert.Equal(8, sh.GetPrice("A1p"));
        }
        [Fact]
        public void MultipleTypeDiscounts()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            productList.Add(sh.products['C']);
            sh.RegisterDiscount("A", new AmountDiscount(sh.products['A'], 4, 0.9));
            sh.RegisterDiscount("ABC", new ComboDiscount(productList, 50));
            sh.RegisterDiscount("C", new CountDiscount(sh.products['C'], 1, 2));
            Assert.Equal(186,sh.GetPrice("AAAAAAABBBCCC")); //280 - 90 - 4 - 0
        }
        [Fact]
        public void MultipleComboDiscounts()
        {
            List<Product> productList1 = new List<Product>();
            List<Product> productList2 = new List<Product>();
            List<Product> productList3 = new List<Product>();
            productList1.Add(sh.products['A']);
            productList1.Add(sh.products['B']);
            sh.RegisterDiscount("AB", new ComboDiscount(productList1, 20));
            productList2.Add(sh.products['A']);
            productList2.Add(sh.products['B']);
            productList2.Add(sh.products['C']);
            sh.RegisterDiscount("ABC", new ComboDiscount(productList2, 20));
            productList3.Add(sh.products['A']);
            productList3.Add(sh.products['C']);
            sh.RegisterDiscount("AC", new ComboDiscount(productList3, 20));
            Assert.Equal(120,sh.GetPrice("ABCD")); // 180 - 60
        }
        [Fact]
        public void MultipleAppliableComboDiscounts()
        {
            List<Product> productList1 = new List<Product>();
            List<Product> productList2 = new List<Product>();
            List<Product> productList3 = new List<Product>();
            productList1.Add(sh.products['A']);
            productList1.Add(sh.products['B']);
            sh.RegisterDiscount("AB", new ComboDiscount(productList1, 20));
            productList2.Add(sh.products['A']);
            productList2.Add(sh.products['B']);
            productList2.Add(sh.products['C']);
            sh.RegisterDiscount("ABC", new ComboDiscount(productList2, 20));
            productList3.Add(sh.products['A']);
            productList3.Add(sh.products['C']);
            sh.RegisterDiscount("AC", new ComboDiscount(productList3, 20));
            Assert.Equal(140,sh.GetPrice("AABBCD")); // 210 - 60 - 10
        }
        [Fact]
        public void PayingWithSuperShopCardMultiDigitID()
        {
            sh.RegisterSuperShopCard("123");
            sh.GetPrice("ABCD123"); //180
            Assert.Equal(160, sh.GetPrice("ABCD123p"));
        }

        [Fact]
        public void ToggleDiscountOnlyForClubMembersCombo()
        {
            List<Product> productList1 = new List<Product>();
            productList1.Add(sh.products['A']);
            productList1.Add(sh.products['B']);
            sh.RegisterDiscount("AB", new ComboDiscount(productList1, 20, true));
            sh.RegisterSuperShopCard("1");
            Assert.Equal(18, sh.GetPrice("AB1"));
            Assert.Equal(30, sh.GetPrice("AB"));
        }
        [Fact]
        public void ToggleDiscountOnlyForClubMembersAmount()
        {
            sh.RegisterDiscount("A", new AmountDiscount(sh.products['A'], 5, 0.9, true));
            sh.RegisterSuperShopCard("1");
            Assert.Equal(41, sh.GetPrice("AAAAA1"));
            Assert.Equal(50, sh.GetPrice("AAAAA"));
        }
        [Fact]
        public void ToggleDiscountOnlyForClubMembersCount()
        {
            sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], 1, 2, true));
            sh.RegisterSuperShopCard("1");
            Assert.Equal(9, sh.GetPrice("AA1"));
            Assert.Equal(20, sh.GetPrice("AA"));
        }
        [Fact]
        public void RegisterCouponDiscount()
        {
            sh.RegisterCoupon("112554", 0.9); //10% kupon
            Assert.Equal(40*0.9, sh.GetPrice("AABk112554"));
            Assert.Equal(40, sh.GetPrice("AABk112554"));
        }

        [Fact]
        public void MoreOfTheSameProductByNumber()
        {
            Assert.Equal(230, sh.GetPrice("A2B8C"));
            //10*2+20*8+50=230
        }


        #endregion
    }
}
