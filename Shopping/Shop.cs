using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<string, int> products = new Dictionary<string, int>();
        public void RegisterProduct(string name, int price)
        {
            products.Add(name, price);
        }
        public int GetPrice(string name)
        {
            // Megszamoljuk, hogy az egyes termekek hanyszor szereplnek
            Dictionary<string, int> productCounts = new Dictionary<string, int>();
            foreach (char item in name)
            {
                if (!productCounts.ContainsKey(item.ToString()))
                {
                    productCounts.Add(item.ToString(), 1);
                }
                else
                {
                    productCounts[item.ToString()]++;
                }
            }

            /* osszeadjuk a termekek arat a darabszamukat-, es az erre vonatkozo
            esetleges kedvezmenyeket figyelembe veve */
            int price = 0;
            foreach ((string product, int count) in productCounts)
            {
                if (product == "A" && count >= 5)
                {
                    price += (int)(products["A"] * count * 0.9);
                }
                else
                {
                    price += products[product] * count;
                }
            }
            return price;
        }

        public void RegisterAmountDiscount(string v1, int v2, double v3)
        {
            //throw new NotImplementedException();
        }
    }
}
