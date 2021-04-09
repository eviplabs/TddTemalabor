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
    }
}
