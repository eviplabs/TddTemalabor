using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

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
        public CountDiscount(Product discountedProduct, int required, int freeItem)
        {
            dcProduct = discountedProduct;
            this.required = required;
            this.freeItem = freeItem;
        }
        #endregion

        #region Calculations

        public override double getDiscount(ref Dictionary<char, int> productsInCart, bool hasMembership)
        {
            int relevants = getRelevantItemsFromCart(productsInCart, dcProduct.name);
            if (relevants >= freeItem)
            {
                removeFromCart(ref productsInCart, relevants);
                return (relevants / freeItem) * (freeItem - required) * dcProduct.price;
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
