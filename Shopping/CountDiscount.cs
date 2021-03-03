using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class CountDiscount
    {
        public char ProductName { get; set; }
        public int Buy { get; set; } 
        public int Get { get; set; } //Buy-t fizet Get-et vihet
        public CountDiscount(char productName, int buy, int get)
        {
            this.ProductName = productName;
            this.Buy = buy;
            this.Get = get;
        }
    }
}
