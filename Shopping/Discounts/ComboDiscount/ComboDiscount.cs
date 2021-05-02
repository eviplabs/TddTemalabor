using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class ComboDiscount
    {
        public double newprice { get; set; }
        public bool clubMembershipOnly { get; set; }

        public ComboDiscount(double newprice , bool clubMembershipOnly) {
            this.newprice = newprice;
            this.clubMembershipOnly = clubMembershipOnly;
        }
    }
}
