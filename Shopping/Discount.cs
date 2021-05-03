using System.Collections.Generic;

namespace Shopping
{
    public abstract class Discount
    {
        protected bool membershipRequired;
        #region Abstracts
        public abstract double getDiscount(ref Dictionary<char, uint> productsInCart, bool hasMembership);
        #endregion

        #region Init
        public Discount(bool membershipRequired = false)
        {
            this.membershipRequired = membershipRequired;
        }
        #endregion

        #region Base Functions
        
        protected bool CheckIfEligible(bool hasMembership)
        {
            return !((!membershipRequired) || (membershipRequired && hasMembership));
        }
        protected uint getRelevantItemsFromCart(Dictionary<char, uint> productsInCart, char item)
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
