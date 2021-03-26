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
       /* public void getPrice(Dictionary<char, (int, int)> ProductCount) {
            string name = null; ;
            foreach (var a in ProductCount.Keys)
            {
                for (int i = 0; i < ProductCount[a].Item2; i++)
                {
                    name += a;
                }
            }
            Dictionary<char, (int, int)> Product = name.GroupBy(c => c)
               .Select(c => new { c.Key, Count = c.Count(), Remains = c.Count() })
               .ToDictionary(t => t.Key, t => (t.Count, t.Remains));

            try { 
                



            }
            catch(){
            
            }
            
        }*/


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
