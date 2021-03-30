using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class InMemoryInventory : IInventory
    {
        public int GetProductQuantity(char product)
        {
            throw new NotImplementedException();
        }

        public void RefreshProduct(char product, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
