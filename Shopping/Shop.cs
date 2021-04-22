using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        private CouponCalculator CouponCalculator;
        private Inventory Inventory;
        public Shop()
        {
            Products = new List<Product>();
            amountDiscounts = new AmountDiscounts();
            countDiscountsCalculator = new CountDiscountsCalculator();
            comboDiscountCalculator = new ComboDiscountCalculator();
            supershopPointsCalculator = new SupershopPointsCalculator();
            SupershopPoints = new List<SuperShopPoint>();
            CouponCalculator = new CouponCalculator();
        }
        public Shop(Inventory inventory)
        {
            Products = new List<Product>();
            amountDiscounts = new AmountDiscounts();
            countDiscountsCalculator = new CountDiscountsCalculator();
            comboDiscountCalculator = new ComboDiscountCalculator();
            supershopPointsCalculator = new SupershopPointsCalculator();
            SupershopPoints = new List<SuperShopPoint>();
            CouponCalculator = new CouponCalculator();
            Inventory = inventory;
        }
        public void RegisterProduct(char name, double price, bool weighted = false) 
        {
            Products.Add(new Product(name, price, weighted));
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
            Match customer = Regex.Match(name, @"[v]([\d]+)");
            if (customer.Success)
            {
                clubmember = true;
                name = name.Replace(customer.Value, "");
                id = customer.Groups[1].Value.ToInt();
            }
            Match couponmatch = Regex.Match(name, @"[k]([\d]+)");
            if (couponmatch.Success)
            {
                name = name.Replace(couponmatch.Value, "");
                CouponCalculator.setActiveCoupon(couponmatch.Groups[1].Value.ToInt());
            }
            name = BarcodeHandler(name);
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
                if (Inventory!=null) {
                    Inventory.SetQuanity(key, Inventory.products[key] - ProductCount[key].Item1);
                }
                
            }

            if (SupershopPoints.Count > 0)
            {
                supershopPointsCalculator.AddSupershopPoint(id, price);
            }
            price =CouponCalculator.ActivateCoupon(price);

            return clubmember ? price * 0.9 - (supershoppointusedtopay ? supershopPointsCalculator.GetSupershopPoints(price): 0)
                : price - (supershoppointusedtopay ? supershopPointsCalculator.GetSupershopPoints(price) : 0); 
        }

        public void RegisterAmountDiscount(char name, int amount, double percent, bool clubMembershipExclusive = false)
        {
            amountDiscounts.RegisterAmountDiscount(name, amount, percent, clubMembershipExclusive);
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

        public void RegisterCoupon(string id, double Discount) {
            CouponCalculator.registerCoupon(Convert.ToInt32(id), Discount);
        }

        private string BarcodeHandler(string name)
        {
            MatchCollection barcodeMatches = Regex.Matches(name, @"(\w)([\d]+)");
            foreach (Match match in barcodeMatches)
            {
                GroupCollection groups = match.Groups;
                string replace = "";
                for (int i = 0; i < Convert.ToInt32(groups[2].Value); i++)
                {
                    replace += groups[1].Value;
                }
                name = name.Replace(match.ToString(), replace);
            }
            return name;
        }
    }
}
