﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class CountDiscount : Discount
    {
        #region Variables
        private int required;
        private int freeItem;
        #endregion

        #region Init
        public CountDiscount(int required, int freeItem)
        {
            this.required = required;
            this.freeItem = freeItem;
        }
        #endregion

        #region Calculations
        public override double getDiscount(string shopping_cart, string item, int price)
        {
            return (getRelevantItemsFromCart(shopping_cart, char.Parse(item)) / freeItem) * (freeItem - required) * price;
        }
        #endregion
    }
}
