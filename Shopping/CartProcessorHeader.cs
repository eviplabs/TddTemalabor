using System.Collections.Generic;

namespace Shopping
{
    public partial class CartProcessor
    {
        private DataHolder dh = new DataHolder(0);
        private CartProcessorEvents readingState;
        private enum CartProcessorEvents
        {
            ProductReading,
            MassProductReading,
            UserIDReading,
            SuperShopPayment,
            CouponReading,
        }
        private struct DataHolder
        {
            public DataHolder(int dummy)
            {
                cartManager = new Dictionary<char, uint>();
                ID = null;
                SSpaymentReading = false;
                coupon = null;
                numberSubstring = "";
                currentProduct = '0';
            }
            public Dictionary<char, uint> cartManager { get; set; }
            public string ID { get; set; }
            public bool SSpaymentReading { get; set; }
            public string coupon { get; set; }
            public string numberSubstring { get; set; }
            public char currentProduct { get; set; }
        }

        // Keywords
        private const char superShopPaymentKey = 'p';
        private const char couponKey = 'k';
        private const char userIDKey = 'v';
    }
}
