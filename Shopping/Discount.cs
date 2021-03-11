using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Discount
    {
        public int amount { get; set; }
        public double multiplier { get; set; }
        public Discount(int amount, double multiplier)
        {
            this.amount = amount;
            this.multiplier = multiplier;
        }
    }
}
