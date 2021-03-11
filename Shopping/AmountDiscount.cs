using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class AmountDiscount : Discount
    {
        public int amount { get; set; }
        public double multiplier { get; set; }
        public AmountDiscount(int amount, double multiplier)
        {
            this.amount = amount;
            this.multiplier = multiplier;
        }
        public override double getDiscount(string shpping_cart, char item, int price)
        {
            return (getRelevantItemsFromCart(shpping_cart, item) >= amount) ? getRelevantItemsFromCart(shpping_cart, item) * price * (1-multiplier) : 0;
        }
    }
}
