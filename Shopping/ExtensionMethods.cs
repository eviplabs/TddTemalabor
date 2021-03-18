using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public static class ExtensionMethods
    {
        static public int GetPriceByProductChar(this char self, List<Product> Products) 
            => Products.Where(p => p.Name == self).Select(p => p.Price).Single();
    }
}
