using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class AmountDiscount
    {
        public int Amount { get; set; }
        public double Factor { get; set; }

        public AmountDiscount(int amount, double factor)
        {
            Amount = amount;
            Factor = factor;
        }
    }
}
