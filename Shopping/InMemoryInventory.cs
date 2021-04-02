using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public class InMemoryInventory : IInventory
    {
        private Dictionary<char, int> products = new Dictionary<char, int>();

        public Dictionary<char,int> Products
        {
            get { return products; }
        }
        public int GetProductQuantity(char product)
        {
            return products[product];
        }

        public void RefreshProduct(char product, int quantity)
        {
            if((products.ContainsKey(product)) && (products[product] >= quantity)) { products[product] -= quantity; }
        }

        public void RemoveProducts(string cart)
        {
            List<char> productList = cart.ToList();
            var grouped = productList.GroupBy(p => p);
            foreach(var group in grouped)
            {
                RefreshProduct(group.Key, group.Count());
            }
        }
    }
}
