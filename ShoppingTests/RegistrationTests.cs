using Shopping;
using System;
using Xunit;

namespace ShoppingTests
{
    // Stryker.NET mutation testing tool:
    //  https://github.com/stryker-mutator/stryker-net
    // Moq package:
    //  https://github.com/moq/moq4
    public class RegistrationTests : ShoppingTestBase
    {

        [Fact]
        public void Instantiation()
        {
            Assert.NotNull(s);
        }

        [Fact]
        public void ProductRegistration()
        {
            RegisterProducts(3);

            var price = s.GetPrice("AABCC");
            Assert.Equal(100, price);
        }

        [Fact]
        public void DuplicateRegistrationOfProduct()
        {
            Assert.True(s.RegisterProduct('A', 10));
            Assert.False(s.RegisterProduct('A', 10));
            Assert.False(s.RegisterProduct('A', 200));
            var price = s.GetPrice("A");
            Assert.Equal(10, price);
        }

        [Fact]
        public void RegistrationOfProductWithInvalidName()
        {
            Assert.False(s.RegisterProduct('@', 10)); //'@'='A'-1            
            Assert.False(s.RegisterProduct('[', 10)); //'['='Z'+1
        }

        [Fact]
        public void RegistrationOfProductWithInvalidPrice()
        {
            Assert.False(s.RegisterProduct('A', 0));
            Assert.False(s.RegisterProduct('B', -1));
        }

        [Fact]
        public void NotRegisteredProductInGetPrice()
        {
            RegisterProducts(3);

            var price = s.GetPrice("AABCCZ");
            Assert.Equal(100, price);
        }

        [Fact]
        public void MoreProductWithOneCode()
        {
            RegisterProducts(3);

            var price = s.GetPrice("A2B3C");
            Assert.Equal(110, price);
        }

        [Fact]
        public void RefreshInventory()
        {
            s.RegisterProduct('A', 10, false, 5);  // A regisztráció során mostmár megadható a kezdeti szám (ennyi darab van raktáron kezdetben)
            s.RegisterProduct('B', 20, false, 5);
            var price = s.GetPrice("AB");
            var quantity = s.inventory.GetProductQuantity('A'); // Vásárlás után a darabszámnak eggyel csökkenni kell
            Assert.Equal(4, quantity);

            s.GetPrice("AAAA");
            quantity = s.inventory.GetProductQuantity('A');
            Assert.Equal(0, quantity);
        }

        [Fact]
        public void ProductStorno()
        {
            s.RegisterProduct('B', 20, false, 5);
            s.RegisterProduct('C', 30, false, 5);
            var price = s.GetPrice("BCv111");
            var stornoValue = s.Storno("BCv111", 'C');//("C" termekre);
            Assert.Equal(27, stornoValue);

        }

        [Fact]
        public void RegisterPurchase()
        {
            s.RegisterProduct('E', 75, false, 3);
            s.Cart.Add("E");
            var amountPaid = s.GetCartPrice();
            Assert.Equal(s.cashflowControl.LatestPurchase, amountPaid);
            //ICashflowControl vagy tetszoleges interface hasznalata.
        }
    }
}
