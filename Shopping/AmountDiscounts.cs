using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    class AmountDiscounts
    {
        public Dictionary<char, (int amount, double percent)> Discounts { get; }

        public bool clubMembershipExclusive { get; set; }
        public AmountDiscounts()
        {
            Discounts = new Dictionary<char, (int, double)>();
        }

        public void RegisterAmountDiscount(char name, int amount, double percent, bool clubMembershipExclusive = false)
        {
            Discounts[name] = (amount, percent);
            this.clubMembershipExclusive = clubMembershipExclusive;
        }

        public void getPrice(Dictionary<char, (int,int)> ProductCount, double price, List<Product> Products) 
        {
            Dictionary<char, (int, int)> forfor = new Dictionary<char, (int, int)>(ProductCount);
            foreach (var key in forfor.Keys)
            {
                if (Discounts.ContainsKey(key) && ProductCount[key].Item2 >= Discounts[key].amount)
                {
                    Products.ForEach(p => { if (p.Name == key) p.Price = ProductCount[key].Item1 * Discounts[key].percent * key.GetPriceByProductChar(Products); });
                    ProductCount[key] = (1,ProductCount[key].Item2-Discounts[key].amount);
                }
            }
        }
    }
}
