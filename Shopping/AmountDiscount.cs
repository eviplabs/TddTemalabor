using System.Collections.Generic;

namespace Shopping
{
    public class AmountDiscount : Discount
    {
        #region Variables
        private Product dcProduct;
        private uint amount;
        private double multiplier;
        #endregion

        #region Init
        public AmountDiscount(Product discountedProduct, uint amount, double multiplier, bool membershipRequired = false) :base(membershipRequired)
        {
            dcProduct = discountedProduct;
            this.amount = amount;
            this.multiplier = multiplier;
        }
        #endregion

        #region Calculations

        public override double getDiscount(ref Dictionary<char, uint> productsInCart, bool hasMembership)
        {
            if (CheckIfIsNotEligible(hasMembership))
            {
                return 0;
            }
            uint relevants = getRelevantItemsFromCart(productsInCart, dcProduct.name);
            if(relevants >= amount)
            {
                return relevants * dcProduct.price * (1 - multiplier);
            }
            return 0;
        }
        #endregion
    }
}
