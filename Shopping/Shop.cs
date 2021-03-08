using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<char, int> products;

        public Shop() 
        {
            products = new Dictionary<char, int>();
        }
        public void RegisterProduct(char name, int price)
        {
            products.Add(Char.ToUpper(name), price);
        }
        public int GetPrice(string shopping_cart) 
        {
            int price = 0;
            foreach (var item in shopping_cart)
            {
                if (products.ContainsKey(item))
                {
                    price += products[item];
                }
            }
            return price;
        }

        public void RegisterAmountDiscount(char name, int amount, double discount)
        {
            //TODO
        }
    }
}
