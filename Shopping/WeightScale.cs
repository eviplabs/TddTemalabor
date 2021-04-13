using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping
{
    public class WeightScale : IWeightScale
    {
        public int LastMeasuredValue { get; set; }

        public int GetCurrentWeight()
        {
            // szerintem ennek valamilyen random értéket kellene visszaadnia
            throw new NotImplementedException();
        }
    }
}
