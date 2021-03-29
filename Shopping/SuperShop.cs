using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class SuperShop : Discount
    {
        private int points;
        public SuperShop()
        {
            points = 0;
        }

        public override double getDiscount(string shopping_cart, string items, int price)
        {
            if (points > price)
            {
                points -= price;
                return price;
            }
            else
            {
                price -= points;
                points = 0;
                return price;
            }
        }

    }
}
