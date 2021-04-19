namespace Shopping
{
    public partial class CartProcessor
    {
        private static CartProcessorEvents readingState;
        private enum CartProcessorEvents
        {
            ProductReading,
            MassProductReading,
            UserIDReading,
            SuperShopPayment,
            CouponReading,
        }
        // Keywords
        private const char superShopPaymentKey = 'p';
        private const char couponKey = 'k';
        private const char userIDKey = 'v';
    }
}
