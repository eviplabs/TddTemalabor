using Shopping;
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
            sh.RegisterProduct('Y', 5);
        }
        #endregion

        #region Helper Methods
        private void AssertPrice(double expected, string cart)
        {
            uint result = sh.GetPrice(cart);
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
        public void IsPriceUInt()
        {
            var price = sh.GetPrice("YY");
            //Ellenörzi, hogy a price objektum integer-e
            Assert.IsType<uint>(price);
        }

        [Fact]
        public void IsProductNameCaseSensitive()
        {
            sh.RegisterProduct('z', 10);
            AssertPrice(10, "Z");
        }
        [Fact]
        public void RoundingGetPrice()
        {
            sh.RegisterDiscount("Y", new AmountDiscount(sh.products['Y'], 5, 0.9));
            AssertPrice(23, "YYYYY");
        }
        #endregion
    }
}
