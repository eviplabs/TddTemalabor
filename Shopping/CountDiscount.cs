using System.Collections.Generic;

namespace Shopping
{
    public class CountDiscount : Discount
    {
        #region Variables
        private Product dcProduct;
        private uint required;
        private uint freeItem;
        #endregion

        #region Init
        public CountDiscount(Product discountedProduct, uint required, uint freeItem, bool membershipRequired = false) : base(membershipRequired)
        {
            dcProduct = discountedProduct;
            this.required = required;
            this.freeItem = freeItem;
        }
        #endregion

        #region Calculations

        public override double getDiscount(ref Dictionary<char, uint> productsInCart, bool hasMembership)
        {
            if (CheckIfEligible(hasMembership))
            {
                return 0;
            }
            uint relevants = getRelevantItemsFromCart(productsInCart, dcProduct.name);
            if (relevants > required)
            {
                uint discountedExtras = relevants % freeItem > required ? relevants % freeItem - required : 0;
                return ((relevants / freeItem) * (freeItem - required) + discountedExtras) * dcProduct.price;
            }
            return 0;
        }
        #endregion
    }
}
