using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    class AmountDiscounts
    {
        public Dictionary<char, (int amount, double percent)> Discounts { get; }
        public AmountDiscounts()
        {
            Discounts = new Dictionary<char, (int, double)>();
        }

        public void RegisterAmountDiscount(char name, int amount, double percent)
        {
            Discounts[name] = (amount, percent);
        }

        public void getPrice(Dictionary<char, int> ProductCount, double price, List<Product> Products) 
        {
            Dictionary<char, int> forfor = new Dictionary<char, int>(ProductCount);
            foreach (var key in forfor.Keys)
            {
                if (Discounts.ContainsKey(key) && ProductCount[key] >= Discounts[key].amount)
                {
                    Products.ForEach(p => { if (p.Name == key) p.Price = ProductCount[key] * Discounts[key].percent * key.GetPriceByProductChar(Products); });
                    ProductCount[key] = 1;
                }
            }
        }
    }
}
