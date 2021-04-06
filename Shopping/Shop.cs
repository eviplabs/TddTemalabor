﻿using System;
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
            
            double price = GetPriceSumWithoutDiscounts(shopping_cart);
            price -= GetDiscountSum(shopping_cart);

            if (memberShip)
            {
                price -= MembershipDiscount.getDiscount(price);
            }
            int endPrice = Convert.ToInt32(Math.Round(price, MidpointRounding.AwayFromZero));

            if (hasSSId)
            {
                bool superShopPayment = shopping_cart.Contains(superShopPaymentKey);
                string userID = GetUserID(shopping_cart);
                if (userID != null)
                {
                    endPrice -= (int)superShopPoints[userID].getDiscount(shopping_cart);
                }
                if (userID != null)
                {
                    superShopPoints[userID].addPoints(endPrice);
                }
            }
            return endPrice;
        }
        private int GetPriceSumWithoutDiscounts(string shopping_cart)
        {
            return shopping_cart.Where(i => char.IsUpper(i)).Sum(i => products[i].price);
        }
        private double GetDiscountSum(string shopping_cart)
        {
            return productDiscounts.Sum(d => d.Value.getDiscount(shopping_cart));
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
