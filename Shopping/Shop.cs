using System;
using System.Collections.Generic;
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
            double price = 0;

            Dictionary<char, int> ProductCount = new Dictionary<char, int>();

            foreach(char c in name)
            {
                if (ProductCount.ContainsKey(c))
                {
                    ProductCount[c]++;
                }
                else
                {
                    ProductCount[c] = 1;
                }
            }


            foreach (var key in ProductCount.Keys)
            {
                if (Discounts.ContainsKey(key))
                {
                    if (ProductCount[key] >= Discounts[key].Item1)
                    {
                        price += ProductCount[key] * Discounts[key].Item2 * Products[key];
                    }
                    else
                    {
                        price += ProductCount[key] * Products[key];
                    }
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
    }
}
