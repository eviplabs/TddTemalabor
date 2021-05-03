using Xunit;
using Shopping;

namespace ShoppingTests
{
    public class CouponTests : TestBase
    {
        #region Init
        public CouponTests() { }
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
