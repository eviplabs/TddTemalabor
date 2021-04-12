using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<char, int> productPrices = new Dictionary<char, int>();
        private Dictionary<char, AmountDiscount> amountDiscounts = new Dictionary<char, AmountDiscount>();
        private Dictionary<char, CountDiscount> countDiscounts = new Dictionary<char, CountDiscount>();
        private List<ComboDiscount> comboDiscounts = new List<ComboDiscount>();
        private HashSet<char> weighBasedProducts = new HashSet<char>();
        public SuperShop superShop = new SuperShop();
        public InMemoryInventory inventory = new InMemoryInventory();
        public Dictionary<char, int> cart = new Dictionary<char, int>();


        public bool ProductRegistered(char name)
        {
            return productPrices.ContainsKey(name);
        }

        // suly alapu termeknel 1kg arat taroljuk
        public bool RegisterProduct(char name, int price, bool isWeighBased = false, int quantity=0)
        {
            if ((name < 'A' || name > 'Z') || price <= 0) return false;
            if (ProductRegistered(name)) return false;

            productPrices.Add(name, price);

            if (isWeighBased) { weighBasedProducts.Add(name); }
            else { inventory.Products.Add(name, quantity); }
            return true;
        }
        public double GetPrice(string cart)
        {
            // Megszamoljuk, hogy az egyes termekek hanyszor szerepelnek
            Dictionary<char, int> productCounts = new Dictionary<char, int>();

            double weightBasedPrice = CalculateWeightBasedPrice(cart);
            // suly alapu termekek eltavolitasa a kosarbol
            cart = Regex.Replace(cart,
                @"(['A-Z'])(['1-9']['0-9']*)",
                m => (weighBasedProducts.Contains(m.Groups[1].Value[0])) ?
                "" :
                m.Groups[1].Value + m.Groups[2].Value
                );

            // handling barcodes (CRD P012) (test: MoreProductWithOneCode)
            cart = Regex.Replace(cart, @"(['A-Z'])(['1-9']['0-9']*)", m => new String(m.Groups[1].Value[0], Int32.Parse(m.Groups[2].Value)));
            foreach (char item in cart)
            {
                if (!productPrices.ContainsKey(item)) continue;

                if (!productCounts.ContainsKey(item))
                {
                    productCounts.Add(item, 1);
                }
                else
                {
                    productCounts[item]++;
                }
            }

            inventory.RemoveProducts(cart);

            /* osszeadjuk a termekek arat a darabszamukat-, es az erre vonatkozo
            esetleges kedvezmenyeket figyelembe veve */
            List<int> prices = new List<int>();

            CheckNewMember(cart);

            prices.Add(GetAmountDiscountPrice(productCounts, superShop.IsAClubMember(cart)));

            prices.Add(getUpdatedCountDiscountPrice(productCounts, superShop.IsAClubMember(cart)));

            prices.Add(ComboDiscount(GetRelevantComboDiscount(productCounts), superShop.IsAClubMember(cart), productCounts));

            double price = weightBasedPrice + prices.Min();

            return superShop.ProccessCart(cart, price);
        }

        private double CalculateWeightBasedPrice(string cart)
        {
            double weightBasedPrice = 0;
            // a suly alapu termekek legfelejebb csak olyan akcioban
            // szereplhetnek, ami a vegosszeget erinti
            var matches = new Regex(@"(['A-Z'])(['1-9']['0-9']*)").Matches(cart);
            foreach (Match match in matches)
            {
                char product = match.Groups[1].Value[0];
                int weighInGrams = Int32.Parse(match.Groups[2].Value);
                if (weighBasedProducts.Contains(product))
                {
                    weightBasedPrice += productPrices[product] * (weighInGrams / 1000.0);
                }
            }
            return weightBasedPrice;
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
                    price += (int)(productPrices[product] * count * amountDiscounts[product].Factor);
                }
                else
                {
                    price += productPrices[product] * count;
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
                double actualItemPrice = productPrices[item];

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

                price += productPrices[product] * count;

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
                        if (productPrices.ContainsKey(character) && cart.ContainsKey(character)) { matches++; }
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
                sumPriceOfComboProducts += productPrices[item];
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
                if (!superShop.CustomerPoints.ContainsKey(userid))
                {
                    superShop.RegisterSuperShopCard(userid);
                }
            }
            return;

        }
        public void RegisterCouponDiscount(string couponCode, double value)
        {
            superShop.CouponList.Add(new CouponDiscount(couponCode, value));
        }
        public double Storno(string cart, char product)
        {
            inventory.RefreshProduct(product, inventory.GetProductQuantity(product) + 1);
            var newCart = cart.Remove(cart.IndexOf(product),1);
            return GetPrice(cart) - GetPrice(newCart);
        }

        public void AddToCart(char product)
        {
            int price = productPrices[product];
            cart.Add(product, price);
        }

        public double GetCartPrice()
        {
            double price = 0;
            foreach (var item in cart)
            {
                price += item.Value;
            }
            return price;
        }
    }
}
