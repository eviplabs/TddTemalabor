using System.Collections.Generic;
using Xunit;
using Shopping;

namespace ShoppingTests
{
    public class AllDcTests : TestBase
    {
        #region Init
        public AllDcTests()
        {
            sh.RegisterDiscount("A", new AmountDiscount(sh.products['A'], 4, 0.9));
            sh.RegisterDiscount("ABC", new ComboDiscount(GetProductListABC(), 50));
            sh.RegisterDiscount("C", new CountDiscount(sh.products['C'], 1, 2));
        }
        #endregion

        #region Helper Methods
        private List<Product> GetProductListABC()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            productList.Add(sh.products['C']);
            return productList;
        }
        #endregion

        #region Data
        public static IEnumerable<object[]> GetDcCombinationData(int numTests)
        {
            var data = new List<object[]>
            {
                new object[] {100, "ABC3"},
                new object[] {86, "A5BC"},
                new object[] {86, "A4C2"},
                new object[] {136, "A5BC3"},
            };
            return data;
        }
        #endregion

        [Theory]
        [MemberData(nameof(GetDcCombinationData), parameters: 2)]
        public void MultipleTypeDiscounts(uint expected, string cart)
        {
            AssertPrice(expected, cart);
        }

        [Fact]
        public void MultipleTypeDiscountsTwoOfTheSameProduct()
        {
            Assert.Throws<System.ArgumentException>(delegate { sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], 2, 4)); }); 
        }
    }
}
