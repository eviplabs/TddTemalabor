using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class MembershipDiscount : Discount
    {
        public override double getDiscount(string shopping_cart)
        {
            // TODO return price * 0.1;
            return 0;
        }
    }
}
