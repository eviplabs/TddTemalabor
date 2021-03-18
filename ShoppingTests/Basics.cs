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
        private readonly Shop Shop = new Shop();
        [Fact]
        public void Instantiation()
        {
            Assert.NotNull(Shop);
        }

        [Fact]
        public void ProductRegistration()
        {
            Shop.RegisterProduct('A', 10);
            var price = Shop.GetPrice("A");
            Assert.Equal(10, price);
        }

        [Fact]
        public void ProductRegistration2()
        {
            Shop.RegisterProduct('B', 20);
            var price = Shop.GetPrice("B");
            Assert.Equal(20, price);
        }

        [Fact]
        public void ProductRegistration3()
        {
            Shop.RegisterProduct('C', 40);
            var price = Shop.GetPrice("C");
            Assert.Equal(40, price);
        }

        [Fact]
        public void PriceSum()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            var price = Shop.GetPrice("ABC");
            Assert.Equal(60, price);
        }

        [Fact]
        public void PriceSum2()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterProduct('D', 40);
            var price = Shop.GetPrice("ABCDA");
            Assert.Equal(110, price);
        }

        [Fact]
        public void PriceSum3() 
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterProduct('D', 40);
            Shop.RegisterProduct('E', 50);
            var price = Shop.GetPrice("ABCDE");
            Assert.Equal(150, price);
        }

        [Fact]
        public void RegisterAmountDiscount1()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 100);
            Shop. RegisterAmountDiscount('A', 5, 0.9);
            var price = Shop.GetPrice("AAAAAAB");
            Assert.Equal(154, price);
        }

        [Fact]
        public void RegisterAmountDiscount2()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 100);
            Shop.RegisterAmountDiscount('A', 5, 0.9);
            Shop.RegisterAmountDiscount('B', 3, 0.8);
            var price = Shop.GetPrice("AAAABBBB");
            Assert.Equal(360, price);
        }

        [Fact]
        public void RegisterCountDiscount1()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('E', 50);
            Shop.RegisterCountDiscount('A', 3, 4);
            // 3 áráért 4-et vihet​
            var price = Shop.GetPrice("AAAAAEEE");
            // 5*10+3*50 helyett 4*10+3*50​
            Assert.Equal(190, price);
        }

        [Fact]
        public void RegisterCountDiscount2()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterCountDiscount('A', 3, 4);
            Shop.RegisterCountDiscount('B', 2, 3);
            var price = Shop.GetPrice("AAAABBBC");
            // 4*10 + 3​*20 + 30 --> 3*10 + 2*20 + 30
            Assert.Equal(100, price);
        }

        [Fact]
        public void RegisterCountDiscount3()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterCountDiscount('A', 3, 5);
            Shop.RegisterCountDiscount('B', 2, 4);
            var price = Shop.GetPrice("AAAAABBBBCC");
            Assert.Equal(130, price);
        }

        [Fact]
        public void RegisterComboDiscount1()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 50);
            Shop.RegisterProduct('D', 100);
            Shop.RegisterComboDiscount("ABC", 60);
            var price = Shop.GetPrice("CAAAABB");
            // ABC+AAAB -> 60+3*10+20​
            Assert.Equal(110, price);
        }

        [Fact]
        public void RegisterComboDiscount2()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 50);
            Shop.RegisterProduct('D', 100);
            Shop.RegisterComboDiscount("ABC", 60);
            Shop.RegisterComboDiscount("BBB", 40);
            var price = Shop.GetPrice("CAAAABBBB");
            // ABC+AAAB -> 60+3*10+20​
            Assert.Equal(130, price);
        }

        [Fact]
        public void RegisterComboDiscount3()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 50);
            Shop.RegisterProduct('D', 100);
            Shop.RegisterComboDiscount("ABC", 60);
            var price = Shop.GetPrice("ABCABC");
            // ABC+AAAB -> 60+3*10+20​
            Assert.Equal(120, price);
        }

        [Fact]
        public void RegisterClubMembership()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 50);
            Shop.RegisterProduct('D', 100);
            var price = Shop.GetPrice("ABCDt");
            //(10+20+50+100)*0.9 = 162
            Assert.Equal(162, price);
        }

        [Fact]
        public void When_MoretInGetPrice_Expect_IgnoreThemExceptOne()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 50);
            Shop.RegisterProduct('D', 100);
            var price = Shop.GetPrice("ABCDtttttt");
            //(10+20+50+100)*0.9 = 162
            Assert.Equal(162, price);
        }

        [Fact]
        public void Register_Club_Membership_With_Different_Prices()
        {
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterProduct('D', 150);
            var price = Shop.GetPrice("ABCDt");
            //(10+20+30+150)*0.9 = 189
            Assert.Equal(189, price);
        }

        [Fact]
        public void Register_Club_Membership_Exclusive_Combo_Discount()
        {
            Shop.RegisterProduct('A', 20);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterProduct('D', 150);
            //A RegisterComboDiscount 3. paramétere lenne, hogy csak tagok használhatják-e
            Shop.RegisterComboDiscount("ABC", 60, true);
            //(60)*0.9 = 54
            var price = Shop.GetPrice("ABCt");
            Assert.Equal(54, price);

            //20+20+30 = 70
            price = Shop.GetPrice("ABC");
            Assert.Equal(70, price);
        }

        [Fact]
        public void AddPointsToSupershopCard()
        {
            Shop.RegisterProduct('A', 20);
            Shop.RegisterProduct('B', 30);
            Shop.RegisterProduct('C', 50);

            var price = Shop.GetPrice("ABC1");
            var points = Shop.GetSupershopPoints(price);
            Assert.Equal(1,points);
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
    }
}
