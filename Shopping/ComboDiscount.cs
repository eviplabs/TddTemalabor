using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class ComboDiscount : Discount
    {
        #region Variables
        public int newPrice { get; set; }
        public bool membership { get; set; }
        #endregion

        #region Init
        public ComboDiscount(int newPrice, bool membership)
        {
            this.newPrice = newPrice;
            this.membership = membership;
        }
        #endregion

        #region Calculations
        public override double getDiscount(string shopping_cart, string items, int prices)
        {
            int maxOccurence = shopping_cart.Length;
            foreach (char item in items.ToCharArray())
            {
                if (!shopping_cart.Contains(item))
                {
                    return 0;
                }
                else
                {
                    int currentOccurence = getRelevantItemsFromCart(shopping_cart, item);
                    if (maxOccurence > currentOccurence)
                    {
                        maxOccurence = currentOccurence;
                    }
                }
            }
            return (prices - newPrice) * maxOccurence;
        }
        #endregion
    }
}
