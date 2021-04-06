using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Product
    {
        public char name { get; private set; }
        public int price { get; private set; }
        public Product(char name, int price)
        {
            this.name = name;
            this.price = price;
        }
    }
}
