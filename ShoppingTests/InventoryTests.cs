using Shopping;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ShoppingTests
{
    public class InventoryTests
    {
        public readonly Shop Shop = new Shop();
        public readonly IInventory Inventory = new Inventory();


        [Fact]
        public void GetInventoryQuantity()
        {
            Shop.RegisterProduct('A', 10);
            Inventory.SetQuanity('A', 5);
            Assert.Equal(5, Inventory.Products["A"]);
            var price = Shop.GetPrice("A");
            Assert.Equal(4, Inventory.Products["A"]);

            Assert.Equal(10, price);
            

        }

    }
}
