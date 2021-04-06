using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public abstract class Discount
    {
        #region Abstracts
        public abstract double getDiscount(string shopping_cart);
        public abstract double getDiscount(Dictionary<char, int> productsInCart, bool hasMembership);
        #endregion

        #region Base Functions
        protected int getRelevantItemsFromCart(string shopping_cart, char item)
        {
            return shopping_cart.ToCharArray().Count(c => c == item);
        }
        protected int getRelevantItemsFromCart(Dictionary<char, int> productsInCart, char item)
        {
            try
            {
                return productsInCart[item];
            }
            catch
            {
                return 0;
            }
        }
        #endregion
    }
}
