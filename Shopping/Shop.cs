using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<string, int> products = new Dictionary<string, int>();
        private List<AmountDiscount> amountDiscounts = new List<AmountDiscount>();
        public void RegisterProduct(string name, int price)
        {
            products.Add(name, price);
        }
        public int GetPrice(string name)
        {
            // Megszamoljuk, hogy az egyes termekek hanyszor szerepelnek
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
            bool discounted = false;
            foreach ((string product, int count) in productCounts)
            {
                if (products.ContainsKey(product))
                {
                    if (amountDiscounts != null)
                    {
                        foreach (var discount in amountDiscounts)
                        {
                            if (product.Equals(discount.ProductName) && count >= discount.Amount)
                            {
                                price += (int)(products[product] * count * discount.Factor);
                                discounted = true;
                                break;
                            }
                        }
                        if (discounted == false) { price += products[product] * count; }
                        discounted = false; //Discount resetelése a következő termék számára az iterációban.
                    }
                    else
                    {
                        price += products[product] * count;
                    }
                }
            }
            return price;
        }

        public void RegisterAmountDiscount(string name, int amount, double factor)
        {
            amountDiscounts.Add(new AmountDiscount(name,amount,factor));
        }

        public void RegisterCountDiscount(string name, int amountToPay, int amountToGet)
        {
            //A következő teszt zöldítéséhez implementálandó.
        }
    }
}
