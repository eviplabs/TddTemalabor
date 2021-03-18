using System;
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
            bool memberShip = hasMembership(shopping_cart);
            bool superShop = WantsToPayWithSupershop(shopping_cart);
            int userID = GetUserID(shopping_cart);
            double price = GetPriceSumWithoutDiscounts(shopping_cart);

            price -= GetDiscountSum(shopping_cart);

            int endPrice = Convert.ToInt32(Math.Round(
                (memberShip) ? price * 0.9 : price, MidpointRounding.AwayFromZero));

            if (superShop && userID != 0) //átmeneti megoldás
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
            return discounts.Sum(d => d.Value.getDiscount(shopping_cart, d.Key, GetPriceSumWithoutDiscounts(d.Key)));
        }
        private bool hasMembership(string shopping_cart)
        {
            if (shopping_cart.Contains("t"))
            {
                products['t'] = 0;
                return true;
            }
            return false;
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
            return 0; //átmeneti megoldás
        }
        private bool WantsToPayWithSupershop(string shopping_cart)
        //kódduplikáció a hasMembership-el, ha ez nem változik, érdemes kiszervezni a belsejét
        {
            if (shopping_cart.Contains("p"))
            {
                products['p'] = 0;
                return true;
            }
            return false;
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
