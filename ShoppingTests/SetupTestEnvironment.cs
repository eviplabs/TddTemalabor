using Shopping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingTests
{
    abstract public class SetupTestEnvironment
    {
        static public void SetupEnvironment(Shop shop)
        {
            shop.RegisterProduct('A', 10);
            shop.RegisterProduct('B', 20);
            shop.RegisterProduct('C', 30);
            shop.RegisterProduct('D', 40);
            shop.RegisterProduct('E', 50);
        }
    }
}
