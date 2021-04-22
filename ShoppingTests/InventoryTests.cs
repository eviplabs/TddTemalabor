using Shopping;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ShoppingTests
{
    public class InventoryTests
    {



        [Fact]
        public void GetInventoryQuantity()
        {
            Inventory Inventory = new Inventory();
            Shop Shop = new Shop(Inventory);

            Inventory.SetQuanity('A', 5);
            Shop.RegisterProduct('A', 10);
            
            Assert.Equal(5, Inventory.products['A']);
            var price = Shop.GetPrice("A");
            Assert.Equal(4, Inventory.products['A']);

            Assert.Equal(10, price);
        }

        [Fact]
        public void ReturnItem()
        {
            Inventory Inventory = new Inventory();
            Shop Shop = new Shop(Inventory);

            Inventory.SetQuanity('A', 5);
            Shop.RegisterProduct('A', 10);

            Assert.Equal(5, Inventory.products['A']);
            var originalPrice = Shop.GetPrice("AAAAA");
            Assert.Equal(50, originalPrice);

            var priceAfterReturn=Shop.ReturnItem("A");
            Assert.Equal(1, Inventory.products['A']);
            Assert.Equal(40, priceAfterReturn);
        }
    }
}
