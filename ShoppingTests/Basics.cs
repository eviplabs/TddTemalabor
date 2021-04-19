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
        public void ClubMemberShipDiscount()
        {
            sh.RegisterSuperShopCard("1");
            Assert.Equal(18, sh.GetPrice("Bv1"));
            AssertPrice(18, "Bv1");
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
        public void PayingWithSuperShopCardMultiDigitID()
        {
            sh.RegisterSuperShopCard("123");
            sh.GetPrice("ABCDv123"); //180
            AssertPrice(160, "ABCDv123p");
        }

        [Fact]
        public void RegisterCouponDiscount()
        {
            sh.RegisterCoupon("112554", 0.9); //10% kupon
            AssertPrice(40*0.9, "AABk112554");
            AssertPrice(40, "AABk112554");
        }


        #endregion
    }
}
