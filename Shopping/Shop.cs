using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<string, int> products = new Dictionary<string, int>();
        private List<AmountDiscount> amountDiscounts = new List<AmountDiscount>();
        private List<CountDiscount> countDiscounts = new List<CountDiscount>();
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
            price = getUpdatedCountDiscountPrice(name, price);
            return price;
        }

        public void RegisterAmountDiscount(string name, int amount, double factor)
        {
            amountDiscounts.Add(new AmountDiscount(name,amount,factor));
        }

        public void RegisterCountDiscount(string name, int amountToBuy, int amountToGet)
        {
            countDiscounts.Add(new CountDiscount(name, amountToBuy, amountToGet));
        }
        private int getUpdatedCountDiscountPrice(string cart, int price)
        {
            // Összeszedi a különböző elemeket, és azoknak a számát
            Dictionary<string, int> itemsAndCounts = new Dictionary<string, int>();
            foreach (char itemchar in cart)
            {
                string item = itemchar.ToString();
                if (itemsAndCounts.ContainsKey(item))
                {
                    itemsAndCounts[item]++;
                }
                else
                {
                    itemsAndCounts.Add(item, 1);
                }
            }
            // Az egyes termékeknek ha van CountDiscount akciója, akkor elosztja maradéktalanul a termékek
            // számát a m-el (az n-t fizet m-t vihetből), és kivonja a termék árát az eredmény szorzatával az egészből
            foreach(KeyValuePair<string,int> kvp in itemsAndCounts)
            {
                // Ha nincs ilyen akció átugorja az iterációt
                if (countDiscounts.Where(r => r.ProductName == kvp.Key).Count() == 0)
                {
                    continue;
                }
                CountDiscount countDiscount = countDiscounts.Where(r => r.ProductName == kvp.Key).First<CountDiscount>();
                // Ha van amountDiscount, akkor az is kell az új ár kiszámolásához
                double actualItemPrice = products[kvp.Key];
                if (amountDiscounts.Where(r => r.ProductName == kvp.Key).Count() != 0)
                {
                    actualItemPrice *= amountDiscounts.Where(r => r.ProductName == kvp.Key).First<AmountDiscount>().Factor;
                }
                price -= (int)(actualItemPrice / (double)(countDiscount.Get - countDiscount.Buy)); //példányosított egyet fizet egyet vihet akció esetén nullával osztás fordul elő
            }
            // Ez a megoldás nem veszi figyelembe, ha később lesz több amountdiscount/countdiscount ugyan azon a terméken
            return price;
        }
        public void RegisterComboDiscount(string combo, int comboprice)
        {
            throw new NotImplementedException();
            // Következő játékos Implementálandó része
        }
    }
}
