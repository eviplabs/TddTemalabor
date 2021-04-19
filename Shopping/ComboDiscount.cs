using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public class ComboDiscount : IDiscount
    {

        private List<DiscountData> comboDiscounts = new List<DiscountData>();

        public ComboDiscount()
        {
        }

        public bool RegisterDiscount(string combo, int comboprice, bool isMemberOnly)
        {
            comboDiscounts.Add(new DiscountData(combo, comboprice, isMemberOnly));
            return true;
        }

        public int CalculatePrice(Dictionary<char, int> cart, Dictionary<char, int> products, bool isMember)
        {
            DiscountData discountData = GetRelevantComboDiscount(cart, products);
            if (discountData == null || (isMember == false && discountData.isMemberOnly))
            {
                return GetPriceWithoutDiscount(cart, products);
            }
            int sumPriceOfComboProducts = 0;
            foreach (var item in discountData.ComboProducts)
            {
                sumPriceOfComboProducts += products[item];
            }
            int price = GetPriceWithoutDiscount(cart, products);
            return price - (sumPriceOfComboProducts - discountData.ComboPrice);

        }

        private DiscountData GetRelevantComboDiscount(Dictionary<char, int> cart, Dictionary<char, int> products)
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
            public List<char> ComboProducts = new List<char>();
            public int ComboPrice { get; set; }
            public bool isMemberOnly { get; set; }

            public DiscountData(string combo, int price, bool isMemberOnly)
            {
                ComboProducts = combo.ToList();
                ComboPrice = price;
                this.isMemberOnly = isMemberOnly;

            }

        }
    }
}
