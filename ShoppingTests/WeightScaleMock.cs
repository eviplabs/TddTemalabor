using System;
using System.Collections.Generic;
using System.Text;
using Shopping;

namespace ShoppingTests
{
    class WeightScaleMock : IWeightScale
    {
        public int LastMeasuredValue { get; set; }

        public WeightScaleMock()
        {
            LastMeasuredValue = 2000;
        }
        public int GetCurrentWeight()
        {
            return 2000;
        }
    }
}
