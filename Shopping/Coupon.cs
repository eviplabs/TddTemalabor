namespace Shopping
{
    public class Coupon
    {
        public string code { get; set; }
        public double value { get; set; }
        public Coupon(string couponCode, double value)
        {
            code = couponCode;
            this.value = value;
        }
    }
}
