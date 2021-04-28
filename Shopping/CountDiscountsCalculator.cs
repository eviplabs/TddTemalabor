using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    class CountDiscountsCalculator
    {

        private Dictionary<char, CountDiscount> Discounts { get; }

        public CountDiscountsCalculator()
        {
            Discounts = new Dictionary<char, CountDiscount>();
        }

        public void RegisterCountDiscount(char name, int count, int bonus, bool isDiscountclubMembershipExclusive = false)
        {
            Discounts[name] = new CountDiscount(count, bonus, isDiscountclubMembershipExclusive);
        }

        public (int,int) CountDiscountCalculator(char ProductID, Dictionary<char, (int, int)> ProductCount)
        {
            int pc = ProductCount[ProductID].Item1;
            int a = Discounts[ProductID].count;
            int b = Discounts[ProductID].bonus;
            
            return (pc - (pc / b) * (b - a),pc-b);
        }

        public void ApplyDiscount(Dictionary<char, (int, int)> ProductCount, bool isUserClubMember)
        {
            if(Discounts.Count > 0) 
            {
                Dictionary<char, (int, int)> forfor = new Dictionary<char, (int, int)>(ProductCount);

                foreach (var key in forfor.Keys)
                {

                    if (Discounts.ContainsKey(key) && ProductCount[key].Item2 >= Discounts[key].bonus)
                    {
                        if (Discounts[key].clubMembershipExclusive == false || Discounts[key].clubMembershipExclusive == true && isUserClubMember)
                        {
                            ProductCount[key] = CountDiscountCalculator(key, ProductCount);
                        }
                    }
                }
            }
        }
    }
}
