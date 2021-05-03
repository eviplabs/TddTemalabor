using Xunit;
using Shopping;
using System.Collections.Generic;

namespace ShoppingTests
{
    public class AmountDcTests : TestBase
    {
        #region Init
        public AmountDcTests()
        {
            sh.RegisterDiscount("B", new AmountDiscount(sh.products['B'], 5, 0.9));
        }
        #endregion

        #region Data
        public static IEnumerable<object[]> GetBasicAmountDcCalcData(int numTests)
        {
            var data = new List<object[]>
            {
                new object[] {54, "A6", 4, 0.9},
                new object[] {35, "A5", 5, 0.7},
                new object[] {85, "A10", 8, 0.85},
                new object[] {34, "A6", 3, 0.571},
                new object[] {20, "A2" , 4, 0.9},
                new object[] {224, "A6BCD", 4, 0.9},
                new object[] {205, "A5BCD", 5, 0.7},
                new object[] {255, "A10BCD", 8, 0.85},
                new object[] {204, "A6BCD", 3, 0.571},
                new object[] {190, "A2BCD" , 4, 0.9}
            };
            return data;
        }
        public static IEnumerable<object[]> GetMultipleDcData(int numTests)
        {
            var data = new List<object[]>
            {
                new object[] {315, "A10C5", (10U, 0.9), (5U, 0.9)},
                new object[] {88, "A6B2", (5U, 0.8), (4U, 0.7)},
                new object[] {80, "A5B2", (4U, 0.8), (3U, 0.7)},
            };
            return data;
        }

        #endregion

        [Theory]
        [MemberData(nameof(GetBasicAmountDcCalcData), parameters: 4)]
        public void BasicAmountDcCalc(uint expected, string cart, uint amount, double multiplier)
        {
            sh.RegisterDiscount("A", new AmountDiscount(sh.products['A'], amount, multiplier));
            AssertPrice(expected, cart);
        }

        [Fact]
        public void AmountDcWithInvalidData()
        {
            AssertPrice(160, "A6D");
        }

        [Fact]
        public void AmountDcWithDifferentProducts()
        {
            sh.RegisterProduct('E', 10);
            sh.RegisterProduct('G', 100);
            sh.RegisterDiscount("E", new AmountDiscount(sh.products['E'], 5, 0.9));
            AssertPrice(154, "E6G");
        }
        [Theory]
        [InlineData(405, "D5v12", "12")]
        [InlineData(500, "D5", "2")]
        public void ToggleDcOnlyForSSMembers(uint expected, string cart, string userID)
        {
            sh.RegisterDiscount("D", new AmountDiscount(sh.products['D'], 5, 0.9, true));
            sh.RegisterSuperShopCard(userID);
            AssertPrice(expected, cart);
        }

        [Theory]
        [MemberData(nameof(GetMultipleDcData), parameters: 4)]
        public void AmountDcWithMultipleDiscounts(uint expected, string cart,
                      (uint req, double multiplier) pr1, (uint req, double multiplier) pr2)
        {
            sh.RegisterDiscount("A", new AmountDiscount(sh.products['A'], pr1.req, pr1.multiplier));
            sh.RegisterDiscount("C", new AmountDiscount(sh.products['C'], pr2.req, pr2.multiplier));
            AssertPrice(expected, cart);
        }

    }
}
