using Xunit;
using Shopping;
using System.Collections.Generic;

namespace ShoppingTests
{
    public class CountDcTests
    {
        #region Variables
        private readonly Shop sh = new Shop();
        #endregion

        #region Init
        public CountDcTests()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 20);
            sh.RegisterProduct('C', 50);
            sh.RegisterProduct('D', 100);
            sh.RegisterDiscount("D", new CountDiscount(sh.products['D'], 2, 3)); // default test dc
        }
        #endregion

        #region Data
        public static IEnumerable<object[]> GetProductDcData(int numTests)
        {
            var data = new List<object[]>
            {
                // dc properly functions
                new object[] { 10, "A2", 1, 2},
                new object[] { 20, "A3", 2, 3},
                new object[] { 20, "A4", 2, 4},
                new object[] { 30, "A5", 3, 5},
                // dc functions with more than necessery products
                new object[] { 20, "A3", 1, 2},
                new object[] { 30, "A4", 2, 3},
                new object[] { 30, "A5", 2, 4},
                new object[] { 40, "A6", 3, 5},
                // dc properly functions with other products
                new object[] { 180, "A2BCD", 1, 2},
                new object[] { 190, "A3BCD", 2, 3},
                new object[] { 190, "A4BCD", 2, 4},
                new object[] { 200, "A5BCD", 3, 5},
                // dc properly functions with other products and more than necessary products
                new object[] { 190, "A3BCD", 1, 2},
                new object[] { 200, "A4BCD", 2, 3},
                new object[] { 200, "A5BCD", 2, 4},
                new object[] { 210, "A6BCD", 3, 5},
            };
            return data;
        }
        public static IEnumerable<object[]> GetInvalidDcData(int numTests)
        {
            var data = new List<object[]>
            {
                // dc with not enough products
                new object[] { 20, "AA", 2, 3},
                new object[] { 20, "AA", 2, 4},
                new object[] { 30, "AAA", 3, 5},
                new object[] { 10, "A", 2, 3},
                new object[] { 10, "A", 2, 4},
                new object[] { 20, "AA", 3, 5},
                // dc with not enough products + other products
                new object[] { 190, "AABCD", 2, 3},
                new object[] { 190, "AABCD", 2, 4},
                new object[] { 200, "AAABCD", 3, 5},
                new object[] { 180, "ABCD", 2, 3},
                new object[] { 180, "ABCD", 2, 4},
                new object[] { 190, "AABCD", 3, 5},
            };
            return data;
        }
        public static IEnumerable<object[]> GetUserIDData(int numTests)
        {
            var data = new List<object[]>
            {
                new object[] { 9, "AAv1", 1}, // 1-digint userID
                new object[] { 9, "AAv123", 123}, // multidigit userID
                new object[] { 20, "AA", 1}, // no userID
            };
            return data;
        }
        public static IEnumerable<object[]> GetDataForMultipleDc(int numTests)
        {
            var data = new List<object[]>
            {
                // none of them works
                new object[] { 50, "AB2", (2U, 3U), (2U, 3U)},
                new object[] { 40, "A2B", (2U, 3U), (2U, 3U)},
                // only one valid data for dc
                new object[] { 70, "AB4", (3U, 4U), (3U, 4U)},
                new object[] { 50, "A4B", (3U, 4U), (3U, 4U)},
                // both dc valid
                new object[] { 60, "A3B3", (2U, 3U), (2U, 3U)},
            };
            return data;
        }
        #endregion

        #region Helper Methods
        private void AssertPrice(double expected, string cart)
        {
            uint result = sh.GetPrice(cart);
            Assert.Equal(expected, result);
        }
        #endregion

        [Theory]
        [MemberData(nameof(GetProductDcData), parameters: 4)]
        public void PriceCalcWithDc(uint expected, string cart, uint payedFor, uint value)
        {
            sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], payedFor, value));
            AssertPrice(expected, cart);
        }

        [Theory]
        [MemberData(nameof(GetInvalidDcData), parameters: 4)]
        public void PriceCalcWithoutValidDc(uint expected, string cart, uint payedFor, uint value)
        {
            sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], payedFor, value));
            AssertPrice(expected, cart);
        }

        [Theory]
        [MemberData(nameof(GetUserIDData), parameters: 3)]
        public void DcForSSOnly(uint expected, string cart, string userID)
        {
            sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], 1, 2, true));
            sh.RegisterSuperShopCard(userID);
            AssertPrice(expected, cart);
        }

        [Fact]
        public void CalculateCountDcWithoutReleventProducts()
        {
            AssertPrice(240, "AAAACCCC");
        }

        [Theory]
        [MemberData(nameof(GetDataForMultipleDc), parameters: 4)]
        public void MultipleCountDc(uint expected, string cart,
                (uint payedFor, uint value) productA, (uint payedFor, uint value) productB)
        {
            sh.RegisterDiscount("A", new CountDiscount(sh.products['A'], productA.payedFor, productA.value));
            sh.RegisterDiscount("B", new CountDiscount(sh.products['B'], productB.payedFor, productB.value));
            AssertPrice(expected, cart);
        }

        [Fact]
        public void RegisterCountDiscountWithOnlyOneFreeItem()
        {
            sh.RegisterDiscount("C", new CountDiscount(sh.products['C'], 2, 4));
            AssertPrice(100, "C3");
        }

        [Theory]
        [InlineData(100, "C4")]
        [InlineData(150, "C5")]
        [InlineData(200, "C6")]
        [InlineData(200, "C7")]
        [InlineData(200, "C8")]
        [InlineData(250, "C9")]
        [InlineData(300, "C10")]
        [InlineData(300, "C11")]
        [InlineData(300, "C12")]
        [InlineData(350, "C13")]
        public void RegisterCountDiscountWithoutAppliedTheWhole(int expected, string cart)
        {
            sh.RegisterDiscount("C", new CountDiscount(sh.products['C'], 2, 4));
            AssertPrice(expected, cart);
        }
    }
}
