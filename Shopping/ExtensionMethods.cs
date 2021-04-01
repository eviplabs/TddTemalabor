using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public static class ExtensionMethods
    {
        static public double GetPriceByProductChar(this char self, List<Product> Products) 
            => Products.Where(p => p.Name == self).Select(p => p.Price).Single();

        static public string ReplaceNumbersFromName(this string name) 
        {
            foreach (var n in name.Where(n => char.IsDigit(n)))
                name = name.Replace(n.ToString(), "");
            return name;
        }
        public static Int32 ToInt(this string number)
        {
            return Convert.ToInt32(number);
        }
    }
}
