using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class ComboDiscount
    {
        public List<char> comboProducts= new List<char>();
        public int comboDiscount { get; set; }

        public ComboDiscount()
        {
        }

        public void AddItem(char item)
        {
            comboProducts.Add(item);
        }
    }
}
