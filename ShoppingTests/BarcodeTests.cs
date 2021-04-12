using Shopping;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ShoppingTests
{
    public class BarcodeTests
    {
        public readonly Shop Shop = new Shop();

        [Fact]
        public void A2B2equalsAABB() {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            var price = Shop.GetPrice("A2B2");
            var price2 = Shop.GetPrice("AABB");
            Assert.Equal(price, price2);

        }

        [Fact]
        public void getPriceFromWeight() 
        {
            Shop.RegisterProduct('A', 10, false);
            Shop.RegisterProduct('B', 100, true); //kilo ár 
            Shop.RegisterProduct('C', 30, false);
            var price = Shop.GetPrice("AB1200C");
            Assert.Equal(160, price);
            //10+100*1.2+30 = 160
        }
    }
}
