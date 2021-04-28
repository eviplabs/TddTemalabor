using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    public static class ExtensionMethods
    {
        public static double GetPriceByProductChar(this char self, List<Product> Products) 
            => Products.Where(p => p.Name == self).Select(p => p.Price).Single();

        public static Int32 ToInt(this string number)
        {
            return Convert.ToInt32(number);
        }

        public static Dictionary<char, (int, int)> ConvertStringToDictionary(this string self) 
        {
            return self.GroupBy(c => c)
                   .Select(c => new { c.Key, Count = c.Count(), Remains = c.Count() })
                   .ToDictionary(t => t.Key, t => (t.Count, t.Remains));
        }

        public static string RebuildName(this Dictionary<char, (int, int)> self) 
        {
            string name = "";
            foreach (var a in self.Keys)
            {
                for (int i = 0; i < self[a].Item2; i++)
                {
                    name += a;
                }
            }
            return name;
        }
    }
}
