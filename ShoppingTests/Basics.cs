using Shopping;
using System;
using System.Collections.Generic;
using Xunit;

namespace ShoppingTests
{
    // Stryker.NET mutation testing tool: 
    //  https://github.com/stryker-mutator/stryker-net
    // Moq package:
    //  https://github.com/moq/moq4
    public class Basics
    {
        [Fact]
        public void Instantiation()
        {
            var pc = new Shop();
            Assert.NotNull(pc);
        }
        [Fact]
        public void ProductRegistration()
        {
            var sh = new Shop();
            sh.RegisterProduct('A', 10);
            var price = sh.GetPrice("AAA");
            //Ellenörzi, hogy a price objektum integer-e
            Assert.IsType<int>(price);
        }
    }   
}
