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
            var pc = new Shop();
            Assert.NotNull(pc);
        }
    }
}
