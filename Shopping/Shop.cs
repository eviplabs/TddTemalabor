using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        private List<Product> Products;
        private AmountDiscounts amountDiscounts;
        private CountDiscountsCalculator countDiscounts;
        private Dictionary<string, (int, bool)> ComboDiscounts;
        private Dictionary<int, double> SupershopPoints;
        public Shop()
        {
            Products = new List<Product>();
            amountDiscounts = new AmountDiscounts();
            countDiscounts = new CountDiscountsCalculator();
            ComboDiscounts = new Dictionary<string, (int,bool)>();
            SupershopPoints = new Dictionary<int, double>();
        }
        public void RegisterProduct(char name, int price) 
        {
            Products.Add(new Product(name, price));
        }

        public double GetPrice(string name) 
        {
            bool clubmember = false;
            double price = 0;
            if (name.Contains("t")) 
            {
                clubmember = true; 
                name = name.Replace("t", "");               
            }

            int id = 0;
            if (name.Any(char.IsDigit))
            {
                id = (int)Char.GetNumericValue(name[name.Length-1]);
                name = name.Replace(id.ToString(), "");
            }

            Dictionary<char, int> ProductCount = name.GroupBy(c => c)
                .Select(c => new { c.Key, Count = c.Count() })
                .ToDictionary(t => t.Key, t => t.Count);

            countDiscounts.getPrice(ProductCount);

            price += amountDiscounts.getPrice(ProductCount, price, Products);

            string comboString;
            int count = CountDiscount(Products, ComboDiscounts, name);

            foreach (var item in ComboDiscounts)
            {
                
                if (item.Value.Item2 == false || (item.Value.Item2 == true && clubmember))
                {
                    comboString = new string(name);
                    int combo = 0;
                    for (int i = 0; i < count; i++)
                    {
                        combo = 0;
                        foreach (var c in item.Key)
                        {
                            comboString = comboString.Remove(comboString.IndexOf(c), c.ToString().Length);
                            price -= c.GetPriceByProductChar(Products);
                            combo++;
                        }
                    }
                    if (combo == item.Key.Length)
                    {
                        price += item.Value.Item1 * count;
                    }
                }
            }

            if (SupershopPoints.Count > 0)
            {
                SupershopPoints.Add(id, GetSupershopPoints(price));
            }

            return clubmember ? price * 0.9 : price; 
        }

        public void RegisterAmountDiscount(char name, int amount, double percent)
        {
            amountDiscounts.RegisterAmountDiscount(name,amount, percent);
        }

        public void RegisterCountDiscount(char name, int count, int bonus)
        {
            countDiscounts.RegisterCountDiscount(name, count, bonus);
        }

        public void RegisterComboDiscount(string name, int newprice, bool clubMembership=false)
        {
            ComboDiscounts[name] = (newprice,clubMembership);
        }

        public double GetSupershopPoints(double price) {
            return price * 0.01;
        }

        public int CountDiscount(List<Product> products, Dictionary<string, (int,bool)> combos, string name) 
        {
            
            Dictionary<char, int> path = new Dictionary<char, int>();
            foreach (var item in products.Select(p=>p.Name))
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
