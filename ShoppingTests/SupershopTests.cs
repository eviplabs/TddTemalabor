using System;
using Xunit;
using Shopping;

namespace ShoppingTests
{
    public class SupershopTests
    {
        private readonly Shop Shop = new Shop();

        public SupershopTests()
        {
            SetupTestEnvironment.SetupEnvironment(Shop);
        }
        [Fact]
        public void AddPointsToSupershopCard()
        {           
            var price = Shop.GetPrice("ABC1");
            var points = Shop.GetSupershopPoints(price);
            Assert.Equal(0.6, points);
        }

        [Fact (Skip = "skipped due to CRDP012")]
        public void AddPointsToSupershopCardMoreNumberInProductsName()
        {           
            var price = Shop.GetPrice("AB2C3D1");
            var points = Shop.GetSupershopPoints(price);
            Assert.Equal(1, points);
        }

        [Fact]
        public void PayWithSupershopPoints()
        {            
            var price = Shop.GetPrice("ABC1p");
            Assert.Equal(59.4, price);
        }

        [Fact]
        public void MoreThanOneDigitCustomerID()
        {          
            var price = Shop.GetPrice("ABCpv230");
            Assert.Equal(53.4, price);
        }

        [Fact (Skip = "skipped due to CRDP09")]
        public void MoreThanOneDigitCustomerIDWithV()
        {
            var price = Shop.GetPrice("ABCv21");
            Assert.Equal(90, price);
        }

        [Fact]
        public void When_UserIDProvided_Expect_ClubmembershipDiscountIsActivated()
        {
            var price = Shop.GetPrice("ABCv99");
            Assert.Equal(54, price);
        }
    }
}
