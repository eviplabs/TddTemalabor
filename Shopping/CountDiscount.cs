using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class CountDiscount : IDiscount
    {

        private Dictionary<char, DiscountData> countDiscounts = new Dictionary<char, DiscountData>();

        public CountDiscount()
        {
        }

        public void RegisterDiscount(char product, int amountToBuy, int amountToGet, bool isMemberOnly)
        {
            countDiscounts.Add(product, new DiscountData(amountToBuy, amountToGet, isMemberOnly));
        }

        public int CalculatePrice(Dictionary<char, int> cart, Dictionary<char, int> products, bool isMember)
        {

            int price = GetPriceWithoutDiscount(cart, products);

            // Az egyes termékeknek ha van CountDiscount akciója, akkor elosztja maradéktalanul a termékek
            // számát a m-el (az n-t fizet m-t vihetből), és kivonja a termék árát az eredmény szorzatával az egészből
            foreach ((char item, int count) in cart)
            {
                // Ha nincs ilyen akció átugorja az iterációt
                if (!countDiscounts.ContainsKey(item))
                {
                    continue;
                }
                DiscountData countDiscount = countDiscounts[item];
                // Ha csak tagokra érvényes az akció, de a vásárló nem tag, átugorja az iterációt
                if (!isMember && countDiscounts[item].isMemberOnly)
                {
                    continue;
                }
                double actualItemPrice = products[item];

                // (count / countDiscount.Get): ennyiszer tudjuk a jelenlegi kosarunknal kihasznalni a 3-at fizet 4-et vihet tipusu akciot
                // countDiscount.Get-countDiscount.Buy: minden egyes alkalommal amikor kihasznalasra kerül, ennyi termek arat kell levonni
                price -= (int)(actualItemPrice * ((int)(count / countDiscount.Get) * (countDiscount.Get - countDiscount.Buy)));
            }
            return price;

        }

        private int GetPriceWithoutDiscount(Dictionary<char, int> cart, Dictionary<char, int> products)
        {
            int price = 0;
            foreach ((char product, int count) in cart)
            {

                price += products[product] * count;

            }
            return price;
        }

        private class DiscountData
        {
            public int Buy { get; set; }
            public int Get { get; set; } //Buy-t fizet Get-et vihet
            public bool isMemberOnly { get; set; }

            public DiscountData(int buy, int get, bool isMemberOnly)
            {
                this.Buy = buy;
                this.Get = get;
                this.isMemberOnly = isMemberOnly;
            }

        }
    }
}
