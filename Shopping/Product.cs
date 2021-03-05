using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class Product
    {
        public char name { get; set; }
        public int price { get; set; }

        public Product(char name, int price)
        {
            this.name = name;
            this.price = price;
        }
    }
}
