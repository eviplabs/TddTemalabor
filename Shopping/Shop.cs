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
            bool memberShip = false;
            if (shopping_cart.Contains("t"))
            {
                memberShip = true;
                products['t'] = 0;
            }

            double price = GetPriceWithoutDiscounts(shopping_cart);
            foreach (var item in discounts) price -= item.Value.getDiscount(shopping_cart, item.Key, GetPriceWithoutDiscounts(item.Key));
            if (memberShip)
            {
                return Convert.ToInt32(Math.Round(price*0.9, MidpointRounding.AwayFromZero));
            }

            return Convert.ToInt32(Math.Round(price, MidpointRounding.AwayFromZero));
        }

        private int GetPriceWithoutDiscounts(string shopping_cart) {
            int price = 0;
            foreach (var item in shopping_cart) price += products[item];
            return price;
        }
        #endregion
    }
}
