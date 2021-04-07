using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class SuperShop
    {
        private int points;
        public SuperShop()
        {
            points = 0;
        }

        public int getDiscount(double price)
        {
            price *= 0.1;
            int discount = 0;
            if (points > price)
            {
                points -= (int)price;
                discount = (int)price;
            }
            else
            {
                discount = points;
                points = 0;
            }
            return discount;
        }
        public void addPoints(double price)
        {
            this.points += (int)Math.Round(0.01 * price,MidpointRounding.AwayFromZero); 
        }
    }
}
