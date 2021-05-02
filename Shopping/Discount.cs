using System.Collections.Generic;

namespace Shopping
{
    public abstract class Discount
    {
        protected bool membershipRequired;
        #region Abstracts
        public abstract double getDiscount(ref Dictionary<char, int> productsInCart, bool hasMembership);
        #endregion

        #region Init
        public Discount(bool membershipRequired = false)
        {
            this.membershipRequired = membershipRequired;
        }
        #endregion

        #region Base Functions
        
        protected bool CheckIfIsNotEligible(bool hasMembership)
        {
            return !hasMembership && membershipRequired;
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
