using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        #region Variables
        private Dictionary<char, int> products;
        private Dictionary<string, Discount> discounts;
        #endregion

        #region Init
        public Shop() 
        {
            products = new Dictionary<char, int>();
            discounts = new Dictionary<string, Discount>();
        }
        #endregion

        #region Registration
        public void RegisterProduct(char name, int price)
        {
            products.Add(Char.ToUpper(name), price);
        }
        public void RegisterAmountDiscount(string name, int amount, double discount)
        {
            discounts.Add(name.ToUpper(), new AmountDiscount(amount, discount));
        }
        public void RegisterCountDiscount(string name, int required, int freeItem)
        {
            discounts.Add(name.ToUpper(), new CountDiscount(required, freeItem));
        }
        public void RegisterComboDiscount(string name, int newPrice, bool membership = false)
        {
            discounts.Add(name.ToUpper(), new ComboDiscount(newPrice, membership));
        }
        #endregion

        #region Calculations
        public int GetPrice(string shopping_cart) 
        {
            bool memberShip = hasMembership(shopping_cart);
            double price = GetPriceSumWithoutDiscounts(shopping_cart);

            price -= GetDiscountSum(shopping_cart);

            return Convert.ToInt32(Math.Round(
                (memberShip)? price * 0.9 : price, MidpointRounding.AwayFromZero));
        }

        private int GetPriceSumWithoutDiscounts(string shopping_cart)
        {
            return shopping_cart.Sum(i => products[i]);
        }
        private double GetDiscountSum(string shopping_cart)
        {
            return discounts.Sum(d => d.Value.getDiscount(shopping_cart, d.Key, GetPriceSumWithoutDiscounts(d.Key)));
        }
        private bool hasMembership(string shopping_cart)
        {
            if (shopping_cart.Contains("t"))
            {
                products['t'] = 0;
                return true;    
            }
            return false;
        }
        #endregion
    }
}
