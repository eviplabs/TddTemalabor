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

        // Keywords
        private const char membershipKey = 't';
        private const char superShopPaymentKey = 'p';
        #endregion

        #region Init
        public Shop()
        {
            products = new Dictionary<char, Product>();
            productDiscounts = new Dictionary<string, Discount>();
            superShopPoints = new Dictionary<string, SuperShop>();
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
        #endregion

        #region Calculations
        public int GetPrice(string shopping_cart)
        {
            bool memberShip = shopping_cart.Contains(membershipKey);
            bool hasSSId = shopping_cart.Where(i => char.IsDigit(i)).Any(); // has SuperShop ID

            var productsInCart = getProductsFromCart(shopping_cart);
            
            double price = GetPriceSumWithoutDiscounts(productsInCart);
            price = GetDiscountSum(price, ref productsInCart, memberShip); // ref keyword helps in keeping the changes to the variables
            if (memberShip)
            {
                price -= MembershipDiscount.getDiscount(price);
            }
            if (hasSSId)
            {
                bool superShopPayment = shopping_cart.Contains(superShopPaymentKey);
                string userID = GetUserID(shopping_cart);
                if (superShopPayment)
                {
                    price -= superShopPoints[userID].getDiscount(price);
                }
                superShopPoints[userID].addPoints(price);
            }
            return Convert.ToInt32(Math.Round(price, MidpointRounding.AwayFromZero));
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
