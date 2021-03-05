using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<char, Product> products;

        public Shop() 
        {
            products = new Dictionary<char, Product>();
        }
        public void RegisterProduct(char name, int price)
        {
            products.Add(name, new Product(name, price));
        }
        public int GetPrice(string shopping_cart) 
        {
            int price = 0;

            return price;
        }
    }
}
