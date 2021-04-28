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
        public Dictionary<char, (int actual, int ToCountBy)> ProductCount;
        private AmountDiscountCalculator amountDiscountCalculator;
        private CountDiscountsCalculator countDiscountsCalculator;
        private ComboDiscountCalculator comboDiscountCalculator;
        private SupershopPointsCalculator supershopPointsCalculator;
        private CouponCalculator couponCalculator;
        private Inventory Inventory ;
        public Shop()
        {
            Init();
        }
        public Shop(Inventory Inventory)
        {
            Init();
            this.Inventory = Inventory;
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
                couponCalculator.setActiveCoupon(couponmatch.Groups[1].Value.ToInt());
            }

            name = BarcodeHandler(name);

            ProductCount = name.ConvertStringToDictionary();

            amountDiscountCalculator.ApplyDiscount(ProductCount, price, Products);
            countDiscountsCalculator.ApplyDiscount(ProductCount, clubmember);

            name = ProductCount.RebuildName();

            price = comboDiscountCalculator.ApplyDiscount(name, clubmember, price, Products);

            price = SumPrice(price);

            supershopPointsCalculator.AddSupershopPoint(id, price);

            price = couponCalculator.ActivateCoupon(price);

            return FinalPrice(price, supershoppointusedtopay, clubmember);
        }

        private double SumPrice(double price) 
        {
            foreach (var key in ProductCount.Keys)
            {

                if (Inventory != null)
                {
                    if (Inventory.products[key] >= ProductCount[key].actual)
                    {
                        Inventory.SetQuanity(key, Inventory.products[key] - ProductCount[key].actual);
                        price += ProductCount[key].Item1 * key.GetPriceByProductChar(Products);
                    }
                }
                else
                {
                    price += ProductCount[key].Item1 * key.GetPriceByProductChar(Products);
                }
            }

            return price;
        }

        private double FinalPrice(double price, bool supershoppointusedtopay, bool clubmember) 
        {
            double supershopPoints = (supershoppointusedtopay ? supershopPointsCalculator.GetSupershopPoints(price) : 0);
            return clubmember ? price * 0.9 - supershopPoints : price - supershopPoints;
        }

        private string BarcodeHandler(string name)
        {
            MatchCollection barcodeMatches = Regex.Matches(name, @"(\w)([\d]+)");
            foreach (Match match in barcodeMatches)
            {
                GroupCollection groups = match.Groups;
                string replace = "";
                for (int i = 0; i < groups[2].Value.ToInt(); i++)
                {
                    replace += groups[1].Value;
                }
                name = name.Replace(match.ToString(), replace);
            }
            return name;
        }

        public double ReturnItem(string product)
        {
            char c = Convert.ToChar(product);
            Inventory.SetQuanity(c, 1);
            double count = ProductCount[c].Item1 - 1;
            double price = Products.First(p => p.Name.Equals(c)).Price;
            return count * price;
        }

        private void Init()
        {
            Products = new List<Product>();
            ProductCount = new Dictionary<char, (int, int)>();
            amountDiscountCalculator = new AmountDiscountCalculator();
            countDiscountsCalculator = new CountDiscountsCalculator();
            comboDiscountCalculator = new ComboDiscountCalculator();
            supershopPointsCalculator = new SupershopPointsCalculator();
            couponCalculator = new CouponCalculator();
        }

        public void RegisterAmountDiscount(char name, int amount, double percent, bool clubMembershipExclusive = false)
        {
            amountDiscountCalculator.RegisterAmountDiscount(name, amount, percent, clubMembershipExclusive);
        }

        public void RegisterCountDiscount(char name, int count, int bonus, bool isDiscountclubMembershipExclusive = false)
        {
            countDiscountsCalculator.RegisterCountDiscount(name, count, bonus, isDiscountclubMembershipExclusive);
        }
        public void RegisterComboDiscount(string name, int newprice, bool clubMembership = false)
        {
            comboDiscountCalculator.RegisterComboDiscount(name, newprice, clubMembership);
        }

        public void RegisterCoupon(string id, double Discount)
        {
            couponCalculator.registerCoupon(Convert.ToInt32(id), Discount);
        }

        public double GetSupershopPoints(double price) 
        {
            return supershopPointsCalculator.GetSupershopPoints(price);
        }
    }
}
