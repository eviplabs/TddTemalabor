using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class Product
    {
        private char name;
        public int price { get; set; }

        public Product(char name, int price)
        {
            this.name = name;
            this.price = price;
        }
    }
}
