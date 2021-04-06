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
        private List<Product> dcProducts;
        private int newPrice;
        private bool membershipRequired;
        #endregion

        #region Init
        public ComboDiscount(List<Product> discountedProducts, int newPrice, bool membershipRequired = false)
        {
            dcProducts = discountedProducts;
            this.newPrice = newPrice;
            this.membershipRequired = membershipRequired;
        }
        #endregion

        #region Calculations
        public override double getDiscount(string shopping_cart, string items, int price)
        {
            if (!areConditionsFulfilled(shopping_cart))
            {
                return 0;
            }
            int maxOccurence = shopping_cart.Length;
            foreach (var product in dcProducts)
            {
                int currentOccurence = getRelevantItemsFromCart(shopping_cart, product.name);
                if (maxOccurence > currentOccurence)
                {
                    maxOccurence = currentOccurence;
                }
            }
            return (price - newPrice) * maxOccurence;
        }
        private bool areConditionsFulfilled(string shopping_cart)
        {
            if (membershipRequired && (!shopping_cart.Contains('t'))
                || dcProducts.Where(i => !shopping_cart.Contains(i.name)).Any())
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
