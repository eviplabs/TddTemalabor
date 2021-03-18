using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class SuperShopPoint
    {
        public int id { get; set; }

        public double value { get; set; }

        public SuperShopPoint(){}

        public SuperShopPoint(int id, double value)
        {
            this.id = id;
            this.value = value;
        }
    }
}
