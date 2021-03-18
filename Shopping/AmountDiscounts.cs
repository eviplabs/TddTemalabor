using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class AmountDiscounts
    {
        private Dictionary<char, (int, double)> Discounts;
        public AmountDiscounts()
        {
            Discounts = new Dictionary<char, (int, double)>();
        }

        public void RegisterAmountDiscount(char name, int amount, double percent)
        {
            Discounts[name] = (amount, percent);
        }

        public double getPrice(Dictionary<char, int> ProductCount, double price, List<Product> Products) 
        {
            foreach (var key in ProductCount.Keys)
            {
                if (Discounts.ContainsKey(key) && ProductCount[key] >= Discounts[key].Item1)
                {
                    price += ProductCount[key] * Discounts[key].Item2 * key.GetPriceByProductChar(Products);
                }
                else
                {
                    price += ProductCount[key] * key.GetPriceByProductChar(Products);
                }
            }
            return price;
        }
    }
}
