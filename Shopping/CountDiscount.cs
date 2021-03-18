using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class CountDiscount
    {

        public int count { get; set; }

        public int bonus { get; set; }

        public CountDiscount(){}

        public CountDiscount(int count, int bonus)
        {
            this.count = count;
            this.bonus = bonus;
        }

    }
}
