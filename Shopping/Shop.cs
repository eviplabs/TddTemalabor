using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Shopping
{
    public class Shop
    {
        private ProductData productData = new ProductData();
        public Cart Cart;
        public SuperShop superShop = new SuperShop();

        public IInventory inventory;
        public IWeightScale weightScale;

        public AmountDiscount amountDiscount = new AmountDiscount();
        public CountDiscount countDiscount = new CountDiscount();
        public ComboDiscount comboDiscount = new ComboDiscount();

        public Shop(IInventory inventory, IWeightScale scale)
        {
            this.inventory = inventory;
            weightScale = scale;
            Cart = new Cart(productData, weightScale);
        }

        // suly alapu termeknel 1kg arat taroljuk
        public bool RegisterProduct(char name, int price, bool isWeightBased = false, int quantity = 0)
        {
            if ((name < 'A' || name > 'Z') || price <= 0) return false;
            if (ProductRegistered(name)) return false;

            productData.Prices.Add(name, price);

            if (isWeightBased) { productData.ProductsToWeigh.Add(name); }
            else { inventory.Add(name, quantity); }
            return true;
        }

        public bool RegisterAmountDiscount(char name, int amount, double factor, bool isMemberOnly = false)
        {
            if (!ProductRegistered(name)) return false;
            return amountDiscount.RegisterDiscount(name, amount, factor, isMemberOnly);
        }

        public void RegisterCountDiscount(char name, int amountToBuy, int amountToGet, bool isMemberOnly = false)
        {
            countDiscount.RegisterDiscount(name, amountToBuy, amountToGet, isMemberOnly);
        }

        //A kombó kedvezményben megadott elemek és összeg feldolgozása
        public void RegisterComboDiscount(string combo, int comboprice, bool isMemberOnly = false)
        {
            comboDiscount.RegisterDiscount(combo, comboprice, isMemberOnly);
        }

        public void RegisterCouponDiscount(string couponCode, double value)
        {
            superShop.CouponList.Add(new CouponDiscount(couponCode, value));
        }

        public double GetPrice(string cart)
        {
            // Megszamoljuk, hogy az egyes termekek hanyszor szerepelnek
            Dictionary<char, int> productCounts = new Dictionary<char, int>();

            double weightBasedPrice = CalculateWeightBasedPrice(cart);
            // suly alapu termekek eltavolitasa a kosarbol
            cart = Regex.Replace(cart,
                @"(['A-Z'])(['1-9']['0-9']*)",
                m => (productData.ProductsToWeigh.Contains(m.Groups[1].Value[0])) ?
                "" :
                m.Groups[1].Value + m.Groups[2].Value
                );


            // handling barcodes (CRD P012) (test: MoreProductWithOneCode)
            cart = Regex.Replace(cart, @"(['A-Z'])(['1-9']['0-9']*)", m => new String(m.Groups[1].Value[0], Int32.Parse(m.Groups[2].Value)));
            foreach (char item in cart)
            {
                if (!productData.Prices.ContainsKey(item)) continue;

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

            prices.Add(amountDiscount.CalculatePrice(productCounts, productData.Prices, superShop.IsAClubMember(cart)));

            prices.Add(countDiscount.CalculatePrice(productCounts, productData.Prices, superShop.IsAClubMember(cart)));

            prices.Add(comboDiscount.CalculatePrice(productCounts, productData.Prices, superShop.IsAClubMember(cart)));

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
                if (productData.ProductsToWeigh.Contains(product))
                {
                    weightBasedPrice += productData.Prices[product] * (weighInGrams / 1000.0);
                }
            }
            return weightBasedPrice;
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

        public double Storno(string cart, char product)
        {
            inventory.RefreshProduct(product, inventory.GetProductQuantity(product) + 1);
            var newCart = cart.Remove(cart.IndexOf(product), 1);
            return GetPrice(cart) - GetPrice(newCart);
        }

        public double GetCartPrice()
        {
            inventory.RemoveProducts(Cart.Receipt);
            return Cart.GetTotal();
        }

        public bool ProductRegistered(char name)
        {
            return productData.Prices.ContainsKey(name);
        }

    }
}
