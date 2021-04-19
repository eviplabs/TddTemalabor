using Shopping;
using System;
using Xunit;

namespace ShoppingTests
{
    // Stryker.NET mutation testing tool:
    //  https://github.com/stryker-mutator/stryker-net
    // Moq package:
    //  https://github.com/moq/moq4
    public class BarcodeTests : ShoppingTestBase
    {

        [Fact]
        public void Instantiation()
        {
            Assert.NotNull(s);
        }

        [Fact]
        public void WeighBasedPricing()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20, true);
            var price = s.GetPrice("A2B1200");
            Assert.Equal(44, price); //2*10+1.2*20
        }

        [Fact]
        void BarcodeReader()
        {
            s.RegisterProduct('A', 10, false, 3);
            s.RegisterProduct('B', 20, false, 3);
            s.RegisterProduct('C', 30, false, 3);
            s.RegisterProduct('D', 40, false, 3);
            s.AddToCart('A');
            s.AddToCart('B');
            s.AddToCart('C');
            s.AddToCart('D');
            Assert.Equal(s.GetPrice("ABCD"), s.GetCartPrice()); // Ha marad a GetPrice
        }

        [Fact]
        public void BarCodeReaderAndWeightScale()
        {
            s.RegisterProduct('A', 10);
            s.RegisterProduct('B', 20, true);
            s.AddToCart('A');
            s.AddToCart('B'); // addToCart()-ban le kell merni a sulyt
            string weightOfProductB = s.weightScale.LastMeasuredValue.ToString();
            Assert.Equal(s.GetPrice("AB" + weightOfProductB), s.GetCartPrice());
        }
    }
}
