using Shopping;
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
            sh.RegisterProduct('Q', 4, true); // Legyen 4/10g
        }
        #endregion

        #region Helper Methods
        private void AssertPrice(double expected, string cart)
        {
            int result = sh.GetPrice(cart);
            Assert.Equal(expected, result);
        }
        #endregion

        [Theory]
        [InlineData(120,"AAABBC")]
        [InlineData(180,"ABCD")]
        [InlineData(50,"AAAAA")]
        [InlineData(1800,"AAAAAAAAAABBBBBBBBBBCCCCCCCCCCDDDDDDDDDD")]
        public void PriceCalculationTheory(int expected, string cart)
        {
            AssertPrice(expected, cart);
        }

        [Fact]
        public void PriceCalculationWithoutPreRegisteredProducts()
        {
            sh.RegisterProduct('G', 30);
            sh.RegisterProduct('E', 60);
            AssertPrice(230, "BGGGEE");
        }

        [Theory]
        [InlineData(1000, "D10")]
        [InlineData(20000, "D200")]
        [InlineData(580, "Q1200D")] // 1200/10=120; 120*4=480; 480+100=580.
        [InlineData(580, "Q1204D")]
        [InlineData(580, "Q1201D")]
        [InlineData(584, "Q1205D")]
        [InlineData(104, "Q12D")]
        public void PriceByWeight(int expected, string cart)
        {
            AssertPrice(expected, cart);
        }
        [Theory]
        [InlineData(380, "AB16C")]
        [InlineData(1280, "A4B2C20DD")]
        [InlineData(460, "AABBC2DDD")]
        [InlineData(1080, "A100ABC1")]
        public void MoreOfTheSameProductByNumber(int expected, string cart)
        {
            AssertPrice(expected, cart);
        }
    }
}
