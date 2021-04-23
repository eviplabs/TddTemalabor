using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public interface IInventory
    {
        void RefreshProduct(char product, int quantity);

        int GetProductQuantity(char product);

        void RemoveProducts(string cart);

        void Add(char product, int quantity);
    }
}
