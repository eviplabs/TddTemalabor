using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    class AmountDiscount
    {
        public int amount { get; set; }
        public double percent { get; set; }

        public AmountDiscount(int amount, double percent)
        {
            this.amount = amount;
            this.percent = percent;
        }
    }
}
