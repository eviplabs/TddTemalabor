using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class AmountDiscount : Discount
    {
        #region Variables
        private Product dcProduct;
        private int amount;
        private double multiplier;
        #endregion

        #region Init
        public AmountDiscount(Product discountedProduct, int amount, double multiplier)
        {
            dcProduct = discountedProduct;
            this.amount = amount;
            this.multiplier = multiplier;
        }
        #endregion

        #region Calculations

        public override double getDiscount(Dictionary<char, int> productsInCart, bool hasMembership)
        {
            int relevants = getRelevantItemsFromCart(productsInCart, dcProduct.name);
            return (relevants >= amount) ? relevants * dcProduct.price * (1 - multiplier) : 0;
        }
        #endregion
    }
}
