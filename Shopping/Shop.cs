using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        #region Variables
        //Collections
        private Dictionary<char, int> products;
        private Dictionary<string, Discount> productDiscounts;
        private Dictionary<string, SuperShop> superShopPoints;
        // Keywords
        private const char membershipKey = 't';
        private const char superShopPaymentKey = 'p';
        #endregion

        #region Init
        public Shop()
        {
            products = new Dictionary<char, int>();
            productDiscounts = new Dictionary<string, Discount>();
            superShopPoints = new Dictionary<string, SuperShop>();
        }
        #endregion

        #region Registration
        public void RegisterProduct(char name, int price)
        {
            products.Add(Char.ToUpper(name), price);
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

            if (memberShip)
            {
                products[membershipKey] = 0;
            }
            if (superShop)
            {
                products[superShopPaymentKey] = 0;
            }

            string userID = GetUserID(shopping_cart);
            double price = GetPriceSumWithoutDiscounts(shopping_cart);

            price -= GetDiscountSum(shopping_cart);

            int endPrice = Convert.ToInt32(Math.Round(
                (memberShip) ? price * 0.9 : price, MidpointRounding.AwayFromZero));

            if (superShop && userID != null)
            {
                endPrice -= (int)superShopPoints[userID].getDiscount(shopping_cart, "", endPrice);
            }
            if(userID != null)
            {
                superShopPoints[userID].addPoints(endPrice);
            }

            return endPrice;
        }
        private int GetPriceSumWithoutDiscounts(string shopping_cart)
        {
            return shopping_cart.Sum(i => products[i]);
        }
        private double GetDiscountSum(string shopping_cart)
        {
            double sumOfDiscounts = 0;
            (string, Discount) selectedComboDiscount = ("", null);
            foreach (var item in productDiscounts)
            {
                sumOfDiscounts += item.Value.getDiscount(shopping_cart, item.Key, GetPriceSumWithoutDiscounts(item.Key));
            }
            return selectedComboDiscount.Item1.Equals("") ? sumOfDiscounts : sumOfDiscounts + selectedComboDiscount.Item2.getDiscount(shopping_cart, selectedComboDiscount.Item1, GetPriceSumWithoutDiscounts(selectedComboDiscount.Item1));

        }
        private string GetUserID(string shopping_cart)
        {
            foreach (char c in shopping_cart)
            {
                if (char.IsDigit(c))
                {
                    products[c] = 0;
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
