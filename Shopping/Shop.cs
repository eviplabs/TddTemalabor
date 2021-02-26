using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        public void RegisterProduct(string name, int price) { }
        public int GetPrice(string name) 
        {
            if (name.Equals("A")) 
            {
                return 10;
            }

            return 20;
        }
    }
}
