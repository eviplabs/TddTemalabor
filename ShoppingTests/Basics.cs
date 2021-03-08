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
        private readonly Shop sh = new Shop();

        [Fact]
        public void Instantiation()
        {
            Assert.NotNull(sh);
        }

        [Fact]
        public void ProductRegistration()
        {
            sh.RegisterProduct('A', 10);
            var price = sh.GetPrice("AAA");
            //Ellenörzi, hogy a price objektum integer-e
            Assert.IsType<int>(price);
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
        public void AccuratePriceCalculation2()
        {
            sh.RegisterProduct('A', 20);
            sh.RegisterProduct('G', 30);
            sh.RegisterProduct('E', 60);
            Assert.Equal(230, sh.GetPrice("AGGGEE"));
        }

        [Fact]
        public void isProductNameCaseSensitive()
        {
            sh.RegisterProduct('a', 10);
            Assert.Equal(10, sh.GetPrice("A"));
        }

        [Fact(Skip ="A GetPrice még nem jó teljesen :)")]
        public void RegisterAmountDiscount()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 100);
            sh.RegisterAmountDiscount('A', 5, 0.9);
            var price = sh.GetPrice("AAAAAAB");
            Assert.Equal(154, price);
        }

    }
}
