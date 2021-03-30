using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public interface IInventory
    {
        void RefreshProduct(char product, int quantity);

        int GetProductQuantity(char product);
    }
}
