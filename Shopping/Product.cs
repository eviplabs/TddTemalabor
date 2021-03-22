using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Product
    {
        public double Price { get; set; }

        public char Name { get; set; }

        public Product(){}

        public Product(char name,int price)
        {
            this.Price = price;
            this.Name = name;
        }
    }
}
