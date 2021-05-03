using System;

namespace Shopping
{
    class SuperShop
    {
        private int points;
        public SuperShop()
        {
            points = 0;
        }
        public double processSuperShop(double price, bool sspay)
        {
            double discount = getClubDiscount(price);
            price -= discount;
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
        private double getClubDiscount(double price)
        {
            return price * 0.1;
        }
        private int addPoints(double price)
        {
            return (int)Math.Round(0.01 * price,MidpointRounding.AwayFromZero); 
        }
    }
}
