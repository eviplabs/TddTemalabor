using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Shopping
{
    public class SuperShop
    {
        private List<CouponDiscount> couponDiscounts = new List<CouponDiscount>();
        private Dictionary<int, int> supershopPoints = new Dictionary<int, int>();

        public List<CouponDiscount> CouponList
        {
            get { return couponDiscounts; }
        }

        public Dictionary<int, int> CustomerPoints
        {
            get { return supershopPoints; }
        }

        public void RegisterSuperShopCard(int id)
        {
            supershopPoints.Add(id, 0);
        }

        public double CouponDiscount(string cart, double price)
        {
            int index = cart.IndexOf('k') + 1;
            string couponCode = cart.Substring(index);
            foreach (var coupon in couponDiscounts)
            {
                if (coupon.couponCode.Equals(couponCode))
                {
                    price *= coupon.value;
                    couponDiscounts.Remove(coupon);
                    return price;
                }
            }

            return price;
        }

        public bool IsAClubMember(string cart)
        {
            var result = new Regex(@"^[A-Z]+[^k](\d+)|^(\d+)").Match(cart);
            result = new Regex(@"(\d+)").Match(result.ToString());
            if (result.Success)
            {
                int userid = int.Parse(result.Value);
                if (supershopPoints.ContainsKey(userid)) { return true; }
            }
            return false;
        }

        private double GetUpdatedClubMembershipPrice(string name, double price)
        {
            if (IsAClubMember(name))
            {
                return (int)(price * 0.9);
            }
            return price;
        }

        private void checkAndAddSupershopPoints(string name, double price)
        {
            var result = new Regex(@"(\d+)").Match(name);
            if (result.Success)
            {
                int userid = int.Parse(result.Value);                
                supershopPoints[userid] += Convert.ToInt32(price) / 100;
            }
        }

        private double getSupershopAppliedPrice(string name, double price)
        {
            var result = new Regex(@"(\d+)").Match(name);

            if (!name.Contains('p'))
            {
                return price; //A vevő nem szeretne szupershoppal fizetni 
            }
            else
            {
                int userid = int.Parse(result.Value);
                double newPrice = Math.Max(0, price - supershopPoints[userid]);
                supershopPoints[userid] = Math.Max(0, supershopPoints[userid] - Convert.ToInt32(price));
                return newPrice; //A vevő pontjaival frissített ár
            }
        }

        public double ProccessCart(string cart, double price)
        {
            price = CouponDiscount(cart, price);

            price = GetUpdatedClubMembershipPrice(cart, price);

            checkAndAddSupershopPoints(cart, price);

            price = getSupershopAppliedPrice(cart, price);

            return price;
        }
    }
}
