using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public abstract class Discount
    {
        public abstract double getDiscount(string shopping_cart, string items, int prices);
        protected int getRelevantItemsFromCart(string shopping_cart, char item)
        {
            return shopping_cart.ToCharArray().Count(c => c == item);
        }
    }
}
