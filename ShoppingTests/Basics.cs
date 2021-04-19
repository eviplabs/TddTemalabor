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

        #region Helper Methods
        private void AssertPrice(double expected, string cart)
        {
            int result = sh.GetPrice(cart);
            Assert.Equal(expected, result);
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
        public void IsProductNameCaseSensitive()
        {
            sh.RegisterProduct('z', 10);
            AssertPrice(10, "Z");
        }

        [Fact]
        public void RegisterAmountDiscount()
        {
            sh.RegisterDiscount("A", new AmountDiscount(sh.products['A'], 5, 0.9));
            AssertPrice(154, "AAAAAAD");
        }

        [Fact]
        public void RegisterAmountDiscountWithFalseDiscount()
        {
            sh.RegisterDiscount("B", new AmountDiscount(sh.products['B'], 5, 0.9));
            AssertPrice(160, "AAAAAAD");
        }

        [Fact]
        public void RegisterAmountDiscountWithDifferentCharacters()
        {
            sh.RegisterProduct('E', 10);
            sh.RegisterProduct('G', 100);
            sh.RegisterDiscount("E", new AmountDiscount(sh.products['E'], 5, 0.9));
            AssertPrice(154, "EEEEEEG");
        }

        [Fact]
        public void RegisterCountDiscount()
        {
            sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], 2, 3));
            AssertPrice(120, "AAAD");
        }

        [Fact]
        public void RegisterCountDiscountWithoutClaimingFreeProducts()
        {
            sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], 2, 3));
            AssertPrice(120, "AAD");
        }

        [Fact]
        public void RegisterComboDiscount()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            productList.Add(sh.products['C']);
            sh.RegisterDiscount("ABC", new ComboDiscount(productList, 60));
            AssertPrice(110, "CAAAABB");
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
            AssertPrice(130, "CAAAABB");
        }

        [Fact]
        public void RegisterComboDiscountWithMultipleAppliableDiscounts()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            productList.Add(sh.products['C']);
            sh.RegisterDiscount("ABC", new ComboDiscount(productList, 60));
            AssertPrice(130, "AABBCCA");
        }

        [Fact]
        public void RoundingGetPrice()
        {
            sh.RegisterProduct('Z', 5);
            sh.RegisterDiscount("Z", new AmountDiscount(sh.products['Z'], 5, 0.9));
            Assert.Equal(23, sh.GetPrice("ZZZZZ"));
            AssertPrice(23, "ZZZZZ");
        }

        [Fact]
        public void ClubMemberShipDiscount()
        {
            sh.RegisterSuperShopCard("1");
            Assert.Equal(18, sh.GetPrice("Bv1"));
            AssertPrice(18, "Bv1");
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
            AssertPrice(160, "AABBCC");
            //(60+60)*0,9  comboDiscount és MemberShipDiscount is
            AssertPrice(108, "AABBCCv1");
        }

        [Fact]
        public void PayingWithSuperShopCard()
        {
            sh.RegisterSuperShopCard("1");
            sh.GetPrice("ABCDv1"); //180
            AssertPrice(160, "ABCDv1p");
        }

        [Fact]
        public void PayingWithSuperShopCardWithoutPoints()
        {
            sh.RegisterSuperShopCard("1");
            sh.GetPrice("Av1"); // ezért 0 pont jár
            AssertPrice(162, "ABCDv1p");
        }

        [Fact]
        public void PayingWithSuperShopCardRemainingPoints()
        {
            sh.RegisterSuperShopCard("1");
            sh.GetPrice("DDDDDDDDDDDv1"); //1100
            AssertPrice(0, "Av1p");//1 pontja marad, de az ár 0
            AssertPrice(8, "Av1p");
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
            AssertPrice(186, "AAAAAAABBBCCC"); //280 - 90 - 4 - 0
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
            AssertPrice(120, "ABCD"); // 180 - 60
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
            AssertPrice(140, "AABBCD"); // 210 - 60 - 10
        }
        [Fact]
        public void PayingWithSuperShopCardMultiDigitID()
        {
            sh.RegisterSuperShopCard("123");
            sh.GetPrice("ABCDv123"); //180
            AssertPrice(160, "ABCDv123p");
        }

        [Fact]
        public void ToggleDiscountOnlyForClubMembersCombo()
        {
            List<Product> productList1 = new List<Product>();
            productList1.Add(sh.products['A']);
            productList1.Add(sh.products['B']);
            sh.RegisterDiscount("AB", new ComboDiscount(productList1, 20, true));
            sh.RegisterSuperShopCard("1");
            AssertPrice(18, "ABv1");
            AssertPrice(30, "AB");
        }
        [Fact]
        public void ToggleDiscountOnlyForClubMembersAmount()
        {
            sh.RegisterDiscount("A", new AmountDiscount(sh.products['A'], 5, 0.9, true));
            sh.RegisterSuperShopCard("1");
            AssertPrice(41, "AAAAAv1");
            AssertPrice(50, "AAAAA");
        }
        [Fact]
        public void ToggleDiscountOnlyForClubMembersCount()
        {
            sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], 1, 2, true));
            sh.RegisterSuperShopCard("1");
            AssertPrice(9, "AAv1");
            AssertPrice(20, "AA");
        }
        [Fact]
        public void RegisterCouponDiscount()
        {
            sh.RegisterCoupon("112554", 0.9); //10% kupon
            AssertPrice(40*0.9, "AABk112554");
            AssertPrice(40, "AABk112554");
        }

        [Fact]
        public void MoreOfTheSameProductByNumber()
        {
            AssertPrice(230, "A2B8C");
            //10*2+20*8+50=230
        }
        [Fact]
        public void PriceByWeight()
        {
            sh.RegisterProduct('Q', 40); // Legyen 40/100g
            AssertPrice(580, "Q1200C"); // 1200/100=12. 12*40=480. 480+100=580.
        }
        #endregion
    }
}
