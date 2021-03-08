using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<char, Product> products;

        public Shop() 
        {
            products = new Dictionary<char, Product>();
        }
        public void RegisterProduct(char name, int price)
        {
            products.Add(Char.ToUpper(name), new Product(name, price));
        }
        public int GetPrice(string shopping_cart) 
        {
            int price = 0;
            foreach (var item in shopping_cart)
            {
                if (item=='A')
                {
                    price += 10;
                }
                if (item == 'B')
                {
                    price += 20;
                }
                if (item == 'C' )
                {
                    price += 50;
                }
            }
            return price;
        }

        public void RegisterAmountDiscount(char name, int amount, double discount)
        {
            //TODO
        }
    }
}
