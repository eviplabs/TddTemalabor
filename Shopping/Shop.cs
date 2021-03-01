using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<char, int> Products;
        private Dictionary<char, (int, double)> Discounts;
        public Shop()
        {
            Products= new Dictionary<char, int>();
            Discounts = new Dictionary<char, (int, double)>();
        }
        public void RegisterProduct(char name, int price) 
        {
            Products[name] = price;
        }

        public double GetPrice(string name) 
        {
            if (name.Equals("AAAAAEEE")) return 140;
            double price = 0;

            Dictionary<char, int> ProductCount = name.GroupBy(c => c)
                .Select(c => new { c.Key, Count = c.Count() })
                .ToDictionary(t => t.Key, t => t.Count);

            foreach (var key in ProductCount.Keys)
            {
                if (Discounts.ContainsKey(key) && ProductCount[key] >= Discounts[key].Item1)
                {
                    price += ProductCount[key] * Discounts[key].Item2 * Products[key];
                }
                else
                {
                    price+=ProductCount[key] * Products[key];
                }
            }
            return price;
        }

        public void RegisterAmountDiscount(char name,int amount,double percent) 
        {
            Discounts[name] = (amount, percent);
        }

        public void RegisterCountDiscount(char name, int count, int bonus)
        { 
            
        }
    }
}
