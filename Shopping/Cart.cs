using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class Cart
    {
        public Dictionary<char, int> Content { get; }
        private ProductData data;
        private IWeightScale scale;
        public string Receipt { get; set; }

        public Cart(ProductData data, IWeightScale scale)
        {
            this.data = data;
            this.scale = scale;
            Content = new Dictionary<char, int>();
        }

        public void Add(string scannable)
        {
            Receipt += scannable;
            if (scannable.Length == 1)
            {
                char product = char.Parse(scannable);
                if (Content.ContainsKey(product))
                {
                    Content[product]++;
                }
                else
                {
                    Content.Add(product, 1);
                }

            }
        }

        public double GetTotal()
        {
            double price = 0;
            foreach (var item in Content)
            {
                if (data.ProductsToWeigh.Contains(item.Key))
                {
                    price += (scale.GetCurrentWeight() / 1000) * data.Prices[item.Key];
                }
                else
                {
                    price += item.Value * data.Prices[item.Key];
                }
            }
            return price;
        }

        public void Empty()
        {
            Content.Clear();
            Receipt = "";
        }
    }
}
