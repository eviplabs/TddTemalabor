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
        public override double getDiscount(string shopping_cart)
        {
            int relevants = getRelevantItemsFromCart(shopping_cart, dcProduct.name);
            return (relevants >= amount) ? relevants * dcProduct.price * (1-multiplier) : 0;
        }

        public override double getDiscount(Dictionary<char, int> productsInCart)
        {
            int relevants = productsInCart[dcProduct.name];
            return (relevants >= amount) ? relevants * dcProduct.price * (1 - multiplier) : 0;
        }
        #endregion
    }
}
