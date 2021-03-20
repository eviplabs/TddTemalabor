using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class CountDiscount
    {
        public int Buy { get; set; } 
        public int Get { get; set; } //Buy-t fizet Get-et vihet
        public bool isMemberOnly { get; set; }

        public CountDiscount(int buy, int get, bool isMemberOnly)
        {
            this.Buy = buy;
            this.Get = get;
            this.isMemberOnly = isMemberOnly;
        }
    }
}
