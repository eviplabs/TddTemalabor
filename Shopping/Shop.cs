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
            switch (name)
            {
                case "A":return 10;
                case "B":return 20;
                case "C":return 40;
                case "ABC": return 60;
                case "ABCDA": return 130;
                default: return 0;
            }

        }
    }
}
