using System.Collections.Generic;
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

            sh.RegisterDiscount("AB", new ComboDiscount(GetProductListAB(), 20));
            sh.RegisterDiscount("AC", new ComboDiscount(GetProductListAC(), 40));
            sh.RegisterDiscount("ABC", new ComboDiscount(GetProductListABC(), 60));
        }
        #endregion

        #region Helper Methods
        private void AssertPrice(double expected, string cart)
        {
            uint result = sh.GetPrice(cart);
            Assert.Equal(expected, result);
        }
        private List<Product> GetProductListABC()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            productList.Add(sh.products['C']);
            return productList;   
        }
        private List<Product> GetProductListAB()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['B']);
            return productList;
        }
        private List<Product> GetProductListAC()
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['A']);
            productList.Add(sh.products['C']);
            return productList;
        }
        private void SpecialXYZComboDcInit(bool membership = false)
        {
            sh.RegisterProduct('X', 10);
            sh.RegisterProduct('Y', 20);
            sh.RegisterProduct('Z', 50);
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['X']);
            productList.Add(sh.products['Y']);
            productList.Add(sh.products['Z']);
            sh.RegisterDiscount("XYZ", new ComboDiscount(productList, 60, membership));
        }
        #endregion

        #region Data
        public static IEnumerable<object[]> GetBasicComboDcCalcData(int numTests)
        {
            var data = new List<object[]>
            {
                new object[] { 20, "AB"},
                new object[] { 40, "AC"},
                new object[] { 60, "ABC"},
                new object[] { 30, "A2B"},
                new object[] { 50, "A2C"},
                new object[] { 70, "A2BC"},
            };
            return data;
        }
        public static IEnumerable<object[]> GetMultipleOccurenceData(int numTests)
        {
            var data = new List<object[]>
            {
                new object[] { 40, "A2B2"},
                new object[] { 80, "A2C2"},
                new object[] { 120, "A2B2C2"},
                new object[] { 50, "A3B2"},
                new object[] { 90, "A3C2"},
                new object[] { 130, "A3B2C2"},
            };
            return data;
        }
        public static IEnumerable<object[]> GetMultipleTypeDcData(int numTests)
        {
            var data = new List<object[]>
            {
                new object[] { 80, "A2B2C"},
                new object[] { 100, "A2BC2"},
                new object[] { 90, "A3B2C"},
                new object[] { 110, "A3BC2"},
            };
            return data;
        }

        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(GetBasicComboDcCalcData), parameters: 2)]
        public void BasicComboDcCalculation(uint expected, string cart)
        {
            AssertPrice(expected, cart);
        }

        [Fact]
        public void PriceCalcWithNoValidDc()
        {
            SpecialXYZComboDcInit();
            AssertPrice(50, "XXXY");
        }

        [Theory]
        [MemberData(nameof(GetMultipleOccurenceData), parameters: 2)]
        public void MultipleOccurencesInComboDc(uint expected, string cart)
        {
            AssertPrice(expected, cart);
        }

        [Theory]
        [MemberData(nameof(GetMultipleTypeDcData), parameters: 2)]
        public void MultipleTypeComboDiscounts(uint expected, string cart)
        {
            AssertPrice(expected, cart);
        }

        [Theory]
        [InlineData(54, "XYZv12", "12", true)]
        [InlineData(80, "XYZ", "12", true)]
        public void ComboDcWithMemberShip(uint expected, string cart, string userID, bool membershipReq)
        {
            SpecialXYZComboDcInit(membershipReq);
            sh.RegisterSuperShopCard(userID);
            AssertPrice(expected, cart);
        }

        [Theory]
        [InlineData(15, "C")]
        [InlineData(30, "CC")]
        public void RegisterComboDiscountWithOnlyOneProduct(uint expected, string cart)
        {
            List<Product> productList = new List<Product>();
            productList.Add(sh.products['C']);
            sh.RegisterDiscount("C", new ComboDiscount(productList, 15));
            AssertPrice(expected, cart);
        }
        #endregion
    }
}
