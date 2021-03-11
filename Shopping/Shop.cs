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
        private Dictionary<char, Discount> discounts;
        #endregion

        public Shop() 
        {
            products = new Dictionary<char, int>();
            discounts = new Dictionary<char, Discount>();
        }

        #region Registration
        public void RegisterProduct(char name, int price)
        {
            products.Add(Char.ToUpper(name), price);
        }
        public void RegisterAmountDiscount(char name, int amount, double discount)
        {
            discounts.Add(Char.ToUpper(name), new AmountDiscount(amount, discount));
        }
        public void RegisterCountDiscount(char name, int required, int freeItem)
        {
            discounts.Add(Char.ToUpper(name), new CountDiscount(required, freeItem));
        }
        public void RegisterComboDiscount(string name, int required)
        {
            throw new NotImplementedException();
        }
        #endregion

        public int GetPrice(string shopping_cart) 
        {
            double price = 0;
            foreach (var item in shopping_cart) price += products[item];
            foreach (var item in discounts) price -= item.Value.getDiscount(shopping_cart, item.Key, products[item.Key]);
            return Convert.ToInt32(Math.Round(price, MidpointRounding.AwayFromZero));
        }
    }
}
