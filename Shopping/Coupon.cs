using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Coupon
    {
        public string code { get; set; }
        public double value { get; set; }
        public Coupon(string couponCode, double value)
        {
            this.code = couponCode;
            this.value = value;
        }
    }
}
