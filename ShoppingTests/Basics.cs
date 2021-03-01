using Shopping;
using System;
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
            var s = new Shop();
            Assert.NotNull(s);
        }

        [Fact]
        public void ProductRegistration()
        {
            var s = new Shop();
            s.RegisterProduct("A", 10);
            var price = s.GetPrice("A");
            Assert.Equal(10, price);
        }

        [Fact]
        public void ProductRegistration2()
        {
            var s = new Shop();
            s.RegisterProduct("B", 20);
            var price = s.GetPrice("B");
            Assert.Equal(20, price);
        }

        [Fact]
        public void ProductRegistration3()
        {
            var s = new Shop();
            s.RegisterProduct("C", 40);
            var price = s.GetPrice("C");
            Assert.Equal(40, price);
        }

        [Fact]
        public void PriceSum()
        {
            var s = new Shop();
            s.RegisterProduct("A", 10);
            s.RegisterProduct("B", 20);
            s.RegisterProduct("C", 30);
            var price = s.GetPrice("ABC");
            Assert.Equal(60, price);
        }

        [Fact]
        public void PriceSum2()
        {
            var s = new Shop();
            s.RegisterProduct("A", 10);
            s.RegisterProduct("B", 20);
            s.RegisterProduct("C", 30);
            s.RegisterProduct("D", 40);
            var price = s.GetPrice("ABCDA");
            Assert.Equal(110, price);
        }

        [Fact]
        public void PriceSum3() 
        {
            var s = new Shop();
            s.RegisterProduct("A", 10);
            s.RegisterProduct("B", 20);
            s.RegisterProduct("C", 30);
            s.RegisterProduct("D", 40);
            s.RegisterProduct("E", 50);
            var price = s.GetPrice("ABCDE");
            Assert.Equal(150, price);
        }

        [Fact]
        public void RegisterAmountDiscount1()
        {
            var Shop = new Shop();
            Shop.RegisterProduct("A", 10);
            Shop.RegisterProduct("B", 100);
            Shop.RegisterAmountDiscount("A", 5, 0.9);
            var price = Shop.GetPrice("AAAAAAB");
            Assert.Equal(154, price);
        }

        [Fact]
        public void RegisterAmountDiscount2()
        {
            var Shop = new Shop();
            Shop.RegisterProduct("A", 10);
            Shop.RegisterProduct("B", 100);
            Shop.RegisterAmountDiscount("A", 5, 0.9);
            Shop.RegisterAmountDiscount("B", 3, 0.8);
            var price = Shop.GetPrice("AAAABBBB");
            Assert.Equal(360, price);
        }

        [Fact]
        public void RegisterCountDiscount1()
        {
            var Shop = new Shop();
            Shop.RegisterProduct("A", 10);​
            Shop.RegisterProduct("E", 50);​
            Shop.RegisterCountDiscount('A', 3, 4);​
            // 3 áráért 4-et vihet​
            var price = Shop.GetPrice("AAAAAEEE");​
            // 5*10+3*50 helyett 4*10+3*50​
            Assert.Equal(140, price);
        }
    }
}
