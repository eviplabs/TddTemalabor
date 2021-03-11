using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Discount
    {
        public int amount { get; set; }
        public double multiplier { get; set; }
        public Discount(int amount, double multiplier)
        {
            this.amount = amount;
            this.multiplier = multiplier;
        }
        public bool AreEnoughEligibleItems(string shpping_cart, char item)
        {
            return (getRelevantItemsFromCart(shpping_cart, item) >= amount) ? true : false;
        }
        private int getRelevantItemsFromCart(string shopping_cart, char item)
        {
            return shopping_cart.ToCharArray().Count(c => c == item);
        }
    }
}
