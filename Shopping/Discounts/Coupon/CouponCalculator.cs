using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class CouponCalculator
    {
        Dictionary<int, Coupon> Coupons;

        Coupon activeCoupon;

        public CouponCalculator()
        {
            Coupons = new Dictionary<int, Coupon>();
            activeCoupon = null;
        }

        public void registerCoupon(int id, double discount) 
        {
            Coupons.Add(id, new Coupon { Id = id, Discount = discount });
        }
        public void setActiveCoupon(int id) 
        {
            if(Coupons.ContainsKey(id))
                activeCoupon = Coupons[id];
        }
        public double ActivateCoupon(double price)
        {
            if (activeCoupon == null)
            {
                return price;
            }
            else
            {
                double returnprice = price * activeCoupon.Discount;
                Coupons.Remove(activeCoupon.Id);
                activeCoupon = null;
                return returnprice;
            }
            
            
        }
    }
}
