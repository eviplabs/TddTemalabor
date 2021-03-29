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
            int discount = 0;
            if (points > price)
            {
                points -= (int)price;
                discount = price;
            }
            else
            {
                discount = points;
                points = 0;
            }
            return discount;
        }
        public int addPoints(int price)
        {
            return points += (int)Math.Round(0.01 * price,MidpointRounding.AwayFromZero); 
        }

    }
}
