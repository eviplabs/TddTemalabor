using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Inventory : IInventory
    {
        public Dictionary<char, int> products = new Dictionary<char, int>();

        public void SetQuanity(char product, int numberOfProduct) {
            if (!products.ContainsKey(product))
            {
                products.Add(product, 10);
            }
           
            products[product] = numberOfProduct;
            
        }
    }
}
