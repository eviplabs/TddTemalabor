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
        public Dictionary<char, (int Actual, int ToCountBy)> ProductCount;
        private AmountDiscountCalculator amountDiscountCalculator;
        private CountDiscountsCalculator countDiscountsCalculator;
        private ComboDiscountCalculator comboDiscountCalculator;
        private SupershopPointsCalculator supershopPointsCalculator;
        private CouponCalculator couponCalculator;
        private Inventory Inventory ;

        private bool SuperShopPointUsedToPay = false;
        private bool ClubMember = false;
        private int id = 0;

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
            ResetFields();

            double price = 0;

            name = NameChecks(name);

            ProductCount = name.ConvertStringToDictionary();

            amountDiscountCalculator.ApplyDiscount(ProductCount, Products);
            countDiscountsCalculator.ApplyDiscount(ProductCount, ClubMember);

            name = ProductCount.RebuildName();

            price = comboDiscountCalculator.ApplyDiscount(name, ClubMember, price, Products);

            price = SumPrice(price);

            supershopPointsCalculator.AddSupershopPoint(id, price);

            price = couponCalculator.ActivateCoupon(price);

            return FinalPrice(price, SuperShopPointUsedToPay, ClubMember); ;
        }

        private double SumPrice(double price) 
        {
            foreach (var key in ProductCount.Keys)
            {

                if (Inventory != null)
                {
                    if (Inventory.products[key] >= ProductCount[key].Actual)
                    {
                        Inventory.SetQuanity(key, Inventory.products[key] - ProductCount[key].Actual);
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


        private string CheckIfSuperPointsUsed(string name)
        {
            if (name.Contains("p"))
            {
                SuperShopPointUsedToPay = true;
                name = name.Replace("p", "");
            }

            return name;
        }

        private string CheckIfClubMember(string name)
        {
            if (name.Contains("t"))
            {
                ClubMember = true;
                name = name.Replace("t", "");
            }

            return name;
        }

        private string CheckIfUserIdUsed(string name)
        {
            Match customer = Regex.Match(name, @"[v]([\d]+)");
            if (customer.Success)
            {
                ClubMember = true;
                name = name.Replace(customer.Value, "");
                id = customer.Groups[1].Value.ToInt();
            }

            return name;
        }



        private string CheckIfCouponUsed(string name)
        {
            Match couponmatch = Regex.Match(name, @"[k]([\d]+)");
            if (couponmatch.Success)
            {
                name = name.Replace(couponmatch.Value, "");
                couponCalculator.setActiveCoupon(couponmatch.Groups[1].Value.ToInt());
            }

            return name;
        }


        private string NameChecks(string name)
        {
            name = CheckIfSuperPointsUsed(name);
            name = CheckIfClubMember(name);
            name = CheckIfUserIdUsed(name);
            name = CheckIfCouponUsed(name);
            name = BarcodeHandler(name);

            return name;
        }

        private void ResetFields()
        {
            SuperShopPointUsedToPay = false;
            ClubMember = false;
            id = 0;
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
