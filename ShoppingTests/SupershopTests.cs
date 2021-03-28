﻿using System;
using Xunit;
using Shopping;

namespace ShoppingTests
{
    public class SupershopTests
    {
        private readonly Shop Shop = new Shop();

        [Fact]
        public void AddPointsToSupershopCard()
        {
            Shop.RegisterProduct('A', 20);
            Shop.RegisterProduct('B', 30);
            Shop.RegisterProduct('C', 50);

            var price = Shop.GetPrice("ABC1");
            var points = Shop.GetSupershopPoints(price);
            Assert.Equal(1, points);
        }

        [Fact]
        public void AddPointsToSupershopCardMoreNumberInProductsName()
        {
            Shop.RegisterProduct('A', 20);
            Shop.RegisterProduct('B', 30);
            Shop.RegisterProduct('C', 50);
            Shop.RegisterProduct('D', 70);

            var price = Shop.GetPrice("AB2C3D1");
            var points = Shop.GetSupershopPoints(price);
            //20+30+50+70 = 170 * 0.01 = 1.7
            Assert.Equal(1.7, points);
        }

        [Fact]
        public void PayWithSupershopPoints()
        {
            Shop.RegisterProduct('A', 20);
            Shop.RegisterProduct('B', 30);
            Shop.RegisterProduct('C', 50);

            var price = Shop.GetPrice("ABC1p");
            var supershopPoints = Shop.GetSupershopPoints(300);
            //20+30+50 - 3 = 97
            Assert.Equal(97, price - supershopPoints);
        }

        [Fact]
        public void MoreThanOneDigitCustomerID()
        {
            Shop.RegisterProduct('A', 20);
            Shop.RegisterProduct('B', 30);
            Shop.RegisterProduct('C', 50);

            var price = Shop.GetPrice("ABCp230");
            //(20+30+50)*0,9
            Assert.Equal(90, price);
        }

        [Fact]
        public void MoreThanOneDigitCustomerIDWithV()
        {
            Shop.RegisterProduct('A', 20);
            Shop.RegisterProduct('B', 30);
            Shop.RegisterProduct('C', 40);

            var price = Shop.GetPrice("ABCv21");
            //(20+30+40)*0,9
            Assert.Equal(81, price);
        }
    }
}
