using System.Collections.Generic;

namespace Shopping
{
    public class CountDiscount : Discount
    {
        #region Variables
        private Product dcProduct;
        private int required;
        private int freeItem;
        #endregion

        #region Init
        public CountDiscount(Product discountedProduct, int required, int freeItem, bool membershipRequired = false) : base(membershipRequired)
        {
            dcProduct = discountedProduct;
            this.required = required;
            this.freeItem = freeItem;
        }
        #endregion

        #region Calculations

        public override double getDiscount(ref Dictionary<char, int> productsInCart, bool hasMembership)
        {
            if (CheckIfIsNotEligible(hasMembership))
            {
                return 0;
            }
            int relevants = getRelevantItemsFromCart(productsInCart, dcProduct.name);
            if (relevants > required)
            {
                removeFromCart(ref productsInCart, relevants);
                if ((double)relevants / freeItem % 1 == 0)
                //if the customer claimed the full discount
                {
                    return (relevants / freeItem) * (freeItem - required) * dcProduct.price;
                }
                else
                //if the customer claimed the discount only partly
                //forExample: if you pay for 2, you could take 4 but you only take 3
                {
                    int discountedExtras = relevants % freeItem > required ? relevants % freeItem - required : 0;
                    return ( (relevants / freeItem) * (freeItem - required) + discountedExtras) * dcProduct.price; // + (freeItem - relevants) * dcProduct.price;
                }
            }
            return 0;
        }

        protected override void removeFromCart(ref Dictionary<char, int> productsInCart, int occurence)
        {
            productsInCart[dcProduct.name] -= occurence;
        }
        #endregion
    }
}
