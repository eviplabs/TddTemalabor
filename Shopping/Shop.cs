﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        #region Variables
        private Dictionary<char, int> products;
        private Dictionary<string, Discount> discounts;
        //superShopPoints: Key => UserID, value => az adott userID hoz tartozó árkedvezmény
        //Hibát dob, ha egyböl fizetni szeretnénk korábban nem használt UserID-vel!!!
        private Dictionary<int, int> superShopPoints;

        private char membershipKey = 't';
        private char superShopPaymentKey = 'p';
        #endregion

        #region Init
        public Shop()
        {
            products = new Dictionary<char, int>();
            discounts = new Dictionary<string, Discount>();
            superShopPoints = new Dictionary<int, int>();
        }
        #endregion

        #region Registration
        public void RegisterProduct(char name, int price)
        {
            products.Add(Char.ToUpper(name), price);
        }
        public void RegisterAmountDiscount(string name, int amount, double discount)
        {
            discounts.Add(name.ToUpper(), new AmountDiscount(amount, discount));
        }
        public void RegisterCountDiscount(string name, int required, int freeItem)
        {
            discounts.Add(name.ToUpper(), new CountDiscount(required, freeItem));
        }
        public void RegisterComboDiscount(string name, int newPrice, bool membership = false)
        {
            discounts.Add(name.ToUpper(), new ComboDiscount(newPrice, membership));
        }
        private void RegisterSuperShopPoints(int userID, int fullPrice)
        {
            int priceToRegister = Convert.ToInt32(Math.Round(fullPrice * 0.01, MidpointRounding.AwayFromZero));
            if (superShopPoints.ContainsKey(userID))
            {
                superShopPoints[userID] += priceToRegister;
            }
            else
            {
                superShopPoints.Add(userID, priceToRegister);
            }
        }
        #endregion

        #region Calculations
        public int GetPrice(string shopping_cart)
        {
            bool memberShip = shopping_cart.hasKeyword(membershipKey);
            bool superShop = shopping_cart.hasKeyword(superShopPaymentKey);

            if (memberShip)
            {
                products[membershipKey] = 0;
            }
            if (superShop)
            {
                products[superShopPaymentKey] = 0;
            }


            int userID = GetUserID(shopping_cart);
            double price = GetPriceSumWithoutDiscounts(shopping_cart);

            price -= GetDiscountSum(shopping_cart);

            int endPrice = Convert.ToInt32(Math.Round(
                (memberShip) ? price * 0.9 : price, MidpointRounding.AwayFromZero));

            if (superShop && userID != 0)
            {
                if (superShopPoints[userID] > endPrice)
                {
                    while (endPrice > 0)
                    {
                        endPrice--;
                        superShopPoints[userID]--;
                    }

                }
                else
                {
                    endPrice -= superShopPoints[userID];
                    superShopPoints[userID] = 0;
                }
            }
            else if (userID != 0)
            {
                RegisterSuperShopPoints(userID, endPrice);
            }

            return endPrice;
        }
        private int GetPriceSumWithoutDiscounts(string shopping_cart)
        {
            return shopping_cart.Sum(i => products[i]);
        }
        private double GetDiscountSum(string shopping_cart)
        {
            double sumOfDiscounts = 0;
            (string, Discount) selectedComboDiscount = ("", null);
            foreach (var item in discounts)
            {
                if (item.Value.GetType().Equals(typeof(ComboDiscount)))
                {
                    if (selectedComboDiscount.Item1.Equals("") ||
                        selectedComboDiscount.Item1.Length < item.Key.Length ||
                        selectedComboDiscount.Item1.Length == item.Key.Length && String.Compare(selectedComboDiscount.Item1, item.Key) > 0)
                    {
                        selectedComboDiscount = (item.Key, item.Value);
                    }
                }
                else
                {
                    sumOfDiscounts += item.Value.getDiscount(shopping_cart, item.Key, GetPriceSumWithoutDiscounts(item.Key));
                }
            }
            return selectedComboDiscount.Item1.Equals("") ? sumOfDiscounts : sumOfDiscounts + selectedComboDiscount.Item2.getDiscount(shopping_cart, selectedComboDiscount.Item1, GetPriceSumWithoutDiscounts(selectedComboDiscount.Item1));

        }
        private int GetUserID(string shopping_cart)
        {
            foreach (char c in shopping_cart)
            {
                if (char.IsDigit(c))
                {
                    products[c] = 0;
                    return (int)Char.GetNumericValue(c);
                }
            }
            return 0;
        }
        /*private int GetSuperShopDiscount(int userID)
        {
            int points = superShopPoints[userID];
            superShopPoints[userID] = 0;
            return points;
        }*/
        #endregion
    }
}
