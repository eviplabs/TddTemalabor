using System;
using System.Collections;
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
        public override double getDiscount(string shpping_cart, string items, int prices)
        {
            foreach (char c in items.ToCharArray())
            {
                if (!shpping_cart.Contains(c))
                {
                    return 0;
                }
            }
            return prices - newPrice;
        }
    }
}
