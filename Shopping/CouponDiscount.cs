using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class CouponDiscount
    {
        public string couponCode { get; set; }
        public double value { get; set; }
        public CouponDiscount(string couponCode, double value)
        {
            this.couponCode = couponCode;
            this.value = value;
        }
    }
}
