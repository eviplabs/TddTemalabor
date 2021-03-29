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
        private Dictionary<string, SuperShop> superShopPoints;

        private char membershipKey = 't';
        private char superShopPaymentKey = 'p';
        #endregion

        #region Init
        public Shop()
        {
            products = new Dictionary<char, int>();
            discounts = new Dictionary<string, Discount>();
            superShopPoints = new Dictionary<string, SuperShop>();
        }
        #endregion

        #region Registration
        public void RegisterProduct(char name, int price)
        {
            products.Add(Char.ToUpper(name), price);
        }
        public void RegisterDiscount(string name, Discount dc)
        {
            discounts.Add(name, dc);
        }
        public void RegisterSuperShopCard(string userID)
        { 
            superShopPoints.Add(userID, new SuperShop());
        }
        #endregion

        #region Calculations
        public int GetPrice(string shopping_cart)
        {
            bool memberShip = shopping_cart.Contains(membershipKey);
            bool superShop = shopping_cart.Contains(superShopPaymentKey);

            if (memberShip)
            {
                products[membershipKey] = 0;
            }
            if (superShop)
            {
                products[superShopPaymentKey] = 0;
            }

            string userID = GetUserID(shopping_cart);
            double price = GetPriceSumWithoutDiscounts(shopping_cart);

            price -= GetDiscountSum(shopping_cart);

            int endPrice = Convert.ToInt32(Math.Round(
                (memberShip) ? price * 0.9 : price, MidpointRounding.AwayFromZero));

            if (superShop && userID != null)
            {
                endPrice -= (int)superShopPoints[userID].getDiscount(shopping_cart, "", endPrice);
            }
            if(userID != null)
            {
                superShopPoints[userID].addPoints(endPrice);
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
        private string GetUserID(string shopping_cart)
        {
            foreach (char c in shopping_cart)
            {
                if (char.IsDigit(c))
                {
                    products[c] = 0;
                    return c.ToString();
                }
            }
            return null;
        }
        #endregion
    }
}
