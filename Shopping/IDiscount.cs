using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    interface IDiscount
    {
        public int CalculatePrice(Dictionary<char, int> cart, Dictionary<char, int> products, bool isMember);
    }
}
