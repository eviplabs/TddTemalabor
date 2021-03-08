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

        public bool ClubOnly { get; set; }

        public ComboDiscount( string combo, int price, bool clubOnly)
        {
            ComboProducts = combo.ToList();
            ComboPrice = price;
            ClubOnly = clubOnly;

        }

        public ComboDiscount()
        {

        }

        public void AddItem(char item)
        {
            ComboProducts.Add(item);
        }

    }
}
