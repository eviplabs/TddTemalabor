using System.Collections.Generic;
using Xunit;
using Shopping;

namespace ShoppingTests
{
    public class AllDcTests
    {
        #region Variables
        private readonly Shop sh = new Shop();
        #endregion

        #region Init
        public AllDcTests()
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
            uint result = sh.GetPrice(cart);
            Assert.Equal(expected, result);
        }
        #endregion

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
        public void MultipleTypeDiscountsTwoOfTheSameProduct()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            productList.Add(sh.products['C']);
            sh.RegisterDiscount("A", new AmountDiscount(sh.products['A'], 4, 0.9));
            Assert.Throws<System.ArgumentException>(delegate { sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], 2, 4)); }); 
        }
    }
}
