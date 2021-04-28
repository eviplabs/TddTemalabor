using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class AmountDiscountCalculator
    {
        private Dictionary<char, AmountDiscount> Discounts { get; }

        private bool clubMembershipExclusive { get; set; }
        public AmountDiscountCalculator()
        {
            Discounts = new Dictionary<char, AmountDiscount>();
        }

        public void RegisterAmountDiscount(char name, int amount, double percent, bool clubMembershipExclusive = false)
        {
            Discounts[name] = new AmountDiscount(amount, percent);
            this.clubMembershipExclusive = clubMembershipExclusive;
        }

        public void ApplyDiscount(Dictionary<char, (int, int)> ProductCount, double price, List<Product> Products)
        {
            if(Discounts.Count > 0) 
            {
                Dictionary<char, (int, int)> forfor = new Dictionary<char, (int, int)>(ProductCount);
                foreach (var key in forfor.Keys)
                {
                    if (Discounts.ContainsKey(key) && ProductCount[key].Item2 >= Discounts[key].amount)
                    {
                        Products.ForEach(p => { if (p.Name == key) p.Price = ProductCount[key].Item1 * Discounts[key].percent * key.GetPriceByProductChar(Products); });
                        ProductCount[key] = (1, ProductCount[key].Item2 - Discounts[key].amount);
                    }
                }
            }
        }
    }
}
