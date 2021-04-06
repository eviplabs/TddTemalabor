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
        public override double getDiscount(string shopping_cart)
        {
            return (getRelevantItemsFromCart(shopping_cart, dcProduct.name) / freeItem) * (freeItem - required) * dcProduct.price;
        }
        #endregion
    }
}
