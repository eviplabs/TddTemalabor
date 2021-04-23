using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class ProductData
    {
        private Dictionary<char, int> productPrices = new Dictionary<char, int>();

        public Dictionary<char, int> Prices
        {
            get { return productPrices; }
        }

        private HashSet<char> weightBasedProducts = new HashSet<char>();

        public HashSet<char> ProductsToWeigh
        {
            get { return weightBasedProducts; }
        }


    }
}
