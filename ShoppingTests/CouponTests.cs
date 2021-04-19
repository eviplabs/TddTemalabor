using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shopping;

namespace ShoppingTests
{
    public class CouponTests
    {
        #region Variables
        private readonly Shop sh = new Shop();
        #endregion

        #region Init
        public CouponTests()
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
        public void RegisterCouponDiscount()
        {
            sh.RegisterCoupon("112554", 0.9); //10% kupon
            AssertPrice(40 * 0.9, "AABk112554");
            AssertPrice(40, "AABk112554");
        }
    }
}
