using Shopping;
using Xunit;

namespace ShoppingTests
{
    public abstract class TestBase
    {
        protected readonly Shop sh = new Shop();
        protected TestBase()
        {
            sh.RegisterProduct('A', 10);
            sh.RegisterProduct('B', 20);
            sh.RegisterProduct('C', 50);
            sh.RegisterProduct('D', 100);
        }
        protected void AssertPrice(double expected, string cart)
        {
            uint result = sh.GetPrice(cart);
            Assert.Equal(expected, result);
        }
    }
}
