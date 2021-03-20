using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class AmountDiscount
    {
        public int Amount { get; set; }
        public double Factor { get; set; }
        public bool isMemberOnly { get; set; }


        public AmountDiscount(int amount, double factor, bool isMemberOnly)
        {
            Amount = amount;
            Factor = factor;
            this.isMemberOnly = isMemberOnly;
        }
    }
}
