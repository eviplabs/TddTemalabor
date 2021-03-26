using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class CountDiscountsCalculator
    {

        public Dictionary<char, CountDiscount> Discounts { get; }

        public CountDiscountsCalculator()
        {
            Discounts = new Dictionary<char, CountDiscount>();
        }

        public void RegisterCountDiscount(char name, int count, int bonus)
        {
            Discounts[name] = new CountDiscount(count, bonus);
        }

        public (int,int) CountDiscountCalculator(char ProductID, Dictionary<char, (int, int)> ProductCount)
        {
            int pc = ProductCount[ProductID].Item1;
            int a = Discounts[ProductID].count;
            int b = Discounts[ProductID].bonus;
            
            return (pc - (pc / b) * (b - a),pc-b);
        }

        public void getPrice(Dictionary<char, (int, int)> ProductCount)
        {
            Dictionary<char, (int, int)> forfor = new Dictionary<char, (int, int)>(ProductCount);

            foreach (var key in forfor.Keys)
            {
                if (Discounts.ContainsKey(key) && ProductCount[key].Item2 >= Discounts[key].bonus)
                {
                    ProductCount[key] = CountDiscountCalculator(key, ProductCount);
                }
            }
        }
    }
}
