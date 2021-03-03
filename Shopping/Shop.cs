using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<char, int> products = new Dictionary<char, int>();
        private List<AmountDiscount> amountDiscounts = new List<AmountDiscount>();
        private List<CountDiscount> countDiscounts = new List<CountDiscount>();
        private ComboDiscount comboDiscount = new ComboDiscount();
        public void RegisterProduct(char name, int price)
        {
            products.Add(name, price);
        }
        public int GetPrice(string name)
        {
            // Megszamoljuk, hogy az egyes termekek hanyszor szerepelnek
            Dictionary<char, int> productCounts = new Dictionary<char, int>();
            foreach (char item in name)
            {
                if (!products.ContainsKey(item)) continue;

                if (!productCounts.ContainsKey(item))
                {
                    productCounts.Add(item, 1);
                }
                else
                {
                    productCounts[item]++;
                }
            }

            /* osszeadjuk a termekek arat a darabszamukat-, es az erre vonatkozo
            esetleges kedvezmenyeket figyelembe veve */
            int price = 0;
            bool discounted = false;
            foreach ((char product, int count) in productCounts)
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
            price = getUpdatedCountDiscountPrice(name, price);
            price -= ComboDiscount(comboDiscount);
            return price;
        }

        public void RegisterAmountDiscount(char name, int amount, double factor)
        {
            amountDiscounts.Add(new AmountDiscount(name,amount,factor));
        }

        public void RegisterCountDiscount(char name, int amountToBuy, int amountToGet)
        {
            countDiscounts.Add(new CountDiscount(name, amountToBuy, amountToGet));
        }

        private int getUpdatedCountDiscountPrice(string cart, int price)
        {
            // Összeszedi a különböző elemeket, és azoknak a számát
            Dictionary<char, int> itemsAndCounts = new Dictionary<char, int>();
            foreach (char item in cart)
            {
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
            foreach((char item, int count) in itemsAndCounts)
            {
                // Ha nincs ilyen akció átugorja az iterációt
                if (countDiscounts.Where(r => r.ProductName == item).Count() == 0)
                {
                    continue;
                }
                CountDiscount countDiscount = countDiscounts.Where(r => r.ProductName == item).First<CountDiscount>();
                // Ha van amountDiscount, akkor az is kell az új ár kiszámolásához
                double actualItemPrice = products[item];
                if (amountDiscounts.Where(r => r.ProductName == item).Count() != 0)
                {
                    actualItemPrice *= amountDiscounts.Where(r => r.ProductName == item).First<AmountDiscount>().Factor;
                }
                price -= (int)(actualItemPrice / (double)(countDiscount.Get - countDiscount.Buy)); //példányosított egyet fizet egyet vihet akció esetén nullával osztás fordul elő
            }
            // Ez a megoldás nem veszi figyelembe, ha később lesz több amountdiscount/countdiscount ugyan azon a terméken
            return price;
        }
        //A kombó kedvezményben megadott elemek és összeg feldolgozása
        public void RegisterComboDiscount(string combo, int comboprice)
        {
            char[] comboItems = combo.ToCharArray();

            foreach (var item in comboItems)
            {
                comboDiscount.AddItem(item);
            }
            comboDiscount.comboDiscount = comboprice;
        }
        //A feldolgozott kombó kedvezmény vizsgálata, hogy érvényes e a kosárra.
        public int ComboDiscount(ComboDiscount combo)
        {
            int sumPriceOfComboProducts = 0;
            foreach (var item in combo.comboProducts)
            {
                if (!products.ContainsKey(item))
                {
                    return 0;
                }
                else
                {
                    sumPriceOfComboProducts += products[item];
                }
            }
            return sumPriceOfComboProducts - comboDiscount.comboDiscount;
        }

        //Következő feldadat
        public void RegisterClubMembership(string v)
        {
            throw new NotImplementedException();
        }
    }
}
