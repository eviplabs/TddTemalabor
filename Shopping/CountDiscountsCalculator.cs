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

        public int CountDiscountCalculator(char ProductID, Dictionary<char, int> ProductCount)
        {
            int pc = ProductCount[ProductID];
            int a = Discounts[ProductID].count;
            int b = Discounts[ProductID].bonus;
            return pc - (pc / b) * (b - a);
        }

        public void getPrice(Dictionary<char,int> ProductCount)
        {
            Dictionary<char, int> forfor = new Dictionary<char, int>(ProductCount);

            foreach (var key in forfor.Keys)
            {
                if (Discounts.ContainsKey(key) && ProductCount[key] >= Discounts[key].bonus)
                {
                    ProductCount[key] = CountDiscountCalculator(key, ProductCount);
                }
            }
        }

    }
}
