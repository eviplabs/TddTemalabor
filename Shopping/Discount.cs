using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Discount
    {
        private int amount;
        private double multiplier;
        public Discount(int amount, double multiplier)
        {
            this.amount = amount;
            this.multiplier = multiplier;
        }
    }
}
