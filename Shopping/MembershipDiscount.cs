using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class MembershipDiscount : Discount
    {
        public override double getDiscount(string shopping_cart, string items, int price)
        {
            return price * 0.1;
        }
    }
}
