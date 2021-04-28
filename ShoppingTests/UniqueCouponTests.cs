﻿using System;
using System.Collections.Generic;
using System.Text;
using Shopping;
using Xunit;

namespace ShoppingTests
{
    public class UniqueCouponTests
    {
        public readonly Shop Shop = new Shop();
        [Fact]
        public void RegisterUniqueCoupon() 
        {
            SetupTestEnvironment.SetupEnvironment(Shop);
            Shop.RegisterCoupon("112554", 0.9);

            var price = Shop.GetPrice("AABk112554");
            Assert.Equal(36, price);
            
            var price2 = Shop.GetPrice("AABk112554");
            Assert.Equal(40, price2);
        }
    }
}
