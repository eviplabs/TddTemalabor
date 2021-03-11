using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class CountDiscount : Discount
    {
        public int required { get; set; }
        public int freeItem { get; set; }
        public CountDiscount(int required, int freeItem)
        {
            this.required = required;
            this.freeItem = freeItem;
        }
        public override double getDiscount(string shpping_cart, string item, int price)
        {
            return (getRelevantItemsFromCart(shpping_cart, char.Parse(item)) / required) * price;
        }
    }
}
