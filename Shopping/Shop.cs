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
            char[] elements = name.ToCharArray();
            int price = 0;
            foreach (var item in elements)
            {
                price += products[item.ToString()];
            }
            return price;
        }

        public void RegisterAmountDiscount(string v1, int v2, double v3)
        {
            throw new NotImplementedException();
        }
    }
}
