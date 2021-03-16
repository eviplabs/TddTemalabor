using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class ComboDiscount : Discount
    {
        public int newPrice { get; set; }
        public ComboDiscount(int newPrice)
        {
            this.newPrice = newPrice;
        }
        public override double getDiscount(string shopping_cart, string items, int prices)
        {
            int maxOccurence = shopping_cart.Length;
            foreach (char item in items.ToCharArray())
            {
                if (!shopping_cart.Contains(item))
                {
                    return 0;
                }
                else
                {
                    int currentOccurence = getDiscountedOccurence(shopping_cart, item);
                    if (maxOccurence > currentOccurence)
                    {
                        maxOccurence = currentOccurence;
                    }
                }
            }
            return (prices - newPrice) * maxOccurence;
        }
        private int getDiscountedOccurence(string shopping_cart, char item)
        {
            return shopping_cart.ToCharArray().Count(c => c == item);
        }
    }
}
