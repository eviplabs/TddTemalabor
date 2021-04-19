using Xunit;
using Shopping;

namespace ShoppingTests
{
    public class AmountDcTests
    {
        #region Variables
        private readonly Shop sh = new Shop();
        #endregion

        #region Init
        public AmountDcTests()
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
        public void RegisterAmountDiscount()
        {
            sh.RegisterDiscount("A", new AmountDiscount(sh.products['A'], 5, 0.9));
            AssertPrice(154, "AAAAAAD");
        }

        [Fact]
        public void RegisterAmountDiscountWithFalseDiscount()
        {
            sh.RegisterDiscount("B", new AmountDiscount(sh.products['B'], 5, 0.9));
            AssertPrice(160, "AAAAAAD");
        }

        [Fact]
        public void RegisterAmountDiscountWithDifferentCharacters()
        {
            sh.RegisterProduct('E', 10);
            sh.RegisterProduct('G', 100);
            sh.RegisterDiscount("E", new AmountDiscount(sh.products['E'], 5, 0.9));
            AssertPrice(154, "EEEEEEG");
        }
        [Fact]
        public void ToggleDiscountOnlyForClubMembersAmount()
        {
            sh.RegisterDiscount("A", new AmountDiscount(sh.products['A'], 5, 0.9, true));
            sh.RegisterSuperShopCard("1");
            AssertPrice(41, "AAAAAv1");
            AssertPrice(50, "AAAAA");
        }
    }
}
