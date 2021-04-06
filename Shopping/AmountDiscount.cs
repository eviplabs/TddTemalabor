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

        public override double getDiscount(ref Dictionary<char, int> productsInCart, bool hasMembership)
        {
            int relevants = getRelevantItemsFromCart(productsInCart, dcProduct.name);
            removeFromCart(ref productsInCart, relevants);
            return (relevants >= amount) ? relevants * dcProduct.price * (1 - multiplier) : 0;
        }

        protected override void removeFromCart(ref Dictionary<char, int> productsInCart, int occurence)
        {
            productsInCart[dcProduct.name] -= occurence;
        }
        #endregion
    }
}
