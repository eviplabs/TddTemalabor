using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
            bool hasSSId = false;
            if (!shopping_cart.Contains('k'))
            {
                hasSSId = shopping_cart.Where(i => char.IsDigit(i)).Any(); // has SuperShop ID
            }
            var productsInCart = getProductsFromCart(shopping_cart);
            double price = GetPriceSumWithoutDiscounts(productsInCart);

            

            price = GetDiscountSum(price, ref productsInCart, hasSSId); // ref keyword helps in keeping the changes to the variables
            if (hasSSId)
            {
                bool superShopPayment = shopping_cart.Contains(superShopPaymentKey);
                string userID = GetUserID(shopping_cart);
                price -= superShopPoints[userID].getMembershipDiscount(price);
                if (superShopPayment)
                {
                    price -= superShopPoints[userID].getDiscount(price);
                }
                superShopPoints[userID].addPoints(price);
            }
            price = CouponDiscount(shopping_cart, price);
            return Convert.ToInt32(Math.Round(price, MidpointRounding.AwayFromZero));
        }

        public double CouponDiscount(string shopping_cart, double price)
        {
            string codeInCart = shopping_cart.Substring(shopping_cart.IndexOf('k') + 1);

            foreach (var coupon in coupons)
            {
                if (coupon.code==codeInCart)
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
        private string GetUserID(string shopping_cart)
        {
            string id = "";
            foreach (char c in shopping_cart)
            {
                if (char.IsDigit(c))
                {
                    id += c;
                }
            }
            return id;
        }
        private Dictionary<char, int> getProductsFromCart(string shopping_cart)
        {
            return shopping_cart.Where(c => char.IsUpper(c)).GroupBy(p => p)
                            .Select(p => new { p.Key, Count = p.Count() }).ToDictionary(p => p.Key, p => p.Count);
        }
        #endregion
    }
}
