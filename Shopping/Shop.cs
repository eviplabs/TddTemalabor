﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        public List<Product> Products;
        private AmountDiscounts amountDiscounts;
        private CountDiscountsCalculator countDiscountsCalculator;
        private ComboDiscountCalculator comboDiscountCalculator;
        private SupershopPointsCalculator supershopPointsCalculator;
        private List<SuperShopPoint> SupershopPoints;
        public Shop()
        {
            Products = new List<Product>();
            amountDiscounts = new AmountDiscounts();
            countDiscountsCalculator = new CountDiscountsCalculator();
            comboDiscountCalculator = new ComboDiscountCalculator();
            supershopPointsCalculator = new SupershopPointsCalculator();
            SupershopPoints = new List<SuperShopPoint>();
        }
        public void RegisterProduct(char name, int price) 
        {
            Products.Add(new Product(name, price));
        }

        public double GetPrice(string name) 
        {
            bool supershoppointusedtopay = false;
            bool clubmember = false;
            double price = 0;
            int id = 0;
            if (name.Contains("p"))
            {
                supershoppointusedtopay = true;
                name = name.Replace("p", "");
            }
            if (name.Contains("t")) 
            {
                clubmember = true; 
                name = name.Replace("t", "");
            }
            if (name.Contains("v"))
            {
                clubmember = true;
                name = name.Replace("v", "");
            }
            if (name.Any(char.IsDigit))
            {
                string path = "";
                foreach (var c in name)
                    path = char.IsDigit(c) ? path += c : null;
                id = Convert.ToInt32(path);
                name = name.ReplaceNumbersFromName();
            }

            Dictionary<char, (int, int)> ProductCount = name.GroupBy(c => c)
                .Select(c => new { c.Key, Count = c.Count(), Remains = c.Count()})
                .ToDictionary(t => t.Key,t => (t.Count, t.Remains));
            


            if (amountDiscounts.Discounts.Count > 0) 
            {
               amountDiscounts.getPrice(ProductCount, price, Products);
            }
            if (countDiscountsCalculator.Discounts.Count > 0)
            {
                countDiscountsCalculator.getPrice(ProductCount, clubmember);
            }
            name = null;
            foreach (var a in ProductCount.Keys) {
                for(int i=0;i<ProductCount[a].Item2; i++) {
                    name += a;
                }
            }
            if (comboDiscountCalculator.ComboDiscounts.Count > 0)
            {
                price = comboDiscountCalculator.getPrice(name, clubmember, price, Products);
            }

            foreach (var key in ProductCount.Keys)
            {
                price += ProductCount[key].Item1 * key.GetPriceByProductChar(Products);
            }

            if (SupershopPoints.Count > 0)
            {
                supershopPointsCalculator.AddSupershopPoint(id, price);
            }

            return clubmember ? price * 0.9 - (supershoppointusedtopay ? supershopPointsCalculator.GetSupershopPoints(price): 0)
                : price - (supershoppointusedtopay ? supershopPointsCalculator.GetSupershopPoints(price) : 0); 
        }

        public void RegisterAmountDiscount(char name, int amount, double percent)
        {
            amountDiscounts.RegisterAmountDiscount(name, amount, percent);
        }

        public void RegisterCountDiscount(char name, int count, int bonus, bool isDiscountclubMembershipExclusive = false)
        {
            countDiscountsCalculator.RegisterCountDiscount(name, count, bonus, isDiscountclubMembershipExclusive);
        }
        public void RegisterComboDiscount(string name, int newprice, bool clubMembership = false)
        {
            comboDiscountCalculator.RegisterComboDiscount(name, newprice, clubMembership);
        }

        public double GetSupershopPoints(double price) {
            return supershopPointsCalculator.GetSupershopPoints(price);
        }
    }
}
