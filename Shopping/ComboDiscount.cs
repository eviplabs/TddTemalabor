﻿using System;
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

        public override double getDiscount(ref Dictionary<char, int> productsInCart, bool hasMembership)
        {
            if (!areConditionsFulfilled(productsInCart, hasMembership))
            {
                return 0;
            }
            int maxOccurence = productsInCart.Sum(i => i.Value);
            foreach (var product in dcProducts)
            {
                int currentOccurence = productsInCart[product.name];
                if (maxOccurence > currentOccurence)
                {
                    maxOccurence = currentOccurence;
                }
            }
            removeFromCart(ref productsInCart, maxOccurence);
            return (dcProducts.Sum(p => p.price) - newPrice) * maxOccurence;
        }

        protected override void removeFromCart(ref Dictionary<char, int> productsInCart, int occurence)
        {
            foreach(var product in dcProducts)
            {
                if (productsInCart.Keys.Contains(product.name))
                {
                   productsInCart[product.name] -= occurence;
                }
            }
        }

        private bool areConditionsFulfilled(Dictionary<char, int> productsInCart, bool hasMembership)
        {
            if (membershipRequired && (!hasMembership)
                || dcProducts.Where(i => !productsInCart.Keys.Contains(i.name)).Any())
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
