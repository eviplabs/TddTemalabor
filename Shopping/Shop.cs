using System;
using System.Collections.Generic;
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
            int price = 0;

            foreach (var item in shopping_cart)
            {
                if (products.ContainsKey(item))
                {
                    //még nem vizsgál semmit az amount-ra
                    if (discounts != null && discounts.ContainsKey(item))
                    {
                        //mivel a pricenak elvileg egésznek kell lennie
                        //TODO: majd egy teszt a kerekitésre + kerekítés megírása ha kell
                        price += Convert.ToInt32(products[item] * 0.9); 
                    }
                    else
                    {
                        price += products[item];
                    }
                }
            }
            return price;
        }
    }
}
