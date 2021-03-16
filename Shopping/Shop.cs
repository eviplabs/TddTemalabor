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

        public Shop() 
        {
            products = new Dictionary<char, int>();
            discounts = new Dictionary<string, Discount>();
        }

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
        public void RegisterComboDiscount(string name, int newPrice)
        {
            discounts.Add(name.ToUpper(), new ComboDiscount(newPrice));
        }
        #endregion

        public int GetPrice(string shopping_cart) 
        {
            if (shopping_cart.Contains("t"))
            {
                products['t'] = -1;
            }

            double price = GetPriceWithoutDiscounts(shopping_cart);
            foreach (var item in discounts) price -= item.Value.getDiscount(shopping_cart, item.Key, GetPriceWithoutDiscounts(item.Key));
            return Convert.ToInt32(Math.Round(price, MidpointRounding.AwayFromZero));
        }

        private int GetPriceWithoutDiscounts(string shopping_cart) {
            int price = 0;
            foreach (var item in shopping_cart) price += products[item];
            return price;
        }

    }
}
