using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class AmountDiscount
    {
        public char ProductName { get; set; }
        public int Amount { get; set; }
        public double Factor { get; set; }

        public AmountDiscount(char name, int amount, double factor)
        {
            ProductName = name;
            Amount = amount;
            Factor = factor;
        }






    }
}
