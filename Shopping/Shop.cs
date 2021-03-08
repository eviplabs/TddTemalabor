using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<char, int> products = new Dictionary<char, int>();
        private Dictionary<char, AmountDiscount> amountDiscounts = new Dictionary<char, AmountDiscount>();
        private Dictionary<char, CountDiscount> countDiscounts = new Dictionary<char, CountDiscount>();
        private List<ComboDiscount> comboDiscounts = new List<ComboDiscount>();
        public void RegisterProduct(char name, int price)
        {
            products.Add(name, price);
        }
        public int GetPrice(string name)
        {
            // Megszamoljuk, hogy az egyes termekek hanyszor szerepelnek
            Dictionary<char, int> productCounts = new Dictionary<char, int>();
            bool clubMemberCart = false;
            foreach (char item in name)
            {
                if (item.Equals('t')) { clubMemberCart = true; }
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
            price = GetAmountDiscountPrice(productCounts, price);

            price = getUpdatedCountDiscountPrice(productCounts, price);

            price -= ComboDiscount(GetRelevantComboDiscount(productCounts), clubMemberCart);

            price = GetUpdatedClubMembershipPrice(name, price);

            return price;
        }

        private int GetUpdatedClubMembershipPrice(string name, int price)
        {
            if (name.Contains("t"))
            {
                return (int)(price * 0.9);
            }
            return price;
        }

        public void RegisterAmountDiscount(char name, int amount, double factor)
        {
            amountDiscounts.Add(name, new AmountDiscount(amount, factor));
        }

        public void RegisterCountDiscount(char name, int amountToBuy, int amountToGet)
        {
            countDiscounts.Add(name, new CountDiscount(amountToBuy, amountToGet));
        }
        private int GetAmountDiscountPrice(Dictionary<char,int> cart, int price)
        {
            foreach ((char product, int count) in cart)
            {
                if (amountDiscounts.ContainsKey(product) && count >= amountDiscounts[product].Amount)
                {
                    price += (int)(products[product] * count * amountDiscounts[product].Factor);
                }
                else
                {
                    price += products[product] * count;
                }
            }
            return price;
        }
        private int getUpdatedCountDiscountPrice(Dictionary<char, int> cart, int price)
        {
            // Az egyes termékeknek ha van CountDiscount akciója, akkor elosztja maradéktalanul a termékek
            // számát a m-el (az n-t fizet m-t vihetből), és kivonja a termék árát az eredmény szorzatával az egészből
            foreach ((char item, int count) in cart)
            {
                // Ha nincs ilyen akció átugorja az iterációt
                if (!countDiscounts.ContainsKey(item))
                {
                    continue;
                }
                CountDiscount countDiscount = countDiscounts[item];
                // Ha van amountDiscount, akkor az is kell az új ár kiszámolásához
                double actualItemPrice = products[item];
                if (amountDiscounts.ContainsKey(item))
                {
                    actualItemPrice *= amountDiscounts[item].Factor;
                }
                // (count / countDiscount.Get): ennyiszer tudjuk a jelenlegi kosarunknal kihasznalni a 3-at fizet 4-et vihet tipusu akciot
                // countDiscount.Get-countDiscount.Buy: minden egyes alkalommal amikor kihasznalasra kerül, ennyi termek arat kell levonni
                price -= (int)(actualItemPrice * ((int)(count / countDiscount.Get) * (countDiscount.Get - countDiscount.Buy)));
            }
            return price;
        }
        //A kombó kedvezményben megadott elemek és összeg feldolgozása
        public void RegisterComboDiscount(string combo, int comboprice, bool onlyforClubMembership)
        {
            comboDiscounts.Add(new ComboDiscount(combo, comboprice, onlyforClubMembership));
        }

        public ComboDiscount GetRelevantComboDiscount(Dictionary<char,int> cart)
        {
            if (comboDiscounts != null)
            {
                foreach( var combo in comboDiscounts)
                {
                    int charCount = combo.ComboProducts.Count();
                    int matches = 0;
                    foreach(char character in combo.ComboProducts)
                    {
                        if(products.ContainsKey(character) && cart.ContainsKey(character)) { matches++; }
                    }
                    if (matches == charCount) { return combo; }
                }
            }
            return null;
        }
        //A feldolgozott kombó kedvezmény vizsgálata, hogy érvényes e a kosárra.
        // jelenleg be van egetve
        public int ComboDiscount(ComboDiscount combo, bool membershipBased)
        {
            if (combo == null||(membershipBased==false && combo.ClubOnly)) { return 0; }
            int sumPriceOfComboProducts = 0;
            foreach (var item in combo.ComboProducts)
            {
                sumPriceOfComboProducts += products[item];
            }
            return sumPriceOfComboProducts - combo.ComboPrice;
        }

    }
}
