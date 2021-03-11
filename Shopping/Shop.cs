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
        private Dictionary<char, Discount> discounts;
        #endregion

        public Shop() 
        {
            products = new Dictionary<char, int>();
            discounts = new Dictionary<char, Discount>();
        }

        #region Registration
        public void RegisterProduct(char name, int price)
        {
            products.Add(Char.ToUpper(name), price);
        }
        public void RegisterAmountDiscount(char name, int amount, double discount)
        {
            discounts.Add(Char.ToUpper(name), new Discount(amount, discount));
        }
        #endregion

        public int GetPrice(string shopping_cart) 
        {
            double price = 0;

            foreach (var item in shopping_cart)
            {
                if (products.ContainsKey(item))
                {
                    //még nem vizsgál semmit az amount-ra
                    if (discounts != null && discounts.ContainsKey(item) && AreEnoughEligibleItems(shopping_cart, item))
                    { 
                            price += products[item] * discounts[item].multiplier;
                    }
                    else
                    {
                        price += products[item];
                    }
                }
            }
            return Convert.ToInt32(Math.Round(price, MidpointRounding.AwayFromZero));
        }
        public bool AreEnoughEligibleItems(string shopping_cart, char item)
        {
            return (shopping_cart.ToCharArray().Count(c => c == item) >= discounts[item].amount) ? true : false;
        }
    }
}
