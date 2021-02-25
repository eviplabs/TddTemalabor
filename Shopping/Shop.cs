using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<string, int> products = new Dictionary<string, int>();
        public void RegisterProduct(string name, int price)
        {
            products.Add(name, price);
        }
        public int GetPrice(string name)
        {
            return products[name];
        }
    }
}
