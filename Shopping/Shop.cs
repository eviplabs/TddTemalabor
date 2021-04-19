using System;
using System.Collections.Generic;
using System.Linq;

namespace Shopping
{
    public class Shop
    {
        #region Variables
        // Collections
        public Dictionary<char, Product> products;
        private Dictionary<string, Discount> productDiscounts;
        private Dictionary<string, SuperShop> superShopPoints;
        private List<Coupon> coupons;

        // Keywords
        private const char superShopPaymentKey = 'p';
        #endregion

        #region Init
        public Shop()
        {
            products = new Dictionary<char, Product>();
            productDiscounts = new Dictionary<string, Discount>();
            superShopPoints = new Dictionary<string, SuperShop>();
            coupons = new List<Coupon>();
        }
        #endregion

        #region Registration
        public void RegisterProduct(char name, int price)
        {
            products.Add(Char.ToUpper(name), new Product(name, price));
        }
        public void RegisterDiscount(string name, Discount dc)
        {
            productDiscounts.Add(name, dc);
        }
        public void RegisterSuperShopCard(string userID)
        { 
            superShopPoints.Add(userID, new SuperShop());
        }

        public void RegisterCoupon(string code, double price)
        {
            coupons.Add(new Coupon(code,price));
        }

        #endregion

        #region Calculations
        public int GetPrice(string shopping_cart)
        {
            // Getprice Variables
            string userID;
            Dictionary<char, int> productsInCart;
            bool SSpay;
            string code;

            // init for the variables
            CartProcessor.processData(shopping_cart, out userID, out productsInCart, out SSpay, out code);

            // initial price calculation
            double price = GetPriceSumWithoutDiscounts(productsInCart);
            price = GetDiscountSum(price, ref productsInCart, (userID != null)); // ref keyword helps in keeping the changes to the variables

            // SS calc
            if (userID != null)
            {
                price -= superShopPoints[userID].processSuperShop(price, SSpay);
            }
            //Coupon calc
            price = CouponDiscount(price, code);

            return Convert.ToInt32(Math.Round(price, MidpointRounding.AwayFromZero));
        }

        public double CouponDiscount(double price, string code)
        {
            foreach (var coupon in coupons)
            {
                if (coupon.code==code)
                {
                    price *= coupon.value;
                    coupons.Remove(coupon);
                    return price;
                }
            }
            return price;
        }

        private int GetPriceSumWithoutDiscounts(Dictionary<char, int> productsInCart)
        {
            return productsInCart.Sum(i => i.Value * products[i.Key].price);
        }
        private double GetDiscountSum(double price, ref Dictionary<char, int> productsInCart, bool membership)
        {
            var orderedDiscounts = productDiscounts.OrderByDescending(d => d.Key.Length).ToDictionary(d => d.Key, d => d.Value);
            foreach (var dc in orderedDiscounts)
            {
                price -= dc.Value.getDiscount(ref productsInCart, membership);
            }
            return price;
        }
        #endregion
    }
}
