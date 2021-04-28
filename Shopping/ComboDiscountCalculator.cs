using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    class ComboDiscountCalculator
    {
        public Dictionary<string, ComboDiscount> ComboDiscounts { get; }

        public ComboDiscountCalculator()
        {
            ComboDiscounts = new Dictionary<string, ComboDiscount>();
        }

        public void RegisterComboDiscount(string name, int newprice, bool clubMembership = false)
        {
           ComboDiscounts[name] = new ComboDiscount(newprice, clubMembership);
        }
        public double getPrice(string name, bool clubmember, double price,List<Product> products)
        {
            string comboString;
            int count = name.GroupBy(c => c).Min(c => c.Count());

            foreach (var item in ComboDiscounts)
            {
                if (item.Value.clubMembershipOnly == false || (item.Value.clubMembershipOnly == true && clubmember))
                {
                    comboString = new string(name);
                    int combo = 0;
                    try
                    {
                        for (int i = 0; i < count; i++)
                        {
                            combo = 0;
                            foreach (var c in item.Key)
                            {
                                comboString = comboString.Remove(comboString.IndexOf(c), c.ToString().Length);
                                price -= c.GetPriceByProductChar(products);
                                combo++;
                            }
                        }
                        if (combo == item.Key.Length)
                        {
                            price += item.Value.newprice * count;
                        }
                    }
                    catch { }
                }
            }
            return price;
        }
    }
}
