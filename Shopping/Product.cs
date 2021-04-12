using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Product
    {
        public double Price { get; set; }

        public char Name { get; set; }

        public bool Weighted { get; set; }

        public Product(){}

        public Product(char name,double price, bool weighted = false)
        {
            this.Price = weighted ? price / 1000 : price;
            this.Name = name;
            this.Weighted = weighted;
        }
    }
}
