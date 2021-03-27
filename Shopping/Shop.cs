using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<char, int> products = new Dictionary<char, int>();
        private Dictionary<char, AmountDiscount> amountDiscounts = new Dictionary<char, AmountDiscount>();
        private Dictionary<char, CountDiscount> countDiscounts = new Dictionary<char, CountDiscount>();
        private Dictionary<int, int> supershopPoints = new Dictionary<int, int>(); // (userid, gyűjtött pontok) párok
        private List<ComboDiscount> comboDiscounts = new List<ComboDiscount>();
        private List<CouponDiscount> couponDiscounts = new List<CouponDiscount>();


        public bool ProductRegistered(char name)
        {
            return products.ContainsKey(name);
        }
        public bool RegisterProduct(char name, int price)
        {
            if ((name < 'A' || name > 'Z') || price <= 0) return false;
            if (ProductRegistered(name)) return false;

            products.Add(name, price);
            return true;
        }
        public double GetPrice(string cart)
        {
            // Megszamoljuk, hogy az egyes termekek hanyszor szerepelnek
            Dictionary<char, int> productCounts = new Dictionary<char, int>();
            // handling barcodes (CRD P012) (test: MoreProductWithOneCode)
            cart = Regex.Replace(cart, @"(['A-Z'])(\d+)", m => new String(m.Groups[1].Value[0], Int32.Parse(m.Groups[2].Value)));
            foreach (char item in cart)
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
            List<int> prices = new List<int>();

            CheckNewMember(cart);

            prices.Add(GetAmountDiscountPrice(productCounts, IsAClubMember(cart)));

            prices.Add(getUpdatedCountDiscountPrice(productCounts, IsAClubMember(cart)));

            prices.Add(ComboDiscount(GetRelevantComboDiscount(productCounts), IsAClubMember(cart), productCounts));

            double price = prices.Min();

            price = CouponDiscount(cart, price);

            price = GetUpdatedClubMembershipPrice(cart, price);

            checkAndAddSupershopPoints(cart, price);

            price = getSupershopAppliedPrice(cart, price);

            return price;
        }

        private double GetUpdatedClubMembershipPrice(string name, double price)
        {
            if (IsAClubMember(name))
            {
                return (int)(price * 0.9);
            }
            return price;
        }

        public bool RegisterAmountDiscount(char name, int amount, double factor, bool isMemberOnly = false)
        {
            if (amount < 2 || !ProductRegistered(name) || (factor <= 0 || factor >= 1)) return false;
            if (amountDiscounts.ContainsKey(name)) return false;

            amountDiscounts.Add(name, new AmountDiscount(amount, factor, isMemberOnly));
            return true;
        }

        public void RegisterCountDiscount(char name, int amountToBuy, int amountToGet, bool isMemberOnly = false)
        {
            countDiscounts.Add(name, new CountDiscount(amountToBuy, amountToGet, isMemberOnly));
        }
        private int GetAmountDiscountPrice(Dictionary<char, int> cart, bool isMember)
        {
            int price = 0;
            foreach ((char product, int count) in cart)
            {
                if (amountDiscounts.ContainsKey(product) && count >= amountDiscounts[product].Amount && (isMember || !amountDiscounts[product].isMemberOnly))
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
        private int getUpdatedCountDiscountPrice(Dictionary<char, int> cart, bool isMember)
        {
            int price = getPriceWithoutDiscount(cart);
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

        private int getPriceWithoutDiscount(Dictionary<char, int> cart)
        {
            int price = 0;
            foreach ((char product, int count) in cart)
            {

                price += products[product] * count;

            }
            return price;
        }

        //A kombó kedvezményben megadott elemek és összeg feldolgozása
        public void RegisterComboDiscount(string combo, int comboprice, bool isMemberOnly = false)
        {
            comboDiscounts.Add(new ComboDiscount(combo, comboprice, isMemberOnly));
        }
        //Megfelelő kombós kedvezmény visszaadása az aktuális cart alapján.
        public ComboDiscount GetRelevantComboDiscount(Dictionary<char, int> cart)
        {
            if (comboDiscounts != null)
            {
                foreach (var combo in comboDiscounts)
                {
                    int charCount = combo.ComboProducts.Count();
                    int matches = 0;
                    foreach (char character in combo.ComboProducts)
                    {
                        if (products.ContainsKey(character) && cart.ContainsKey(character)) { matches++; }
                    }
                    if (matches == charCount) { return combo; }
                }
            }
            return null;
        }

        //Kombó kedvezmény számítása.
        public int ComboDiscount(ComboDiscount combo, bool membershipBased, Dictionary<char, int> cart)
        {
            if (combo == null || (membershipBased == false && combo.isMemberOnly))
            {
                return getPriceWithoutDiscount(cart);
            }
            int sumPriceOfComboProducts = 0;
            foreach (var item in combo.ComboProducts)
            {
                sumPriceOfComboProducts += products[item];
            }
            int price = getPriceWithoutDiscount(cart);
            return price - (sumPriceOfComboProducts - combo.ComboPrice);
        }
        private void CheckNewMember(string cart)
        {
            var result = new Regex(@"(\d+)").Match(cart);
            if (result.Success)
            {
                int userid = int.Parse(result.Value);
                if (!supershopPoints.ContainsKey(userid))
                {
                    RegisterSuperShopCard(userid);
                }
            }
            return;

        }
        public void RegisterSuperShopCard(int id)
        {
            supershopPoints.Add(id,0);
        }

        //Klubtagsag vizsgalata a kosar tartalma alapjan.
        private bool IsAClubMember(string cart)
        {
            var result = new Regex(@"^[A-Z]+[^k](\d+)|^(\d+)").Match(cart);
            result = new Regex(@"(\d+)").Match(result.ToString());
            if (result.Success)
            {
                int userid = int.Parse(result.Value);
                if (supershopPoints.ContainsKey(userid)) { return true; }
            }
            return false;
        }
        //Supershop pontok gyűjtése
        private void checkAndAddSupershopPoints(string name, double price)
        {
            var result = new Regex(@"(\d+)").Match(name);
            if (result.Success)
            {
                int userid = int.Parse(result.Value);
                if (!supershopPoints.ContainsKey(userid))
                {
                    //Itt tul keso hozzaadni.
                }
                supershopPoints[userid] += Convert.ToInt32(price) / 100;
            }
        }
        private double getSupershopAppliedPrice(string name, double price)
        {
            var result = new Regex(@"(\d+)").Match(name);

            if (!name.Contains('p'))
            {
                return price; //A vevő nem szeretne szupershoppal fizetni 
            }
            else
            {
                int userid = int.Parse(result.Value);
                if (supershopPoints[userid] > price)
                {
                    supershopPoints[userid] -= Convert.ToInt32(price);
                    return 0; //A vevőnek több pontja van, mint a kosár ára, ezért csak a pontjaival fizet
                }
                price -= supershopPoints[userid]; //Ha van a vevőnek pontja levonja, ha nincs akkor nem csinál semmit.
                supershopPoints[userid] = 0; //Volt a vevőnek pontja, nullázza, ha nem akkor nem csinál semmit.
                return price; //A vevő pontjaival frissített ár
            }
        }

        public void RegisterCouponDiscount(string couponCode, double value)
        {
            couponDiscounts.Add(new CouponDiscount(couponCode, value));
        }

        public double CouponDiscount(string cart, double price)
        {
            int index = cart.IndexOf('k') + 1;
            string couponCode = cart.Substring(index);
            foreach (var coupon in couponDiscounts)
            {
                if (coupon.couponCode.Equals(couponCode))
                {
                    price *= coupon.value;
                    couponDiscounts.Remove(coupon);
                    return price;
                }
            }

            return price;
        }
    }
}
