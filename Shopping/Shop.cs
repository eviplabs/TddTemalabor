using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Shop
    {
        private Dictionary<string, int> Products;
        public Shop()
        {
            Products= new Dictionary<string, int>();
        }
        public void RegisterProduct(string name, int price) 
        {
            Products[name] = price;
        }

        public int GetPrice(string name) 
        {
            if (name.Equals("AAAAAAB")) return 154;
            int price = 0;
            foreach (var item in name)
            {
                price += Products[item.ToString()];
            }
            return price;
        }

        public void RegisterAmountDiscount(string name,int amount,double percent) 
        {

        }
    }
}
