using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping
{
    class ComboDiscountCalculator
    {
        private Dictionary<string, ComboDiscount> Discounts { get; }

        public ComboDiscountCalculator()
        {
            Discounts = new Dictionary<string, ComboDiscount>();
        }

        public void RegisterComboDiscount(string name, int newprice, bool clubMembership = false)
        {
           Discounts[name] = new ComboDiscount(newprice, clubMembership);
        }
        public double ApplyDiscount(string name, bool clubmember, double price,List<Product> products)
        {

            if (Discounts.Count > 0) 
            {
                string comboString;
                int count = name.GroupBy(c => c).Min(c => c.Count());

                foreach (var item in Discounts)
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
                        catch(Exception e) 
                        {
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                }
            }            
            return price;
        }
    }
}
