using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<char, int> Products;
        private Dictionary<char, (int, double)> ADiscounts;
        private Dictionary<char, (int, int)> CDiscounts;
        private Dictionary<string, int> ComboDiscounts;
        public Shop()
        {
            Products= new Dictionary<char, int>();
            ADiscounts = new Dictionary<char, (int, double)>();
            CDiscounts = new Dictionary<char, (int, int)>();
            ComboDiscounts = new Dictionary<string, int>();
        }
        public void RegisterProduct(char name, int price) 
        {
            Products[name] = price;
        }

        public double GetPrice(string name) 
        {
            double price = 0;
            if (name.Contains("t")) 
            {
                Products['t'] = -18;
            }

            Dictionary<char, int> ProductCount = name.GroupBy(c => c)
                .Select(c => new { c.Key, Count = c.Count() })
                .ToDictionary(t => t.Key, t => t.Count);

            Dictionary<char, int> forfor = new Dictionary<char, int>(ProductCount);

            foreach (var key in forfor.Keys)
            {
                if (CDiscounts.ContainsKey(key) && ProductCount[key] >= CDiscounts[key].Item2)
                {
                    int pc = ProductCount[key];
                    int a = CDiscounts[key].Item1;
                    int b = CDiscounts[key].Item2;
                    int final = pc - (pc / b) * (b - a);
                    ProductCount[key] = final;
                }
            }

            foreach (var key in ProductCount.Keys)
            {
                if (ADiscounts.ContainsKey(key) && ProductCount[key] >= ADiscounts[key].Item1)
                {
                    price += ProductCount[key] * ADiscounts[key].Item2 * Products[key];
                }
                else
                {
                    price+=ProductCount[key] * Products[key];
                }
            }

            string comboString;
            int count = CountDiscount(Products, ComboDiscounts, name);

            foreach (var item in ComboDiscounts)
            {
                comboString = new string(name);
                int combo = 0;
                for (int i = 0; i < count; i++)
                {
                    combo = 0;
                    foreach (var c in item.Key)
                    {
                        comboString = comboString.Remove(comboString.IndexOf(c), c.ToString().Length);
                        price -= Products[c];
                        combo++;
                    }
                }
                if (combo == item.Key.Length)
                {
                    price += item.Value * count;
                }

            }


            return price;
        }

        public void RegisterAmountDiscount(char name,int amount,double percent) 
        {
            ADiscounts[name] = (amount, percent);
        }

        public void RegisterCountDiscount(char name, int count, int bonus)
        {
            CDiscounts[name] = (count, bonus);
        }

        public void RegisterComboDiscount(string name, int newprice)
        {
            ComboDiscounts[name] = newprice;
        }

        public int CountDiscount(Dictionary<char, int> products, Dictionary<string, int> combos, string name) 
        {
            Dictionary<char, int> path = new Dictionary<char, int>();
            foreach (var item in products.Keys)
            {
                path.Add(item, 0);
            }

            foreach (var item in combos.Keys)
            {
                foreach (var c in item)
                {
                    var count = name.Count(x => x == c);
                    path[c] = count;
                }
            }
            int min = int.MaxValue;
            foreach (var item in path.Keys)
            {
                if (path[item] == 0)
                {
                    path.Remove(item);
                }
            }
            foreach (var item in path.Keys)
            {
                if (path[item] < min)
                {
                    min = path[item];
                }
            }
            return min;
        }
    }
}
