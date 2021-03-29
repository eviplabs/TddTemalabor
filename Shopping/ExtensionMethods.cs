using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public static class ExtensionMethods
    {
        static public bool hasKeyword(this string self, char keyword)
            => self.Contains(keyword);
    }
}
