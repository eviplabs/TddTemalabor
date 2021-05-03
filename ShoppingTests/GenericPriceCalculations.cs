using Shopping;
using System.Collections.Generic;
using Xunit;

namespace ShoppingTests
{
    public class GenericPriceCalculations
    {
        #region Variables
        private readonly Shop sh = new Shop();
        #endregion

        #region Init
        public GenericPriceCalculations()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 20);
            sh.RegisterProduct('C', 50);
            sh.RegisterProduct('D', 100);
            sh.RegisterProduct('Q', 4, true); // Legyen 4 Huf / 10g
        }
        #endregion

        #region Data
        public static IEnumerable<object[]> GetBasicCalcData(int numTests)
        {
            var data = new List<object[]>
            {
                new object[] {120, "AAABBC" },
                new object[] {180, "ABCD" },
                new object[] {50, "AAAAA" },
                new object[] {1800, "AAAAAAAAAABBBBBBBBBBCCCCCCCCCCDDDDDDDDDD" }
            };
            return data;
        }

        public static IEnumerable<object[]> GetWeightedProductData(int numTests)
        {
            var data = new List<object[]>
            {
                new object[] { 4, "Q10" },
                new object[] { 4, "Q14" },
                new object[] { 8, "Q15" },
                new object[] { 1000, "D10" },
                new object[] { 20000, "D200" },
                new object[] { 580, "Q1200D" },
                new object[] { 580, "Q1204D" },
                new object[] { 580, "Q1201D" },
                new object[] { 584, "Q1205D" },
                new object[] { 104, "Q12D" },
                new object[] { 208, "DQ12DQ12" }
            };
            return data;
        }
        public static IEnumerable<object[]> GetMassProductData(int numTests)
        {
            var data = new List<object[]>
            {
                new object[] { 380, "AB16C" },
                new object[] { 1280, "A4B2C20DD" },
                new object[] { 460, "AABBC2DDD" },
                new object[] { 1080, "A100ABC1" },
            };
            return data;
        }
        #endregion Data

        #region Helper Methods
        private void AssertPrice(double expected, string cart)
        {
            uint result = sh.GetPrice(cart);
            Assert.Equal(expected, result);
        }
        #endregion

        #region Facts
        [Fact]
        public void PriceCalculationWithoutPreRegisteredProducts()
        {
            sh.RegisterProduct('G', 30);
            sh.RegisterProduct('E', 60);
            AssertPrice(230, "BGGGEE");
        }
        #endregion

        #region Theories
        [Theory]
        [MemberData(nameof(GetBasicCalcData), parameters: 2)]
        public void BasicPriceCalculation(int expected, string cart)
        {
            AssertPrice(expected, cart);
        }

        [Theory]
        [MemberData(nameof(GetWeightedProductData), parameters: 2)]
        public void PriceByWeight(int expected, string cart)
        {
            AssertPrice(expected, cart);
        }

        [Theory]
        [MemberData(nameof(GetMassProductData), parameters: 2)]
        public void MoreOfTheSameProductByNumber(int expected, string cart)
        {
            AssertPrice(expected, cart);
        }
        #endregion
    }
}
