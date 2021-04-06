using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public abstract class Discount
    {
        #region Abstracts
        public abstract double getDiscount(ref Dictionary<char, int> productsInCart, bool hasMembership);
        protected abstract void removeFromCart(ref Dictionary<char, int> productsInCart, int occurence);
        #endregion

        #region Base Functions
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
