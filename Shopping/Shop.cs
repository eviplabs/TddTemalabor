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
        private ComboDiscountCalculator comboDiscountCalculator;
        private Dictionary<int, double> SupershopPoints;
        public Shop()
        {
            Products = new List<Product>();
            amountDiscounts = new AmountDiscounts();
            countDiscounts = new CountDiscountsCalculator();
            comboDiscountCalculator = new ComboDiscountCalculator();
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

            price = amountDiscounts.getPrice(ProductCount, price, Products);

            price= comboDiscountCalculator.getPrice(name, clubmember, price, Products);

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
        public void RegisterComboDiscount(string name, int newprice, bool clubMembership = false)
        {
            comboDiscountCalculator.RegisterComboDiscount(name,newprice, clubMembership);
        }

        public double GetSupershopPoints(double price) {
            return price * 0.01;
        }
    }
}
