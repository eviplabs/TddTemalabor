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

        // Further Discounts
        private MembershipDiscount memberDc;

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
            bool superShop = shopping_cart.Contains(superShopPaymentKey);

            /*if (memberShip)
            {
                products[membershipKey] = 0;
            }
            if (superShop)
            {
                products[superShopPaymentKey] = 0;
            }*/

            string userID = GetUserID(shopping_cart);
            double price = GetPriceSumWithoutDiscounts(shopping_cart);

            price -= GetDiscountSum(shopping_cart);
            price = memberDc.getDiscount(price);

            int endPrice = Convert.ToInt32(Math.Round(
                (memberShip) ? price * 0.9 : price, MidpointRounding.AwayFromZero));

            if (superShop && userID != null)
            {
                endPrice -= (int)superShopPoints[userID].getDiscount(shopping_cart);
            }
            if(userID != null)
            {
                superShopPoints[userID].addPoints(endPrice);
            }

            return endPrice;
        }
        private int GetPriceSumWithoutDiscounts(string shopping_cart)
        {
            return shopping_cart.Sum(i => products[i].price);
        }
        private double GetDiscountSum(string shopping_cart)
        {
            double sumOfDiscounts = 0;
            (string, Discount) selectedComboDiscount = ("", null);
            foreach (var item in productDiscounts)
            {
                sumOfDiscounts += item.Value.getDiscount(shopping_cart);
            }
            return selectedComboDiscount.Item1.Equals("") ? sumOfDiscounts : sumOfDiscounts + selectedComboDiscount.Item2.getDiscount(shopping_cart);

        }
        private string GetUserID(string shopping_cart)
        {
            foreach (char c in shopping_cart)
            {
                if (char.IsDigit(c))
                {
                    // products[c] = 0;
                    return c.ToString();
                }
            }
            return null;
        }
        private Dictionary<char, int> getProductsFromCart(string shopping_cart)
        {
            Dictionary<char, int> products_in_cart = new Dictionary<char, int>();
            foreach(char item in shopping_cart)
            {
                if (char.IsUpper(item))
                {
                    try
                    {
                        products_in_cart[item]++;
                    }
                    catch
                    {
                        products_in_cart[item] = 1;
                    }
                }
            }
            return products_in_cart;
        }
        #endregion
    }
}
