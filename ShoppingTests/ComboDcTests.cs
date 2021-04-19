using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shopping;

namespace ShoppingTests
{
    public class ComboDcTests
    {
        #region Variables
        private readonly Shop sh = new Shop();
        #endregion

        #region Init
        public ComboDcTests()
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
    }
}
