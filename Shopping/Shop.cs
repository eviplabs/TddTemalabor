using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<string, int> Products;
        private Dictionary<string, (int, double)> Discounts;
        public Shop()
        {
            Products= new Dictionary<string, int>();
            Discounts = new Dictionary<string, (int, double)>();
        }
        public void RegisterProduct(string name, int price) 
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


            foreach(var key in ProductCount.Keys)
            {
                if (Discounts.ContainsKey(key.ToString()))
                {
                    if (ProductCount[key] >= Discounts[key.ToString()].Item1)
                    {
                        price += ProductCount[key] * Discounts[key.ToString()].Item2 * Products[key.ToString()];
                    }
                    else
                    {
                        price += ProductCount[key] * Products[key.ToString()];
                    }
                }
                else
                {
                    price+=ProductCount[key] * Products[key.ToString()];
                }
            }

            return price;
        }

        public void RegisterAmountDiscount(string name,int amount,double percent) 
        {
            Discounts[name] = (amount, percent);
        }
    }
}
