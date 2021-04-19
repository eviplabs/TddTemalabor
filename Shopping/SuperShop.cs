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
        public double getMembershipDiscount(double price)
        {
            return price * 0.1;
        }
        public double processSuperShop(double price, bool sspay)
        {
            double discount = price * 0.1;
            price *= 0.9;
            if (sspay)
            {
                if (points > price)
                {
                    points -= (int)price;
                    discount += price;
                }
                else
                {
                    discount += points;
                    points = 0;
                }

            }
            points += addPoints(price - discount);
            return discount;
        }
        private int addPoints(double price)
        {
            return (int)Math.Round(0.01 * price,MidpointRounding.AwayFromZero); 
        }
    }
}
