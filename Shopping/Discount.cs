using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Metadata.Ecma335;

namespace Shopping
{
    public abstract class Discount
    {
        #region Abstracts
        public abstract double getDiscount(ref Dictionary<char, int> productsInCart, bool hasMembership);
        protected abstract void removeFromCart(ref Dictionary<char, int> productsInCart, int occurence);
        #endregion

        #region Init
        public Discount(bool membershipNeeded = false)
        {
            this.membershipNeeded = membershipNeeded;
        }
        protected bool membershipNeeded;
        #endregion

        #region Base Functions
        
        protected bool CheckIfIsnNotEligible(bool hasMembership)
        {
            return !hasMembership && membershipNeeded;
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
