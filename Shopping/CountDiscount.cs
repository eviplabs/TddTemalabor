using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class CountDiscount
    {
        public int Buy { get; set; } 
        public int Get { get; set; } //Buy-t fizet Get-et vihet
        public CountDiscount(int buy, int get)
        {
            this.Buy = buy;
            this.Get = get;
        }
    }
}
