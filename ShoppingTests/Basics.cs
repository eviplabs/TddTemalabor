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
        private readonly Shop sh;
        public Basics() 
        {
            sh = new Shop();
        }
        [Fact]
        public void AccuratePriceCalculation()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 20);
            sh.RegisterProduct('C', 50);
            Assert.Equal(120 ,sh.GetPrice("AAABBC"));
        }
        [Fact]
        public void isProductNameCaseSensitive()
        {
            sh.RegisterProduct('a', 10);
            Assert.Equal(10, sh.GetPrice("A"));
        }
    }   
}
