using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public class ComboDiscount
    {
        public List<char> ComboProducts= new List<char>();
        public int ComboPrice { get; set; }
        public bool isMemberOnly { get; set; }

        public ComboDiscount( string combo, int price, bool isMemberOnly)
        {
            ComboProducts = combo.ToList();
            ComboPrice = price;
            this.isMemberOnly = isMemberOnly;

        }

        public ComboDiscount()
        {

        }

    }
}
