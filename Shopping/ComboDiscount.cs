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
        private int newPrice { get; set; }
        private bool membershipRequired { get; set; }
        #endregion

        #region Init
        public ComboDiscount(int newPrice, bool membershipRequired)
        {
            this.newPrice = newPrice;
            this.membershipRequired = membershipRequired;
        }
        #endregion

        #region Calculations
        public override double getDiscount(string shopping_cart, string items, int prices)
        {
            if (!areConditionsFulfilled(shopping_cart, items))
            {
                return 0;
            }
            int maxOccurence = shopping_cart.Length;
            foreach (char item in items.ToCharArray())
            {
                int currentOccurence = getRelevantItemsFromCart(shopping_cart, item);
                if (maxOccurence > currentOccurence)
                {
                    maxOccurence = currentOccurence;
                }
            }
            return (prices - newPrice) * maxOccurence;
        }
        private bool areConditionsFulfilled(string shopping_cart, string items)
        {
            if (membershipRequired && (!shopping_cart.hasKeyword('t'))
                || items.ToCharArray().Where(i => !shopping_cart.Contains(i)).Any())
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
