using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class AmountDiscount : IDiscount
    {

        private Dictionary<char, DiscountData> amountDiscounts = new Dictionary<char, DiscountData>();

        public AmountDiscount()
        {
        }

        public bool RegisterDiscount(char product, int amount, double factor, bool isMemberOnly)
        {
            if (amount < 2 || (factor <= 0 || factor >= 1) || amountDiscounts.ContainsKey(product)) return false;

            amountDiscounts.Add(product, new DiscountData(amount, factor, isMemberOnly));
            return true;
        }

        public int CalculatePrice(Dictionary<char, int> cart, Dictionary<char, int> products, bool isMember)
        {
            int price = 0;
            foreach ((char product, int count) in cart)
            {
                if (amountDiscounts.ContainsKey(product) && count >= amountDiscounts[product].Amount
                    && (isMember || !amountDiscounts[product].isMemberOnly))
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

        private class DiscountData
        {
            public DiscountData(int amount, double factor, bool isMemberOnly)
            {
                Amount = amount;
                Factor = factor;
                this.isMemberOnly = isMemberOnly;
            }

            public int Amount { get; set; }
            public double Factor { get; set; }
            public bool isMemberOnly { get; set; }
        }
    }
}
