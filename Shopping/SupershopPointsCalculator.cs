using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class SupershopPointsCalculator
    {
        private List<SuperShopPoint> SupershopPoints;
        public SupershopPointsCalculator()
        {
            SupershopPoints = new List<SuperShopPoint>();
        }

        public void AddSupershopPoint(int id, double price)
        {
            SupershopPoints.Add(new SuperShopPoint(id, GetSupershopPoints(price)));
        }

        public List<SuperShopPoint> GetSupershopPointsList() 
        {
            return SupershopPoints;
        }

        public double GetSupershopPoints(double price) 
        {
            return price * 0.01;
        }
    }
}
