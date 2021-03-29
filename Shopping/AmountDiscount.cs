using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class AmountDiscount : Discount
    {
        #region Variables
        private int amount;
        private double multiplier;
        #endregion

        #region Init
        public AmountDiscount(int amount, double multiplier)
        {
            this.amount = amount;
            this.multiplier = multiplier;
        }
        #endregion

        #region Calculations
        public override double getDiscount(string shopping_cart, string item, int price)
        {
            return (getRelevantItemsFromCart(shopping_cart, char.Parse(item)) >= amount) ? getRelevantItemsFromCart(shopping_cart, char.Parse(item)) * price * (1-multiplier) : 0;
        }
        #endregion
    }
}
